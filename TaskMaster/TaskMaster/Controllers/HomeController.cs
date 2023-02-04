using Microsoft.AspNetCore.Mvc;
using TaskMaster.Services;

namespace TaskMaster.Controllers;

public class HomeController : Controller
{
    public HomeController(Service service)
    {
        
    }
    public IActionResult Index()
    {
        return View();
    }
}