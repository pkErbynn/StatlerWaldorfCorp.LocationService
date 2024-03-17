using System;
using Microsoft.EntityFrameworkCore;
using StatlerWaldorfCorp.LocationService.Models;

namespace StatlerWaldorfCorp.LocationService.Persistence
{
	public class LocationRecordRepository: ILocationRecordRepository
	{
        // add psql package ref
        // Get request
        private LocationDbContext locationDbContext;

		public LocationRecordRepository(LocationDbContext locationDb)
		{
            this.locationDbContext = locationDb;
		}

        public LocationRecord Add(LocationRecord locationRecord)
        {
            this.locationDbContext.Add(locationRecord);
            this.locationDbContext.SaveChanges();
            return locationRecord;
        }

        public LocationRecord Delete(Guid memberId, Guid locationRecordId)
        {
            LocationRecord locationRecord = this.Get(memberId, locationRecordId);
            this.locationDbContext.Remove(locationRecord);
            this.locationDbContext.SaveChanges();
            return locationRecord;
        }

        public LocationRecord Get(Guid memberId, Guid locationRecordId)
        {
            return this.locationDbContext.locationsRecords
                .Single(lr => lr.MemberId == memberId && lr.Id == locationRecordId);
        }

        public Dictionary<Guid, SortedList<long, LocationRecord>> GetAllLocationRecords()
        {
            throw new NotImplementedException();
        }

        public ICollection<LocationRecord> GetAllLocationRecordsForMember(Guid memberId)
        {
            throw new NotImplementedException();
        }

        public LocationRecord GetLatestLocationForMember(Guid memberId)
        {
            // continue here
        }

        public LocationRecord Update(LocationRecord locationRecord)
        {
            this.locationDbContext.Entry(locationRecord).State = EntityState.Modified;
            this.locationDbContext.SaveChanges();
            return locationRecord;
        }
    }
}

