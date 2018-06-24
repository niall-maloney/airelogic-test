using System.Threading;
using System.Threading.Tasks;
using AireLogic.Models;

namespace AireLogic.Repositories
{
    public interface IIssueRepository
    {
        Task<int> AddBug(Issue bug, CancellationToken token);

        Task DeleteBug(int id, CancellationToken token);

        Task<Issue[]> GetBugAll(CancellationToken token);

        Task<Issue> GetBugSingle(int id, CancellationToken token);

        Task UpdateBug(Issue bug, CancellationToken token);
    }
}