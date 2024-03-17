using System;
using StatlerWaldorfCorp.LocationService.Models;

namespace StatlerWaldorfCorp.LocationService.Persistence
{
	public interface ILocationRecordRepository
	{
        LocationRecord Add(LocationRecord locationRecord);
        LocationRecord Update(LocationRecord locationRecord);
        LocationRecord Get(Guid memberId, Guid locationRecordId);
        LocationRecord Delete(Guid memberId, Guid locationRecordId);

        LocationRecord GetLatestLocationForMember(Guid memberId);

        ICollection<LocationRecord> GetAllLocationRecordsForMember(Guid memberId);

        Dictionary<Guid, SortedList<long, LocationRecord>> GetAllLocationRecords();
    }
}

