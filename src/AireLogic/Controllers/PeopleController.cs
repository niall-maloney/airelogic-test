using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using AireLogic.Models;
using AireLogic.Repositories;
using AireLogic.ViewModels;
using AutoMapper;

namespace AireLogic.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IPersonRepository _personRepository;

        public PeopleController(IPersonRepository storage)
        {
            _personRepository = storage;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var people = await _personRepository.GetPersonAll(ct);
            ViewData["People"] = Mapper.Map<PersonViewModel[]>(people);

            return View();
        }
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var person = await _personRepository.GetPersonSingle(id, ct);
            ViewData["Person"] = Mapper.Map<PersonViewModel>(person);

            return View();
        }
    }
}
