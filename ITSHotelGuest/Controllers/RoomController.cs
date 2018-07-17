using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ITSHotelGuest.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using ITSHotelGuest.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.WindowsAzure.Storage;

namespace ITSHotelGuest.Controllers
{
    public class RoomController : Controller
    {
        private readonly ILogger _logger;
        private readonly RegistryManager _registryManager;
        private readonly IConfiguration _configuration;
        private readonly IRoomGuest _roomGuest;


        public RoomController(IConfiguration configuration, ILogger<RoomController> logger, IRoomGuest roomGuest)
        {
            _configuration = configuration;
            _logger = logger;
            _registryManager = RegistryManager
             .CreateFromConnectionString(_configuration["IotHubConnectionString"]);
            _roomGuest = roomGuest;
        }
        //-------------------------------------------------------------------------------------------------------------
        public async Task<IActionResult> Index(string hash, string deviceId)
        {
            var roomGuest = new RoomGuestModel
            {
                Token = hash,
                RoomNumber = Convert.ToInt32(deviceId)
            };

            var exist = await _roomGuest.CheckRoomGuest(roomGuest);
            if (!exist)
            {
                return RedirectToAction("Error", "Home");
            }           
            var room = new RoomData();
            room.RoomNumber = deviceId;

            var twin = await _registryManager.GetTwinAsync(deviceId);
            if (twin.IsDesiredpropertyEmpty("PassCode12") && twin.IsDesiredpropertyEmpty("PassCode34"))
            {
                var passCode12 = twin.DesiredProperty("PassCode12");
                var passCode34 = twin.DesiredProperty("PassCode34");
                room.PassCode = passCode12 + passCode12;
            }
            if (twin.Properties.Reported.Contains("Light"))
            {
                room.Light = (bool)twin.Properties.Reported["Light"];
            }
            if (twin.Properties.Reported.Contains("AlarmSet"))
            {
                room.AlarmSet = (bool)twin.Properties.Reported["AlarmSet"];
            }
            if (twin.Properties.Reported.Contains("ConditionerTemp"))
            {
                room.ConditionerTemperature = (decimal)twin.Properties.Reported["ConditionerTemp"];
            }
            if (twin.Properties.Reported.Contains("Conditioner"))
            {
                room.ConditionerSet = (bool)twin.Properties.Reported["Conditioner"];
            }
            if (twin.Properties.Reported.Contains("AlarmH") && twin.Properties.Reported.Contains("AlarmM"))
            {
                var hour = (string)twin.Properties.Reported["AlarmH"];
                var minutes = (string)twin.Properties.Reported["AlarmM"];
                room.AlarmTime = Convert.ToDateTime(hour + " : " + minutes);
            }

            
            
            return View(room);
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> SetTemperature(string deviceId, [FromBody] SetTemperatureRequest request)
        {
            try
            {
                var twin = await _registryManager.GetTwinAsync(deviceId);
                if (twin == null)
                {
                    return NotFound();
                }
                twin.Properties.Desired["ConditionerTemp"] = Convert.ToInt32(request.Temperature.ToString().Substring(0, 2));
                await _registryManager.UpdateTwinAsync(deviceId, twin, twin.ETag);
                var result = new
                {
                    StatusCode = 200,
                    Message = "device updated"
                };
                return Ok(result);
            }
            catch (Exception exception)
            {
                var result = new
                {
                    StatusCode = 500,
                    Message = "Error during set temperature"
                };
                return Json(result);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> SetAlarm(string deviceId, [FromBody] SetAlarmRequest request)
        {
            try
            {
                var twin = await _registryManager.GetTwinAsync(deviceId);
                if (twin == null)
                {
                    return NotFound();
                }
                var AllAlarm = request.Alarm;
                var substrings = AllAlarm.Split(":");
                var result1 = substrings[0];             // separare i twin da spedire H o M
                var result2 = substrings[1];
                twin.Properties.Desired["AlarmTimeH"] = result1;
                twin.Properties.Desired["AlarmTimeM"] = result2;
                await _registryManager.UpdateTwinAsync(deviceId, twin, twin.ETag);
                var result = new
                {
                    StatusCode = 200,
                    Message = "device updated"
                };
                return Ok(result);
            }
            catch (Exception exception)
            {
                var result = new
                {
                    StatusCode = 500,
                    Message = "Error during set alarm"
                };
                return Json(result);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> ToggleAlarm(string deviceId, [FromBody] SetAlarmToggleRequest request)
        {
            try
            {
                var twin = await _registryManager.GetTwinAsync(deviceId);
                if (twin == null)
                {
                    return NotFound();
                }

                twin.Properties.Desired["AlarmSet"] = request.ToggleA;
                await _registryManager.UpdateTwinAsync(deviceId, twin, twin.ETag);
                var result = new
                {
                    StatusCode = 200,
                    Message = "device updated"
                };
                return Ok(result);
            }
            catch (Exception exception)
            {
                var result = new
                {
                    StatusCode = 500,
                    Message = "Error during set alarm"
                };
                return Json(result);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> ToggleLight(string deviceId, [FromBody] SetLightRequest request)
        {
            try
            {
                var twin = await _registryManager.GetTwinAsync(deviceId);

                if (twin == null)
                {
                    return NotFound();
                }

                twin.Properties.Desired["Light"] = request.Light;
                await _registryManager.UpdateTwinAsync(deviceId, twin, twin.ETag);
                var result = new
                {
                    StatusCode = 200,
                    Message = "device updated"
                };
                return Ok(result);
            }
            catch (Exception exception)
            {
                var result = new
                {
                    StatusCode = 500,
                    Message = "Error during set light"
                };
                return Json(result);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> ToggleCondit(string deviceId, [FromBody] SetConditRequest request)
        {
            try
            {
                var twin = await _registryManager.GetTwinAsync(deviceId);
                var twing = await _registryManager.GetTwinAsync(deviceId);

                if (twin == null)
                {
                    return NotFound();
                }

                twin.Properties.Desired["Conditioner"] = request.Condit;
                await _registryManager.UpdateTwinAsync(deviceId, twin, twin.ETag);
                var result = new
                {
                    StatusCode = 200,
                    Message = "device updated"
                };
                return Ok(result);
            }
            catch (Exception exception)
            {
                var result = new
                {
                    StatusCode = 500,
                    Message = "Error during set power"
                };
                return Json(result);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> SetCode(string deviceId, [FromBody] SetCodeRequest request)
        {
            try
            {
                var twin = await _registryManager.GetTwinAsync(deviceId);
                if (twin == null)
                {
                    return NotFound();
                }

                var divider = request.Code.ToCharArray();

                var passCode12 = divider[0].ToString() + divider[1].ToString();
                var passCode34 = divider[2].ToString() + divider[3].ToString();

                twin.Properties.Desired["PassCode12"] = passCode12;
                twin.Properties.Desired["PassCode34"] = passCode34;
                await _registryManager.UpdateTwinAsync(deviceId, twin, twin.ETag);

                var result = new
                {
                    StatusCode = 200,
                    Message = "device updated"
                };
                return Ok(result);
            }
            catch (Exception exception)
            {
                var result = new
                {
                    StatusCode = 500,
                    Message = "Error during set power"
                };
                return Json(result);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<JsonResult> CurrentTemp(string deviceId)
        {
            var storageAccount = CloudStorageAccount.Parse(_configuration["StorageConnectionString"]);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var eventStoreContainer = blobClient.GetContainerReference("temperatures");
            var blob = eventStoreContainer.GetBlockBlobReference($"{deviceId}");

            var json = await blob.DownloadTextAsync();
            var current = JsonConvert.DeserializeObject<JObject>(json);

            var result = new
            {
                temp = current.Value<decimal>("Data")
            };

            return Json(result);
        }
        //-------------------------------------------------------------------------------------------------------------
        //[HttpGet]
        //public async Task<JsonResult> CurrentLight(string deviceId)
        //{
        //    var twin = await _registryManager.GetTwinAsync(deviceId);

        //    var result = new
        //    {
        //        Light = (bool)twin.Properties.Reported[twin.DesiredProperty("Light")]
        //    };

        //    return Json(result);
        //}
        //-------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> CurrentAlarm(string deviceId)
        {
            var twin = await _registryManager.GetTwinAsync(deviceId);
            var alarm = DateTime.MinValue;
            if (twin.Properties.Reported.Contains("AlarmTimeH") && twin.Properties.Reported.Contains("AlarmTimeM"))
            {
                alarm = (DateTime)(twin.Properties.Reported["AlarmTimeH"] + ":" + twin.Properties.Reported["AlarmTimeM"]);
            }
            var result = new
            {
                AlarmTime = alarm.ToShortTimeString()
            };

            return Json(result);
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<JsonResult> Status(string deviceId)
        {
            var twin = await _registryManager.GetTwinAsync(deviceId);

            var ToggleA = false;
            var Light = false;
            var Condit = false;

            if (twin.Properties.Reported.Contains("AlarmSet"))
            {
                ToggleA = (bool)twin.Properties.Reported["AlarmSet"];
            }
            if (twin.Properties.Reported.Contains("Light"))
            {
                Light = (bool)twin.Properties.Reported["Light"];
            }
            if (twin.Properties.Reported.Contains("Conditioner"))
            {
                Condit = (bool)twin.Properties.Reported["Conditioner"];
            }
            var result = new
            {
                toggleA = ToggleA,
                light = Light,
                condit = Condit
            };
            return Json(result);
        }
        
        //[HttpGet]
        //public async Task<JsonResult> CurrentCondit(string deviceId)
        //{
        //    var twin = await _registryManager.GetTwinAsync(deviceId);
        //    var result = new
        //    {
        //        condit = (bool)twin.Properties.Reported["Conditioner"]
        //    };

        //    return Json(result);
        //}
    }
}
