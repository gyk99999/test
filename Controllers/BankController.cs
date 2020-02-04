using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWBA.Data;
using NWBA.Models;
using NWBA.Utilities;
using NWBA.Attributes;
using System.Linq;
using SimpleHashing;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NWBA.Controllers
{
    [AuthorizeCustomer]
    public class BankController : Controller
    {
        private readonly NwbaContext _context;

        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public BankController(NwbaContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            // Lazy loading.
            // The Customer.Accounts property will be lazy loaded upon demand.
            var customer = await _context.Customers.FindAsync(CustomerID);

            // OR
            // Eager loading.
            //var customer = await _context.Customers.Include(x => x.Accounts).
            //    FirstOrDefaultAsync(x => x.CustomerID == _customerID);

            return View(customer);
        }
        [HttpPost]
        public async Task<IActionResult> Atm(char transationType, int fromAccount, int toAccount, decimal amount, string comment)
        {
            var customer = await _context.Customers.FindAsync(CustomerID);

            if (amount <= 0)
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;

                return View(customer);
            }
            switch (transationType)
            {
                case 'D':
                    
                    //Business object
                    DepositTransaction dt = new DepositTransaction(fromAccount, 0, amount, comment);
                    string message = dt.ExecuteAsync(_context).Result;
                    if (message != "true")
                    {
                        ModelState.AddModelError("Failed", message);
                        return View(customer);
                    }
                    break;
                case 'W':
                    //Business object
                    WithdrawTransaction wt = new WithdrawTransaction(fromAccount, 0, amount, comment);
                    string wtMessage = wt.ExecuteAsync(_context).Result;
                    if (wtMessage != "true")
                    {
                        ModelState.AddModelError("Failed", wtMessage);
                        return View(customer);
                    }
                    break;
                case 'T':
                    if(toAccount==0)
                    {
                        ModelState.AddModelError("Failed", "Please chose to account.");
                        ViewBag.Amount = amount;
                        ViewBag.Comment = comment;
                        ViewBag.fromAccount = fromAccount;
                        ViewBag.toAccount = toAccount;

                        return View(customer);
                    }
                    //Business object
                    TransferTransaction tt = new TransferTransaction(fromAccount, toAccount, amount,comment);
                    string ttMessage = tt.ExecuteAsync(_context).Result;
                    if (ttMessage != "true")
                    {
                        ModelState.AddModelError("Failed", ttMessage);
                        return View(customer);
                    }
                    break;
            }
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Profile()
        {
            // Lazy loading.
            // The Customer.Accounts property will be lazy loaded upon demand.
            var customer = await _context.Customers.FindAsync(CustomerID);
            var login = _context.Logins.FirstOrDefault(x => x.CustomerID == CustomerID);

            return View(Tuple.Create(customer, login));
        }
        public async Task<IActionResult> Atm()
        {
            // Lazy loading.
            // The Customer.Accounts property will be lazy loaded upon demand.
            var customer = await _context.Customers.FindAsync(CustomerID);
            // 321fdsafsdafdsaf
            return View(customer);
        }
        // rewqrewqrewqrwerq
        public async Task<IActionResult> Statement()
        {
            // Lazy loading.
            // The Customer.Accounts property will be lazy loaded upon demand.
            var customer = await _context.Customers.FindAsync(CustomerID);
            // 11111
            return View(Tuple.Create(customer, new List<Transaction>()));
        }
        [HttpPost]
        public async Task<IActionResult> Statement(int accountNumber,int pageIndex)
        {
            if(pageIndex<1)
            {
                pageIndex = 1;
                ViewData["PreviousPageIndex"] = 1;
            }
            ViewData["AccountNumber"] = accountNumber;
            int totalPage = _context.Transactions.Where(x => x.AccountNumber == accountNumber).Count()/4+1;
            int startRow = 0;
            if (pageIndex>1&& pageIndex <= totalPage)
            {
                startRow = (pageIndex - 1) * 4;
                ViewData["PreviousPageIndex"] = pageIndex - 1;
            }
            else
            {
                ViewData["PreviousPageIndex"] = 1;
            }
            if(pageIndex< totalPage)
            {
                startRow = 0;
                ViewData["NextPageIndex"] = pageIndex+1;
            }
            else
            {
                ViewData["NextPageIndex"] = pageIndex;
            }
            
            List<Transaction> list = _context.Transactions.Where(x => x.AccountNumber == accountNumber).OrderByDescending(c => c.TransactionTimeUtc).Skip(startRow).Take(4).ToList();
            var customer = await _context.Customers.FindAsync(CustomerID);
            return View(Tuple.Create(customer, list));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(string Name, string Address, string City, string PostCode)
        {
            Regex regex = new Regex(@"^[ A-Za-z]*$");
            
            var customer = await _context.Customers.FindAsync(CustomerID);
            var login = _context.Logins.FirstOrDefault(x => x.CustomerID == CustomerID);
            if (!regex.IsMatch(Name))
            {
                ModelState.AddModelError("UpdateFailed", "Customer name should be string.");
                return View("~/Views/Bank/Profile.cshtml", Tuple.Create(customer, login));
            }
            customer.Name = Name;
            customer.Address = Address;
            customer.City = City;
            customer.PostCode = PostCode;
            await _context.SaveChangesAsync();
            ModelState.AddModelError("UpdateSuccess", "Update success.");
            return View("~/Views/Bank/Profile.cshtml", Tuple.Create(customer, login));
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string LoginID,string OldPassword,string newPassword,string confirmPassword)
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            var login = await _context.Logins.FindAsync(LoginID);
            if (login == null || !PBKDF2.Verify(login.PasswordHash, OldPassword))
            {
                ModelState.AddModelError("PasswordFailed", "Password incorrect, please try again.");
                return View("~/Views/Bank/Profile.cshtml", Tuple.Create(customer, login));

            }
            else if(newPassword!=confirmPassword)
            {
                ModelState.AddModelError("PasswordFailed", "New password is different of confirm password.");
                return View("~/Views/Bank/Profile.cshtml", Tuple.Create(customer, login));
            }

            login.PasswordHash = PBKDF2.Hash(newPassword);
            await _context.SaveChangesAsync();
            ModelState.AddModelError("PasswordSuccess", "Change password success.");
            return View("~/Views/Bank/Profile.cshtml", Tuple.Create(customer, login));
        }
    }

}
