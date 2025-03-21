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
    public class TransactionController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public TransactionController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Transaction
        [HttpGet("")]
        public IActionResult GetAll()
        {
            var transactions = _dbContext.Transaction
                .Select(t => new Model.Transaction
                {
                    id = t.Id,
                    user_id = t.User_id,
                    account = t.Account,
                    closingAvailableBalance = t.ClosingAvailableBalance,
                    closingBalance = t.ClosingBalance,
                    description = t.Description,
                    customDescription = t.CustomDescription,
                    forwardAvailableBalance = t.ForwardAvailableBalance,
                    openingBalance = t.OpeningBalance,
                    relatedMessage = t.RelatedMessage,
                    sequenceNumber = t.SequenceNumber,
                    statementNumber = t.StatementNumber,
                    transactionReference = t.TransactionReference,
                    transactionDate = t.TransactionDate,
                    category = t.Category,
                    debitCredit = t.DebitCredit,
                    date_of_transaction = t.Date_of_transaction,
                })
                .ToList();

            if (transactions.Any())
            {
                return Ok(transactions);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/Transaction/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            Entities.Transactions entityTransaction = _dbContext.Transaction.FirstOrDefault(t => t.Id == id);

            if (entityTransaction != null)
            {
                Model.Transaction modelTransaction = new Model.Transaction
                {
                    id = entityTransaction.Id,
                    user_id = entityTransaction.User_id,
                    account = entityTransaction.Account,
                    closingAvailableBalance = entityTransaction.ClosingAvailableBalance,
                    closingBalance = entityTransaction.ClosingBalance,
                    description = entityTransaction.Description,
                    customDescription = entityTransaction.CustomDescription,
                    forwardAvailableBalance = entityTransaction.ForwardAvailableBalance,
                    openingBalance = entityTransaction.OpeningBalance,
                    relatedMessage = entityTransaction.RelatedMessage,
                    sequenceNumber = entityTransaction.SequenceNumber,
                    statementNumber = entityTransaction.StatementNumber,
                    transactionReference = entityTransaction.TransactionReference,
                    transactionDate = entityTransaction.TransactionDate,
                    category = entityTransaction.Category,
                    debitCredit = entityTransaction.DebitCredit,
                    date_of_transaction = entityTransaction.Date_of_transaction

                };

                return Ok(modelTransaction);
            }
            else
            {
                return NotFound();
            }

        }

        // POST: api/Transaction
        [HttpPost("")]
        public IActionResult Create([FromBody] Model.Transaction transactionModel)
        {
            if (ModelState.IsValid)
            {
                Entities.Transactions newTransaction = new Entities.Transactions()
                {
                    Id = transactionModel.id,
                    User_id = null,
                    Account = transactionModel.account,
                    ClosingAvailableBalance = transactionModel.closingAvailableBalance,
                    ClosingBalance = transactionModel.closingBalance,
                    Description = transactionModel.description,
                    CustomDescription = transactionModel.customDescription,
                    ForwardAvailableBalance = transactionModel.forwardAvailableBalance,
                    OpeningBalance = transactionModel.openingBalance,
                    RelatedMessage = transactionModel.relatedMessage,
                    SequenceNumber = transactionModel.sequenceNumber,
                    StatementNumber = transactionModel.statementNumber,
                    TransactionReference = transactionModel.transactionReference,
                    TransactionDate = transactionModel.transactionDate,
                    Category = transactionModel.category,
                    DebitCredit = transactionModel.debitCredit,
                    Date_of_transaction = DateTime.Now.ToString()

            };

                _dbContext.Transaction.Add(newTransaction); 
                try
                {
                    _dbContext.SaveChanges();
                    return StatusCode(201, "Transaction created successfully.");
                }
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
                {
                    return StatusCode(406, $"ID '{transactionModel.id}' already exists.");
                }
                catch (DbUpdateException ex)
                {
                    // Handle other database update exceptions
                    return StatusCode(500, "Error creating transaction. Please try again later.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        // PUT: api/Transaction/{id}
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Model.Transaction transactionModel)
        {
            if (ModelState.IsValid)
            {
                Entities.Transactions existingTransaction = _dbContext.Transaction.FirstOrDefault(t => t.Id == id);

                if (existingTransaction != null)
                {
                    existingTransaction.User_id = transactionModel.user_id;
                    existingTransaction.Account = transactionModel.account;
                    existingTransaction.ClosingAvailableBalance = transactionModel.closingAvailableBalance;
                    existingTransaction.ClosingBalance = transactionModel.closingBalance;
                    existingTransaction.Description = transactionModel.description;
                    existingTransaction.CustomDescription = transactionModel.customDescription;
                    existingTransaction.ForwardAvailableBalance = transactionModel.forwardAvailableBalance;
                    existingTransaction.OpeningBalance = transactionModel.openingBalance;
                    existingTransaction.RelatedMessage = transactionModel.relatedMessage;
                    existingTransaction.SequenceNumber = transactionModel.sequenceNumber;
                    existingTransaction.StatementNumber = transactionModel.statementNumber;
                    existingTransaction.TransactionReference = transactionModel.transactionReference;
                    existingTransaction.TransactionDate = transactionModel.transactionDate;
                    existingTransaction.Category = transactionModel.category;
                    existingTransaction.DebitCredit = transactionModel.debitCredit;
                    existingTransaction.Date_of_transaction = DateTime.Now.ToString();


                    _dbContext.SaveChanges();

                    return Ok("Transaction updated successfully.");
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

        // DELETE: api/Transaction/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            Entities.Transactions transaction = _dbContext.Transaction.FirstOrDefault(t => t.Id == id);

            if (transaction != null)
            {
                _dbContext.Transaction.Remove(transaction);
                _dbContext.SaveChanges();
                return StatusCode(201, "Transaction deleted successfully.");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
