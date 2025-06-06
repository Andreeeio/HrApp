using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Department.Command.DeleteDepartment;

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITeamAuthorizationService _teamAuthorizationService;
    private readonly ILogger<DeleteDepartmentCommandHandler> _logger;

    public DeleteDepartmentCommandHandler(ILogger<DeleteDepartmentCommandHandler> logger, 
        IDepartmentRepository departmentRepository,
        IUserRepository userRepository,
        ITeamAuthorizationService teamAuthorizationService)
    {
        _departmentRepository = departmentRepository;
        _logger = logger;
        _userRepository = userRepository;
        _teamAuthorizationService = teamAuthorizationService;
    }

    public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting department {DepartmentId}", request.DepartmentId);

        if (!_teamAuthorizationService.Authorize(ResourceOperation.Delete))
            throw new UnauthorizedException("You are not authorized to delete department!");

        if(_departmentRepository.CountDepartmentAsync().Result <= 1)
            throw new BadRequestException("You cannot delete the last department.");

        var department = await _departmentRepository.GetDepartmentByIdAsync(request.DepartmentId);
        if (department == null)
            throw new BadRequestException($"Department with ID {request.DepartmentId} does not exist.");

        var ceo = await _userRepository.GetUserAsync(department.HeadOfDepartmentId);
        if (ceo == null)
        {
            await _departmentRepository.DeleteDepartmentAsync(request.DepartmentId);
        }
        else
        {
            await _userRepository.AddRolesForUserAsync(ceo.Email, new List<string> { Roles.Senior.ToString() });
            await _departmentRepository.DeleteDepartmentAsync(request.DepartmentId);
        }
    }
}