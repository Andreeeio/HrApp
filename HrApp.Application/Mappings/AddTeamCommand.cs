using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Mappings
{
    public class AddTeamCommand : Profile
    {
        public AddTeamCommand()
        {
            CreateMap<AddTeamCommand, Domain.Entities.Team>();
        }
    }
}
