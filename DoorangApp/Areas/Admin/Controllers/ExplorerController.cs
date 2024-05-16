using Doorang.Business.Exceptions;
using Doorang.Business.Services.Abstacts;
using Doorang.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Text;

namespace DoorangApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExplorerController : Controller
    {
        private readonly IExplorerServices _services;

        public ExplorerController(IExplorerServices services)
        {
            _services = services;
        }

        public IActionResult Index()
        {
            var item=_services.GetAllExplorer();
            return View(item);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Explorer explorer)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _services.AddExplorer(explorer);
            }
            catch (FileContentException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
             
            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            var item=_services.GetExplorer(x=>x.Id == id);
            if (item == null) throw new NullReferenceException();
            try
            {
                _services.RemoveExplorer(item.Id);
            }
            catch (FileNameNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return RedirectToAction("Index");
                
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("index");

        }
        public IActionResult Update(int id)
        {
            var item=_services.GetExplorer(x=>x.Id==id);    
            if (item == null) throw new NullReferenceException() ;
            return View(item);
        }
        [HttpPost]
        public IActionResult Update(Explorer explorer)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _services.UpdateExplorer(explorer.Id, explorer);
            }
            catch (FileContentException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();

            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("index");
        }

    }
}
