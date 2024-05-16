
using Doorang.Business.Services.Abstacts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DoorangApp.Controllers
{
    public class HomeController : Controller
    {
      
        private readonly IExplorerServices _explorerServices;

        public HomeController(IExplorerServices explorerServices)
        {
            _explorerServices = explorerServices;
        }

        public IActionResult Index()
        {
            var item=_explorerServices.GetAllExplorer();
            return View(item);
        }

      
    }
}