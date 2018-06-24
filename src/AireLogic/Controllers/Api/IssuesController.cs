using System;
using System.Linq;
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
    public class IssuesController : Controller
    {
        private readonly IIssueRepository _issueRepository;

        public IssuesController(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            // retrieve issues from repository
            var data = await _issueRepository.GetBugAll(ct);

            // map model to response dto
            var result = Mapper.Map<IssueDto[]>(data);

            // return successful response
            return Ok(new SuccessResult { Results = result, Status = "Successful" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct)
        {
            try
            {
                // retrieve issue from repository
                var data = await _issueRepository.GetBugSingle(id, ct);

                // return not found if data is null
                if (data == null)
                {
                    return NotFound();
                }

                // map model to response dto
                var result = Mapper.Map<IssueDto>(data);

                // return successful response
                return Ok(new SuccessResult { Results = new[] { result }, Status = "Successful" });
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IssueCreateDto item, CancellationToken ct)
        {
            try
            {
                // map dto to model
                var toAdd = Mapper.Map<Issue>(item);

                // create self link and open timestamp
                toAdd.SelfLink = $"{Request.Path}";
                toAdd.DateTimeOpened = DateTime.UtcNow;

                // add new issue to the repository
                var data = await _issueRepository.AddBug(toAdd, ct);

                // map model to response dto
                var result = Mapper.Map<IssueDto>(toAdd);

                // return created response
                return Created($"/api/bugs/{data}", new SuccessResult { Results = new[] { result }, Status = "Successful" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] IssueUpdateDto item, CancellationToken ct)
        {
            try
            {
                // return non content response if item is null
                if (item == null)
                {
                    return NoContent();
                }

                // retrieve the issue that is going to be updated
                var original = await _issueRepository.GetBugSingle(id, ct);

                // return not found response if existing item cannot be found
                if (original == null)
                {
                    return NotFound();
                }

                var originalStatus = original.Status;

                // map the update dto to model
                Issue toUpdate = Mapper.Map(item, original);

                // add closed datetime is issue transitions from open to closed
                if (originalStatus == IssueStatus.Open && item.Status == IssueStatus.Closed)
                {
                    toUpdate.DateTimeClosed = DateTime.UtcNow;
                }

                // remove closed datetime is issue transitions from open to closed
                if (originalStatus == IssueStatus.Closed && item.Status == IssueStatus.Open)
                {
                    toUpdate.DateTimeClosed = null;
                }

                // update the issue
                await _issueRepository.UpdateBug(toUpdate, ct);

                // map to dto
                var result = Mapper.Map<IssueDto>(toUpdate);

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
                // retrieve the issue that is going to be deleted
                var data = await _issueRepository.GetBugSingle(id, ct);

                // delete the issue
                await _issueRepository.DeleteBug(id, ct);

                // map to response dto
                var result = Mapper.Map<IssueDto>(data);

                // return successful response
                return Ok(new SuccessResult { Results = new[] { result }, Status = "Successful" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody]JsonPatchDocument<Issue> bugPatch, CancellationToken ct)
        {
            try
            {
                // retrieve the issue that is going to be updated
                var data = await _issueRepository.GetBugSingle(id, ct);

                // apply the json patch
                bugPatch.ApplyTo(data);

                // update the issue in the repository
                await _issueRepository.UpdateBug(data, ct);

                // map model to repsonse dto
                var result = Mapper.Map<IssueDto>(data);

                // return succesfull result
                return Ok(new SuccessResult { Results = new[] { result }, Status = "Successful" });
            }
            catch
            {
                // TODO catch errors
                return BadRequest();
            }
        }
    }
}
