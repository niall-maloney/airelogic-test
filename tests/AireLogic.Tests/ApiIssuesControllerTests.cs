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
    public class ApiIssuesControllerTests
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private IssuesController _controller;
        private IIssueRepository _mockRepo;

        [OneTimeSetUp]
        public void Setup()
        {
            var issuesJson = File.ReadAllText(Path.Combine(".", "issues.json"));
            var issues = JsonConvert.DeserializeObject<Issue[]>(issuesJson);
            var mockRepo = new Mock<IIssueRepository>();
            mockRepo.Setup(m => m.GetBugAll(_cts.Token)).ReturnsAsync(issues);
            mockRepo.Setup(m => m.GetBugSingle(It.Is<int>(i => i > 0), _cts.Token)).ReturnsAsync(issues[0]);
            mockRepo.Setup(m => m.GetBugSingle(-1, _cts.Token)).ReturnsAsync((Issue)null);
            mockRepo.Setup(m => m.AddBug(It.IsAny<Issue>(), _cts.Token)).ReturnsAsync(3);

            AutoMapper.Mapper.Reset();
            AutoMapper.Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<Issue, IssueDto>().ReverseMap();
                mapper.CreateMap<Issue, IssueCreateDto>().ReverseMap();
                mapper.CreateMap<Issue, IssueUpdateDto>().ReverseMap();
            });

            _mockRepo = mockRepo.Object;
            
            _controller = new IssuesController(_mockRepo)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Test]
        public async Task IssuesControllerGet_WithoutParams_Should_ReturnAllIssues()
        {
            var expected = await _mockRepo.GetBugAll(_cts.Token);

            var result = await _controller.Get(_cts.Token);

            var objResult = (ObjectResult)result;
            var response = (SuccessResult)objResult.Value;

            var actual = response.Results;

            Assert.AreEqual(objResult.StatusCode, 200);
            Assert.IsTrue(response.Status == "Successful");
            Assert.AreEqual(expected.Length, actual.Length);
        }

        [Test]
        public async Task IssuesControllerGet_WithoutParams_Should_ReturnSingleIssueWithSameId()
        {
            var id = 1;
            var expected = await _mockRepo.GetBugSingle(id, _cts.Token);

            var result = await _controller.Get(id, _cts.Token);

            var objResult = (ObjectResult)result;
            var response = (SuccessResult)objResult.Value;

            var actual = (IssueDto)response.Results[0];

            Assert.AreEqual(objResult.StatusCode, 200);
            Assert.IsTrue(response.Status == "Successful");
            Assert.AreEqual(1, response.Results.Length);
            Assert.AreEqual(expected.Uuid, actual.Uuid);
        }

        [Test]
        public async Task IssuesControllerGet_WithUnknownParams_Should_ReturnErrorNotFound()
        {
            var id = -1;
            var result = await _controller.Get(id, _cts.Token);

            var objResult = (StatusCodeResult)result;
            Assert.AreEqual(404, objResult.StatusCode);
        }

        [Test]
        public async Task IssuesControllerPost_WithNewIssue_Should_AddIssue()
        {
            var issueJson = File.ReadAllText(Path.Combine(".", "issue.json"));
            var issue = JsonConvert.DeserializeObject<IssueCreateDto>(issueJson);

            var result = await _controller.Post(issue, _cts.Token);

            var objResult = (ObjectResult)result;
            var response = (SuccessResult)objResult.Value;

            Assert.AreEqual(objResult.StatusCode, 201);
            Assert.IsTrue(response.Status == "Successful");
            Assert.AreEqual(1, response.Results.Length);
        }
    }
}