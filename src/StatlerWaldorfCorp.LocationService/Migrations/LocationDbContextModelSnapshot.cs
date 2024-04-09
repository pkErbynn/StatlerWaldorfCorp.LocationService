﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StatlerWaldorfCorp.LocationService.Persistence;

#nullable disable

namespace StatlerWaldorfCorp.LocationService.Migrations
{
    [DbContext(typeof(LocationDbContext))]
    partial class LocationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.27")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("StatlerWaldorfCorp.LocationService.Models.LocationRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<float>("Altitude")
                        .HasColumnType("real");

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid");

                    b.Property<long>("Timestamp")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("LocationRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
