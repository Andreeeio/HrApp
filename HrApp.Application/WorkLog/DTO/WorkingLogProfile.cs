using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.WorkLog.DTO
{
    public class WorkingLogProfile : Profile
    {
        public WorkingLogProfile()
        {
            CreateMap<HrApp.Domain.Entities.WorkLog, WorkLogDTO>();
            CreateMap<WorkLogDTO, HrApp.Domain.Entities.WorkLog>();
        }
    }
}
