﻿using HrApp.Application.Feedback.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Feedback.Command.AddAnonymousFeedback
{
    public class AddAnonymousFeedbackCommand : AnonymousFeedbackDTO, IRequest
    {
    }
}
