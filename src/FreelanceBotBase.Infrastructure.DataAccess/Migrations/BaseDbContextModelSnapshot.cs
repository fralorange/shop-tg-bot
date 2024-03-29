﻿// <auto-generated />
using System;
using FreelanceBotBase.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FreelanceBotBase.Infrastructure.DataAccess.Migrations
{
    [DbContext(typeof(BaseDbContext))]
    partial class BaseDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FreelanceBotBase.Domain.DeliveryPoint.DeliveryPoint", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("ManagerId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId")
                        .IsUnique();

                    b.ToTable("DeliveryPoint", (string)null);
                });

            modelBuilder.Entity("FreelanceBotBase.Domain.User.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("UserId"));

                    b.Property<long?>("DeliveryPointId")
                        .HasColumnType("bigint");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer");

                    b.HasKey("UserId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("FreelanceBotBase.Domain.DeliveryPoint.DeliveryPoint", b =>
                {
                    b.HasOne("FreelanceBotBase.Domain.User.User", "Manager")
                        .WithOne("DeliveryPoint")
                        .HasForeignKey("FreelanceBotBase.Domain.DeliveryPoint.DeliveryPoint", "ManagerId");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("FreelanceBotBase.Domain.User.User", b =>
                {
                    b.Navigation("DeliveryPoint");
                });
#pragma warning restore 612, 618
        }
    }
}
