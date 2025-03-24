using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project6._1Api.Entities;
using project6._1Api.Model;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace project6._1Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public class SubscriptionController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public SubscriptionController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Subscription
        [HttpGet("")]
        public IActionResult GetAll()
        {
            var subscriptions = _dbContext.Subscription
                .Select(s => new Model.Subscription
                {
                    Subscription_id = s.Subscription_id,
                    Plan_type = s.Plan_type,
                    Price = s.Price,
                    Validity_period = s.Validity_period
                })
                .ToList();

            if (subscriptions.Any())
            {
                return Ok(subscriptions);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/Subscription/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var subscription = _dbContext.Subscription.FirstOrDefault(s => s.Subscription_id == id);

            if (subscription != null)
            {
                var modelSubscription = new Model.Subscription
                {
                    Subscription_id = subscription.Subscription_id,
                    Plan_type = subscription.Plan_type,
                    Price = subscription.Price,
                    Validity_period = subscription.Validity_period
                };

                return Ok(modelSubscription);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/Subscription
        [HttpPost("")]
        public IActionResult Create([FromBody] Model.Subscription subscriptionModel)
        {
            if (ModelState.IsValid)
            {
                var newSubscription = new Entities.Subscriptions()
                {
                    Plan_type = subscriptionModel.Plan_type,
                    Price = subscriptionModel.Price,
                    Validity_period = subscriptionModel.Validity_period
                };

                _dbContext.Subscription.Add(newSubscription);
                try
                {
                    _dbContext.SaveChanges();
                    return StatusCode(201, "Subscription created successfully.");
                }
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
                {
                    return StatusCode(406, $"Subscription ID '{subscriptionModel.Subscription_id}' already exists.");
                }
                catch (DbUpdateException ex)
                {
                    return StatusCode(500, "Error creating subscription. Please try again later.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT: api/Subscription/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Model.Subscription subscriptionModel)
        {
            if (ModelState.IsValid)
            {
                var existingSubscription = _dbContext.Subscription.FirstOrDefault(s => s.Subscription_id == id);

                if (existingSubscription != null)
                {
                    existingSubscription.Plan_type = subscriptionModel.Plan_type;
                    existingSubscription.Price = subscriptionModel.Price;
                    existingSubscription.Validity_period = subscriptionModel.Validity_period;

                    _dbContext.SaveChanges();
                    return Ok("Subscription updated successfully.");
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Subscription/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var subscription = _dbContext.Subscription.FirstOrDefault(s => s.Subscription_id == id);

            if (subscription != null)
            {
                _dbContext.Subscription.Remove(subscription);
                _dbContext.SaveChanges();
                return StatusCode(201, "Subscription deleted successfully.");
            }
            else
            {
                return NotFound();
            }
        }
    }
}