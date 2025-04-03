using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project6._1Api.Entities;
using project6._1Api.Model;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
[Produces("application/json", "application/xml")]
[Consumes("application/json", "application/xml")]
public class PreferenceController : ControllerBase
{
    private readonly databaseContext _context;

    public PreferenceController(databaseContext context)
    {
        _context = context;
    }

    // GET: api/Preference
    [HttpGet]
    public IActionResult GetAll()
    {
        var preferences = _context.Preferences
            .Select(p => new project6._1Api.Model.Preferences
            {
                preferences_id = p.Preferences_id,
                profile_id = p.Profile_id,
                min_age = p.Min_age,
                content_restrictions = p.Content_restrictions
            })
            .ToList();

        return preferences.Any() ? Ok(preferences) : NotFound();
    }

    // GET: api/Preference/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var preference = _context.Preferences
            .Where(p => p.Preferences_id == id)
            .Select(p => new project6._1Api.Model.Preferences
            {
                preferences_id = p.Preferences_id,
                profile_id = p.Profile_id,
                min_age = p.Min_age,
                content_restrictions = p.Content_restrictions
            })
            .FirstOrDefault();

        return preference != null ? Ok(preference) : NotFound();
    }


    // POST: api/Preference
    [HttpPost]
    public IActionResult Create([FromBody] project6._1Api.Model.Preferences model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if profile exists
        var profileExists = _context.Profile.Any(p => p.Profile_id == model.profile_id);
        if (!profileExists)
            return BadRequest("Invalid profile ID");

        var entity = new project6._1Api.Entities.Preferences
        {
            Profile_id = model.profile_id,
            Min_age = model.min_age,
            Content_restrictions = model.content_restrictions
        };

        _context.Preferences.Add(entity);

        try
        {
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = entity.Preferences_id }, new project6._1Api.Model.Preferences
            {
                preferences_id = entity.Preferences_id,
                profile_id = entity.Profile_id,
                min_age = entity.Min_age,
                content_restrictions = entity.Content_restrictions
            });
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
        {
            return BadRequest("Invalid profile ID");
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Error creating preference");
        }
    }

    // PUT: api/Preference/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] project6._1Api.Model.Preferences model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = _context.Preferences.Find(id);
        if (entity == null)
            return NotFound();

        // Don't allow changing profile_id as it's a key relationship
        entity.Min_age = model.min_age;
        entity.Content_restrictions = model.content_restrictions;

        try
        {
            _context.SaveChanges();
            return Ok(new
            {
                message = "Preference updated successfully",
                preference = new project6._1Api.Model.Preferences
                {
                    preferences_id = entity.Preferences_id,
                    profile_id = entity.Profile_id,
                    min_age = entity.Min_age,
                    content_restrictions = entity.Content_restrictions
                }
            });
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Error updating preference");
        }
    }

    // DELETE: api/Preference/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var entity = _context.Preferences.Find(id);
        if (entity == null)
            return NotFound();

        try
        {
            _context.Preferences.Remove(entity);
            _context.SaveChanges();
            return Ok(new { message = $"Preference with ID {id} deleted successfully" });
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Error deleting preference");
        }
    }
}