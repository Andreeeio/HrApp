using AutoMapper;
using HrApp.Application.EmployeeRates.Command.AddTaskRate;
using HrApp.Application.Users.Command.AddUser;
using HrApp.Domain.Entities;

namespace HrApp.Application.Users.DTO;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AddUserCommand, User>();
        CreateMap<User, UserDTO>();
        CreateMap<User, AddTaskRateCommand>();
    }
}
