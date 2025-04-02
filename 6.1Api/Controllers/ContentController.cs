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

    [HttpPost("{contentId}/episodes")]
    public IActionResult AddEpisode(int contentId, [FromBody] Episode model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var series = _context.Series
            .FirstOrDefault(s => s.Content_id == contentId);

        if (series == null)
            return BadRequest("Specified content is not a series");

        if (series.Series_id != model.series_id)
            return BadRequest("Series ID mismatch");

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
            return CreatedAtAction(nameof(GetEpisode), new { contentId, episodeId = episode.Episode_id }, new
            {
                episode_id = episode.Episode_id,
                series_id = episode.Series_id,
                title = episode.Title
            });
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
        {
            return BadRequest("Invalid series ID");
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

    public IActionResult GetEpisode(int contentId, int episodeId)
    {
        var episode = _context.Episode
            .Where(e => e.Episode_id == episodeId)
            .Join(_context.Series,
                e => e.Series_id,
                s => s.Series_id,
                (e, s) => new { Episode = e, Series = s })
            .Where(x => x.Series.Content_id == contentId)
            .Select(x => new
            {
                episode_id = x.Episode.Episode_id,
                series_id = x.Episode.Series_id,
                content_id = contentId,
                season_number = x.Episode.Season_number,
                episode_number = x.Episode.Episode_number,
                title = x.Episode.Title,
                duration = x.Episode.Duration
            })
            .FirstOrDefault();

        if (episode == null)
            return NotFound();

        return Ok(episode);
    }

    [HttpGet("{contentId}/episodes")]
    public IActionResult GetEpisodes(int contentId)
    {
        var episodes = _context.Episode
            .Join(_context.Series,
                episode => episode.Series_id,
                series => series.Series_id,
                (episode, series) => new { Episode = episode, Series = series })
            .Where(x => x.Series.Content_id == contentId)
            .OrderBy(x => x.Episode.Season_number)
            .ThenBy(x => x.Episode.Episode_number)
            .Select(x => new
            {
                x.Episode.Episode_id,
                x.Episode.Series_id,
                content_id = contentId,
                x.Episode.Season_number,
                x.Episode.Episode_number,
                x.Episode.Title,
                x.Episode.Duration
            })
            .ToList();

        return Ok(episodes);
    }


    [HttpGet("{contentId}/Genres")]
    public IActionResult GetContentGenres(int contentId)
    {
        var genres = _context.Content_Genre
            .Where(cg => cg.content_id == contentId)
            .Select(cg => new Genre
            {
                genre_id = cg.Genre.genre_id,
                genre_name = cg.Genre.genre_name
            })
            .ToList();

        return Ok(genres);
    }

    [HttpPost("{contentId}/Genres/{genreId}")]
    public IActionResult AssignGenreToContent(int contentId, int genreId)
    {
        if (!_context.Content.Any(c => c.Content_id == contentId))
            return BadRequest("Invalid content_id");

        if (!_context.Genre.Any(g => g.genre_id == genreId))
            return BadRequest("Invalid genre_id");

        _context.Content_Genre.Add(new Content_Genre
        {
            content_id = contentId,
            genre_id = genreId
        });

        try
        {
            _context.SaveChanges();
            return CreatedAtAction(
                nameof(GetContentGenres),
                new { contentId = contentId },
                new { content_id = contentId, genre_id = genreId });
        }
        catch (DbUpdateException)
        {
            return Conflict("This genre is already assigned to the content");
        }
    }

    [HttpDelete("{contentId}/Genres/{genreId}")]
    public IActionResult RemoveGenreFromContent(int contentId, int genreId)
    {
        var assignment = _context.Content_Genre
            .FirstOrDefault(cg => cg.content_id == contentId && cg.genre_id == genreId);

        if (assignment == null)
            return NotFound();

        _context.Content_Genre.Remove(assignment);
        _context.SaveChanges();
        return NoContent();
    }

}