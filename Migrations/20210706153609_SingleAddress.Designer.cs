﻿// <auto-generated />
using System;
using IPNetworkEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IPNetworkEFCore.Migrations
{
    [DbContext(typeof(IPNetworkTestDBContext))]
    [Migration("20210706153609_SingleAddress")]
    partial class SingleAddress
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IPNetworkEFCore.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<byte[]>("_ipbytes")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varbinary(16)")
                        .HasColumnName("IP");

                    b.HasKey("Id");

                    b.HasIndex("_ipbytes")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("IPNetworkEFCore.Network", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("PrefixLength")
                        .HasColumnType("int");

                    b.Property<byte[]>("_last")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varbinary(16)")
                        .HasColumnName("Last");

                    b.Property<byte[]>("_prefixbytes")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varbinary(16)")
                        .HasColumnName("Prefix");

                    b.HasKey("Id");

                    b.HasIndex("_prefixbytes", "_last")
                        .IsUnique();

                    b.ToTable("Networks");
                });
#pragma warning restore 612, 618
        }
    }
}