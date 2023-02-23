using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Labb_2_databaser_Saraebba.DataAccess;

public partial class BokhandelContext : DbContext
{
    public BokhandelContext()
    {
    }

    public BokhandelContext(DbContextOptions<BokhandelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Butiker> Butikers { get; set; }

    public virtual DbSet<Böcker> Böckers { get; set; }

    public virtual DbSet<Författare> Författares { get; set; }

    public virtual DbSet<Förlag> Förlags { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Kunder> Kunders { get; set; }

    public virtual DbSet<LagerSaldo> LagerSaldos { get; set; }

    public virtual DbSet<OrderDetaljer> OrderDetaljers { get; set; }

    public virtual DbSet<Ordrar> Ordrars { get; set; }

    public virtual DbSet<VKundOrdrar> VKundOrdrars { get; set; }

    public virtual DbSet<VTitlarPerFörfattare> VTitlarPerFörfattares { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=BOKHANDEL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Butiker>(entity =>
        {
            entity.HasKey(e => e.ButikId).HasName("PK_ButikId");

            entity.ToTable("Butiker");

            entity.HasIndex(e => e.Telefon, "UQ__Butiker__92EB41699364E76F").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Butiker__A9D1053418D006D3").IsUnique();

            entity.Property(e => e.Adress).HasMaxLength(200);
            entity.Property(e => e.Butiksnamn).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Stad).HasMaxLength(50);
        });

        modelBuilder.Entity<Böcker>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PK_ISBN");

            entity.ToTable("Böcker");

            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN");
            entity.Property(e => e.Språk).HasMaxLength(50);
            entity.Property(e => e.Titel).HasMaxLength(80);
            entity.Property(e => e.Utgivningsdatum).HasColumnType("date");

            entity.HasOne(d => d.Förlag).WithMany(p => p.Böckers)
                .HasForeignKey(d => d.FörlagId)
                .HasConstraintName("FK_FörlagId_Förlag");
        });

        modelBuilder.Entity<Författare>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Pk_Id");

            entity.ToTable("Författare");

            entity.Property(e => e.Efternamn).HasMaxLength(150);
            entity.Property(e => e.Födelsedatum).HasColumnType("date");
            entity.Property(e => e.Förnamn).HasMaxLength(100);

            entity.HasMany(d => d.BokIsbns).WithMany(p => p.Författares)
                .UsingEntity<Dictionary<string, object>>(
                    "FörfattareTillBöcker",
                    r => r.HasOne<Böcker>().WithMany()
                        .HasForeignKey("BokIsbn")
                        .HasConstraintName("FK_BokISBN_Böcker"),
                    l => l.HasOne<Författare>().WithMany()
                        .HasForeignKey("FörfattareId")
                        .HasConstraintName("FK_FörfattareId_Författare"),
                    j =>
                    {
                        j.HasKey("FörfattareId", "BokIsbn").HasName("PK_BöckertillFörfattare");
                        j.ToTable("FörfattareTillBöcker");
                    });
        });

        modelBuilder.Entity<Förlag>(entity =>
        {
            entity.HasKey(e => e.FörlagId).HasName("Pk_FörlagId");

            entity.ToTable("Förlag");

            entity.Property(e => e.Namn).HasMaxLength(50);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("Pk_GenreId");

            entity.Property(e => e.GenreNamn).HasMaxLength(30);

            entity.HasMany(d => d.Isbns).WithMany(p => p.Genres)
                .UsingEntity<Dictionary<string, object>>(
                    "GenresTillBöcker",
                    r => r.HasOne<Böcker>().WithMany()
                        .HasForeignKey("Isbn")
                        .HasConstraintName("Fk_ISBN_Böcker1"),
                    l => l.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .HasConstraintName("Fk_GenreId_Genres"),
                    j =>
                    {
                        j.HasKey("GenreId", "Isbn").HasName("Pk_GenresTillBöcker");
                        j.ToTable("GenresTillBöcker");
                    });
        });

        modelBuilder.Entity<Kunder>(entity =>
        {
            entity.HasKey(e => e.KundId).HasName("Pk_KundId");

            entity.ToTable("Kunder");

            entity.HasIndex(e => e.Email, "UQ__Kunder__A9D10534467222A4").IsUnique();

            entity.Property(e => e.Adress).HasMaxLength(200);
            entity.Property(e => e.Efternamn)
                .HasMaxLength(30)
                .HasColumnName("EFternamn");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Förnamn).HasMaxLength(20);
            entity.Property(e => e.Stad).HasMaxLength(50);
        });

        modelBuilder.Entity<LagerSaldo>(entity =>
        {
            entity.HasKey(e => new { e.ButikId, e.Isbn });

            entity.ToTable("LagerSaldo");

            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN");

            entity.HasOne(d => d.Butik).WithMany(p => p.LagerSaldos)
                .HasForeignKey(d => d.ButikId)
                .HasConstraintName("FK_ButikId_Butiker");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.LagerSaldos)
                .HasForeignKey(d => d.Isbn)
                .HasConstraintName("FK_ISBN_Böcker");
        });

        modelBuilder.Entity<OrderDetaljer>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.Isbn }).HasName("Pk_OrderDetaljer");

            entity.ToTable("OrderDetaljer");

            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.OrderDetaljers)
                .HasForeignKey(d => d.Isbn)
                .HasConstraintName("Fk_ISBN_Böcker2");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetaljers)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("Fk_OrderId_Ordrar");
        });

        modelBuilder.Entity<Ordrar>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("Pk_OrderId");

            entity.ToTable("Ordrar");

            entity.Property(e => e.Leveransadress).HasMaxLength(250);
            entity.Property(e => e.OrderDatum).HasColumnType("date");

            entity.HasOne(d => d.Kund).WithMany(p => p.Ordrars)
                .HasForeignKey(d => d.KundId)
                .HasConstraintName("Fk_KundId_Kunder");
        });

        modelBuilder.Entity<VKundOrdrar>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vKundOrdrar");

            entity.Property(e => e.Kund).HasMaxLength(51);
            entity.Property(e => e.OrderDatum).HasColumnType("date");
            entity.Property(e => e.OrderNummer).HasColumnName("Order nummer");
            entity.Property(e => e.OrderTotal)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Order Total");
        });

        modelBuilder.Entity<VTitlarPerFörfattare>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vTitlarPerFörfattare");

            entity.Property(e => e.Lagervärde)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Namn).HasMaxLength(251);
            entity.Property(e => e.Titlar)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Ålder)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
