using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StatlerWaldorfCorp.LocationService.Models;
using StatlerWaldorfCorp.LocationService.Persistence;

namespace StatlerWaldorfCorp.LocationService.Tests.Integration;

public class PostgresIntegrationTest
{
    private IConfigurationRoot config;
    private LocationDbContext locationDbContext;

    public PostgresIntegrationTest()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // It's common to use a named connection string in .NET Core/NET 6 projects
        var connectionString = config.GetSection("postgres:cstr").Value;
        connectionString = connectionString.Replace("{DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME"))
                                        .Replace("{DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
                                        .Replace("{DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"));


        var optionsBuilder = new DbContextOptionsBuilder<LocationDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        this.locationDbContext = new LocationDbContext(optionsBuilder.Options);
    }


    [Fact]
    public void ShouldPersistRecord()
    {
        LocationRecordRepository repository = new LocationRecordRepository(locationDbContext);

        LocationRecord firstRecord = new LocationRecord()
        {
            Id = Guid.NewGuid(),
            Timestamp = 1,
            MemberId = Guid.NewGuid(),
            Latitude = 12.3f
        };
        repository.Add(firstRecord);

        LocationRecord targetRecord = repository.Get(firstRecord.MemberId, firstRecord.Id);

        // assert values equal first and targetRecord
        Assert.Equal(firstRecord.Timestamp, targetRecord.Timestamp);
        Assert.Equal(firstRecord.MemberId, targetRecord.MemberId);
        Assert.Equal(firstRecord.Id, targetRecord.Id);
        Assert.Equal(firstRecord.Latitude, targetRecord.Latitude);
    }

    [Fact]
    public void ShouldUpdateRecord()
    {
        LocationRecordRepository repository = new LocationRecordRepository(locationDbContext);

        LocationRecord firstRecord = new LocationRecord()
        {
            Id = Guid.NewGuid(),
            Timestamp = 1,
            MemberId = Guid.NewGuid(),
            Latitude = 12.3f
        };
        repository.Add(firstRecord);

        //LocationRecord targetRecord = repository.Get(firstRecord.MemberId, firstRecord.Id);

        // modify firstRecord.
        firstRecord.Longitude = 12.5f;
        firstRecord.Latitude = 47.09f;
        repository.Update(firstRecord);

        LocationRecord target2 = repository.Get(firstRecord.MemberId, firstRecord.Id);

        Assert.Equal(firstRecord.Timestamp, target2.Timestamp);
        Assert.Equal(firstRecord.Longitude, target2.Longitude);
        Assert.Equal(firstRecord.Latitude, target2.Latitude);
        Assert.Equal(firstRecord.Id, target2.Id);
        Assert.Equal(firstRecord.MemberId, target2.MemberId);
    }

    [Fact]
    public void ShouldDeleteRecord()
    {
        LocationRecordRepository repository = new LocationRecordRepository(locationDbContext);
        Guid memberId = Guid.NewGuid();

        LocationRecord firstRecord = new LocationRecord()
        {
            Id = Guid.NewGuid(),
            Timestamp = 1,
            MemberId = memberId,
            Latitude = 12.3f
        };
        repository.Add(firstRecord);
        LocationRecord secondRecord = new LocationRecord()
        {
            Id = Guid.NewGuid(),
            Timestamp = 2,
            MemberId = memberId,
            Latitude = 24.4f
        };
        repository.Add(secondRecord);

        int initialCount = repository.GetAllLocationRecordsForMember(memberId).Count();
        repository.Delete(memberId, secondRecord.Id);
        int afterCount = repository.GetAllLocationRecordsForMember(memberId).Count();

        LocationRecord target1 = repository.Get(firstRecord.MemberId, firstRecord.Id);
        LocationRecord target2 = repository.Get(firstRecord.MemberId, secondRecord.Id);

        Assert.Equal(initialCount - 1, afterCount);
        Assert.Equal(target1.Id, firstRecord.Id);
        Assert.NotNull(target1);
        Assert.Null(target2);
    }

    [Fact]
    public void ShouldGetAllLocationRecordsForMember()
    {
        LocationRecordRepository repository = new LocationRecordRepository(locationDbContext);
        Guid memberId = Guid.NewGuid();

        int initialCount = repository.GetAllLocationRecordsForMember(memberId).Count();

        LocationRecord firstRecord = new LocationRecord()
        {
            Id = Guid.NewGuid(),
            Timestamp = 1,
            MemberId = memberId,
            Latitude = 12.3f
        };
        repository.Add(firstRecord);
        LocationRecord secondRecord = new LocationRecord()
        {
            Id = Guid.NewGuid(),
            Timestamp = 2,
            MemberId = memberId,
            Latitude = 24.4f
        };
        repository.Add(secondRecord);

        ICollection<LocationRecord> records = repository.GetAllLocationRecordsForMember(memberId);
        int afterCount = records.Count();

        Assert.Equal(initialCount + 2, afterCount);
        Assert.NotNull(records.FirstOrDefault(r => r.Id == firstRecord.Id));
        Assert.NotNull(records.FirstOrDefault(r => r.Id == secondRecord.Id));
    }

    [Fact]
    public void ShouldGetLatestForMember()
    {
        LocationRecordRepository repository = new LocationRecordRepository(locationDbContext);
        Guid memberId = Guid.NewGuid();

        LocationRecord firstRecord = new LocationRecord()
        {
            Id = Guid.NewGuid(),
            Timestamp = 1,
            MemberId = memberId,
            Latitude = 12.3f
        };
        repository.Add(firstRecord);
        LocationRecord secondRecord = new LocationRecord()
        {
            Id = Guid.NewGuid(),
            Timestamp = 2,
            MemberId = memberId,
            Latitude = 24.4f
        };
        repository.Add(secondRecord);

        LocationRecord latest = repository.GetLatestLocationForMember(memberId);

        Assert.NotNull(latest);
        Assert.Equal(latest.Id, secondRecord.Id);
        Assert.NotEqual(latest.Id, firstRecord.Id);
    }


    /*
     user, password, locationservice
    */

}
