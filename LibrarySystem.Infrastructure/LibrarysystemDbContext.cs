using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure;

public partial class LibrarysystemDbContext : IdentityDbContext<AppUser>
{
    public LibrarysystemDbContext(DbContextOptions<LibrarysystemDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<Borrow> Borrows { get; set; }
    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Process> Processes { get; set; }
    public virtual DbSet<Request> Requests { get; set; }
    public virtual DbSet<Workflow> Workflows { get; set; }
    public virtual DbSet<WorkflowSequence> WorkflowSequences { get; set; }
    public virtual DbSet<WorkflowAction> WorkflowActions { get; set; }
    public virtual DbSet<NextStepRules> NextStepRules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=PostgreSQLConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.IdBook).HasName("book_pkey");

            entity.ToTable("book");

            entity.HasIndex(e => e.Isbn, "book_isbn_key").IsUnique();

            entity.Property(e => e.IdBook).HasColumnName("id_book");
            entity.Property(e => e.Author)
                .HasMaxLength(255)
                .HasColumnName("author");
            entity.Property(e => e.Availablebook)
                .HasDefaultValue(0)
                .HasColumnName("availablebook");
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .HasColumnName("category");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("isbn");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.Publisher)
                .HasMaxLength(255)
                .HasColumnName("publisher");
            entity.Property(e => e.Purchasedate).HasColumnName("purchasedate");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Language)
                .HasMaxLength(100)
                .HasColumnName("language");
        });

        modelBuilder.Entity<Borrow>(entity =>
        {
            entity.HasKey(e => e.IdBorrow).HasName("borrow_pkey");

            entity.ToTable("borrow");

            entity.Property(e => e.IdBorrow).HasColumnName("id_borrow");
            entity.Property(e => e.DateBorrow).HasColumnName("date_borrow");
            entity.Property(e => e.DateReturn).HasColumnName("date_return");
            entity.Property(e => e.IdBook).HasColumnName("id_book");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Penalty)
                .HasPrecision(10, 2)
                .HasColumnName("penalty");

            entity.HasOne(d => d.IdBookNavigation).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.IdBook)
                .HasConstraintName("borrow_id_book_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("borrow_id_user_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "user_email_key").IsUnique();

            entity.HasIndex(e => e.Librarycard, "user_librarycard_key").IsUnique();

            entity.Property(e => e.IdUser)
            .HasColumnName("id_user")
            .ValueGeneratedOnAdd();
            entity.Property(e => e.Cardexp).HasColumnName("cardexp");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fname)
                .HasMaxLength(100)
                .HasColumnName("fname");
            entity.Property(e => e.Librarycard)
                .HasMaxLength(50)
                .HasColumnName("librarycard");
            entity.Property(e => e.Lname)
                .HasMaxLength(100)
                .HasColumnName("lname");
            entity.Property(e => e.Notreturnbook)
                .HasDefaultValue(0)
                .HasColumnName("notreturnbook");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Position)
                .HasMaxLength(50) 
                .HasColumnName("position");
            entity.Property(e => e.Penalty)
                .HasMaxLength(50)
                .HasColumnName("penalty");
        });

        modelBuilder.Entity<Process>(entity =>
        {
            entity.HasKey(e => e.Id_process).HasName("process_pkey");

            entity.ToTable("process");

            entity.Property(e => e.Id_process).HasColumnName("id_process");
            entity.Property(e => e.processname).HasMaxLength(255).HasColumnName("processname");
            entity.Property(e => e.description).HasColumnName("description");
            entity.Property(e => e.startdate).HasColumnName("startdate");
            entity.Property(e => e.enddate).HasColumnName("enddate");
        });

        // Konfigurasi untuk entitas Request
        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id_request).HasName("request_pkey");

            entity.ToTable("request");

            entity.Property(e => e.Id_request).HasColumnName("id_request");
            entity.Property(e => e.Id_workflow).HasColumnName("id_workflow");
            entity.Property(e => e.Id_requester).HasMaxLength(255).HasColumnName("id_requester");
            entity.Property(e => e.requesttype).HasMaxLength(100).HasColumnName("requesttype");
            entity.Property(e => e.status).HasMaxLength(50).HasColumnName("status");
            entity.Property(e => e.currentstepId).HasColumnName("currentstepid");
            entity.Property(e => e.requestdate).HasColumnName("requestdate");
            entity.Property(e => e.Id_process).HasColumnName("id_process");

            entity.HasOne(d => d.Process).WithMany(p => p.Requests)
                .HasForeignKey(d => d.Id_process)
                .HasConstraintName("request_id_process_fkey");

            entity.HasOne(d => d.Workflow).WithMany(p => p.Requests)
                .HasForeignKey(d => d.Id_workflow)
                .HasConstraintName("request_id_workflow_fkey");
        });

        // Konfigurasi untuk entitas Workflow
        modelBuilder.Entity<Workflow>(entity =>
        {
            entity.HasKey(e => e.Id_workflow).HasName("workflow_pkey");

            entity.ToTable("workflow");

            entity.Property(e => e.Id_workflow).HasColumnName("id_workflow");
            entity.Property(e => e.workflowname).HasMaxLength(255).HasColumnName("workflowname");
            entity.Property(e => e.description).HasColumnName("description");
        });

        // Konfigurasi untuk entitas WorkflowSequence
        modelBuilder.Entity<WorkflowSequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("workflow_sequence_pkey");

            entity.ToTable("workflow_sequence");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Id_workflow).HasColumnName("id_workflow");
            entity.Property(e => e.steporder).HasColumnName("steporder");
            entity.Property(e => e.stepname).HasMaxLength(255).HasColumnName("stepname");
            entity.Property(e => e.requiredrole).HasMaxLength(100).HasColumnName("requiredrole");

            entity.HasOne(d => d.Workflow).WithMany(p => p.WorkflowSequences)
                .HasForeignKey(d => d.Id_workflow)
                .HasConstraintName("workflow_sequence_id_workflow_fkey");

            entity.HasOne(d => d.Role)
            .WithMany()
            .HasForeignKey(d => d.requiredrole)
            .HasConstraintName("workflow_sequence_requiredrole_fkey")
            .OnDelete(DeleteBehavior.Restrict);

        });

        modelBuilder.Entity<WorkflowAction>(entity =>
        {
            entity.HasKey(e => e.Id_action).HasName("workflow_action_pkey");

            entity.ToTable("workflow_action");

            entity.Property(e => e.Id_action).HasColumnName("id_action");
            entity.Property(e => e.Id_request).HasColumnName("id_request");
            entity.Property(e => e.Id_step).HasColumnName("id_step");
            entity.Property(e => e.action).HasMaxLength(255).HasColumnName("action");
            entity.Property(e => e.actiondate).HasColumnName("actiondate");
            entity.Property(e => e.comments).HasColumnName("comments");

            entity.HasOne(d => d.Request).WithMany(p => p.WorkflowActions)
                .HasForeignKey(d => d.Id_request)
                .HasConstraintName("workflow_action_id_request_fkey");

            entity.HasOne(d => d.WorkflowSequence).WithMany(p => p.WorkflowActions)
                .HasForeignKey(d => d.Id_step)
                .HasConstraintName("workflow_action_id_step_fkey");
        });

        modelBuilder.Entity<NextStepRules>(entity =>
        {
            entity.HasKey(e => e.Id_rule).HasName("next_step_rule_pkey");

            entity.ToTable("next_step_rule");

            entity.Property(e => e.Id_rule).HasColumnName("id_rule");
            entity.Property(e => e.Id_currentstep).HasColumnName("id_currentstep");
            entity.Property(e => e.Id_nextstep).HasColumnName("id_nextstep");
            entity.Property(e => e.conditiontype)
                .HasMaxLength(100)
                .HasColumnName("conditiontype");
            entity.Property(e => e.conditionvalue)
                .HasMaxLength(255)
                .HasColumnName("conditionvalue");

            // Relasi ke WorkflowSequence sebagai current step
            entity.HasOne(d => d.CurrentStep)
                .WithMany()
                .HasForeignKey(d => d.Id_currentstep)
                .HasConstraintName("next_step_rule_id_currentstep_fkey")
                .OnDelete(DeleteBehavior.Restrict);

            // Relasi ke WorkflowSequence sebagai next step
            entity.HasOne(d => d.NextStep)
                .WithMany()
                .HasForeignKey(d => d.Id_nextstep)
                .HasConstraintName("next_step_rule_id_nextstep_fkey")
                .OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

