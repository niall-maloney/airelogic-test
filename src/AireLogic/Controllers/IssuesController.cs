using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AireLogic.Models;
using AireLogic.Repositories;
using System.Threading;
using AireLogic.ViewModels;
using AutoMapper;

namespace AireLogic.Controllers
{
    public class IssuesController : Controller
    {
        IIssueRepository _issueRepository;
        IPersonRepository _personRepository;

        public IssuesController(IIssueRepository issueRepositroy, IPersonRepository personRepository)
        {
            _issueRepository = issueRepositroy;
            _personRepository = personRepository;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var data = await _issueRepository.GetBugAll(ct);
            var issues = Mapper.Map<IssueViewModel[]>(data);

            for (int i = 0; i < issues.Length; i++)
            {
                var id = (int)issues[i].Assignee;
                var assignee = await _personRepository.GetPersonSingle(id, ct);

                issues[i].AssigneeName = $"{assignee.FirstName} {assignee.LastName}";
            }

            ViewData["Issues"] = issues;

            return View();
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var issues = await _issueRepository.GetBugSingle(id, ct);
            ViewData["Issue"] = Mapper.Map<IssueViewModel>(issues);

            var people = await _personRepository.GetPersonAll(ct);
            ViewData["People"] = Mapper.Map<PersonViewModel[]>(people);

            if (ViewData["Issue"] == null) return Error();

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
