﻿// <auto-generated />
using System;
using GossipCheck.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GossipCheck.DAO.Migrations
{
    [DbContext(typeof(GossipCheckDBContext))]
    [Migration("20210607162352_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GossipCheck.DAO.Entities.MbfcReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BiasRating")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("FactualReporting")
                        .HasColumnType("int");

                    b.Property<int>("MbfcCredibilityRating")
                        .HasColumnType("int");

                    b.Property<int>("MediaType")
                        .HasColumnType("int");

                    b.Property<string>("PageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reasoning")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TrafficPopularity")
                        .HasColumnType("int");

                    b.Property<int?>("WorldPressFreedomRank")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("MbfcReports");
                });
#pragma warning restore 612, 618
        }
    }
}