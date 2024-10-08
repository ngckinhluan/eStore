﻿// <auto-generated />
using System;
using BusinessObjects.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BusinessObjects.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240812042517_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BusinessObjects.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            CategoryName = "Beverages"
                        },
                        new
                        {
                            CategoryId = 2,
                            CategoryName = "Condiments"
                        },
                        new
                        {
                            CategoryId = 3,
                            CategoryName = "Confections"
                        },
                        new
                        {
                            CategoryId = 4,
                            CategoryName = "Dairy Products"
                        },
                        new
                        {
                            CategoryId = 5,
                            CategoryName = "Grains/Cereals"
                        });
                });

            modelBuilder.Entity("BusinessObjects.Entities.Member", b =>
                {
                    b.Property<int>("MemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MemberId"));

                    b.Property<string>("City")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("CompanyName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Country")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Password")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("MemberId");

                    b.ToTable("Members");

                    b.HasData(
                        new
                        {
                            MemberId = 1,
                            City = "New York",
                            CompanyName = "Doe Enterprises",
                            Country = "USA",
                            Email = "john.doe@example.com",
                            Password = "password123"
                        },
                        new
                        {
                            MemberId = 2,
                            City = "London",
                            CompanyName = "Smith Co.",
                            Country = "UK",
                            Email = "jane.smith@example.com",
                            Password = "securepassword"
                        });
                });

            modelBuilder.Entity("BusinessObjects.Entities.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OrderId"));

                    b.Property<string>("Freight")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("MemberId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("RequiredDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ShippedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("OrderId");

                    b.HasIndex("MemberId");

                    b.ToTable("Orders");

                    b.HasData(
                        new
                        {
                            OrderId = 1,
                            Freight = "Standard",
                            MemberId = 1,
                            OrderDate = new DateTime(2024, 8, 12, 4, 25, 17, 735, DateTimeKind.Utc).AddTicks(8561),
                            RequiredDate = new DateTime(2024, 8, 19, 4, 25, 17, 735, DateTimeKind.Utc).AddTicks(8564),
                            ShippedDate = new DateTime(2024, 8, 14, 4, 25, 17, 735, DateTimeKind.Utc).AddTicks(8569)
                        },
                        new
                        {
                            OrderId = 2,
                            Freight = "Express",
                            MemberId = 2,
                            OrderDate = new DateTime(2024, 8, 12, 4, 25, 17, 735, DateTimeKind.Utc).AddTicks(8570),
                            RequiredDate = new DateTime(2024, 8, 19, 4, 25, 17, 735, DateTimeKind.Utc).AddTicks(8571),
                            ShippedDate = new DateTime(2024, 8, 15, 4, 25, 17, 735, DateTimeKind.Utc).AddTicks(8571)
                        });
                });

            modelBuilder.Entity("BusinessObjects.Entities.OrderDetail", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<float>("UnitPrice")
                        .HasColumnType("real");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");

                    b.HasData(
                        new
                        {
                            OrderId = 1,
                            ProductId = 1,
                            Quantity = 2,
                            UnitPrice = 18f
                        },
                        new
                        {
                            OrderId = 1,
                            ProductId = 2,
                            Quantity = 3,
                            UnitPrice = 19f
                        },
                        new
                        {
                            OrderId = 2,
                            ProductId = 3,
                            Quantity = 1,
                            UnitPrice = 10f
                        },
                        new
                        {
                            OrderId = 2,
                            ProductId = 4,
                            Quantity = 2,
                            UnitPrice = 22f
                        });
                });

            modelBuilder.Entity("BusinessObjects.Entities.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("ProductName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<float>("UnitPrice")
                        .HasColumnType("real");

                    b.Property<int>("UnitsInStock")
                        .HasColumnType("integer");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            CategoryId = 1,
                            ProductName = "Chai",
                            UnitPrice = 18f,
                            UnitsInStock = 39,
                            Weight = 0f
                        },
                        new
                        {
                            ProductId = 2,
                            CategoryId = 1,
                            ProductName = "Chang",
                            UnitPrice = 19f,
                            UnitsInStock = 17,
                            Weight = 0f
                        },
                        new
                        {
                            ProductId = 3,
                            CategoryId = 2,
                            ProductName = "Aniseed Syrup",
                            UnitPrice = 10f,
                            UnitsInStock = 13,
                            Weight = 0f
                        },
                        new
                        {
                            ProductId = 4,
                            CategoryId = 2,
                            ProductName = "Chef Anton's Cajun Seasoning",
                            UnitPrice = 22f,
                            UnitsInStock = 53,
                            Weight = 0f
                        },
                        new
                        {
                            ProductId = 5,
                            CategoryId = 2,
                            ProductName = "Chef Anton's Gumbo Mix",
                            UnitPrice = 21.35f,
                            UnitsInStock = 0,
                            Weight = 0f
                        });
                });

            modelBuilder.Entity("BusinessObjects.Entities.Order", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Member", "Member")
                        .WithMany("Orders")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");
                });

            modelBuilder.Entity("BusinessObjects.Entities.OrderDetail", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObjects.Entities.Product", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Product", b =>
                {
                    b.HasOne("BusinessObjects.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Member", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("BusinessObjects.Entities.Product", b =>
                {
                    b.Navigation("OrderDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
