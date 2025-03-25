using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project6._1Api.Entities;
using project6._1Api.Model;
using System.Data.SqlClient;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[Produces("application/json", "application/xml")]
[Consumes("application/json", "application/xml")]
public class RoleController : ControllerBase
{
    private readonly databaseContext _context;

    public RoleController(databaseContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public IActionResult GetAll()
    {
        var roles = _context.Role
            .Select(r => new Role
            {
                role_id = r.Role_id,
                role_name = r.Role_name,
                permissions = r.Permissions
            })
            .ToList();

        return roles.Any() ? Ok(roles) : NotFound();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var entity = _context.Role.FirstOrDefault(r => r.Role_id == id);
        if (entity == null) return NotFound();

        return Ok(new Role
        {
            role_id = entity.Role_id,
            role_name = entity.Role_name,
            permissions = entity.Permissions
        });
    }

    [HttpPost("")]
    public IActionResult Create([FromBody] Role model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Roles
        {
            Role_name = model.role_name,
            Permissions = model.permissions
        };

        _context.Role.Add(entity);

        try
        {
            _context.SaveChanges();
            return StatusCode(201, "Role created successfully.");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
        {
            return Conflict($"Role '{model.role_name}' already exists.");
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Error creating role. Please try again later.");
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Role model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = _context.Role.FirstOrDefault(r => r.Role_id == id);
        if (entity == null) return NotFound();

        entity.Role_name = model.role_name;
        entity.Permissions = model.permissions;

        try
        {
            _context.SaveChanges();
            return Ok("Role updated successfully.");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
        {
            return Conflict($"Role '{model.role_name}' already exists.");
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Error updating role. Please try again later.");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var entity = _context.Role.FirstOrDefault(r => r.Role_id == id);
        if (entity == null) return NotFound();

        try
        {
            _context.Role.Remove(entity);
            _context.SaveChanges();
            return Ok("Role deleted successfully.");
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Error deleting role. It may be in use by existing users.");
        }
    }
}