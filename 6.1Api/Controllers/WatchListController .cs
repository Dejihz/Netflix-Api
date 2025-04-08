using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project6._1Api.Entities;
using project6._1Api.Model;
using System.Data.SqlClient;


namespace project6._1Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public class WatchListController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public WatchListController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }


        // GET: api/WatchList/{profileId}
        [HttpGet("{profileId}")]
        public IActionResult GetByProfile(int profileId)
        {
            var watchlist = _dbContext.Watch_List
                .Where(w => w.Profile_id == profileId)
                .Select(w => new Model.WatchList
                {
                    watchlist_id = w.Watchlist_id,
                    profile_id = w.Profile_id,
                    content_id = w.Content_id,
                    added_date = w.Added_date
                })
                .ToList();

            return watchlist.Any() ? Ok(watchlist) : NotFound();
        }

        // POST: api/WatchList
        [HttpPost("")]
        public IActionResult Create([FromBody] Model.WatchList watchListModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newItem = new Entities.WatchLists
            {
                Profile_id = watchListModel.profile_id,
                Content_id = watchListModel.content_id,
                Added_date = watchListModel.added_date
            };
            

            _dbContext.Watch_List.Add(newItem);
            try
            {
                _dbContext.SaveChanges();
                return StatusCode(201, "Added to watchlist.");
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
            {
                return BadRequest("Invalid profile or content ID.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error updating watchlist.");
            }
        }
        // PUT: api/WatchList/{watchlistId}
        [HttpPut("{watchlistId}")]
        public IActionResult UpdateWatchlistItem(int watchlistId, [FromBody] Model.WatchList watchListModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingItem = _dbContext.Watch_List.FirstOrDefault(w => w.Watchlist_id == watchlistId);
            if (existingItem == null)
                return NotFound();

            // Only update these fields (don't allow changing the IDs)
            existingItem.Added_date = watchListModel.added_date;

            try
            {
                _dbContext.SaveChanges();
                return Ok(new
                {
                    watchlist_id = existingItem.Watchlist_id,
                    profile_id = existingItem.Profile_id,
                    content_id = existingItem.Content_id,
                    added_date = existingItem.Added_date
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Error updating watchlist item.");
            }
        }


        // DELETE: api/WatchList/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _dbContext.Watch_List.FirstOrDefault(w => w.Watchlist_id == id);
            if (item == null) return NotFound();

            _dbContext.Watch_List.Remove(item);
            _dbContext.SaveChanges();
            return Ok("Removed from watchlist.");
        }
    }
}
