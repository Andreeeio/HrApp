using AutoMapper;
using HrApp.Application.WorkLog.Command.AddWorkLogExportHistory;
using HrApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Mappings
{
    public class AddWorkLogExportHistoryCommandProfile : Profile
    {
        public AddWorkLogExportHistoryCommandProfile() 
        {
            CreateMap<AddWorkLogExportHistoryCommand, WorkLogExportHistory>();
        }
    }
}
