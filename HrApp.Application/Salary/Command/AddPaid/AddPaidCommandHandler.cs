using AutoMapper;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Salary.Command.AddPaid;

public class AddPaidCommandHandler : IRequestHandler<AddPaidCommand>
{
    private readonly ISalaryRepository _salaryRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AddPaidCommandHandler> _logger;
    private readonly IMapper _mapper;

    public AddPaidCommandHandler(ILogger<AddPaidCommandHandler> logger ,ISalaryRepository salaryRepository, IMapper mapper, IUserRepository userRepository)
    {
        _salaryRepository = salaryRepository;
        _logger = logger;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task Handle(AddPaidCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new paid record for user with ID: {UserId}", request.UserId);

        var paid = _mapper.Map<Paid>(request);

        var user = await _userRepository.GetUserByEmail(request.Email);
        if (user == null)
            throw new BadRequestException($"User with email {request.Email} not found.");
        paid.UserId = user.Id;

        await _salaryRepository.AddPaid(paid);
    }
}
