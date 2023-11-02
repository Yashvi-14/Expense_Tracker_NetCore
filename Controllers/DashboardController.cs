using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Expense_Tracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Data for the last 7 days
            DateTime StartDate = DateTime.Today.AddDays(-9);
            DateTime EndDate = DateTime.Today;

            // Fetch transactions from the database
            List<Transactions> SelectedTransactions = await _context.Transactions
                .Include(x => x.Categories)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate)
                .ToListAsync();

            // Calculate total income
            int TotalIncome = SelectedTransactions
                 .Where(i => i.Categories.Type == "Income")
                 .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");

            // Calculate total expense
            int TotalExpense = SelectedTransactions
                .Where(i => i.Categories.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");

            // Calculate balance (total income - expense)
            int Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);



            //Donghnut chart for Expense by category
            ViewBag.DoughnutChartData = SelectedTransactions
                .Where(i => i.Categories.Type == "Expense")
                .GroupBy(j => j.Categories.CategoryId)
                .Select(k => new
                {
                    categoryTitleWithIcon = k.First().Categories.Icon + " " + k.First().Categories.Title,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                })
                .OrderByDescending(l => l.amount)
                .ToList();

            //Spline Chart - Income vs Expense

            //Income Data
            List<SplineChartData> IncomeSummary = SelectedTransactions
                .Where(i => i.Categories.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();

            //Expense Data
            List<SplineChartData> ExpenseSummary = SelectedTransactions
                .Where(i => i.Categories.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();

            //Combine Income & Expense Data
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from day in Last7Days
                                      join income in IncomeSummary on day equals income.day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,
                                      };
            //Recent Transactions
            ViewBag.RecentTransactions = await _context.Transactions
                .Include(i => i.Categories)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();


            return View();




        }

        public class SplineChartData
        {
            public string day;
            public int income;
            public int expense;

        }


    }
}

