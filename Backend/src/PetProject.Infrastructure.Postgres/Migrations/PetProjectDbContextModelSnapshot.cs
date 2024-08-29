﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetProject.Infrastructure.Postgres;

#nullable disable

namespace PetProject.Infrastructure.Postgres.Migrations
{
    [DbContext(typeof(PetProjectDbContext))]
    partial class PetProjectDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PetProject.Domain.Species.Breed", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("SpeciesId")
                        .HasColumnType("uuid")
                        .HasColumnName("species_id");

                    b.ComplexProperty<Dictionary<string, object>>("BreedName", "PetProject.Domain.Species.Breed.BreedName#BreedName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("breed_name");
                        });

                    b.HasKey("Id")
                        .HasName("pk_breeds");

                    b.HasIndex("SpeciesId")
                        .HasDatabaseName("ix_breeds_species_id");

                    b.ToTable("breeds", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Species.Species", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.ComplexProperty<Dictionary<string, object>>("Name", "PetProject.Domain.Species.Species.Name#SpeciesName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("species_name");
                        });

                    b.HasKey("Id")
                        .HasName("pk_species");

                    b.ToTable("species", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.VolunteerManagement.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date")
                        .HasColumnName("birth_date");

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<int>("HelpStatus")
                        .HasColumnType("integer")
                        .HasColumnName("help_status");

                    b.Property<bool>("IsCastrated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_castrated");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vaccinated");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<Guid?>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "PetProject.Domain.VolunteerManagement.Pet.Address#Address", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("city");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("country");

                            b1.Property<string>("Flat")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("flat");

                            b1.Property<string>("House")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("house");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("street");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("AnimalType", "PetProject.Domain.VolunteerManagement.Pet.AnimalType#AnimalType", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("BreedId")
                                .HasColumnType("uuid")
                                .HasColumnName("breed_id");

                            b1.Property<Guid>("SpeciesId")
                                .HasColumnType("uuid")
                                .HasColumnName("species_id");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("GeneralDescription", "PetProject.Domain.VolunteerManagement.Pet.GeneralDescription#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("general_description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("HealthInformation", "PetProject.Domain.VolunteerManagement.Pet.HealthInformation#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("health_information");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PetName", "PetProject.Domain.VolunteerManagement.Pet.PetName#PetName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("pet_name");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "PetProject.Domain.VolunteerManagement.Pet.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("phone_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhysicalAttributes", "PetProject.Domain.VolunteerManagement.Pet.PhysicalAttributes#PetPhysicalAttributes", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<double>("Height")
                                .HasColumnType("double precision")
                                .HasColumnName("height");

                            b1.Property<double>("Weight")
                                .HasColumnType("double precision")
                                .HasColumnName("weight");
                        });

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_pets_volunteer_id");

                    b.ToTable("pets", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.VolunteerManagement.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.ComplexProperty<Dictionary<string, object>>("AgeExperience", "PetProject.Domain.VolunteerManagement.Volunteer.AgeExperience#AgeExperience", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Years")
                                .HasColumnType("integer")
                                .HasColumnName("age_experience");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("FullName", "PetProject.Domain.VolunteerManagement.Volunteer.FullName#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Name")
                                .IsRequired()
                                .ValueGeneratedOnUpdateSometimes()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("name");

                            b1.Property<string>("Patronymic")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("patronymic");

                            b1.Property<string>("Surname")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("surname");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("GeneralDescription", "PetProject.Domain.VolunteerManagement.Volunteer.GeneralDescription#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(500)
                                .HasColumnType("character varying(500)")
                                .HasColumnName("general_description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "PetProject.Domain.VolunteerManagement.Volunteer.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("phone_number");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("volunteers", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Species.Breed", b =>
                {
                    b.HasOne("PetProject.Domain.Species.Species", null)
                        .WithMany("Breeds")
                        .HasForeignKey("SpeciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_breeds_species_species_id");
                });

            modelBuilder.Entity("PetProject.Domain.VolunteerManagement.Pet", b =>
                {
                    b.HasOne("PetProject.Domain.VolunteerManagement.Volunteer", null)
                        .WithMany("Pets")
                        .HasForeignKey("volunteer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_pets_volunteers_volunteer_id");

                    b.OwnsOne("PetProject.Domain.VolunteerManagement.RequisitesList", "RequisitesList", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.HasKey("PetId")
                                .HasName("pk_pets");

                            b1.ToTable("pets");

                            b1.ToJson("requisites");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_pet_id");

                            b1.OwnsMany("PetProject.Domain.VolunteerManagement.Requisite", "Requisites", b2 =>
                                {
                                    b2.Property<Guid>("RequisitesListPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Description")
                                        .IsRequired()
                                        .HasMaxLength(2000)
                                        .HasColumnType("character varying(2000)")
                                        .HasColumnName("description");

                                    b2.Property<string>("Title")
                                        .IsRequired()
                                        .HasMaxLength(50)
                                        .HasColumnType("character varying(50)")
                                        .HasColumnName("name");

                                    b2.HasKey("RequisitesListPetId", "Id");

                                    b2.ToTable("pets");

                                    b2.ToJson("requisites");

                                    b2.WithOwner()
                                        .HasForeignKey("RequisitesListPetId")
                                        .HasConstraintName("fk_pets_pets_requisites_list_pet_id");
                                });

                            b1.Navigation("Requisites");
                        });

                    b.OwnsOne("PetProject.Domain.VolunteerManagement.PetPhotosList", "PetPhotosList", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.ToJson("pet_photos");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");

                            b1.OwnsMany("PetProject.Domain.VolunteerManagement.PetPhoto", "PetPhotos", b2 =>
                                {
                                    b2.Property<Guid>("PetPhotosListPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<bool>("IsMain")
                                        .HasColumnType("boolean")
                                        .HasColumnName("is_main");

                                    b2.Property<string>("Path")
                                        .IsRequired()
                                        .HasMaxLength(500)
                                        .HasColumnType("character varying(500)")
                                        .HasColumnName("path");

                                    b2.HasKey("PetPhotosListPetId", "Id");

                                    b2.ToTable("pets");

                                    b2.ToJson("pet_photos");

                                    b2.WithOwner()
                                        .HasForeignKey("PetPhotosListPetId")
                                        .HasConstraintName("fk_pets_pets_pet_photos_list_pet_id");
                                });

                            b1.Navigation("PetPhotos");
                        });

                    b.Navigation("PetPhotosList")
                        .IsRequired();

                    b.Navigation("RequisitesList")
                        .IsRequired();
                });

            modelBuilder.Entity("PetProject.Domain.VolunteerManagement.Volunteer", b =>
                {
                    b.OwnsOne("PetProject.Domain.VolunteerManagement.SocialLinksList", "SocialLinksList", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers");

                            b1.ToJson("social_links");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("PetProject.Domain.VolunteerManagement.SocialLink", "SocialLinks", b2 =>
                                {
                                    b2.Property<Guid>("SocialLinksListVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Title")
                                        .IsRequired()
                                        .ValueGeneratedOnUpdateSometimes()
                                        .HasMaxLength(50)
                                        .HasColumnType("character varying(50)")
                                        .HasColumnName("name");

                                    b2.Property<string>("Url")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasColumnName("url");

                                    b2.HasKey("SocialLinksListVolunteerId", "Id");

                                    b2.ToTable("volunteers");

                                    b2.ToJson("social_links");

                                    b2.WithOwner()
                                        .HasForeignKey("SocialLinksListVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_social_links_list_volunteer_id");
                                });

                            b1.Navigation("SocialLinks");
                        });

                    b.OwnsOne("PetProject.Domain.VolunteerManagement.RequisitesList", "RequisitesList", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers");

                            b1.ToJson("requisites");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("PetProject.Domain.VolunteerManagement.Requisite", "Requisites", b2 =>
                                {
                                    b2.Property<Guid>("RequisitesListVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Description")
                                        .IsRequired()
                                        .HasMaxLength(2000)
                                        .HasColumnType("character varying(2000)")
                                        .HasColumnName("description");

                                    b2.Property<string>("Title")
                                        .IsRequired()
                                        .ValueGeneratedOnUpdateSometimes()
                                        .HasMaxLength(50)
                                        .HasColumnType("character varying(50)")
                                        .HasColumnName("name");

                                    b2.HasKey("RequisitesListVolunteerId", "Id");

                                    b2.ToTable("volunteers");

                                    b2.ToJson("requisites");

                                    b2.WithOwner()
                                        .HasForeignKey("RequisitesListVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_requisites_list_volunteer_id");
                                });

                            b1.Navigation("Requisites");
                        });

                    b.Navigation("RequisitesList")
                        .IsRequired();

                    b.Navigation("SocialLinksList")
                        .IsRequired();
                });

            modelBuilder.Entity("PetProject.Domain.Species.Species", b =>
                {
                    b.Navigation("Breeds");
                });

            modelBuilder.Entity("PetProject.Domain.VolunteerManagement.Volunteer", b =>
                {
                    b.Navigation("Pets");
                });
#pragma warning restore 612, 618
        }
    }
}
