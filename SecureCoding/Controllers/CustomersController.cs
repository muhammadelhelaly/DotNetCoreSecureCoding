using Microsoft.AspNetCore.Mvc;
using SecureCoding.Models;

namespace SecureCoding.Controllers;
public class CustomersController : Controller
{
    private static List<Customer> _customers = new();

    public IActionResult Index()
    {
        return View(_customers);
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

        _customers.Add(model);

        return RedirectToAction(nameof(Index));
    }
}