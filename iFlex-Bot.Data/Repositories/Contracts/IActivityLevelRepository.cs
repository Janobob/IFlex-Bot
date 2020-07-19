using iFlex_Bot.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Data.Repositories.Contracts
{
    public interface IActivityLevelRepository
    {
        Task<IEnumerable<ActivityLevel>> GetActivityLevelsAsync();
        Task<ActivityLevel> GetActivityLevelByIdAsync(int activityId);
        Task AddActivityLevelAsync(ActivityLevel activityLevel);
        Task DeleteActivityLevelAsync(ActivityLevel activityLevel);
        Task DeleteActivityLevelByIdAsync(int activityId);
        Task UpdateActivityLevelAsync(ActivityLevel activity);
        Task SaveChangesAsync();
    }
}
