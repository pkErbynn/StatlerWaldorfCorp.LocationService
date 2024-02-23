using System;
using Microsoft.AspNetCore.Mvc;
using StatlerWaldorfCorp.LocationService.Models;
using StatlerWaldorfCorp.LocationService.Persistence;

namespace StatlerWaldorfCorp.LocationService.Controllers
{
    [Route("locations/{memberId}")]
    public class LocationRecordController: Controller
    {
        private ILocationRecordRepository locationRepository;

        public LocationRecordController(ILocationRecordRepository locationRecordRepository)
        {
            this.locationRepository = locationRecordRepository;
        }

        [HttpPost]
        public IActionResult AddLocation(Guid memberId,
            [FromBody] LocationRecord locationRecord)
        {
            locationRepository.Add(locationRecord);
            return this.Created($"/locations/{locationRecord.Id}", locationRecord);
        }

        [HttpGet]
        public IActionResult GetLocationsForMember(Guid memberId)
        {
            return this.Ok(locationRepository.AllLocationRecordsForMember(memberId));
        }

        [HttpGet("latest")]
        public IActionResult GetLatestLocationForMember(Guid memberId)
        {
            return this.Ok(locationRepository.GetLatestLocationRecordForMember(memberId));
        }
    }
}

