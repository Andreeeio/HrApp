using AutoMapper;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Salary.Command.UpdatePaid;

public class UpdatePaidCommandHandler : IRequestHandler<UpdatePaidCommand>
{
    private readonly ISalaryRepository _salaryRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UpdatePaidCommandHandler> _logger;

    public UpdatePaidCommandHandler(ILogger<UpdatePaidCommandHandler> logger, ISalaryRepository salaryRepository, IUserRepository userRepository)
    {
        _logger = logger;
        _salaryRepository = salaryRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(UpdatePaidCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating paid entry with ID: {Id}", request.Id);

        if(!await _userRepository.IfUserExistAsync(request.Email))
            throw new BadRequestException($"User with email {request.Email} does not exist.");

        var paid = await _salaryRepository.GetPaidByIdAsync(request.Id);
        if (paid == null)
            throw new BadRequestException($"Paid with ID {request.Id} not found.");
       
        paid.BaseSalary = request.BaseSalary;
        await _salaryRepository.UpdatePaidAsync(paid);
    }
}
