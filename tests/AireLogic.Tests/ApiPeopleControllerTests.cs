using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AireLogic.Controllers.Api;
using AireLogic.Dto;
using AireLogic.Models;
using AireLogic.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AireLogic.Tests
{
    public class ApiPeopleControllerTests
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private PeopleController _controller;
        private IPersonRepository _mockRepo;

        [OneTimeSetUp]
        public void Setup()
        {
            var peopleJson = File.ReadAllText(Path.Combine(".", "people.json"));
            var people = JsonConvert.DeserializeObject<Person[]>(peopleJson);
            var mockRepo = new Mock<IPersonRepository>();
            mockRepo.Setup(m => m.GetPersonAll(_cts.Token)).ReturnsAsync(people);
            mockRepo.Setup(m => m.GetPersonSingle(It.Is<int>(i => i > 0), _cts.Token)).ReturnsAsync(people[0]);
            mockRepo.Setup(m => m.GetPersonSingle(-1, _cts.Token)).ReturnsAsync((Person)null);
            mockRepo.Setup(m => m.AddPerson(It.IsAny<Person>(), _cts.Token)).ReturnsAsync(3);

            AutoMapper.Mapper.Reset();
            AutoMapper.Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<Person, PersonDto>().ReverseMap();
                mapper.CreateMap<Person, PersonCreateDto>().ReverseMap();
                mapper.CreateMap<Person, PersonUpdateDto>().ReverseMap();
            });

            _mockRepo = mockRepo.Object;
            
            _controller = new PeopleController(_mockRepo)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Test]
        public async Task PeopleControllerGet_WithoutParams_Should_ReturnAllPeople()
        {
            var expected = await _mockRepo.GetPersonAll(_cts.Token);

            var result = await _controller.Get(_cts.Token);

            var objResult = (ObjectResult)result;
            var response = (SuccessResult)objResult.Value;

            var actual = response.Results;

            Assert.AreEqual(objResult.StatusCode, 200);
            Assert.IsTrue(response.Status == "Successful");
            Assert.AreEqual(expected.Length, actual.Length);
        }

        [Test]
        public async Task PeopleControllerGet_WithoutParams_Should_ReturnSinglePersonWithSameId()
        {
            var id = 1;
            var expected = await _mockRepo.GetPersonSingle(id, _cts.Token);

            var result = await _controller.Get(id, _cts.Token);

            var objResult = (ObjectResult)result;
            var response = (SuccessResult)objResult.Value;

            var actual = (PersonDto)response.Results[0];

            Assert.AreEqual(objResult.StatusCode, 200);
            Assert.IsTrue(response.Status == "Successful");
            Assert.AreEqual(1, response.Results.Length);
            Assert.AreEqual(expected.Uuid, actual.Uuid);
        }

        [Test]
        public async Task PeopleControllerGet_WithUnknownParams_Should_ReturnErrorNotFound()
        {
            var id = -1;
            var result = await _controller.Get(id, _cts.Token);

            var objResult = (StatusCodeResult)result;
            Assert.AreEqual(404, objResult.StatusCode);
        }

        [Test]
        public async Task PeopleControllerPost_WithNewPerson_Should_AddPerson()
        {
            var PersonJson = File.ReadAllText(Path.Combine(".", "Person.json"));
            var Person = JsonConvert.DeserializeObject<PersonCreateDto>(PersonJson);

            var result = await _controller.Post(Person, _cts.Token);

            var objResult = (ObjectResult)result;
            var response = (SuccessResult)objResult.Value;

            Assert.AreEqual(objResult.StatusCode, 201);
            Assert.IsTrue(response.Status == "Successful");
            Assert.AreEqual(1, response.Results.Length);
        }
    }
}