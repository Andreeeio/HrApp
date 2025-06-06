﻿using AutoMapper;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Users.Command.EditUser;

public class EditUserCommandHandler(ILogger<EditUserCommandHandler> logger,
    IUserRepository userRepository) : IRequestHandler<EditUserCommand>
{
    public async Task Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserAsync(request.Id);
        if (user == null)
            throw new BadRequestException("User not found");

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;

        await userRepository.SaveChangesAsync(); 
        logger.LogInformation("User updated: {UserId}", request.Id);
    }
}
