using AutoMapper;
using HrApp.Application.Salary.Command.AddPaid;
using HrApp.Application.Salary.Command.UpdatePaid;
using HrApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Salary.DTO
{
    public class PaidProfile : Profile
    {
        public PaidProfile() 
        {
            CreateMap<Paid, PaidDTO>();
            CreateMap<AddPaidCommand, Paid>();
            CreateMap<UpdatePaidCommand, Paid>();
            CreateMap<Paid, UpdatePaidCommand>();
        }
    }
}
