using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Contracts.MobileApp.MReport
{
    public class MAddEmergencyReportValidator: AbstractValidator<MAddEmergencyReport>
    {
        public MAddEmergencyReportValidator()
        {
            RuleFor(p => p.MobileUserId).NotEmpty();
            RuleFor(p => p.EmergencyServiceId).NotEmpty();
            RuleFor(p => p.Latitude).NotEmpty();
            RuleFor(p => p.Longitude).NotEmpty();
            RuleFor(p => p.Address).NotEmpty();
        }
        
    }
}