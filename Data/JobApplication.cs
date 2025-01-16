namespace JobTrackrApi;

public record JobApplication
{
    public Guid id { get; set; }
    public string userId { get; set; } = null!;
	public string jobTitle { get; set; } = null!;
    public string company { get; set; } = null!;
    public string? recruiter { get; set; }
    public string? phone { get; set; }
    public string? email { get; set; }
    public string? jobListingUrl { get; set; }
    public string? jobDescription { get; set; }
    public virtual ICollection<StatusHistory> statusHistory { get; set; } = new List<StatusHistory>();
}
