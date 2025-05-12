using AutoMapper;

namespace HrApp.Application.WorkLog.DTO;

public class WorkingLogProfile : Profile
{
    public WorkingLogProfile()
    {
        CreateMap<HrApp.Domain.Entities.WorkLog, WorkLogDTO>();
        CreateMap<WorkLogDTO, HrApp.Domain.Entities.WorkLog>();
    }
}