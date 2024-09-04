using Microsoft.AspNetCore.Mvc;

namespace EnviBad.API.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportRequestController : ControllerBase
    {
        private readonly ILogger<ReportRequestController> _logger;

        public ReportRequestController(ILogger<ReportRequestController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetReadyReports")]
        public IEnumerable<string> GetReadyList()
        {
            return Enumerable.Range(1, 5).Select(index => "Number is " + index)
            .ToArray();
        }
    }
}
