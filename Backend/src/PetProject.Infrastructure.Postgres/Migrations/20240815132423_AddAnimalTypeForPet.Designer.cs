﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetProject.Infrastructure.Postgres;

#nullable disable

namespace PetProject.Infrastructure.Postgres.Migrations
{
    [DbContext(typeof(PetProjectDbContext))]
    [Migration("20240815132423_AddAnimalTypeForPet")]
    partial class AddAnimalTypeForPet
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PetProject.Domain.Entities.Breed", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<Guid?>("SpeciesId")
                        .HasColumnType("uuid")
                        .HasColumnName("species_id");

                    b.HasKey("Id")
                        .HasName("pk_breeds");

                    b.HasIndex("SpeciesId")
                        .HasDatabaseName("ix_breeds_species_id");

                    b.ToTable("breeds", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Entities.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("BirthDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("birth_date");

                    b.Property<string>("BreedName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("breed_name");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("color");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("description");

                    b.Property<string>("HealthInfo")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("health_info");

                    b.Property<double>("Height")
                        .HasColumnType("double precision")
                        .HasColumnName("height");

                    b.Property<int>("HelpStatus")
                        .HasColumnType("integer")
                        .HasColumnName("help_status");

                    b.Property<bool>("IsCastrated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_castrated");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vaccinated");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision")
                        .HasColumnName("weight");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "PetProject.Domain.Entities.Pet.Address#Adress", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("city");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("country");

                            b1.Property<string>("Flat")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("flat");

                            b1.Property<string>("House")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("house");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("street");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("AnimalType", "PetProject.Domain.Entities.Pet.AnimalType#AnimalType", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("BreedId")
                                .HasColumnType("uuid")
                                .HasColumnName("breed_id");

                            b1.Property<Guid>("SpeciesId")
                                .HasColumnType("uuid")
                                .HasColumnName("species_id");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("OwnerPhoneNumber", "PetProject.Domain.Entities.Pet.OwnerPhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(11)
                                .HasColumnType("character varying(11)")
                                .HasColumnName("phone_number");
                        });

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.ToTable("pets", null, t =>
                        {
                            t.HasCheckConstraint("ck_pet_height", "\"height\" > 0");

                            t.HasCheckConstraint("ck_pet_weight", "\"weight\" > 0");
                        });
                });

            modelBuilder.Entity("PetProject.Domain.Entities.PetPhoto", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsMain")
                        .HasColumnType("boolean")
                        .HasColumnName("is_main");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("path");

                    b.Property<Guid?>("PetId")
                        .HasColumnType("uuid")
                        .HasColumnName("pet_id");

                    b.HasKey("Id")
                        .HasName("pk_pet_photos");

                    b.HasIndex("PetId")
                        .HasDatabaseName("ix_pet_photos_pet_id");

                    b.ToTable("pet_photos", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Entities.Species", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_species");

                    b.ToTable("species", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Entities.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("description");

                    b.Property<int>("Experience")
                        .HasColumnType("integer")
                        .HasColumnName("experience");

                    b.Property<int>("PetsAdopted")
                        .HasColumnType("integer")
                        .HasColumnName("pets_adopted");

                    b.Property<int>("PetsFoundHomeQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("pets_found_home_quantity");

                    b.Property<int>("PetsInTreatment")
                        .HasColumnType("integer")
                        .HasColumnName("pets_in_treatment");

                    b.ComplexProperty<Dictionary<string, object>>("FullName", "PetProject.Domain.Entities.Volunteer.FullName#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("first_name");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("last_name");

                            b1.Property<string>("Patronymic")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("patronymic");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "PetProject.Domain.Entities.Volunteer.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(11)
                                .HasColumnType("character varying(11)")
                                .HasColumnName("phone_number");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("volunteers", null, t =>
                        {
                            t.HasCheckConstraint("ck_volunteer_experience", "\"experience\" >= 0");

                            t.HasCheckConstraint("ck_volunteer_pets_adopted", "\"pets_adopted\" >= 0");

                            t.HasCheckConstraint("ck_volunteer_pets_found_home_quantity", "\"pets_found_home_quantity\" >= 0");

                            t.HasCheckConstraint("ck_volunteer_pets_in_treatment", "\"pets_in_treatment\" >= 0");
                        });
                });

            modelBuilder.Entity("PetProject.Domain.Entities.Breed", b =>
                {
                    b.HasOne("PetProject.Domain.Entities.Species", null)
                        .WithMany("Breeds")
                        .HasForeignKey("SpeciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_breeds_species_species_id");
                });

            modelBuilder.Entity("PetProject.Domain.Entities.Pet", b =>
                {
                    b.OwnsMany("PetProject.Domain.Entities.ValueObjects.Requisite", "Requisites", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            b1.HasKey("PetId", "Id")
                                .HasName("pk_pets");

                            b1.ToTable("pets");

                            b1.ToJson("requisites");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_pet_id");
                        });

                    b.Navigation("Requisites");
                });

            modelBuilder.Entity("PetProject.Domain.Entities.PetPhoto", b =>
                {
                    b.HasOne("PetProject.Domain.Entities.Pet", null)
                        .WithMany("Photos")
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_pet_photos_pets_pet_id");
                });

            modelBuilder.Entity("PetProject.Domain.Entities.Volunteer", b =>
                {
                    b.OwnsMany("PetProject.Domain.Entities.ValueObjects.SocialNetwork", "SocialNetworks", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            b1.HasKey("VolunteerId", "Id");

                            b1.ToTable("volunteers");

                            b1.ToJson("social_networks");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_volunteer_id");
                        });

                    b.OwnsMany("PetProject.Domain.Entities.ValueObjects.Requisite", "Requisites", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            b1.HasKey("VolunteerId", "Id");

                            b1.ToTable("volunteers");

                            b1.ToJson("requisites");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_volunteer_id");
                        });

                    b.Navigation("Requisites");

                    b.Navigation("SocialNetworks");
                });

            modelBuilder.Entity("PetProject.Domain.Entities.Pet", b =>
                {
                    b.Navigation("Photos");
                });

            modelBuilder.Entity("PetProject.Domain.Entities.Species", b =>
                {
                    b.Navigation("Breeds");
                });
#pragma warning restore 612, 618
        }
    }
}
