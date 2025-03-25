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
public class ReferralController : ControllerBase
{
    private readonly databaseContext _context;

    public ReferralController(databaseContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public IActionResult GetAll()
    {
        var referrals = _context.Referral
            .Select(r => new Referral
            {
                referral_id = r.Referral_id,
                referrer_user_id = r.Referrer_user_id,
                referred_user_id = r.Referred_user_id,
                discount_applied = r.Discount_applied
            })
            .ToList();

        return referrals.Any() ? Ok(referrals) : NotFound();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var entity = _context.Referral.FirstOrDefault(r => r.Referral_id == id);
        if (entity == null) return NotFound();

        return Ok(new Referral
        {
            referral_id = entity.Referral_id,
            referrer_user_id = entity.Referrer_user_id,
            referred_user_id = entity.Referred_user_id,
            discount_applied = entity.Discount_applied
        });
    }

    [HttpPost("")]
    public IActionResult Create([FromBody] Referral model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (model.referrer_user_id == model.referred_user_id)
        {
            return BadRequest("Referrer and referred user cannot be the same");
        }

        var entity = new Referrals
        {
            Referrer_user_id = model.referrer_user_id,
            Referred_user_id = model.referred_user_id,
            Discount_applied = model.discount_applied
        };

        _context.Referral.Add(entity);

        try
        {
            _context.SaveChanges();
            return StatusCode(201, "Referral created successfully.");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
        {
            return BadRequest("One or both user IDs are invalid");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
        {
            return Conflict("This referral relationship already exists");
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Error creating referral. Please try again later.");
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Referral model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (model.referrer_user_id == model.referred_user_id)
        {
            return BadRequest("Referrer and referred user cannot be the same");
        }

        var entity = _context.Referral.FirstOrDefault(r => r.Referral_id == id);
        if (entity == null) return NotFound();

        entity.Referrer_user_id = model.referrer_user_id;
        entity.Referred_user_id = model.referred_user_id;
        entity.Discount_applied = model.discount_applied;

        try
        {
            _context.SaveChanges();
            return Ok("Referral updated successfully.");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
        {
            return BadRequest("One or both user IDs are invalid");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
        {
            return Conflict("This referral relationship already exists");
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Error updating referral. Please try again later.");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var entity = _context.Referral.FirstOrDefault(r => r.Referral_id == id);
        if (entity == null) return NotFound();

        try
        {
            _context.Referral.Remove(entity);
            _context.SaveChanges();
            return Ok("Referral deleted successfully.");
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Error deleting referral. Please try again later.");
        }
    }
}