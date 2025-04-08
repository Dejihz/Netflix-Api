using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project6._1Api.Entities;
using project6._1Api.Model;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[Produces("application/json", "application/xml")]
[Consumes("application/json", "application/xml")]
public class ProfileController : ControllerBase
{
    private readonly databaseContext _context;

    public ProfileController(databaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var profiles = _context.Profile
            .Select(p => new Profile
            {
                profile_id = p.Profile_id,
                user_id = p.User_id,
                name = p.Name,
                profile_photo = p.Profile_photo,
                age = p.Age,
                language = p.Language
            })
            .ToList();

        return profiles.Any() ? Ok(profiles) : NotFound();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var entity = _context.Profile.Find(id);
        if (entity == null) return NotFound();

        return Ok(new Profile
        {
            profile_id = entity.Profile_id,
            user_id = entity.User_id,
            name = entity.Name,
            profile_photo = entity.Profile_photo,
            age = entity.Age,
            language = entity.Language
        });
    }

    [HttpPost]
    public IActionResult Create([FromBody] Profile model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Profiles
        {
            User_id = model.user_id,
            Name = model.name,
            Profile_photo = model.profile_photo,
            Age = model.age,
            Language = model.language
        };

        _context.Profile.Add(entity);

        try
        {
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = entity.Profile_id }, entity);
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
        {
            return BadRequest("Invalid user ID provided");
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Error creating profile");
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Profile model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = _context.Profile.Find(id);
        if (entity == null) return NotFound();

        entity.Name = model.name;
        entity.Profile_photo = model.profile_photo;
        entity.Age = model.age;
        entity.Language = model.language;

        try
        {
            _context.SaveChanges();
            return Ok("Profile updated successfully");
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Error updating profile");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var entity = _context.Profile.Find(id);
        if (entity == null) return NotFound();

        try
        {
            _context.Profile.Remove(entity);
            _context.SaveChanges();
            return Ok("Profile deleted successfully");
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Error deleting profile");
        }
    }

}