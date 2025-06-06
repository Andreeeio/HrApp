using AutoMapper;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Department.Command.AddDepartment;

public class AddDepartmentCommandHandler : IRequestHandler<AddDepartmentCommand>
{
    private readonly ILogger _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public AddDepartmentCommandHandler(ILogger<AddDepartmentCommandHandler> logger, IDepartmentRepository repositoryd,IUserRepository repositoryu, IMapper mapper)
    {
        _logger = logger;
        _departmentRepository = repositoryd;
        _mapper = mapper;
        _userRepository = repositoryu;
    }
    public async Task Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(request.HeadOfDepartmentEmail);
        if(user == null)
            throw new BadRequestException($"User with email {request.HeadOfDepartmentEmail} does not exist.");

        user.TeamId = null;

        var department = _mapper.Map<Domain.Entities.Department>(request);
        department.HeadOfDepartment = user;
        department.HeadOfDepartmentId = user.Id;
        _logger.LogInformation("Adding department {DepartmentName}", department.Name);

        await _departmentRepository.CreateDepartmentAsync(department);
        await _userRepository.AddRolesForUserAsync(request.HeadOfDepartmentEmail, new List<string> { "Ceo" });

    }
}