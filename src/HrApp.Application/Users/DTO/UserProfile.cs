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
        
        CreateMap<User, UserRaport>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(u => u.Id))
            .ForMember(dest => dest.YearRoundSalary, opt => opt.MapFrom(src => src.SalaryHistory.Sum(s => s.Salary)));
    }
}
