using System;
using StatlerWaldorfCorp.LocationService.Models;

namespace StatlerWaldorfCorp.LocationService.Persistence
{
	public interface ILocationRecordRepository
	{
        LocationRecord Add(LocationRecord locationRecord);
        LocationRecord Update(LocationRecord locationRecord);
        LocationRecord Get(Guid locationRecordId, Guid memberId);
        LocationRecord Delete(Guid locationRecordId, Guid memberId);

        LocationRecord GetLatestLocationRecordForMember(Guid memberId);

        ICollection<LocationRecord> AllLocationRecordsForMember(Guid memberId);
    }
}

