using AutoMapper;
using HrApp.Application.WorkLog.Command.AddWorkLogExportHistory;
using HrApp.Domain.Entities;

namespace HrApp.Application.WorkLog.DTO;

public class WorkLogProfile : Profile
{
    public WorkLogProfile()
    {
        CreateMap<Domain.Entities.WorkLog, WorkLogDTO>();

        CreateMap<AddWorkLogExportHistoryCommand, WorkLogExportHistory>();

        CreateMap<WorkLogDTO, Domain.Entities.WorkLog>();
    }
}