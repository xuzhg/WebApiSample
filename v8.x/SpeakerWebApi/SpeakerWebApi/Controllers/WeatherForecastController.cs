using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using SpeakerWebApi.Models;

namespace SpeakerWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISpeakerSubmissionsRepository _repository;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISpeakerSubmissionsRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [ODataAttributeRouting]
        [HttpPatch("/odata/Classes/{classId}/Students/{studentId}/SpeakerSubmissions/{speakerSubmissionId}")]
        public async Task<SpeakerSubmissionResource> UpdateSpeakerSubmission(
            int classId,
            int studentId,
            int speakerSubmissionId,
            Delta<SpeakerSubmissionResource> speakerSubmissionDelta)
        {
            SpeakerSubmissionResource updatedSpeakerSubmission = await _repository.UpdateSubmissionAsync(classId, studentId, speakerSubmissionId, speakerSubmissionDelta);
            return updatedSpeakerSubmission;
        }
    }
}
