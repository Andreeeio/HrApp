using AutoMapper;
using HrApp.Application.Salary.Command.AddPaid;
using HrApp.Application.Salary.Command.UpdatePaid;
using HrApp.Domain.Entities;

namespace HrApp.Application.Salary.DTO;

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
