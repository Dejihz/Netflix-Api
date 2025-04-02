using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project6._1Api.Entities;
using project6._1Api.Model;
using System.Data.SqlClient;

namespace project6._1Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public class WatchHistoryController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public WatchHistoryController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST: api/WatchHistory
        [HttpPost("")]
        public IActionResult Create([FromBody] Model.WatchHistory historyModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newRecord = new Entities.WatchHistories
            {
                Profile_id = historyModel.profile_id,
                Content_id = historyModel.content_id,
                Watch_duration = historyModel.watch_duration,
                Completed = historyModel.completed,
                Watch_date = DateTime.UtcNow // Use server time instead of client-provided time
            };

            _dbContext.Watch_History.Add(newRecord);
            try
            {
                _dbContext.SaveChanges();
                return CreatedAtAction(
                    nameof(GetByProfile),
                    new { profileId = newRecord.Profile_id },
                    new
                    {
                        history_id = newRecord.History_id,
                        profile_id = newRecord.Profile_id,
                        content_id = newRecord.Content_id,
                        watch_date = newRecord.Watch_date,
                        watch_duration = newRecord.Watch_duration,
                        completed = newRecord.Completed
                    });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
            {
                return BadRequest("Invalid profile or content ID.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error saving history.");
            }
        }

        // GET: api/WatchHistory/{profileId}
        [HttpGet("{profileId}")]
        public IActionResult GetByProfile(int profileId)
        {
            var history = _dbContext.Watch_History
                .Where(h => h.Profile_id == profileId)
                .OrderByDescending(h => h.Watch_date)
                .Select(h => new Model.WatchHistory
                {
                    history_id = h.History_id,
                    profile_id = h.Profile_id,
                    content_id = h.Content_id,
                    watch_date = h.Watch_date,
                    watch_duration = h.Watch_duration,
                    completed = h.Completed
                })
                .ToList();

            return history.Any() ? Ok(history) : NotFound();
        }

        // PUT: api/WatchHistory/{historyId}
        [HttpPut("{historyId}")]
        public IActionResult Update(int historyId, [FromBody] Model.WatchHistory historyModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingRecord = _dbContext.Watch_History.Find(historyId);
            if (existingRecord == null) return NotFound();

            // Only allow updating these fields
            existingRecord.Watch_duration = historyModel.watch_duration;
            existingRecord.Completed = historyModel.completed;
            existingRecord.Watch_date = DateTime.UtcNow; // Update timestamp on modification

            try
            {
                _dbContext.SaveChanges();
                return Ok(new
                {
                    history_id = existingRecord.History_id,
                    profile_id = existingRecord.Profile_id,
                    content_id = existingRecord.Content_id,
                    watch_date = existingRecord.Watch_date,
                    watch_duration = existingRecord.Watch_duration,
                    completed = existingRecord.Completed
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Error updating watch history.");
            }
        }

        // DELETE: api/WatchHistory/{historyId}
        [HttpDelete("{historyId}")]
        public IActionResult Delete(int historyId)
        {
            var record = _dbContext.Watch_History.Find(historyId);
            if (record == null) return NotFound();

            _dbContext.Watch_History.Remove(record);

            try
            {
                _dbContext.SaveChanges();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error deleting watch history.");
            }
        }
    }
}