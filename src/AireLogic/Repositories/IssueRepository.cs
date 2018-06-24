using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AireLogic.Models;

namespace AireLogic.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private readonly ConcurrentDictionary<int, Issue> _bugStore = new ConcurrentDictionary<int, Issue>();

        public async Task<int> AddBug(Issue newBug, CancellationToken token)
        {
            // create an id and append to self link
            newBug.Uuid = newBug.GetHashCode();
            newBug.SelfLink += $"/{newBug.Uuid}";

            var result = await Task.Run(() => _bugStore.TryAdd(newBug.Uuid, newBug));

            if (!result) throw new ArgumentException("Duplicate item.");

            return newBug.Uuid;
        }

        public async Task DeleteBug(int id, CancellationToken token)
        {
            var result = await Task.Run(() => _bugStore.TryRemove(id, out var _));

            if (!result) throw new ArgumentException("Item does not exist.");
        }

        public async Task<Issue[]> GetBugAll(CancellationToken token)
        {
            var issues = await Task.Run(() => { return _bugStore.Values; });

            if (issues == null) throw new ArgumentException("Item does not exist.");

            // order issues by date opened
            var result = issues.OrderByDescending(e => e.DateTimeOpened.Year)
                               .ThenBy(e => e.DateTimeOpened.Date)
                               .ThenBy(e => e.DateTimeOpened.TimeOfDay)
                               .ToArray();

            return result;
        }

        public async Task<Issue> GetBugSingle(int id, CancellationToken token)
        {
            var result = await Task.Run(() => { return _bugStore.TryGetValue(id, out var bug) ? bug : null; });

            if (result == null) throw new ArgumentException("Item does not exist.");

            return result;
        }

        public async Task UpdateBug(Issue bug, CancellationToken token)
        {
            var original = await GetBugSingle(bug.Uuid, token);

            var result = await Task.Run(() => _bugStore.TryUpdate(bug.Uuid, bug, original));

            if (!result) throw new ArgumentException("Item update failed.");
        }
    }
}