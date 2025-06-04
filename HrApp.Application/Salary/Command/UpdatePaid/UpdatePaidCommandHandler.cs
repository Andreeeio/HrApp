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
    private readonly IMapper _mapper;

    public UpdatePaidCommandHandler(ILogger<UpdatePaidCommandHandler> logger, ISalaryRepository salaryRepository, IMapper mapper, IUserRepository userRepository)
    {
        _logger = logger;
        _salaryRepository = salaryRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task Handle(UpdatePaidCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating paid entry with ID: {Id}", request.Id);

        if(!await _userRepository.IfUserExist(request.Email))
            throw new BadRequestException($"User with email {request.Email} does not exist.");

        var paid = await _salaryRepository.GetPaidById(request.Id);
        if (paid == null)
            throw new BadRequestException($"Paid with ID {request.Id} not found.");
       
        _mapper.Map(request, paid); 
        await _salaryRepository.UpdatePaid(paid);
    }
}
