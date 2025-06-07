using AutoMapper;
using HrApp.Domain.Entities;

namespace HrApp.Application.Users.DTO;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDTO>();
    }
}
