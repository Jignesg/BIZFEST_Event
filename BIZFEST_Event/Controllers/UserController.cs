using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Interfaces;
using BIZFEST_Event.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing.QrCode.Internal;
using QRCode = QRCoder.QRCode;

namespace BIZFEST_Event.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _HostEnvironment;
        public UserController(IUserRepository userRepository, ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _userRepository = userRepository;
            _db = db;
            _HostEnvironment = hostEnvironment;
        }
        public IActionResult UserView()
        {
            string EventId = HttpContext.Request.Query["EventId"];
            ViewBag.EventId = EventId;
            return View();
        }
        public IActionResult Registration()
        {            
            return View();
        }
        public IActionResult EventDetail()
        {
            return View();
        }
        //public IActionResult Create()
        //{
        //    UsersRegistration user = new UsersRegistration();
        //    return PartialView("_UserCreatePartial", user);
        //}
        //public IActionResult EditUser(int id)
        //{
        //    UsersRegistration user = new UsersRegistration();
        //    user =_db.UserRegistration.Where(x => x.Id == id).FirstOrDefault();

        //    return PartialView("_UserCreatePartial", user);
        //}

        [HttpPost]
        public IActionResult AddUser(UsersRegistration user)
        {                       
            _userRepository.CreateUser(user);          
            return RedirectToAction("Registration");
        }


        //[HttpDelete]
        //public async Task<IActionResult> DeleteUser(int Id)
        //{
        //    await _userRepository.SoftDeleteUser(Id);
        //    return RedirectToAction("UserView");
        //}
    }
}
