using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SecureCoding.Models;
using SecureCoding.ViewModels;

namespace SecureCoding.Controllers;
public class CustomersController : Controller
{
    private static List<Customer> _customers = new();

    private readonly IDataProtector _dataProtector;

    public CustomersController(IDataProtectionProvider dataProtectionProvider)
    {
        _dataProtector = dataProtectionProvider.CreateProtector("SecureCoding");
    }

    public IActionResult Index()
    {
        var viewModel = _customers.Select(c => new CustomerViewModel
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
        var customer = _customers.SingleOrDefault(c => c.Id == customerId);

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

        model.Id = _customers.Count + 1;

        _customers.Add(model);

        return RedirectToAction(nameof(Index));
    }
}