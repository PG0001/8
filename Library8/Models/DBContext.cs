using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Library8.Models;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BackgroundJobLog> BackgroundJobLogs { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectChat> ProjectChats { get; set; }

    public virtual DbSet<ProjectUser> ProjectUsers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TaskComment> TaskComments { get; set; }

    public virtual DbSet<TaskItem> TaskItems { get; set; }

    public virtual DbSet<TaskTimeline> TaskTimelines { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<TaskSummary> VwTaskSummaries { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder
optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");
        var config = builder.Build();
        var connectionString =
       config.GetConnectionString("defaultConnection");
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BackgroundJobLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Backgrou__3214EC0729B32467");

            entity.Property(e => e.JobName).HasMaxLength(200);
            entity.Property(e => e.RunTime).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Files__3214EC0725A6299F");

            entity.HasIndex(e => e.RelatedEntityId, "IX_Files_RelatedEntityId");

            entity.Property(e => e.EntityType).HasMaxLength(20);
            entity.Property(e => e.FileName).HasMaxLength(300);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.UploadedOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.Files)
                .HasForeignKey(d => d.UploadedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Files__UploadedB__48CFD27E");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC078D956271");

            entity.HasIndex(e => e.UserId, "IX_Notifications_UserId");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__4D94879B");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Projects__3214EC071D68FE22");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Projects__Create__2D27B809");
        });

        modelBuilder.Entity<ProjectChat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProjectC__3214EC078770C6B5");

            entity.ToTable("ProjectChat");

            entity.Property(e => e.FileUrl).HasMaxLength(500);
            entity.Property(e => e.SentOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectChats)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectCh__Proje__440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectChats)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectCh__UserI__44FF419A");
        });

        modelBuilder.Entity<ProjectUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProjectU__3214EC07C204B523");

            entity.HasIndex(e => e.ProjectId, "IX_ProjectUsers_ProjectId");

            entity.HasIndex(e => e.UserId, "IX_ProjectUsers_UserId");

            entity.Property(e => e.AddedOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectUsers)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectUs__Proje__30F848ED");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectUs__UserI__31EC6D26");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07652348A6");

            entity.HasIndex(e => e.Name, "UQ__Roles__737584F68EAFEB23").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TaskComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaskComm__3214EC077E8FEF60");

            entity.HasIndex(e => e.TaskId, "IX_TaskComments_TaskId");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskComments)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaskComme__TaskI__3A81B327");

            entity.HasOne(d => d.User).WithMany(p => p.TaskComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaskComme__UserI__3B75D760");
        });

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaskItem__3214EC0746211CCC");

            entity.HasIndex(e => e.AssignedTo, "IX_TaskItems_AssignedTo");

            entity.HasIndex(e => e.ProjectId, "IX_TaskItems_ProjectId");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Priority).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.TaskItems)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK__TaskItems__Assig__36B12243");

            entity.HasOne(d => d.Project).WithMany(p => p.TaskItems)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaskItems__Proje__35BCFE0A");
        });

        modelBuilder.Entity<TaskTimeline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaskTime__3214EC07350AAD3D");

            entity.ToTable("TaskTimeline");

            entity.Property(e => e.Action).HasMaxLength(200);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskTimelines)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaskTimel__TaskI__3F466844");

            entity.HasOne(d => d.User).WithMany(p => p.TaskTimelines)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaskTimel__UserI__403A8C7D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0743250AE3");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053489AEBEE8").IsUnique();

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleId__29572725");
        });

        modelBuilder.Entity<TaskSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TaskSummary");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
