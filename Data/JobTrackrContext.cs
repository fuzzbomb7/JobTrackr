using Microsoft.EntityFrameworkCore;

namespace JobTrackrApi;

public partial class JobTrackrContext : DbContext
{
	public JobTrackrContext()
	{
	}

	public JobTrackrContext(DbContextOptions<JobTrackrContext> options)
		: base(options)
	{
	}

	public virtual DbSet<JobApplication> JobApplications { get; set; }

	public virtual DbSet<StatusHistory> StatusHistories { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
		=> optionsBuilder.UseSqlServer("Server=tcp:jobtrackr.database.windows.net,1433;Initial Catalog=JobTrackr;Persist Security Info=False;User ID=trackradmin;Password=fe$DE5uX9hbs;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<JobApplication>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__JobAppli__3214EC074A32EE81");

			entity.ToTable("JobApplication");

			entity.Property(e => e.Company).HasMaxLength(100);
			entity.Property(e => e.Email).HasMaxLength(150);
			entity.Property(e => e.JobListingUrl)
				.HasMaxLength(500)
				.HasColumnName("JobListingURL");
			entity.Property(e => e.JobTitle).HasMaxLength(150);
			entity.Property(e => e.Phone).HasMaxLength(50);
			entity.Property(e => e.Recruiter).HasMaxLength(50);

			entity.HasMany(d => d.StatusHistories)
				.WithOne(p => p.Application)
				.HasForeignKey(d => d.ApplicationId)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK_StatusHistory_JobApplication");
		});

		modelBuilder.Entity<StatusHistory>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__StatusHi__3214EC078F947771");

			entity.ToTable("StatusHistory");

			entity.Property(e => e.Date).HasColumnType("datetime");
			entity.Property(e => e.StatusId).HasMaxLength(10);

			entity.HasOne(d => d.Application).WithMany(p => p.StatusHistories)
				.HasForeignKey(d => d.ApplicationId)
				.OnDelete(DeleteBehavior.NoAction)
				.HasConstraintName("FK_StatusHistory_JobApplication");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
