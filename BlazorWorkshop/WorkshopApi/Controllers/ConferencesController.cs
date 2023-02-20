using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost]
        public ActionResult<ConferenceDetails> AddConference([FromBody] ConferenceDetails conference)
        {
            var conf = conference.ToEntity();
            
            _dbContext.Conferences.Add(conf);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetConferenceById), new {id = conf.ID}, conf);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateConference(Guid id, [FromBody] ConferenceDetails conference)
        {
            if (id != conference.ID)
            {
                return BadRequest();
            }

            var conf = conference.ToEntity();
            _dbContext.Entry(conf).State = EntityState.Modified;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Conferences.Any(c => c.ID == id))
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
