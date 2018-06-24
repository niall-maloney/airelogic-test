using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using AireLogic.Models;
using AireLogic.Repositories;
using AireLogic.Dto;
using AutoMapper;

namespace AireLogic.Controllers.Api
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        private readonly IPersonRepository _personRepository;

        public PeopleController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            // retrieve people from repository
            var data = await _personRepository.GetPersonAll(ct);

            // map model to response dto
            var result = Mapper.Map<PersonDto[]>(data);

            // return successful response
            return Ok(new SuccessResult { Results = result, Status = "Successful" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct)
        {
            try
            {
                // retrieve person from repository
                var data = await _personRepository.GetPersonSingle(id, ct);

                // return not found if data is null
                if (data == null)
                {
                    return NotFound();
                }

                // map model to response dto
                var result = Mapper.Map<PersonDto>(data);

                // return successful response
                return Ok(new SuccessResult { Results = new[] { result }, Status = "Successful" });
            }
            catch (ArgumentException)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PersonCreateDto item, CancellationToken ct)
        {
            try
            {
                // map dto to model
                var toAdd = Mapper.Map<Person>(item);

                // create self link
                toAdd.SelfLink = $"{Request.Path}";

                // add new person to the repository
                var data = await _personRepository.AddPerson(toAdd, ct);

                // map model to response dto
                var result = Mapper.Map<PersonDto>(toAdd);

                // return created response
                return Created($"/api/people/{data}", new SuccessResult { Results = new[] { result }, Status = "Successful" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PersonUpdateDto item, CancellationToken ct)
        {
            try
            {
                // return non content response if item is null
                if (item == null)
                {
                    return NoContent();
                }

                // the 'unassigned' person can not be edited
                if (id <= 0)
                {
                    return Forbid("You can't edit this person");
                }

                // retrieve the person that is going to be updated
                var original = await _personRepository.GetPersonSingle(id, ct);

                // return not found response if existing item cannot be found
                if (original == null)
                {
                    return NotFound();
                }

                // map the update dto to model
                Person toUpdate = Mapper.Map(item, original);

                // update the person in the repository
                await _personRepository.UpdatePerson(toUpdate, ct);

                // map to dto
                var result = Mapper.Map<PersonDto>(toUpdate);

                // return successful response
                return Ok(new SuccessResult { Results = new[] { result }, Status = "Successful" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            try
            {
                // the 'unassigned' person can not be deleted
                if (id <= 0)
                {
                    return Forbid("You can't delete this person");
                }

                // retrieve the person that is going to be updated
                var data = await _personRepository.GetPersonSingle(id, ct);

                // delete person from repository
                await _personRepository.DeletePerson(id, ct);

                // map model to response dto
                var result = Mapper.Map<PersonDto>(data);

                // return successful response
                return Ok(new SuccessResult { Results = new[] { result }, Status = "Successful" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody]JsonPatchDocument<Person> personPatch, CancellationToken ct)
        {
            try
            {
                // the 'unassigned' person can not be edited
                if (id <= 0)
                {
                    return Forbid("You can't edit this person");
                }

                // retrieve the person that is going to be updated
                var data = await _personRepository.GetPersonSingle(id, ct);

                // apply the json patch
                personPatch.ApplyTo(data);

                // update the person in the repository
                await _personRepository.UpdatePerson(data, ct);

                // map model to repsonse dto
                var result = Mapper.Map<PersonDto>(data);

                // return succesfull result
                return Ok(new SuccessResult { Results = new[] { result }, Status = "Successful" });
            }
            catch
            {
                // TODO specific catch errors
                return BadRequest();
            }
        }
    }
}
