using AutoMapper;
using HrApp.Application.Interfaces;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;

namespace HrApp.Application.Services;

public class RaportService : IRaportService
{
    private readonly IOverallRaportRepository _overallRaportRepository;
    private readonly IMapper _mapper;

    public RaportService(IOverallRaportRepository overallRaportRepository,
        IMapper mapper)
    {
        _overallRaportRepository = overallRaportRepository;
        _mapper = mapper;
    }

    public async Task GenerateRaport()
    {
        var users = await _overallRaportRepository.GetUserToCreateRaportAsync();
        var usersRaport = _mapper.Map<List<UserRaport>>(users);

        var teams = await _overallRaportRepository.GetTeamToCreateRaportAsync();
        var teamsRaport = _mapper.Map<List<TeamRaport>>(teams);

        var assignments = await _overallRaportRepository.GetAssignmentToCreateRaportAsync();
        var assignmentsRaport = _mapper.Map<List<AssignmentRaport>>(assignments);

        var overallRaport = new OverallRaport
        {
            Id = Guid.NewGuid(),
            Name = $"Raport {DateTime.Now:dd-MM-yyyy}",
            BackupDate = DateOnly.FromDateTime(DateTime.Now)
        };
        await _overallRaportRepository.CreateRaportAsync(overallRaport);

        foreach (var userRaport in usersRaport)
            userRaport.OverallRaportId = overallRaport.Id;

        foreach (var teamRaport in teamsRaport)
            teamRaport.OverallRaportId = overallRaport.Id;

        foreach (var assignmentRaport in assignmentsRaport)
            assignmentRaport.OverallRaportId = overallRaport.Id;

        await _overallRaportRepository.CreateRaportAsync(usersRaport);

        await _overallRaportRepository.CreateRaportAsync(teamsRaport);

        await _overallRaportRepository.CreateRaportAsync(assignmentsRaport);
    }
}
