using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWBA.Data;
using NWBA.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NWBA.Controllers
{
    
    public class BillPayController : Controller
    {
        private readonly NwbaContext _context;

        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public BillPayController(NwbaContext context) => _context = context;
        // GET: /<controller>/
        public async Task<IActionResult> index()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            List<Payee> list = _context.Payees.ToList();

            return View(Tuple.Create(customer, list));
        }
        //create billpay
        public async Task<IActionResult> CreateBillPay(int accountNumber,int payeeID,decimal amount,string date,char period)
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            List<Payee> list = _context.Payees.ToList();
            BillPay billPay = new BillPay();
            billPay.AccountNumber = accountNumber;
            billPay.PayeeID = payeeID;
            billPay.Amount = amount;
            billPay.Period = period;
            billPay.ScheduleDate = Convert.ToDateTime(date);
            _context.BillPays.Add(billPay);
            await _context.SaveChangesAsync();
            return await BillPayRecords();
        }
        //show billPay list
        public async Task<IActionResult> BillPayRecords()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            List<int> list = new List<int>();
            foreach(var account in customer.Accounts)
            {
                list.Add(account.AccountNumber);
            }
            List<BillPay> billPays = _context.BillPays.Where(b => list.Contains(b.AccountNumber)).ToList();
            return View("~/Views/BillPay/BillPayRecords.cshtml",  billPays);
        }
        //show edit page
        public async Task<IActionResult> EditBillPay(int id)
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            List<Payee> list = _context.Payees.ToList();
            var billPay= await _context.BillPays.FindAsync(id);
            return View(Tuple.Create(customer, list,billPay));
        }
        //update billPay
        public async Task<IActionResult> UpdateBillPay(int billPayID,int accountNumber, int payeeID, decimal amount, string date, char period)
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            List<Payee> list = _context.Payees.ToList();
            var billPay = await _context.BillPays.FindAsync(billPayID); ;
            billPay.AccountNumber = accountNumber;
            billPay.PayeeID = payeeID;
            billPay.Amount = amount;
            billPay.Period = period;
            billPay.ScheduleDate = Convert.ToDateTime(date);
            await _context.SaveChangesAsync();
            return await BillPayRecords();
        }
    }
}
