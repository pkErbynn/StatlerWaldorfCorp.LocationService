using Microsoft.AspNetCore.Mvc;
using StatlerWaldorfCorp.LocationService.Controllers;
using StatlerWaldorfCorp.LocationService.Models;
using StatlerWaldorfCorp.LocationService.Persistence;

namespace StatlerWaldorfCorp.LocationService.Tests;

public class LocationRecordControllerTest
{
    [Fact]
    public void AddLocation_ShouldAdd()
    {
        ILocationRecordRepository repository = new InMemoryLocationRecordRepository();
        LocationRecordController controller = new LocationRecordController(repository);
        Guid memberGuid = Guid.NewGuid();

        controller.AddLocation(memberGuid, new LocationRecord()
        {
            Id = Guid.NewGuid(),
            MemberId = memberGuid,
            Timestamp = 1,
        });
        controller.AddLocation(memberGuid, new LocationRecord()
        {
            Id = Guid.NewGuid(),
            MemberId = memberGuid,
            Timestamp = 2
        });

        Assert.Equal(2, repository.GetAllLocationRecordsForMember(memberGuid).Count());
    }

    [Fact]
    public void GetLocationsForMember_ShouldReturnEmtpyListForNewMember()
    {
        ILocationRecordRepository repository = new InMemoryLocationRecordRepository();
        LocationRecordController controller = new LocationRecordController(repository);
        Guid memberGuid = Guid.NewGuid();

        ICollection<LocationRecord> locationRecords =
            ((controller.GetLocationsForMember(memberGuid) as ObjectResult).Value
                as ICollection<LocationRecord>);

        Assert.Equal(0, locationRecords.Count());
    }

    [Fact]
    public void GetLocationsForMember_ShouldTrackAllLocationsForMember()
    {
        ILocationRecordRepository repository = new InMemoryLocationRecordRepository();
        LocationRecordController controller = new LocationRecordController(repository);
        Guid memberGuid = Guid.NewGuid();

        controller.AddLocation(memberGuid, new LocationRecord()
        {
            Id = Guid.NewGuid(),
            Timestamp = 1,
            MemberId = memberGuid,
            Latitude = 12.3f
        });
        controller.AddLocation(memberGuid, new LocationRecord()
        {
            Id = Guid.NewGuid(),
            Timestamp = 2,
            MemberId = memberGuid,
            Latitude = 23.4f
        });
        controller.AddLocation(Guid.NewGuid(), new LocationRecord()
        {
            Id = Guid.NewGuid(),
            Timestamp = 3,
            MemberId = Guid.NewGuid(),
            Latitude = 23.4f
        });

        ICollection<LocationRecord> locationRecords =
            ((controller.GetLocationsForMember(memberGuid) as ObjectResult).Value as ICollection<LocationRecord>);

        Assert.Equal(2, locationRecords.Count());
    }

}
