using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AireLogic.Models;

namespace AireLogic.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ConcurrentDictionary<int, Person> _personStore = new ConcurrentDictionary<int, Person>();

        public async Task<int> AddPerson(Person newPerson, CancellationToken token)
        {
            // create an id and append to self link
            newPerson.Uuid = newPerson.GetHashCode();
            newPerson.SelfLink += $"/{newPerson.Uuid}";

            var result = await Task.Run(() => _personStore.TryAdd(newPerson.Uuid, newPerson)).ConfigureAwait(false);

            if (!result)
            {
                throw new ArgumentException("Duplicate item.");
            }
            return newPerson.Uuid;
        }

        public async Task DeletePerson(int id, CancellationToken token)
        {
            var result = await Task.Run(() => _personStore.TryRemove(id, out var _)).ConfigureAwait(false);

            if (!result)
            {
                throw new ArgumentException("Item does not exist.");
            }
        }

        public async Task<Person[]> GetPersonAll(CancellationToken token)
        {
            var result = await Task.Run(() => { return _personStore.Values.ToArray(); }).ConfigureAwait(false);

            if (result == null)
            {
                throw new ArgumentException("Item does not exist.");
            }

            return result;
        }

        public async Task<Person> GetPersonSingle(int id, CancellationToken token)
        {
            var result = await Task.Run(() => { return _personStore.TryGetValue(id, out var person) ? person : null; }).ConfigureAwait(false);

            if (result == null)
            {
                throw new ArgumentException("Item does not exist.");
            }

            return result;
        }

        public async Task UpdatePerson(Person person, CancellationToken token)
        {
            var original = await GetPersonSingle(person.Uuid, token).ConfigureAwait(false);

            var result = await Task.Run(() => _personStore.TryUpdate(person.Uuid, person, original)).ConfigureAwait(false);

            if (!result)
            {
                throw new ArgumentException("Item update failed.");
            }
        }
    }
}