using AutoMapper;
using HrApp.Application.Users.DTO;
using HrApp.Domain.Repositories;
using MediatR;

namespace HrApp.Application.Users.Query.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO?>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDTO?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(request.UserId);
        return _mapper.Map<UserDTO?>(user);
    }
}
