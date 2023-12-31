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
    [Migration("20230708191152_InitialCreate+Settings11")]
    partial class InitialCreateSettings11
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

            modelBuilder.Entity("ToneTracker.Setting", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ControlId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SettingName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ControlId");

                    b.ToTable("Setting");
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
                    b.HasOne("ToneTracker.Pedal.Pedal", "Pedal")
                        .WithMany("Dials")
                        .HasForeignKey("PedalId");

                    b.Navigation("Pedal");
                });

            modelBuilder.Entity("ToneTracker.Setting", b =>
                {
                    b.HasOne("ToneTracker.Dial.Dial", "Dial")
                        .WithMany("Settings")
                        .HasForeignKey("ControlId");

                    b.HasOne("ToneTracker.Toggle.Toggle", "Toggle")
                        .WithMany("Settings")
                        .HasForeignKey("ControlId");

                    b.Navigation("Dial");

                    b.Navigation("Toggle");
                });

            modelBuilder.Entity("ToneTracker.Toggle.Toggle", b =>
                {
                    b.HasOne("ToneTracker.Pedal.Pedal", "Pedal")
                        .WithMany("Toggles")
                        .HasForeignKey("PedalId");

                    b.Navigation("Pedal");
                });

            modelBuilder.Entity("ToneTracker.Dial.Dial", b =>
                {
                    b.Navigation("Settings");
                });

            modelBuilder.Entity("ToneTracker.Pedal.Pedal", b =>
                {
                    b.Navigation("Dials");

                    b.Navigation("Toggles");
                });

            modelBuilder.Entity("ToneTracker.Toggle.Toggle", b =>
                {
                    b.Navigation("Settings");
                });
#pragma warning restore 612, 618
        }
    }
}
