using AutoMapper;
using HrApp.Application.EmployeeRates.Command.AddTaskRate;

namespace HrApp.Application.EmployeeRates.DTO;

public class EmployeeRatesProfile : Profile
{
    public EmployeeRatesProfile()
    {
        CreateMap<AddTaskRateCommand, Domain.Entities.EmployeeRate>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id));
    }
}