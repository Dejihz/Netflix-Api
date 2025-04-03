using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project6._1Api.Entities;
using project6._1Api.Model;
using System.Data.SqlClient;

[Route("api/[controller]")]
[Authorize]
[ApiController]
[Produces("application/json", "application/xml")]
[Consumes("application/json", "application/xml")]
public class EpisodeController : ControllerBase
{
    private readonly databaseContext _context;

    public EpisodeController(databaseContext context)
    {
        _context = context;
    }

    // GET: api/Episode/{episodeId}
    [HttpGet("{episodeId}")]
    public IActionResult GetById(int episodeId)
    {
        var episode = _context.Episode
            .Where(e => e.Episode_id == episodeId)
            .Select(e => new
            {
                e.Episode_id,
                e.Series_id,
                e.Season_number,
                e.Episode_number,
                e.Title,
                e.Duration,
                Content = _context.Series
                    .Where(s => s.Series_id == e.Series_id)
                    .Select(s => new
                    {
                        s.Content_id,
                        Content = _context.Content
                            .FirstOrDefault(c => c.Content_id == s.Content_id)
                    })
                    .FirstOrDefault()
            })
            .FirstOrDefault();

        if (episode == null)
            return NotFound();

        return Ok(episode);
    }

    // POST: api/Episode
    [HttpPost]
    public IActionResult Create([FromBody] Episode model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_context.Series.Any(s => s.Series_id == model.series_id))
            return BadRequest("Invalid series ID");

        var episode = new Episodes
        {
            Series_id = model.series_id,
            Season_number = model.season_number,
            Episode_number = model.episode_number,
            Title = model.title,
            Duration = model.duration
        };

        _context.Episode.Add(episode);

        try
        {
            _context.SaveChanges();
            return CreatedAtAction(
                nameof(GetById),
                new { episodeId = episode.Episode_id },
                new
                {
                    episode.Episode_id,
                    episode.Series_id,
                    episode.Season_number,
                    episode.Episode_number,
                    episode.Title,
                    episode.Duration
                });
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
        {
            return Conflict("Episode with this season and episode number already exists");
        }
        catch (Exception)
        {
            return StatusCode(500, "Error creating episode");
        }
    }

    // PUT: api/Episode/{episodeId}
    [HttpPut("{episodeId}")]
    public IActionResult Update(int episodeId, [FromBody] Episode model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var episode = _context.Episode.Find(episodeId);
        if (episode == null)
            return NotFound();

        episode.Title = model.title;
        episode.Duration = model.duration;
        episode.Season_number = model.season_number;
        episode.Episode_number = model.episode_number;

        try
        {
            _context.SaveChanges();
            return Ok(new
            {
                episode.Episode_id,
                episode.Series_id,
                episode.Season_number,
                episode.Episode_number,
                episode.Title,
                episode.Duration
            });
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
        {
            return Conflict("Episode with this season and episode number already exists");
        }
        catch (Exception)
        {
            return StatusCode(500, "Error updating episode");
        }
    }

    // DELETE: api/Episode/{episodeId}
    [HttpDelete("{episodeId}")]
    public IActionResult Delete(int episodeId)
    {
        var episode = _context.Episode.Find(episodeId);
        if (episode == null)
            return NotFound();

        _context.Episode.Remove(episode);

        try
        {
            _context.SaveChanges();
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Error deleting episode");
        }
    }
}