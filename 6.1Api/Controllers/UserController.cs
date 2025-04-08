using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project6._1Api.Entities;
using project6._1Api.Model;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace project6._1Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public class UserController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public UserController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/User
        [HttpGet("")]
        public IActionResult GetAll()
        {
            var users = _dbContext.User
                .Select(u => new Model.User
                {
                    user_id = u.User_id,
                    email = u.Email,
                    password = u.Password,
                    account_status = u.Account_status,
                    subscription_id = u.Subscription_id,
                    role_id = u.Role_id,
                    referred_by = u.Referred_by,
                })
                .ToList();

            if (users.Any())
            {
                return Ok(users);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Entities.Users entityUser = _dbContext.User.FirstOrDefault(u => u.User_id == id);

            if (entityUser != null)
            {
                Model.User modelUser = new Model.User
                {
                    user_id = entityUser.User_id,
                    //username = entityUser.Username,
                    //password = entityUser.Password,
                    //level = entityUser.Level

                    email = entityUser.Email,
                    password = entityUser.Password,
                    account_status = entityUser.Account_status,
                    subscription_id = entityUser.Subscription_id,
                    role_id = entityUser.Role_id,
                    referred_by = entityUser.Referred_by
                };

                return Ok(modelUser);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/User
        [HttpPost("")]
        public IActionResult Create([FromBody] Model.User userModel)
        {
            if (ModelState.IsValid)
            {
                Entities.Users newUser = new Entities.Users()
                {
                    Email = userModel.email,
                    Password = HashString(userModel.password),
                    Account_status = userModel.account_status,
                    Subscription_id = userModel.subscription_id,
                    Role_id = userModel.role_id,
                    Referred_by = userModel.referred_by,
                };

                _dbContext.User.Add(newUser); 
                try
                {
                    _dbContext.SaveChanges();
                    return StatusCode(201, "User created successfully.");
                }
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
                {
                    return StatusCode(406, $"Email '{userModel.email}' already exists.");
                }
                catch (DbUpdateException ex)
                {
                    return StatusCode(500, "Error creating user. Please try again later.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Model.User userModel)
        {
            if (ModelState.IsValid)
            {
                Entities.Users existingUser = _dbContext.User.FirstOrDefault(u => u.User_id == id);

                if (existingUser != null)
                {
                    existingUser.Email = userModel.email;
                    existingUser.Password = HashString(userModel.password);
                    existingUser.Account_status = userModel.account_status;
                    existingUser.Subscription_id = userModel.subscription_id;
                    existingUser.Role_id = userModel.role_id;
                    existingUser.Referred_by = userModel.referred_by;


                    _dbContext.SaveChanges();

                    return Ok("User updated successfully.");
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

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //db.DeleteUser(id);
            
            Entities.Users user = _dbContext.User.FirstOrDefault(u => u.User_id == id);

            if (user != null)
            {
                _dbContext.User.Remove(user);
                _dbContext.SaveChanges();
                return StatusCode(201, "User deleted successfully.");
            }
            else
            {
                return NotFound();
            }
        }

        public static string HashString(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
