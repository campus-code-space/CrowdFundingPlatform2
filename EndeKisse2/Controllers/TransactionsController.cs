using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EndeKisse2.Data;
using EndeKissie2.Models;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace EndeKisse2.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        
        public TransactionsController(ApplicationDbContext context, IConfiguration configuration, IEmailSender emailSender )
        {
            _context = context;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transaction.Include(t => t.Project).Include(t => t.Sender);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.Project)
                .Include(t => t.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Id");
            ViewData["SenderId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,CalcShare,TransactionDate,Amount,SenderId,ProjectId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                string? firstName = _context.ApplicationUser.Where(i => i.Id == transaction.SenderId).Select(fn => fn.FirstName).FirstOrDefaultAsync().ToString();
                string? lastName = _context.ApplicationUser.Where(i => i.Id == transaction.SenderId).Select(fn => fn.LastName).FirstOrDefaultAsync().ToString();
                string? email = _context.ApplicationUser.Where(i => i.Id == transaction.SenderId).Select(fn => fn.Email).FirstOrDefaultAsync().ToString();
                string? public_key = _configuration.GetValue<string>("Chapa:PUBLICKEY");

                _context.Add(transaction);
                bool pay = await CreateChapaPaymentAsync(public_key, transaction.Amount, "ETB", email, firstName, lastName, "title", "decription");

                if (pay)
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _context.Remove(transaction);
                }
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Id", transaction.ProjectId);
            ViewData["SenderId"] = new SelectList(_context.ApplicationUser, "Id", "Id", transaction.SenderId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Id", transaction.ProjectId);
            ViewData["SenderId"] = new SelectList(_context.ApplicationUser, "Id", "Id", transaction.SenderId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,CalcShare,TransactionDate,Amount,SenderId,ProjectId")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Id", transaction.ProjectId);
            ViewData["SenderId"] = new SelectList(_context.ApplicationUser, "Id", "Id", transaction.SenderId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.Project)
                .Include(t => t.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction != null)
            {
                _context.Transaction.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.Id == id);
        }

        public async Task<bool> CreateChapaPaymentAsync(string? publicKey, double amount, string currency, string? email,
    string? firstName, string? lastName, string title, string description)
        {
            using var client = new HttpClient();

            // Chapa API URL
            string chapaUrl = "https://api.chapa.co/v1/hosted/pay";

            // Prepare the data
            var paymentData = new
            {
                public_key = publicKey,
                tx_ref = Guid.NewGuid().ToString(),
                amount = amount,
                currency = "ETB",
                email = email,
                first_name = firstName,
                last_name = lastName,
                title = "title",
                description = "description",
                callback_url = "https://127.0.0.1:7233/",
                logo = "https://chapa.link/asset/images/chapa_swirl.svg", // Example logo
                meta = new { title = "test" }
            };

            // Serialize the data
            string jsonData = JsonSerializer.Serialize(paymentData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Send POST request
            try
            {
                var response = await client.PostAsync(chapaUrl, content);

                // Check if the response was successful
                if (!response.IsSuccessStatusCode)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error creating payment: {errorResponse}");
                    return false;
                }

                // Read and process the response
                //string responseContent = await response.Content.ReadAsStringAsync();
                //var result = JsonSerializer.Deserialize<ChapaResponse>(responseContent);

                //// Log or process the checkout URL (optional)
                //Console.WriteLine($"Checkout URL: {result.data.checkout_url}");

                // Return success
                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Exception occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<IActionResult> Refund(int projectId)
        {
            bool refundResult = await RefundAllDonorsAsync(projectId);

            if (refundResult)
            {
                return Ok("Refunds processed successfully for all donors of the project.");
            }
            else
            {
                return BadRequest("Failed to process refunds for the project donors.");
            }
        }


        public async Task<bool> RefundAllDonorsAsync(int projectId)
        {
            try
            {
                // Retrieve all transactions for the specific project
                //&& t.Status == "Completed"
                var transactions = await _context.Transaction
                    .Where(t => t.ProjectId == projectId)
                    .ToListAsync();

                if (transactions == null || !transactions.Any())
                {
                    Console.WriteLine("No transactions found for the specified project.");
                    return false;
                }

                // Iterate through each transaction and process refunds
                foreach (var transaction in transactions)
                {
                    // Get the sender's details
                    var sender = await _context.ApplicationUser.FirstOrDefaultAsync(u => u.Id == transaction.SenderId);
                    if (sender == null)
                    {
                        Console.WriteLine($"Sender not found for transaction ID {transaction.Id}.");
                        continue; // Skip to the next transaction
                    }

                    // Process the refund
                    bool refundSuccess = await ProcessRefundToAccount(sender.Id, sender.AccountNum.ToString(), transaction.Amount);
                    if (!refundSuccess)
                    {
                        Console.WriteLine($"Failed to process refund for transaction ID {transaction.Id}.");
                        continue; // Skip to the next transaction
                    }

                    // Update the transaction status
                    transaction.Status = "Refunded";
                    //transaction.RefundDate = DateTime.UtcNow;

                    // Send email notification
                    await SendRefundNotificationEmail(sender.Email, sender.FirstName, sender.LastName, transaction.Amount);
                }

                // Save changes to the database after processing all refunds
                await _context.SaveChangesAsync();

                Console.WriteLine("Refunds processed for all donors of the project.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing refunds: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ProcessRefundToAccount(string id, string accountNumber, double amount)
        {
            //Console.WriteLine($"Refunding {amount:C} to account {accountNumber}...");
            string? firstName = _context.ApplicationUser.Where(i => i.Id == id).Select(fn => fn.FirstName).FirstOrDefaultAsync().ToString();
            string? lastName = _context.ApplicationUser.Where(i => i.Id == id).Select(fn => fn.LastName).FirstOrDefaultAsync().ToString();
            string? email = _context.ApplicationUser.Where(i => i.Id == id).Select(fn => fn.Email).FirstOrDefaultAsync().ToString();
            string? public_key = _configuration.GetValue<string>("Chapa:PUBLICKEY");

            
            bool pay = await CreateChapaPaymentAsync(public_key, amount, "ETB", email, firstName, lastName, "title", "decription");

            //await Task.Delay(1000); // Simulate async processing
            return pay; // Return true if refund is successful
        }



        private async Task SendRefundNotificationEmail(string email, string fname, string lname ,double amount)
        {
            string subject = "Refund Processed";
            string message = $"Dear {fname} {lname},\n\nYour refund of {amount:C} has been processed successfully.\n\nThank you.";

            await _emailSender.SendEmailAsync(email, subject,message);

            Console.WriteLine($"Sending email to {email}:\nSubject: {subject}\nMessage: {message}");
            await Task.CompletedTask; // Simulate async email sending
        }
        
    }
}
