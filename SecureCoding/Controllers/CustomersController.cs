using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureCoding.Data;
using SecureCoding.Models;
using SecureCoding.ViewModels;

namespace SecureCoding.Controllers;
public class CustomersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IDataProtector _dataProtector;

    public CustomersController(IDataProtectionProvider dataProtectionProvider, ApplicationDbContext context)
    {
        _dataProtector = dataProtectionProvider.CreateProtector("SecureCoding");
        _context = context;
    }

    public IActionResult Index(string searchValue)
    {
        List<Customer> customers = [];

        if (!string.IsNullOrEmpty(searchValue))
        {
            //customers = _context.Customers
            //    .FromSqlRaw($"SELECT * FROM Customers WHERE LastName Like '%{searchValue}%'").ToList();

            customers = _context.Customers
              .FromSqlInterpolated($"SELECT * FROM Customers WHERE LastName LIKE {"%" + searchValue + "%"}").ToList();

            //customers = _context.Customers.Where(c => c.LastName.Contains(searchValue)).ToList();
        }
        else
        {
            customers = _context.Customers.FromSqlRaw("SELECT * FROM Customers").ToList();
        }
        
        var viewModel = customers.Select(c => new CustomerViewModel
        {
            Key = _dataProtector.Protect(c.Id.ToString()),
            FirstName = c.FirstName,
            LastName = c.LastName,
            Gender = c.Gender,
            Email = c.Email,
            Address = c.Address
        });

        return View(viewModel);
    }

    public IActionResult Details(string id)
    {
        var customerId = int.Parse(_dataProtector.Unprotect(id));
        var customer = _context.Customers.Find(customerId);

        return View(customer);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Customer model)
    {
        if(!ModelState.IsValid)
            return View(model);

        _context.Customers.Add(model);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}