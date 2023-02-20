using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WorkshopApi.Database;
using WorkshopApi.Extensions;
using WorkshopApi.SignalR;
using WorkshopConfTool.Shared.Models;

namespace WorkshopApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class ConferencesController : ControllerBase
    {
        private readonly ConferencesDbContext _dbContext;
        private readonly IHubContext<ConferencesHub> _hub;

        public ConferencesController(ConferencesDbContext dbContext, IHubContext<ConferencesHub> hub)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
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

        [HttpPost]
        public ActionResult<ConferenceDetails> AddConference([FromBody] ConferenceDetails conference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var conf = conference.ToEntity();
            
            _dbContext.Conferences.Add(conf);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetConferenceById), new {id = conf.ID}, conf);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConference(Guid id, [FromBody] ConferenceDetails conference)
        {
            if (id != conference.ID)
            {
                return BadRequest();
            }

            var conf = conference.ToEntity();
            _dbContext.Entry(conf).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                await _hub.Clients.All.SendAsync("ConferenceUpdated", conf.ToDetail());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _dbContext.Conferences.AnyAsync(c => c.ID == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteConference(Guid id)
        {
            var conf = _dbContext.Conferences.Find(id);
            if (conf == null) return NotFound();

            _dbContext.Conferences.Remove(conf);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
