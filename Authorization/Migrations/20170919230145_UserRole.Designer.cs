﻿// <auto-generated />
using Authorization.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Authorization.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170919230145_UserRole")]
    partial class UserRole
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Authorization.Data.Authorization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTime");

                    b.Property<TimeSpan>("Expiration");

                    b.Property<Guid>("Token");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Authorizations");
                });

            modelBuilder.Entity("Authorization.Data.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Authorization.Data.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<int?>("PhotoId");

                    b.Property<int?>("PositionId");

                    b.Property<string>("Surname");

                    b.Property<int?>("TeamId");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.HasIndex("PositionId");

                    b.HasIndex("TeamId")
                        .IsUnique()
                        .HasFilter("[TeamId] IS NOT NULL");

                    b.HasIndex("UserId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Authorization.Data.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("Authorization.Data.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Authorization.Data.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DepartmentId");

                    b.Property<int>("Grade");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("Authorization.Data.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Authorization.Data.RoleModule", b =>
                {
                    b.Property<int>("IdRole");

                    b.Property<int>("IdModule");

                    b.HasKey("IdRole", "IdModule");

                    b.HasIndex("IdModule");

                    b.ToTable("RoleModules");
                });

            modelBuilder.Entity("Authorization.Data.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DepartmentId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Authorization.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int?>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Authorization.Data.Authorization", b =>
                {
                    b.HasOne("Authorization.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Authorization.Data.Employee", b =>
                {
                    b.HasOne("Authorization.Data.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");

                    b.HasOne("Authorization.Data.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId");

                    b.HasOne("Authorization.Data.Team", "Team")
                        .WithOne("Chief")
                        .HasForeignKey("Authorization.Data.Employee", "TeamId");

                    b.HasOne("Authorization.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Authorization.Data.Position", b =>
                {
                    b.HasOne("Authorization.Data.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");
                });

            modelBuilder.Entity("Authorization.Data.RoleModule", b =>
                {
                    b.HasOne("Authorization.Data.Module", "Module")
                        .WithMany("RoleModules")
                        .HasForeignKey("IdModule")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Authorization.Data.Role", "Role")
                        .WithMany("RoleModules")
                        .HasForeignKey("IdRole")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Authorization.Data.Team", b =>
                {
                    b.HasOne("Authorization.Data.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");
                });

            modelBuilder.Entity("Authorization.Data.User", b =>
                {
                    b.HasOne("Authorization.Data.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });
#pragma warning restore 612, 618
        }
    }
}
