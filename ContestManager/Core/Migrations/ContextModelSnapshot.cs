﻿// <auto-generated />
using System;
using Core.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Core.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Core.DataBaseEntities.AuthenticationAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("ServiceId")
                        .HasMaxLength(100);

                    b.Property<string>("ServiceToken");

                    b.Property<int>("Type");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Type", "ServiceId")
                        .IsUnique();

                    b.ToTable("AuthenticationAccounts");
                });

            modelBuilder.Entity("Core.DataBaseEntities.Contest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("Options");

                    b.Property<Guid>("OwnerId");

                    b.Property<string>("SerializedFields");

                    b.Property<int>("State");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Contests");
                });

            modelBuilder.Entity("Core.DataBaseEntities.EmailConfirmationRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AccountId");

                    b.Property<string>("ConfirmationCode")
                        .HasMaxLength(30);

                    b.Property<string>("Email")
                        .HasMaxLength(100);

                    b.Property<bool>("IsUsed");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("Type", "Email", "ConfirmationCode")
                        .IsUnique();

                    b.ToTable("EmailConfirmationRequests");
                });

            modelBuilder.Entity("Core.DataBaseEntities.News", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ContestId");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("MdContent");

                    b.HasKey("Id");

                    b.HasIndex("ContestId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("Core.DataBaseEntities.Participant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ContestId");

                    b.Property<string>("SerializedResults");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("Core.DataBaseEntities.StoredConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("JsonValue");

                    b.Property<string>("TypeName");

                    b.HasKey("Id");

                    b.ToTable("StoredConfigs");
                });

            modelBuilder.Entity("Core.DataBaseEntities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Class");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<int>("Role");

                    b.Property<string>("School");

                    b.Property<int>("Sex");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Core.DataBaseEntities.News", b =>
                {
                    b.HasOne("Core.DataBaseEntities.Contest", "Contest")
                        .WithMany()
                        .HasForeignKey("ContestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
