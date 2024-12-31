namespace JobTrackrApi.Model
{
	public class AddJobApplicationModel
	{
		public string JobTitle { get; set; }
		public string Company { get; set; }
		public string? Recruiter { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }
		public string? JobListingUrl { get; set; }
		public string? JobDescription { get; set; }
		public string ApplicationDate { get; set; }
	}
}
