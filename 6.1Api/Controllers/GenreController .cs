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
public class GenreController : ControllerBase
{
    private readonly databaseContext _dbContext;

    public GenreController(databaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: api/Genre
    [HttpGet]
    public IActionResult GetAll()
    {
        var genres = _dbContext.Genre
            .Select(g => new Genres
            {
                genre_id = g.genre_id,
                genre_name = g.genre_name
            })
            .ToList();

        return genres.Any() ? Ok(genres) : NotFound();
    }

    // GET: api/Genre/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var genre = _dbContext.Genre
            .Where(g => g.genre_id == id)
            .Select(g => new Genres
            {
                genre_id = g.genre_id,
                genre_name = g.genre_name
            })
            .FirstOrDefault();

        return genre != null ? Ok(genre) : NotFound();
    }

    // POST: api/Genre
    [HttpPost]
    public IActionResult Create([FromBody] Genre genreModel)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var newGenre = new project6._1Api.Entities.Genres
        {
            genre_name = genreModel.genre_name
        };

        _dbContext.Genre.Add(newGenre);
        try
        {
            _dbContext.SaveChanges();
            return StatusCode(201, new { message = "Genre created successfully.", genre = newGenre });
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
        {
            return Conflict($"Genre '{genreModel.genre_name}' already exists.");
        }
        catch (Exception)
        {
            return StatusCode(500, "Error creating genre.");
        }
    }

    // PUT: api/Genre/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Genre genreModel)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var existingGenre = _dbContext.Genre.FirstOrDefault(g => g.genre_id == id);
        if (existingGenre == null) return NotFound();

        existingGenre.genre_name = genreModel.genre_name;

        try
        {
            _dbContext.SaveChanges();
            return Ok(new { message = "Genre updated successfully.", genre = existingGenre });
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
        {
            return Conflict($"Genre '{genreModel.genre_name}' already exists.");
        }
        catch (Exception)
        {
            return StatusCode(500, "Error updating genre.");
        }
    }

    // DELETE: api/Genre/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var genre = _dbContext.Genre.FirstOrDefault(g => g.genre_id == id);
        if (genre == null) return NotFound();

        try
        {
            _dbContext.Genre.Remove(genre);
            _dbContext.SaveChanges();
            return Ok(new { message = "Genre deleted successfully." });
        }
        catch (Exception)
        {
            return StatusCode(500, "Error deleting genre.");
        }
    }
}