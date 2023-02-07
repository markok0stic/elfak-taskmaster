using Microsoft.AspNetCore.Mvc;

namespace TaskMaster.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {
        
    }
    public IActionResult Index()
    {
        return View();
    }
}