using Mapster;

namespace JobTrackrApi.Model
{
	public class JobApplicationModel
	{
		public Guid id { get; set; }
		public string jobTitle { get; set; } = null!;
		public string company { get; set; } = null!;
		public string? recruiter { get; set; }
		public string? phone { get; set; }
		public string? email { get; set; }
		public string? jobListingUrl { get; set; }
		public string? jobDescription { get; set; }
		public List<StatusHistoryModel> statusHistory { get; set; } = new List<StatusHistoryModel>();
	}

	public class StatusHistoryModel
	{
		public string? status { get; set; }
		public string? date { get; set; }
	}
}
