﻿// <auto-generated />
using System;
using Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230907210604_InitialConfig")]
    partial class InitialConfig
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Entities.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Patients");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Patient");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Core.Entities.Cat", b =>
                {
                    b.HasBaseType("Core.Entities.Patient");

                    b.HasDiscriminator().HasValue("Cat");
                });

            modelBuilder.Entity("Core.Entities.Dog", b =>
                {
                    b.HasBaseType("Core.Entities.Patient");

                    b.HasDiscriminator().HasValue("Dog");
                });

            modelBuilder.Entity("Core.Entities.Parrot", b =>
                {
                    b.HasBaseType("Core.Entities.Patient");

                    b.HasDiscriminator().HasValue("Parrot");
                });
#pragma warning restore 612, 618
        }
    }
}
