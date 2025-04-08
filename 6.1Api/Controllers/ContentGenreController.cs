using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project6._1Api.Entities;
using project6._1Api.Model;
using System.Data.SqlClient;
using System.Xml.Serialization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
    public async Task<IActionResult> GetByContent(int contentId)
    {
        try
        {
            var genres = await _context.Content_Genre
                .Where(cg => cg.content_id == contentId)
                .Join(_context.Genre,
                    cg => cg.genre_id,
                    g => g.genre_id,
                    (cg, g) => new
                    {
                        g.genre_id,
                        g.genre_name
                    })
                .ToListAsync();

            if (genres.Any())
            {
                return Ok(new { message = "Genres retrieved successfully", data = genres });
            }

            return NotFound("No genres found for the specified content ID");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving genres");
        }
    }

    // POST: api/ContentGenre
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UpdateContentGenresModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _context.Content.AnyAsync(c => c.Content_id == model.ContentId))
            return BadRequest("Invalid content ID");

        // Validate all genre IDs exist
        var invalidGenres = model.GenreIds
            .Except(await _context.Genre.Select(g => g.genre_id).ToListAsync())
            .ToList();

        if (invalidGenres.Any())
            return BadRequest($"Invalid genre IDs: {string.Join(", ", invalidGenres)}");

        try
        {
            // Get current genres for the content (if any exist)
            var currentGenres = await _context.Content_Genre
                .Where(cg => cg.content_id == model.ContentId)
                .ToListAsync();

            // Check for existing assignments to avoid duplicates
            var existingGenreIds = currentGenres.Select(cg => cg.genre_id).ToList();
            var genresToAdd = model.GenreIds
                .Except(existingGenreIds)
                .Select(genreId => new Content_Genre
                {
                    content_id = model.ContentId,
                    genre_id = genreId
                })
                .ToList();

            await _context.Content_Genre.AddRangeAsync(genresToAdd);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetByContent),
                new { contentId = model.ContentId },
                new
                {
                    ContentId = model.ContentId,
                    GenreIds = model.GenreIds,
                    message = "Genres successfully assigned to content"
                });
        }
        catch (Exception)
        {
            return StatusCode(500, "Error assigning genres to content");
        }
    }

    // PUT: api/ContentGenre/{contentId}
    [HttpPut("{contentId}")]
    public async Task<IActionResult> UpdateContentGenres(int contentId, [FromBody] UpdateContentGenresModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _context.Content.AnyAsync(c => c.Content_id == contentId))
            return NotFound("Content not found");

        // Validate all genre IDs exist
        var invalidGenres = model.GenreIds
            .Except(await _context.Genre.Select(g => g.genre_id).ToListAsync())
            .ToList();

        if (invalidGenres.Any())
            return BadRequest($"Invalid genre IDs: {string.Join(", ", invalidGenres)}");

        try
        {
            // Get current genres for the content
            var currentGenres = await _context.Content_Genre
                .Where(cg => cg.content_id == contentId)
                .ToListAsync();

            // Determine genres to add and remove
            var genresToRemove = currentGenres
                .Where(cg => !model.GenreIds.Contains(cg.genre_id))
                .ToList();

            var existingGenreIds = currentGenres.Select(cg => cg.genre_id).ToList();
            var genresToAdd = model.GenreIds
                .Except(existingGenreIds)
                .Select(genreId => new Content_Genre
                {
                    content_id = contentId,
                    genre_id = genreId
                })
                .ToList();

            // Perform updates
            _context.Content_Genre.RemoveRange(genresToRemove);
            await _context.Content_Genre.AddRangeAsync(genresToAdd);
            await _context.SaveChangesAsync();

            return NoContent(); // Successfully updated
        }
        catch (Exception)
        {
            return StatusCode(500, "Error updating content genres");
        }
    }

    // DELETE: api/ContentGenre/{contentId}
    [HttpDelete("{contentId}")]
    public async Task<IActionResult> Delete(int contentId)
    {
        var contentGenres = await _context.Content_Genre
            .Where(cg => cg.content_id == contentId)
            .ToListAsync();

        if (!contentGenres.Any())
            return NotFound("No genres found for the specified content ID");

        try
        {
            _context.Content_Genre.RemoveRange(contentGenres);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Genres successfully removed from content" });
        }
        catch (Exception)
        {
            return StatusCode(500, "Error removing genres from content");
        }
    }
}

public class UpdateContentGenresModel
{
    [XmlElement(ElementName = "ContentId")]
    public int ContentId { get; set; }
    [XmlArray(ElementName = "GenreIds")]
    [XmlArrayItem(ElementName = "GenreId")]
    public List<int> GenreIds { get; set; } = new List<int>();
}