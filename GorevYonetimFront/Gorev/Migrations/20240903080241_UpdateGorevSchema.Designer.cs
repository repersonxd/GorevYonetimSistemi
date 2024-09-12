﻿// <auto-generated />
using System;
using GorevY.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GorevY.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240903080241_UpdateGorevSchema")]
    partial class UpdateGorevSchema
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GorevY.Models.Gorev", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Aciklama")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GorevAdi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GorevDurumuId")
                        .HasColumnType("int");

                    b.Property<int>("KategoriId")
                        .HasColumnType("int");

                    b.Property<int>("KullaniciId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OlusturulmaTarihi")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Tamamlandi")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("TamamlanmaTarihi")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GorevDurumuId");

                    b.HasIndex("KategoriId");

                    b.HasIndex("KullaniciId");

                    b.ToTable("Gorevler", (string)null);
                });

            modelBuilder.Entity("GorevY.Models.GorevDurumu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GorevDurumu");
                });

            modelBuilder.Entity("GorevY.Models.Kategori", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Kategori");
                });

            modelBuilder.Entity("GorevY.Models.Kullanici", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KullaniciAdi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sifre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Kullanicilar", (string)null);
                });

            modelBuilder.Entity("RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens", (string)null);
                });

            modelBuilder.Entity("GorevY.Models.Gorev", b =>
                {
                    b.HasOne("GorevY.Models.GorevDurumu", "GorevDurumu")
                        .WithMany("Gorevler")
                        .HasForeignKey("GorevDurumuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GorevY.Models.Kategori", "Kategori")
                        .WithMany("Gorevler")
                        .HasForeignKey("KategoriId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GorevY.Models.Kullanici", "Kullanici")
                        .WithMany("Gorevler")
                        .HasForeignKey("KullaniciId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GorevDurumu");

                    b.Navigation("Kategori");

                    b.Navigation("Kullanici");
                });

            modelBuilder.Entity("RefreshToken", b =>
                {
                    b.HasOne("GorevY.Models.Kullanici", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GorevY.Models.GorevDurumu", b =>
                {
                    b.Navigation("Gorevler");
                });

            modelBuilder.Entity("GorevY.Models.Kategori", b =>
                {
                    b.Navigation("Gorevler");
                });

            modelBuilder.Entity("GorevY.Models.Kullanici", b =>
                {
                    b.Navigation("Gorevler");
                });
#pragma warning restore 612, 618
        }
    }
}
