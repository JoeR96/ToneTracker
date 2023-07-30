﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToneTracker;

#nullable disable

namespace ToneTracker.Migrations
{
    [DbContext(typeof(ToneTrackerDbContext))]
    [Migration("20230708154835_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.8");

            modelBuilder.Entity("ToneTracker.Dial.Dial", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PedalId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PedalId");

                    b.ToTable("Dials");
                });

            modelBuilder.Entity("ToneTracker.Pedal.Pedal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Pedals");
                });

            modelBuilder.Entity("ToneTracker.Toggle.Toggle", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PedalId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PedalId");

                    b.ToTable("Toggles");
                });

            modelBuilder.Entity("ToneTracker.Dial.Dial", b =>
                {
                    b.HasOne("ToneTracker.Pedal.Pedal", null)
                        .WithMany("Dials")
                        .HasForeignKey("PedalId");
                });

            modelBuilder.Entity("ToneTracker.Toggle.Toggle", b =>
                {
                    b.HasOne("ToneTracker.Pedal.Pedal", null)
                        .WithMany("Toggles")
                        .HasForeignKey("PedalId");
                });

            modelBuilder.Entity("ToneTracker.Pedal.Pedal", b =>
                {
                    b.Navigation("Dials");

                    b.Navigation("Toggles");
                });
#pragma warning restore 612, 618
        }
    }
}