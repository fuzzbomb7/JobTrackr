using Mapster;

namespace JobTrackrApi.Model
{
	public class JobApplicationModel
	{
		public int Id { get; set; }
		public string JobTitle { get; set; } = null!;
		public string Company { get; set; } = null!;
		public string? Recruiter { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }
		public string? JobListingUrl { get; set; }
		public string? JobDescription { get; set; }
		public List<StatusHistoryModel> StatusHistory { get; set; } = new List<StatusHistoryModel>();
	}

	public class StatusHistoryModel
	{
		public string? Status { get; set; }
		public string? Date { get; set; }
	}
}
