using Microsoft.EntityFrameworkCore;

namespace JobTrackrApi;

public partial class JobTrackrContext : DbContext
{
	private readonly IConfiguration configuration;

	public JobTrackrContext()
	{
	}

	public JobTrackrContext(DbContextOptions<JobTrackrContext> options, IConfiguration configuration)
		: base(options)
	{
		this.configuration = configuration;
	}

	public virtual DbSet<JobApplication> JobApplications { get; set; }

	public virtual DbSet<StatusHistory> StatusHistories { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseSqlServer(configuration.GetConnectionString("Jobtrackr"));

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
