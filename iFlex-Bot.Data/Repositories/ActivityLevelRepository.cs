using iFlex_Bot.Data.Entities;
using iFlex_Bot.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Data.Repositories
{
    public class ActivityLevelRepository : IActivityLevelRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityLevelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddActivityLevelAsync(ActivityLevel activityLevel)
        {
            await _context.ActivityLevels.AddAsync(activityLevel);
        }

        public Task DeleteActivityLevelAsync(ActivityLevel activityLevel)
        {
            _context.ActivityLevels.Remove(activityLevel);

            return Task.CompletedTask;
        }

        public async Task DeleteActivityLevelByIdAsync(int activityId)
        {
            _context.ActivityLevels.Remove(await GetActivityLevelByIdAsync(activityId));
        }

        public async Task<ActivityLevel> GetActivityLevelByIdAsync(int activityId)
        {
            return await _context.ActivityLevels.FindAsync(activityId);
        }

        public async Task<IEnumerable<ActivityLevel>> GetActivityLevelsAsync()
        {
            return await _context.ActivityLevels.OrderBy(x => x.Order).ToListAsync();
        }

        public Task UpdateActivityLevelAsync(ActivityLevel activity)
        {
            _context.Entry(activity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
