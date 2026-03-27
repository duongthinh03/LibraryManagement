using System;
using System.Collections.Generic;
using Library.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Persistence;

public partial class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BorrowRecord> BorrowRecords { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Copy> Copies { get; set; }

    public virtual DbSet<CopyTransaction> CopyTransactions { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentImage> DocumentImages { get; set; }

    public virtual DbSet<Fine> Fines { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    public virtual DbSet<ReaderPolicy> ReaderPolicies { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Thesis> Theses { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuditLog__3214EC078744E367");

            entity.Property(e => e.Action).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.EntityName).HasMaxLength(255);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC07C0BB3114");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC07BA5B8CFC");

            entity.HasIndex(e => e.DocumentId, "UQ__Books__1ABEEF0E6A346FC4").IsUnique();

            entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EA2A2F0BA8").IsUnique();

            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("ISBN");

            entity.HasOne(d => d.Document).WithOne(p => p.Book)
                .HasForeignKey<Book>(d => d.DocumentId)
                .HasConstraintName("FK_Books_Documents");
        });

        modelBuilder.Entity<BorrowRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BorrowRe__3214EC07D80F67AA");

            entity.HasIndex(e => e.CopyId, "IX_BorrowRecords_CopyId");

            entity.HasIndex(e => e.ReaderId, "IX_BorrowRecords_ReaderId");

            entity.Property(e => e.BorrowDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Copy).WithMany(p => p.BorrowRecords)
                .HasForeignKey(d => d.CopyId)
                .HasConstraintName("FK__BorrowRec__CopyI__72C60C4A");

            entity.HasOne(d => d.Reader).WithMany(p => p.BorrowRecords)
                .HasForeignKey(d => d.ReaderId)
                .HasConstraintName("FK__BorrowRec__Reade__71D1E811");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07E8592331");

            entity.Property(e => e.CategoryName).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<Copy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Copies__3214EC0734155860");

            entity.HasIndex(e => e.DocumentId, "IX_Copies_DocumentId");

            entity.HasIndex(e => e.CopyCode, "UQ__Copies__900F33262A083F9B").IsUnique();

            entity.Property(e => e.Condition).HasMaxLength(255);
            entity.Property(e => e.CopyCode).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Document).WithMany(p => p.Copies)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK__Copies__Document__693CA210");

            entity.HasOne(d => d.Location).WithMany(p => p.Copies)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Copies__Location__6A30C649");
        });

        modelBuilder.Entity<CopyTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CopyTran__3214EC07B4282445");

            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(500);

            entity.HasOne(d => d.Copy).WithMany(p => p.CopyTransactions)
                .HasForeignKey(d => d.CopyId)
                .HasConstraintName("FK__CopyTrans__CopyI__6E01572D");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC0729520A05");

            entity.HasIndex(e => e.CategoryId, "IX_Documents_CategoryId");

            entity.HasIndex(e => e.DocumentCode, "UQ__Document__2282345B7FA7EA2C").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.DocumentCode).HasMaxLength(50);
            entity.Property(e => e.DocumentType).HasMaxLength(50);
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.Publisher).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Documents)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Documents_Categories");

            entity.HasMany(d => d.Authors).WithMany(p => p.Documents)
                .UsingEntity<Dictionary<string, object>>(
                    "DocumentAuthor",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__DocumentA__Autho__619B8048"),
                    l => l.HasOne<Document>().WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__DocumentA__Docum__60A75C0F"),
                    j =>
                    {
                        j.HasKey("DocumentId", "AuthorId").HasName("PK__Document__4DB340CC7C1E70DB");
                        j.ToTable("DocumentAuthors");
                    });
        });

        modelBuilder.Entity<DocumentImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC07A9937E48");

            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.IsMain).HasDefaultValue(false);

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentImages)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK__DocumentI__Docum__7E37BEF6");
        });

        modelBuilder.Entity<Fine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Fines__3214EC0744F8FC7D");

            entity.HasIndex(e => e.BorrowRecordId, "IX_Fines_BorrowRecordId");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.BorrowRecord).WithMany(p => p.Fines)
                .HasForeignKey(d => d.BorrowRecordId)
                .HasConstraintName("FK__Fines__BorrowRec__778AC167");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC070B277342");

            entity.HasIndex(e => e.LocationCode, "UQ__Location__DDB144D5D83B5EFA").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.LocationCode).HasMaxLength(50);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07A81234D5");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Message).HasMaxLength(500);

            entity.HasOne(d => d.Reader).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ReaderId)
                .HasConstraintName("FK__Notificat__Reade__123EB7A3");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC07C8740D56");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Method).HasMaxLength(50);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Fine).WithMany(p => p.Payments)
                .HasForeignKey(d => d.FineId)
                .HasConstraintName("FK__Payments__FineId__7A672E12");
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Readers__3214EC073FC79AAF");

            entity.HasIndex(e => e.ReaderCode, "UQ__Readers__794D8D06B2957707").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.ReaderCode).HasMaxLength(50);
            entity.Property(e => e.ReaderType).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<ReaderPolicy>(entity =>
        {
            entity.HasKey(e => e.ReaderType).HasName("PK__ReaderPo__B5D76C7A2669C279");

            entity.Property(e => e.ReaderType).HasMaxLength(50);
            entity.Property(e => e.FinePerDay).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservat__3214EC07BEEFDDDF");

            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.ReservationDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Copy).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.CopyId)
                .HasConstraintName("FK__Reservati__CopyI__0A9D95DB");

            entity.HasOne(d => d.Document).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK__Reservati__Docum__09A971A2");

            entity.HasOne(d => d.Reader).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.ReaderId)
                .HasConstraintName("FK__Reservati__Reade__08B54D69");
        });

        modelBuilder.Entity<Thesis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Theses__3214EC070A73E41E");

            entity.HasIndex(e => e.DocumentId, "UQ__Theses__1ABEEF0EB6037DDC").IsUnique();

            entity.Property(e => e.StudentAuthor).HasMaxLength(255);
            entity.Property(e => e.Supervisor).HasMaxLength(255);
            entity.Property(e => e.ThesisType).HasMaxLength(100);

            entity.HasOne(d => d.Document).WithOne(p => p.Thesis)
                .HasForeignKey<Thesis>(d => d.DocumentId)
                .HasConstraintName("FK_Theses_Documents");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAcco__3214EC07CE11244F");

            entity.HasIndex(e => e.Username, "UQ__UserAcco__536C85E41BEF0A77").IsUnique();

            entity.HasIndex(e => e.ReaderId, "UQ__UserAcco__8E67A5E0B460A64B").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(100);
            entity.Property(e => e.VerifyToken).HasMaxLength(200);
            entity.Property(e => e.VerifyTokenExpiredAt).HasColumnType("datetime");

            entity.HasOne(d => d.Reader).WithOne(p => p.UserAccount)
                .HasForeignKey<UserAccount>(d => d.ReaderId)
                .HasConstraintName("FK__UserAccou__Reade__03F0984C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
