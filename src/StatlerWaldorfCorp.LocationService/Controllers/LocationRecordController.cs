using System;
using Microsoft.AspNetCore.Mvc;
using StatlerWaldorfCorp.LocationService.Models;
using StatlerWaldorfCorp.LocationService.Persistence;

namespace StatlerWaldorfCorp.LocationService.Controllers
{
    [Route("locations/{memberId}")]
    public class LocationRecordController : Controller
    {
        private ILocationRecordRepository locationRepository;

        public LocationRecordController(ILocationRecordRepository locationRecordRepository)
        {
            this.locationRepository = locationRecordRepository;
        }

        [HttpPost]
        public IActionResult AddLocation(Guid memberId, [FromBody] LocationRecord locationRecord)
        {
            locationRepository.Add(locationRecord);
            return this.Created($"/locations/{locationRecord.Id}", locationRecord);
        }

        [HttpGet]
        public IActionResult GetLocationsForMember(Guid memberId)
        {
            return this.Ok(locationRepository.GetAllLocationRecordsForMember(memberId));
        }

        [HttpGet("latest")]
        public IActionResult GetLatestLocationForMember(Guid memberId)
        {
            return this.Ok(locationRepository.GetLatestLocationForMember(memberId));
        }
    }

    [Route("locations")]
    public class LocationRecordAllController : Controller
    {
        private ILocationRecordRepository locationRepository;

        public LocationRecordAllController(ILocationRecordRepository locationRecordRepository)
        {
            this.locationRepository = locationRecordRepository;
        }
        [HttpGet]
        public IActionResult GetAllLocationForMembers()
        {
            return this.Ok(locationRepository.GetAllLocationRecords());
        }
    }

    /*

    {
      "3fa85f64-5717-4562-b3fc-2c963f66afa6": {
        "434-3433": {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "latitude": 0,
          "longitude": 0,
          "altitude": 0,
          "timestamp": 0,
          "memberId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        }
      },
      "3fa85f64-5717-4562-b3fc-2c963f66afa7": {
        "434-3436": {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "latitude": 0,
          "longitude": 0,
          "altitude": 0,
          "timestamp": 0,
          "memberId": "3fa85f64-5717-4562-b3fc-2c963f66afa7"
        }
      },
      "3fa85f64-5717-4562-b3fc-2c963f66afa8": {
        "434-3438": {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "latitude": 0,
          "longitude": 0,
          "altitude": 0,
          "timestamp": 0,
          "memberId": "3fa85f64-5717-4562-b3fc-2c963f66afa8"
        }
      }
    }

    */
}

