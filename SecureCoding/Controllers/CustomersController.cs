using Microsoft.AspNetCore.Mvc;
using SecureCoding.Models;

namespace SecureCoding.Controllers;
public class CustomersController : Controller
{
    public IActionResult Index()
    {
        return View();
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

        //Save to database

        return RedirectToAction(nameof(Index));
    }
}