using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project6._1Api.Entities;
using project6._1Api.Model;
using System.Data.SqlClient;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json", "application/xml")]
[Consumes("application/json", "application/xml")]
public class ContentGenreController : ControllerBase
{
    private readonly databaseContext _context;

    public ContentGenreController(databaseContext context)
    {
        _context = context;
    }

    // GET: api/ContentGenre/content/{contentId}
    [HttpGet("{contentId}")]
    public IActionResult GetByContent(int contentId)
    {
        var genres = _context.Content_Genre
            .Where(cg => cg.content_id == contentId)
            .Join(_context.Genre,
                cg => cg.genre_id,
                g => g.genre_id,
                (cg, g) => new
                {
                    g.genre_id,
                    g.genre_name
                })
            .ToList();

        return Ok(genres);
    }

    // POST: api/ContentGenre
    [HttpPost]
    public IActionResult Create([FromBody] ContentGenreModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_context.Content.Any(c => c.Content_id == model.content_id))
            return BadRequest("Invalid content ID");

        if (!_context.Genre.Any(g => g.genre_id == model.genre_id))
            return BadRequest("Invalid genre ID");

        var contentGenre = new Content_Genre
        {
            content_id = model.content_id,
            genre_id = model.genre_id
        };

        _context.Content_Genre.Add(contentGenre);

        try
        {
            _context.SaveChanges();
            return CreatedAtAction(
                nameof(GetByContent),
                new { contentId = model.content_id },
                new
                {
                    model.content_id,
                    model.genre_id
                });
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
        {
            return Conflict("This genre is already assigned to the content");
        }
        catch (Exception)
        {
            return StatusCode(500, "Error assigning genre to content");
        }
    }

    // DELETE: api/ContentGenre/{contentId}/{genreId}
    [HttpDelete("{contentId}/{genreId}")]
    public IActionResult Delete(int contentId, int genreId)
    {
        var contentGenre = _context.Content_Genre
            .FirstOrDefault(cg => cg.content_id == contentId && cg.genre_id == genreId);

        if (contentGenre == null)
            return NotFound();

        _context.Content_Genre.Remove(contentGenre);

        try
        {
            _context.SaveChanges();
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Error removing genre from content");
        }
    }
}