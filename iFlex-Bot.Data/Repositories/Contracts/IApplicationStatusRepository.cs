using iFlex_Bot.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Data.Repositories.Contracts
{
    public interface IApplicationStatusRepository
    {
        Task AddApplicationStatusAsync(ApplicationStatus status);
        Task<ApplicationStatus> GetLastApplicationStatusAsync();
        Task SaveChangesAsync();
    }
}
