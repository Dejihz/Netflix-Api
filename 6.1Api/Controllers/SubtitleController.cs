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
    public class SubtitleController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public SubtitleController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Subtitle/{contentId}
        [HttpGet("{contentId}")]
        public IActionResult GetByContent(int contentId)
        {
            var subtitles = _dbContext.Subtitles
                .Where(s => s.Content_id == contentId)
                .Select(s => new Model.Subtitles
                {
                    subtitle_id = s.Subtitle_id,
                    content_id = s.Content_id,
                    language = s.Language
                })
                .ToList();

            return subtitles.Any() ? Ok(subtitles) : NotFound();
        }

        // POST: api/Subtitle
        [HttpPost("")]
        public IActionResult Create([FromBody] Model.Subtitles subtitleModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newSubtitle = new Entities.Subtitles
            {
                Content_id = subtitleModel.content_id,
                Language = subtitleModel.language
            };

            _dbContext.Subtitles.Add(newSubtitle);
            try
            {
                _dbContext.SaveChanges();
                return CreatedAtAction(
                    nameof(GetByContent),
                    new { contentId = newSubtitle.Content_id },
                    new
                    {
                        subtitle_id = newSubtitle.Subtitle_id,
                        content_id = newSubtitle.Content_id,
                        language = newSubtitle.Language
                    });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
            {
                return BadRequest("Invalid content ID.");
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
            {
                return Conflict("Subtitle for this language already exists.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error adding subtitle.");
            }
        }

        // PUT: api/Subtitle/{subtitleId}
        [HttpPut("{subtitleId}")]
        public IActionResult Update(int subtitleId, [FromBody] Model.Subtitles subtitleModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingSubtitle = _dbContext.Subtitles.Find(subtitleId);
            if (existingSubtitle == null) return NotFound();

            existingSubtitle.Language = subtitleModel.language;

            try
            {
                _dbContext.SaveChanges();
                return Ok(new
                {
                    subtitle_id = existingSubtitle.Subtitle_id,
                    content_id = existingSubtitle.Content_id,
                    language = existingSubtitle.Language
                });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
            {
                return Conflict("Subtitle for this language already exists.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error updating subtitle.");
            }
        }

        // DELETE: api/Subtitle/{subtitleId}
        [HttpDelete("{subtitleId}")]
        public IActionResult Delete(int subtitleId)
        {
            var subtitle = _dbContext.Subtitles.Find(subtitleId);
            if (subtitle == null) return NotFound();

            _dbContext.Subtitles.Remove(subtitle);

            try
            {
                _dbContext.SaveChanges();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error deleting subtitle.");
            }
        }
    }
}