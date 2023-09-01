﻿// <auto-generated />
using ECOM.Services.ShoppingCartAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ECOM.Services.ShoppingCartAPI.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20230825033452_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.6.23329.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ECOM.Services.ShoppingCartAPI.Models.CartDetail", b =>
                {
                    b.Property<int>("CartDetailID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartDetailID"));

                    b.Property<int>("CartHeaderID")
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.HasKey("CartDetailID");

                    b.HasIndex("CartHeaderID");

                    b.ToTable("CartDetails");
                });

            modelBuilder.Entity("ECOM.Services.ShoppingCartAPI.Models.CartHeader", b =>
                {
                    b.Property<int>("CartHeaderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartHeaderID"));

                    b.Property<string>("CouponCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CartHeaderID");

                    b.ToTable("CartHeaders");
                });

            modelBuilder.Entity("ECOM.Services.ShoppingCartAPI.Models.CartDetail", b =>
                {
                    b.HasOne("ECOM.Services.ShoppingCartAPI.Models.CartHeader", "CartHeader")
                        .WithMany()
                        .HasForeignKey("CartHeaderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CartHeader");
                });
#pragma warning restore 612, 618
        }
    }
}