using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Contracts.MobileApp.MReport
{
    public class MAddReportValidator: AbstractValidator<MAddReport>
    {
        public MAddReportValidator()
        {
            // RuleFor(p => p.MobileUserId).NotEmpty();
            RuleFor(p => p.Description).NotEmpty();
            RuleFor(p => p.IssueCategoryKey).NotEmpty();
            // RuleFor(p => p.Image) // to make it in just (.jpg , .png , ...);
            RuleFor(p => p.Latitude).NotEmpty();
            RuleFor(p => p.Longitude).NotEmpty();
            RuleFor(p => p.Address).NotEmpty();
        }
    }
}