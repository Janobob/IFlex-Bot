using iFlex_Bot.Data.Entities;
using iFlex_Bot.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Data.Repositories
{
    public class ApplicationStatusRepository : IApplicationStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddApplicationStatusAsync(ApplicationStatus status)
        {
            await _context.AddAsync(status);
        }

        public async Task<ApplicationStatus> GetLastApplicationStatusAsync()
        {
            return await _context.ApplicationStatus.OrderByDescending(x => x.IssueTime).FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
