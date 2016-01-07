using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Entities;

namespace XiaomiYiApp.Repositories.Interfaces
{
    public interface ICameraConfigurationRepository
    {
        Task<OperationResult<DetailedConfiguration>> GetDetailedConfigurationAsync();

        Task<OperationResult> UpdateConfigurationParameterAsync(String name, String value);

        Task<OperationResult> LoadDetailedConfigurationAsync();
       
    }
}
