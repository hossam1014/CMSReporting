using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Contracts.MobileApp.MReport;

namespace Application.Interfaces.MobileApp
{
    public interface IMEmergencyReportRepo
    {
        public Task<Result> AddEmergencyReportAsync(MAddEmergencyReport addReport);
    }
}