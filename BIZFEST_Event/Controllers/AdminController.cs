using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Interfaces;
using BIZFEST_Event.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BIZFEST_Event.Controllers
{
    public class AdminController : Controller
    {      
        private readonly IEventRepository _IEventRepository;
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db , IEventRepository repository)
        {          
            _IEventRepository = repository;
            _db = db;
        }
        public IActionResult Index()
        {
            var response = _IEventRepository.GetAllEvents();
            return View(response);
        }

        public IActionResult Create()
        {
            UserEvent Event = new UserEvent();
            return PartialView("_EventCreatePartial", Event);
        }
        public IActionResult EditUser(int id)
        {
            UserEvent objEvent = new UserEvent();
            objEvent =_db.UserEvent.Where(x => x.Id == id).FirstOrDefault();
            return PartialView("_EventCreatePartial", objEvent);
        }

        [HttpPost]
        public  IActionResult AddEvent(UserEvent Event)
        {

            _IEventRepository.CreateEvent(Event);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult EditEvent(UserEvent Event)
        {

            _IEventRepository.CreateEvent(Event);
            return RedirectToAction("Index");
        }
        [HttpDelete]       
        public async Task<IActionResult> DeleteCustomer(int Id)
        {
          await  _IEventRepository.SoftDeleteEvent(Id);
            return RedirectToAction("Index");
        }
    }
}
