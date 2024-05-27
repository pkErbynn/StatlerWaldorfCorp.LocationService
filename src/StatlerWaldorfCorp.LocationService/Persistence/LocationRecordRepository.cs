using System;
using Microsoft.EntityFrameworkCore;
using StatlerWaldorfCorp.LocationService.Models;

namespace StatlerWaldorfCorp.LocationService.Persistence
{
	public class LocationRecordRepository: ILocationRecordRepository
	{
        private LocationDbContext locationDbContext;

		public LocationRecordRepository(LocationDbContext locationContext)
		{
            this.locationDbContext = locationContext;
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
            return this.locationDbContext.LocationRecords
                .Single(lr => lr.MemberId == memberId && lr.Id == locationRecordId);
        }

        public Dictionary<Guid, SortedList<long, LocationRecord>> GetAllLocationRecords()
        {
            return new Dictionary<Guid, SortedList<long, LocationRecord>>();
        }

        public ICollection<LocationRecord> GetAllLocationRecordsForMember(Guid memberId)
        {
            return this.locationDbContext.LocationRecords
                .Where(lr => lr.MemberId == memberId)
                .OrderBy(lr => lr.Timestamp)
                .ToList();
        }

        public LocationRecord GetLatestLocationForMember(Guid memberId)
        {
            LocationRecord locationRecord = this.locationDbContext.LocationRecords
                .Where(lr => lr.MemberId == memberId)
                .OrderBy(lr => lr.Timestamp)
                .Last();
            return locationRecord;
        }

        public LocationRecord Update(LocationRecord locationRecord)
        {
            this.locationDbContext.Entry(locationRecord).State = EntityState.Modified;
            this.locationDbContext.SaveChanges();
            return locationRecord;
        }
    }
}

