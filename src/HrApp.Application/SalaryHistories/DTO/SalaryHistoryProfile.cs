using AutoMapper;
using HrApp.Domain.Entities;

namespace HrApp.Application.SalaryHistories.DTO;

public class SalaryHistoryProfile : Profile
{
    public SalaryHistoryProfile()
    {
        CreateMap<SalaryHistory, SalaryHistoryDTO>();
    }
}
