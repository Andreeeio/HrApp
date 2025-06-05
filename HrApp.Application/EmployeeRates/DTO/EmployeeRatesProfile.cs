using AutoMapper;
using HrApp.Application.EmployeeRates.Command.AddTaskRate;

namespace HrApp.Application.EmployeeRates.DTO;

public class EmployeeRatesProfile : Profile
{
    public EmployeeRatesProfile()
    {
        CreateMap<AddTaskRateCommand, Domain.Entities.EmployeeRate>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Domain.Entities.EmployeeRate, EmployeeRateDto>()
            .ForMember(dest => dest.Rater, opt => opt.MapFrom(x => x.RatedBy.Email ?? ""));
    }
}