using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ITSHotelGuest.Models;

namespace ITSHotelGuest.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRoomGuest _roomGuest;

        public HomeController(IRoomGuest roomGuest)
        {
            _roomGuest = roomGuest;
        }

        //http://localhost:60707/Home?hash=19016710316321711325216717494932444262120981810413749&roomN=202

        public async Task<IActionResult> Index(string hash, string roomN)
        {
            //var roomGuest = new RoomGuestModel
            //{
            //    Token = hash,
            //    RoomNumber = Convert.ToInt32(roomN)
            //};

            //var exist = await _roomGuest.CheckRoomGuest(roomGuest);
            //if (exist)
            //{
            //    //get val from azure
            //    var deviceId = roomN;

                return RedirectToAction("Index", "Room", new { hash = hash, deviceId = roomN});
            //}
            //return RedirectToAction("Error");
        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
