using System.Threading;
using System.Threading.Tasks;
using AireLogic.Models;

namespace AireLogic.Repositories
{
    public interface IPersonRepository
    {
        Task<int> AddPerson(Person person, CancellationToken token);

        Task DeletePerson(int id, CancellationToken token);

        Task<Person[]> GetPersonAll(CancellationToken token);

        Task<Person> GetPersonSingle(int id, CancellationToken token);

        Task UpdatePerson(Person person, CancellationToken token);

    }
}