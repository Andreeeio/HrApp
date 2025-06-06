﻿using AutoMapper;
using HrApp.Application.Assignment.DTO;
using HrApp.Application.Assignment.Query.GetAssignmentForTeam;
using HrApp.Application.Interfaces;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Assignment.Query.GetActiveAssignments;

public class GetActiveAssignmentsQueryHandler : IRequestHandler<GetActiveAssignmentsQuery, List<AssignmentDTO>>
{
    private readonly ILogger<GetActiveAssignmentsQueryHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;

    public GetActiveAssignmentsQueryHandler(ILogger<GetActiveAssignmentsQueryHandler> logger,
        IUserContext userContext,
        IAssignmentRepository repository,
        IMapper mapper)
    {
        _logger = logger;
        _userContext = userContext;
        _assignmentRepository = repository;
        _mapper = mapper;
    }

    public async Task<List<AssignmentDTO>> Handle(GetActiveAssignmentsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _userContext.GetCurrentUser();
        _logger.LogInformation("Getting all active assignments");

        if (currentUser == null)
            throw new UnauthorizedException("User is not authenticated");

        var assignments = await _assignmentRepository.GetAssignmentsAsync(false);

        var dto = _mapper.Map<List<AssignmentDTO>>(assignments);

        return dto;
    }
}
