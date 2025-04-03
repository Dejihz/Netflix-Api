using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project6._1Api.Entities;
using project6._1Api.Model;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data.Entity;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
[Produces("application/json", "application/xml")]
[Consumes("application/json", "application/xml")]
public class ContentController : ControllerBase
{
    private readonly databaseContext _context;

    public ContentController(databaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var contentItems = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.AsNoTracking(_context.Content)
            .Select(c => new
            {
                Content = c,
                Film = _context.Film.FirstOrDefault(f => f.Content_id == c.Content_id),
                Series = _context.Series.FirstOrDefault(s => s.Content_id == c.Content_id)
            })
            .ToList()
            .Select(x => new
            {
                content_id = x.Content.Content_id,
                title = x.Content.Title,
                release_year = x.Content.Release_year,
                quality = x.Content.Quality,
                classification = x.Content.Classification,

                type = x.Film != null ? "film" :
                       x.Series != null ? "series" : "unknown",

                film_id = x.Film?.Film_id,
                series_id = x.Series?.Series_id,

                duration = x.Film?.Duration,

                number_of_seasons = x.Series?.Number_of_seasons,
                episodes = x.Series != null ?
                    _context.Episode
                        .Where(e => e.Series_id == x.Series.Series_id)
                        .OrderBy(e => e.Season_number)
                        .ThenBy(e => e.Episode_number)
                        .Select(e => new
                        {
                            e.Episode_id,
                            e.Season_number,
                            e.Episode_number,
                            e.Title,
                            e.Duration
                        })
                        .ToList()
                    : null
            })
            .ToList();

        return Ok(contentItems);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var content = _context.Content
            .Where(c => c.Content_id == id)
            .Select(c => new
            {
                Content = c,
                Film = _context.Film.FirstOrDefault(f => f.Content_id == c.Content_id),
                Series = _context.Series.FirstOrDefault(s => s.Content_id == c.Content_id)
            })
            .FirstOrDefault();

        if (content == null) return NotFound();

        return Ok(new
        {
            content_id = content.Content.Content_id,
            title = content.Content.Title,
            release_year = content.Content.Release_year,
            quality = content.Content.Quality,
            classification = content.Content.Classification,
            type = content.Film != null ? "film" : "series",
            film_id = content.Film?.Film_id,
            series_id = content.Series?.Series_id,
            duration = content.Film?.Duration,
            number_of_seasons = content.Series?.Number_of_seasons
        });
    }

    [HttpPost]
 
    public IActionResult Create([FromBody] ContentCreateModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        using var transaction = _context.Database.BeginTransaction();

        try
        {
            var content = new Contents
            {
                Title = model.title,
                Release_year = model.release_year,
                Quality = model.quality,
                Classification = model.classification
            };

            _context.Content.Add(content);
            _context.SaveChanges();

            if (model.type == "film" && model.duration.HasValue)
            {
                var film = new Films
                {
                    Content_id = content.Content_id,
                    Duration = model.duration.Value
                };
                _context.Film.Add(film);
            }
            else if (model.type == "series" && model.number_of_seasons.HasValue)
            {
                var series = new project6._1Api.Entities.Series
                {
                    Content_id = content.Content_id,
                    Number_of_seasons = model.number_of_seasons.Value
                };
                _context.Series.Add(series);
            }
            else
            {
                throw new ArgumentException("Invalid content type or missing required fields");
            }

            _context.SaveChanges();
            transaction.Commit();

            return CreatedAtAction(nameof(GetById), new { id = content.Content_id }, new
            {
                content_id = content.Content_id,
                title = content.Title,
                type = model.type
            });
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return StatusCode(500, $"Error creating content: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
 
    public IActionResult Update(int id, [FromBody] ContentUpdateModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        using var transaction = _context.Database.BeginTransaction();

        try
        {
            var content = _context.Content.Find(id);
            if (content == null) return NotFound();

            content.Title = model.title;
            content.Release_year = model.release_year;
            content.Quality = model.quality;
            content.Classification = model.classification;

            if (model.type == "film")
            {
                var film = _context.Film.FirstOrDefault(f => f.Content_id == id);
                if (film == null) return BadRequest("Content is not a film");
                film.Duration = model.duration.Value;
            }
            else if (model.type == "series")
            {
                var series = _context.Series.FirstOrDefault(s => s.Content_id == id);
                if (series == null) return BadRequest("Content is not a series");
                series.Number_of_seasons = model.number_of_seasons.Value;
            }

            _context.SaveChanges();
            transaction.Commit();

            return Ok("Content updated successfully");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return StatusCode(500, $"Error updating content: {ex.Message}");
        }
    }

    
    [HttpDelete("{id}")]
 
    public IActionResult Delete(int id)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            var content = _context.Content.Find(id);
            if (content == null) return NotFound();

            var film = _context.Film.FirstOrDefault(f => f.Content_id == id);
            var series = _context.Series.FirstOrDefault(s => s.Content_id == id);

            if (film != null) _context.Film.Remove(film);
            if (series != null) _context.Series.Remove(series);

            _context.Content.Remove(content);

            _context.SaveChanges();
            transaction.Commit();

            return Ok("Content deleted successfully");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
        {
            transaction.Rollback();
            return Conflict("Cannot delete content with dependent records");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return StatusCode(500, $"Error deleting content: {ex.Message}");
        }
    }

}