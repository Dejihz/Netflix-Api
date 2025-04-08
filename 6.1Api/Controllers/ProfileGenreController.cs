using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project6._1Api.Entities;
using project6._1Api.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[Produces("application/json", "application/xml")]
[Consumes("application/json", "application/xml")]
public class ProfileGenrePreferenceController : ControllerBase
{
    private readonly databaseContext _context;

    public ProfileGenrePreferenceController(databaseContext context)
    {
        _context = context;
    }

    // GET: api/ProfileGenrePreference/profile/{profileId}
    [HttpGet("{profileId}")]
    public async Task<IActionResult> GetByProfile(int profileId)
    {
        try
        {
            var genres = await _context.Profile_Genre
                .Where(pg => pg.Profile_id == profileId)
                .Join(_context.Genre,
                    pg => pg.Genre_id,
                    g => g.genre_id,
                    (pg, g) => new
                    {
                        g.genre_id,
                        g.genre_name
                    })
                .ToListAsync();

            if (genres.Any())
            {
                return Ok(new { message = "Genre preferences retrieved successfully", data = genres });
            }

            return NotFound("No genre preferences found for the specified profile ID");
        }
        catch (Exception ex)
        {
            // Log the exception message for further analysis
            // For example: _logger.LogError(ex, "Error occurred while retrieving genre preferences");
            return StatusCode(500, $"An error occurred while retrieving genre preferences: {ex.Message}");
        }
    }

    // POST: api/ProfileGenrePreference
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UpdateProfileGenresModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _context.Profile.AnyAsync(p => p.Profile_id == model.ProfileId))
            return BadRequest("Invalid profile ID");

        // Validate all genre IDs exist
        var invalidGenres = model.GenreIds
            .Except(await _context.Genre.Select(g => g.genre_id).ToListAsync())
            .ToList();

        if (invalidGenres.Any())
            return BadRequest($"Invalid genre IDs: {string.Join(", ", invalidGenres)}");

        try
        {
            // Get current preferences for the profile
            var currentPreferences = await _context.Profile_Genre
                .Where(pg => pg.Profile_id == model.ProfileId)
                .ToListAsync();

            // Check for existing assignments to avoid duplicates
            var existingGenreIds = currentPreferences.Select(pg => pg.Genre_id).ToList();
            var preferencesToAdd = model.GenreIds
                .Except(existingGenreIds)
                .Select(genreId => new Profile_Genre
                {
                    Profile_id = model.ProfileId,
                    Genre_id = genreId
                })
                .ToList();

            await _context.Profile_Genre.AddRangeAsync(preferencesToAdd);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetByProfile),
                new { profileId = model.ProfileId },
                new
                {
                    ProfileId = model.ProfileId,
                    GenreIds = model.GenreIds,
                    message = "Genre preferences successfully assigned to profile"
                });
        }
        catch (Exception ex)
        {
            // Log the exception message for further analysis
            // For example: _logger.LogError(ex, "Error occurred while assigning genre preferences to profile");
            return StatusCode(500, $"Error assigning genre preferences to profile: {ex.Message}");
        }
    }

    // PUT: api/ProfileGenrePreference/{profileId}
    [HttpPut("{profileId}")]
    public async Task<IActionResult> UpdateProfileGenrePreferences(int profileId, [FromBody] UpdateProfileGenresModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _context.Profile.AnyAsync(p => p.Profile_id == profileId))
            return NotFound("Profile not found");

        // Validate all genre IDs exist
        var invalidGenres = model.GenreIds
            .Except(await _context.Genre.Select(g => g.genre_id).ToListAsync())
            .ToList();

        if (invalidGenres.Any())
            return BadRequest($"Invalid genre IDs: {string.Join(", ", invalidGenres)}");

        try
        {
            // Get current preferences for the profile
            var currentPreferences = await _context.Profile_Genre
                .Where(pg => pg.Profile_id == profileId)
                .ToListAsync();

            // Determine preferences to add and remove
            var preferencesToRemove = currentPreferences
                .Where(pg => !model.GenreIds.Contains(pg.Genre_id))
                .ToList();

            var existingGenreIds = currentPreferences.Select(pg => pg.Genre_id).ToList();
            var preferencesToAdd = model.GenreIds
                .Except(existingGenreIds)
                .Select(genreId => new Profile_Genre
                {
                    Profile_id = profileId,
                    Genre_id = genreId
                })
                .ToList();

            // Perform updates
            _context.Profile_Genre.RemoveRange(preferencesToRemove);
            await _context.Profile_Genre.AddRangeAsync(preferencesToAdd);
            await _context.SaveChangesAsync();

            return NoContent(); // Successfully updated
        }
        catch (Exception ex)
        {
            // Log the exception message for further analysis
            // For example: _logger.LogError(ex, "Error occurred while updating profile genre preferences");
            return StatusCode(500, $"Error updating profile genre preferences: {ex.Message}");
        }
    }

    // DELETE: api/ContentGenre/{contentId}
    [HttpDelete("{profile_id}")]
    public async Task<IActionResult> Delete(int profile_id)
    {
        var profileGenres = await _context.Profile_Genre
            .Where(cg => cg.Profile_id == profile_id)
            .ToListAsync();

        if (!profileGenres.Any())
            return NotFound("No profilea found for the specified content ID");

        try
        {
            _context.Profile_Genre.RemoveRange(profileGenres);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Genres successfully removed from content" });
        }
        catch (Exception)
        {
            return StatusCode(500, "Error removing genres from content");
        }
    }
}


public class UpdateProfileGenresModel
{
    [XmlElement(ElementName = "ProfileId")]
    public int ProfileId { get; set; }

    [XmlArray(ElementName = "GenreIds")]
    [XmlArrayItem(ElementName = "GenreId")]
    public List<int> GenreIds { get; set; } = new List<int>();
}

