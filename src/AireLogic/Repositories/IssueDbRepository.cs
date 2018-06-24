using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AireLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace AireLogic.Repositories
{
    public class IssueDbRepository : IIssueRepository
    {
        private readonly IssueContext _context;

        public IssueDbRepository(IssueContext context)
        {
            _context = context;
        }

        public async Task<int> AddBug(Issue newBug, CancellationToken token)
        {
            // create an id and append to self link
            newBug.Uuid = newBug.GetHashCode();
            newBug.SelfLink += $"/{newBug.Uuid}";

            await _context.Issues.AddAsync(newBug, token);
            await _context.SaveChangesAsync(token);

            return newBug.Uuid;
        }

        public async Task DeleteBug(int id, CancellationToken token)
        {
            var toRemove = await GetBugSingle(id, token);

            _context.Remove(toRemove);
            await _context.SaveChangesAsync(token);
        }

        public async Task<Issue[]> GetBugAll(CancellationToken token)
        {
            return await _context.Issues.OrderBy(e => e.DateTimeOpened.Year)
                                  .ThenBy(e => e.DateTimeOpened.Date)
                                  .ThenBy(e => e.DateTimeOpened.TimeOfDay)
                                  .ToArrayAsync(token);
        }

        public async Task<Issue> GetBugSingle(int id, CancellationToken token)
        {
            return await _context.Issues.Where(e => e.Uuid == id).FirstOrDefaultAsync(token);
        }

        public async Task UpdateBug(Issue bug, CancellationToken token)
        {
            _context.Update(bug);
            await _context.SaveChangesAsync(token);
        }
    }
}