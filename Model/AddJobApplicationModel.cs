namespace JobTrackrApi.Model
{
	public class AddJobApplicationModel
	{
		public string jobTitle { get; set; }
		public string company { get; set; }
		public string? recruiter { get; set; }
		public string? phone { get; set; }
		public string? email { get; set; }
		public string? jobListingUrl { get; set; }
		public string? jobDescription { get; set; }
		public string applicationDate { get; set; }
		public string userId { get; set; }
	}
}
