using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AireLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace AireLogic.Repositories
{
    public class PersonDbRepository : IPersonRepository
    {
        private readonly PersonContext _context;

        public PersonDbRepository(PersonContext context)
        {
            _context = context;
        }

        public async Task<int> AddPerson(Person newPerson, CancellationToken token)
        {
            // create an id and append to self link
            newPerson.Uuid = newPerson.GetHashCode();
            newPerson.SelfLink += $"/{newPerson.Uuid}";

            await _context.People.AddAsync(newPerson, token);
            await _context.SaveChangesAsync(token);

            return newPerson.Uuid;
        }

        public async Task DeletePerson(int id, CancellationToken token)
        {
            var toRemove = await GetPersonSingle(id, token);

            _context.Remove(toRemove);
            await _context.SaveChangesAsync(token);
        }

        public async Task<Person[]> GetPersonAll(CancellationToken token)
        {
            return await _context.People.ToArrayAsync(token);
        }

        public async Task<Person> GetPersonSingle(int id, CancellationToken token)
        {
            return await _context.People.Where(e => e.Uuid == id).FirstOrDefaultAsync(token);
        }

        public async Task UpdatePerson(Person person, CancellationToken token)
        {
            _context.Update(person);
            await _context.SaveChangesAsync(token);
        }
    }
}