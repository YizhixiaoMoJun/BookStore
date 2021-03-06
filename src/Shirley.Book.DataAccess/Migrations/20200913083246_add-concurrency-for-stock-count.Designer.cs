﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shirley.Book.DataAccess;

namespace Shirley.Book.DataAccess.Migrations
{
    [DbContext(typeof(BookContext))]
    [Migration("20200913083246_add-concurrency-for-stock-count")]
    partial class addconcurrencyforstockcount
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("BookApi.Model.UserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Pwd")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("Shirley.Book.Model.BookOrder", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BookOrders");
                });

            modelBuilder.Entity("Shirley.Book.Model.BookOrderDetail", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("BookOrderId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Sn")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BookOrderId");

                    b.ToTable("BookOrderDetials");
                });

            modelBuilder.Entity("Shirley.Book.Model.BookStock", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Sn")
                        .HasColumnType("TEXT");

                    b.Property<int>("StockCount")
                        .IsConcurrencyToken()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("BookStocks");
                });

            modelBuilder.Entity("Shirley.Book.Model.OrderStock", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Sn")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("OrderStocks");
                });

            modelBuilder.Entity("Shirley.Book.Model.BookOrderDetail", b =>
                {
                    b.HasOne("Shirley.Book.Model.BookOrder", null)
                        .WithMany("OrderDetails")
                        .HasForeignKey("BookOrderId");
                });
#pragma warning restore 612, 618
        }
    }
}
