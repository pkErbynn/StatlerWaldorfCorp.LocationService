using System;
using StatlerWaldorfCorp.LocationService.Models;

namespace StatlerWaldorfCorp.LocationService.Persistence
{
	public class InMemoryLocationRecordRepository: ILocationRecordRepository
    {
        private static Dictionary<Guid, SortedList<long, LocationRecord>> locationRecords;  // { memberId: [ { timestamp: locationRecord }, ... ] }

        public InMemoryLocationRecordRepository()
        {
            if (locationRecords == null)
            {
                locationRecords = new Dictionary<Guid, SortedList<long, LocationRecord>>();
            }
        }

        public LocationRecord Add(LocationRecord locationRecord)
        {
            var memberRecords = this.getMemberRecords(locationRecord.MemberId);
            memberRecords.Add(locationRecord.Timestamp, locationRecord);
            return locationRecord;
        }

        private SortedList<long, LocationRecord> getMemberRecords(Guid memberId)
        {
            if (!locationRecords.ContainsKey(memberId))
            {
                locationRecords.Add(memberId, new SortedList<long, LocationRecord>());
            }
            var list = locationRecords[memberId];
            return list;
        }

        public ICollection<LocationRecord> GetAllLocationRecordsForMember(Guid memberId)
        {
            var locationRecords = this.getMemberRecords(memberId);
            return locationRecords.Values.Where(l => l.MemberId == memberId).ToList();
        }

        public LocationRecord Delete(Guid memberId, Guid locationRecordId)
        {
            var memberRecords = this.getMemberRecords(memberId);
            LocationRecord lr = memberRecords.Values.Where(l => l.Id == locationRecordId).FirstOrDefault();
            if (lr != null)
            {
                memberRecords.Remove(lr.Timestamp);
            }

            return lr;
        }

        public LocationRecord Get(Guid memberId, Guid locationRecordId)
        {
            var memberRecords = getMemberRecords(memberId);
            var lr = memberRecords.Values.Where(lr => lr.Id == locationRecordId).FirstOrDefault();
            return lr;
        }

        public LocationRecord GetLatestLocationForMember(Guid memberId)
        {
            var memberLocationRecords = getMemberRecords(memberId);
            LocationRecord lr = memberLocationRecords.Values.LastOrDefault();
            return lr;
        }

        public LocationRecord Update(LocationRecord locationRecord)
        {
            return this.Delete(locationRecord.MemberId, locationRecord.Id);
        }
    }
}

