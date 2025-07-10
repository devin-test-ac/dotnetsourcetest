using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/test")]
    public class TestController : ControllerBase
    {
        public TestController()
        {
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTest(
            int id, bool includePointsOfInterest = false)
        {
            var test = new TestServices();
            test.Test();
            test.SecondTest();

            return Ok();
        }




    }
}
