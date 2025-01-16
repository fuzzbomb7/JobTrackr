namespace JobTrackrApi.Model
{
	public class ReportModel
	{
		public int Applied { get; set; }
		public int Rejected { get; set; }
		public int Screened { get; set; }
		public int Interviewing { get; set; }
		public int NoOffer { get; set; }
		public int Offer { get; set; }
		public int Accepted { get; set; }
		public bool Success { get; set; }
		public int Total { get; set; }
	}
}
