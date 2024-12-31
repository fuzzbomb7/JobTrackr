namespace JobTrackrApi;

public partial class JobApplication
{
    public int Id { get; set; }

    public string JobTitle { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string? Recruiter { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? JobListingUrl { get; set; }

    public string? JobDescription { get; set; }

    public virtual ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();
}
