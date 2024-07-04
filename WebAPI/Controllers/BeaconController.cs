using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Shared.Objects;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BeaconController : ControllerBase
    {
        private readonly ILogger<BeaconController> _logger;
        private readonly Context _context;

        public BeaconController(ILogger<BeaconController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        [ActionName("Beacon")]
        [Description("Posts a single beacons data to the database")]
        public async Task<IActionResult> PostBeaconAsync([FromBody] PostBeacon? beaconData)
        {
            KeyValuePair<string, StringValues> contentType = this.Request.Headers.FirstOrDefault(h => h.Key == "Content-Type");

            if (string.IsNullOrEmpty(contentType.Value))
            {
                return await Task.FromResult(new UnsupportedMediaTypeResult());
            }
            else if (contentType.Value == "application/json") { }
            else
            {
                return await Task.FromResult(new UnsupportedMediaTypeResult());
            }

            if(beaconData == null)
            {
                return await Task.FromResult(BadRequest("Bad Request"));
            }

            _context.Beacons.Add(new Beacon
            {
                ID = new Guid(),
                Name = beaconData.Name,
                MacAddress = beaconData.MacAddress,
                UUID = new Guid(beaconData.UUID),
                Major = int.Parse(beaconData.Major),
                Minor = int.Parse(beaconData.Minor),
                RSSI = int.Parse(beaconData.RSSI),
                RSSI1M = int.Parse(beaconData.RSSI1M),
                LocationX = beaconData.LocationX,
                LocationY = beaconData.LocationY,
                TimeStamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return await Task.FromResult(Ok());
        }

        [HttpPost]
        [ActionName("BeaconX")]
        [Description("Posts a list of beacon data related to the x position")]
        public async Task<IActionResult> PostBeaconXAsync([FromBody] PostBeaconX? beaconData)
        {
            //KeyValuePair<string, StringValues> contentType = this.Request.Headers.FirstOrDefault(h => h.Key == "Content-Type");

            //if (string.IsNullOrEmpty(contentType.Value))
            //{
            //    return await Task.FromResult(new UnsupportedMediaTypeResult());
            //}
            //else if (contentType.Value == "application/json") { }
            //else
            //{
            //    return await Task.FromResult(new UnsupportedMediaTypeResult());
            //}

            if (beaconData == null)
            {
                return await Task.FromResult(BadRequest("Bad Request"));
            }

            _context.BeaconTrainingDataX.Add(new BeaconTrainingDataX
            {
                ID = new Guid(),
                B1 = beaconData.b1,
                B2 = beaconData.b2,
                B3 = beaconData.b3,
                B4 = beaconData.b4,
                B5 = beaconData.b5,
                B6 = beaconData.b6,
                B7 = beaconData.b7,
                B8 = beaconData.b8,
                B9 = beaconData.b9,
                B10 = beaconData.b10,
                B11 = beaconData.b11,
                B12 = beaconData.b12,
                B13 = beaconData.b13,
                B14 = beaconData.b14,
                B15 = beaconData.b15,
                B16 = beaconData.b16,
                B17 = beaconData.b17,
                B18 = beaconData.b18,
                B19 = beaconData.b19,
                LocationX = beaconData.locationX,
                TimeStamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return await Task.FromResult(Ok());
        }

        [HttpGet]
        [ActionName("BeaconX")]
        [Description("Gets a predicted location for where x could be")]
        public async Task<IActionResult> GetBeaconXAsync([FromQuery] GetBeaconX? getBeacon)
        {
            if(getBeacon == null)
            {
                return await Task.FromResult(BadRequest("Bad Request"));
            }

            var data = new X_Value.ModelInput();

            if (getBeacon.B1.HasValue) data.B1 = (float)getBeacon.B1;
            if (getBeacon.B2.HasValue) data.B2 = (float)getBeacon.B2;
            if (getBeacon.B3.HasValue) data.B3 = (float)getBeacon.B3;
            if (getBeacon.B4.HasValue) data.B4 = (float)getBeacon.B4;
            if (getBeacon.B5.HasValue) data.B5 = (float)getBeacon.B5;
            if (getBeacon.B6.HasValue) data.B6 = (float)getBeacon.B6;
            if (getBeacon.B7.HasValue) data.B7 = (float)getBeacon.B7;
            if (getBeacon.B8.HasValue) data.B8 = (float)getBeacon.B8;
            if (getBeacon.B9.HasValue) data.B9 = (float)getBeacon.B9;
            if (getBeacon.B10.HasValue) data.B10 = (float)getBeacon.B10;
            if (getBeacon.B11.HasValue) data.B11 = (float)getBeacon.B11;
            if (getBeacon.B12.HasValue) data.B12 = (float)getBeacon.B12;
            if (getBeacon.B13.HasValue) data.B13 = (float)getBeacon.B13;
            if (getBeacon.B14.HasValue) data.B14 = (float)getBeacon.B14;
            if (getBeacon.B15.HasValue) data.B15 = (float)getBeacon.B15;
            if (getBeacon.B16.HasValue) data.B16 = (float)getBeacon.B16;
            if (getBeacon.B17.HasValue) data.B17 = (float)getBeacon.B17;
            if (getBeacon.B18.HasValue) data.B18 = (float)getBeacon.B18;
            if (getBeacon.B19.HasValue) data.B19 = (float)getBeacon.B19;

            var result = X_Value.Predict(data);

            return await Task.FromResult(Ok(result));
        }

        [HttpPost]
        [ActionName("BeaconY")]
        [Description("Posts a list of beacon data related to the y position")]
        public async Task<IActionResult> PostBeaconYAsync([FromBody]PostBeaconY? beaconData)
        {
            //KeyValuePair<string, StringValues> contentType = this.Request.Headers.FirstOrDefault(h => h.Key == "Content-Type");

            //if (string.IsNullOrEmpty(contentType.Value))
            //{
            //    return await Task.FromResult(new UnsupportedMediaTypeResult());
            //}
            //else if (contentType.Value == "application/json") { }
            //else
            //{
            //    return await Task.FromResult(new UnsupportedMediaTypeResult());
            //}

            if (beaconData == null)
            {
                return await Task.FromResult(BadRequest("Bad Request"));
            }

            _context.BeaconTrainingDataY.Add(new BeaconTrainingDataY
            {
                ID = new Guid(),
                B1 = beaconData.b1,
                B2 = beaconData.b2,
                B3 = beaconData.b3,
                B4 = beaconData.b4,
                B5 = beaconData.b5,
                B6 = beaconData.b6,
                B7 = beaconData.b7,
                B8 = beaconData.b8,
                B9 = beaconData.b9,
                B10 = beaconData.b10,
                B11 = beaconData.b11,
                B12 = beaconData.b12,
                B13 = beaconData.b13,
                B14 = beaconData.b14,
                B15 = beaconData.b15,
                B16 = beaconData.b16,
                B17 = beaconData.b17,
                B18 = beaconData.b18,
                B19 = beaconData.b19,
                LocationY = beaconData.locationY,
                TimeStamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return await Task.FromResult(Ok());
        }

        [HttpGet]
        [ActionName("BeaconY")]
        [Description("Gets a predicted location for where y could be")]
        public async Task<IActionResult> GetBeaconY([FromQuery] GetBeaconY getBeacon)
        {
            if (getBeacon == null)
            {
                return await Task.FromResult(BadRequest("Bad Request"));
            }

            var data = new Y_Value.ModelInput();

            if (getBeacon.B1.HasValue) data.B1 = (float)getBeacon.B1;
            if (getBeacon.B2.HasValue) data.B2 = (float)getBeacon.B2;
            if (getBeacon.B3.HasValue) data.B3 = (float)getBeacon.B3;
            if (getBeacon.B4.HasValue) data.B4 = (float)getBeacon.B4;
            if (getBeacon.B5.HasValue) data.B5 = (float)getBeacon.B5;
            if (getBeacon.B6.HasValue) data.B6 = (float)getBeacon.B6;
            if (getBeacon.B7.HasValue) data.B7 = (float)getBeacon.B7;
            if (getBeacon.B8.HasValue) data.B8 = (float)getBeacon.B8;
            if (getBeacon.B9.HasValue) data.B9 = (float)getBeacon.B9;
            if (getBeacon.B10.HasValue) data.B10 = (float)getBeacon.B10;
            if (getBeacon.B11.HasValue) data.B11 = (float)getBeacon.B11;
            if (getBeacon.B12.HasValue) data.B12 = (float)getBeacon.B12;
            if (getBeacon.B13.HasValue) data.B13 = (float)getBeacon.B13;
            if (getBeacon.B14.HasValue) data.B14 = (float)getBeacon.B14;
            if (getBeacon.B15.HasValue) data.B15 = (float)getBeacon.B15;
            if (getBeacon.B16.HasValue) data.B16 = (float)getBeacon.B16;
            if (getBeacon.B17.HasValue) data.B17 = (float)getBeacon.B17;
            if (getBeacon.B18.HasValue) data.B18 = (float)getBeacon.B18;
            if (getBeacon.B19.HasValue) data.B19 = (float)getBeacon.B19;

            var result = Y_Value.Predict(data);

            return await Task.FromResult(Ok(result));

        }
    }
}