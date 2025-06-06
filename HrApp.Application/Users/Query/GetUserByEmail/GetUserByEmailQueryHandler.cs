using AutoMapper;
using HrApp.Application.Users.DTO;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Users.Query.GetUserByEmail;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDTO?>
{
    private readonly IUserRepository _repository;
    private readonly ILogger<GetUserByEmailQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetUserByEmailQueryHandler(ILogger<GetUserByEmailQueryHandler> logger, IUserRepository userRepository, IMapper mapper)
    {
        _repository = userRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UserDTO?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserAsync(request.Email);

        if (user == null)
            throw new BadRequestException($"User with email {request.Email} not found");

        var dto = _mapper.Map<UserDTO>(user);
        return dto;
    }
}