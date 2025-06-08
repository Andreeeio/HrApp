using AutoMapper;
using HrApp.Application.EmploymentHistories.Command.AddEmploymentHistory;
using HrApp.Domain.Entities;

namespace HrApp.Application.EmploymentHistories.DTO;

public class EmploymentHistoryProfile : Profile
{
    public EmploymentHistoryProfile()
    {
        CreateMap<EmploymentHistory,EmploymentHistoryDTO>();
        
        CreateMap<AddEmploymentHistoryCommand, EmploymentHistory>();
    }
}
