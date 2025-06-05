using AutoMapper;
using HrApp.Application.Interfaces;
using HrApp.Application.Users.DTO;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace HrApp.Application.Users.Command.AddUser;

public class AddUserCommandHandler(ILogger<AddUserCommandHandler> logger,
    IUserRepository userRepository,
    IEmailSender emailSender,
    IMapper mapper) : IRequestHandler<AddUserCommand>
{
    private readonly ILogger<AddUserCommandHandler> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IMapper _mapper = mapper;
    public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        if(await _userRepository.IfUserExist(request.Email))
        {
            throw new BadRequestException("User with this email already exists");
        }

        _logger.LogInformation("Creating new user");

        var hmac = new HMACSHA512();

        var user = _mapper.Map<User>(request);

        string password;

        if(string.IsNullOrEmpty(request.Password))
        {
            password = CreatePassword(request);
        }
        else
        {
            password = request.Password;
        }

        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        user.PasswordSalt = hmac.Key;
        user.IsEmailConfirmed = false;
        user.ConfirmationTokenExpiration = DateTime.UtcNow.AddDays(3);
        user.ConfirmationToken = Guid.NewGuid().ToString();

        await _userRepository.CreateUser(user);
        await _emailSender.SendEmailAsync(
            request.Email,
            "Complete your account",
            $@"
            <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                    <p>Please change your password and confirm your email.</p>
            
                    <p><strong>Temporary password:</strong> {password}</p>
                    <p><strong>Confirmation token:</strong> {user.ConfirmationToken}</p>
                    <p>This token is valid until: {user.ConfirmationTokenExpiration:dd.MM.yyyy HH:mm:ss}</p>

                    <p>
                        <a href='https://www.youtube.com/watch?v=dQw4w9WgXcQ'
                           style='display: inline-block; padding: 10px 20px; background-color: #007bff; color: #fff;
                                  text-decoration: none; border-radius: 4px; font-weight: bold;'>
                            Confirm Email
                        </a>
                    </p>
                </body>
            </html>");

        _logger.LogInformation("New user created");

    }

    public string CreatePassword(AddUserCommand request)
    {
        string combined = request.FirstName + request.DateOfBirth.ToString("ddMMyyyy") + request.LastName;
        string normalized = combined.Replace(" ", string.Empty).Normalize(NormalizationForm.FormD); var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
