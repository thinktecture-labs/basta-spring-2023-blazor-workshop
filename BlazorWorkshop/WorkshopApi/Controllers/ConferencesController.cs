using Microsoft.AspNetCore.Mvc;
using WorkshopApi.Database;
using WorkshopApi.Extensions;
using WorkshopConfTool.Shared.Models;

namespace WorkshopApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ConferencesController : ControllerBase
    {
        private readonly ConferencesDbContext _dbContext;

        public ConferencesController(ConferencesDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet]
        public ActionResult<IEnumerable<ConferenceOverview>> GetConferenceOverview()
        {
            var data = _dbContext.Conferences
                .OrderBy(c => c.DateFrom)
                .Select(c => c.ToOverview())
                .ToArray();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public ActionResult<ConferenceDetails> GetConferenceById(Guid id)
        {
            var data = _dbContext.Conferences
                .Single(c => c.ID == id)
                .ToDetail();

            return Ok(data);
        }

    }
}
