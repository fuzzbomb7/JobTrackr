namespace JobTrackrApi;

public record StatusHistory
{
    public string status { get; set; } = null!;
    public DateTime date { get; set; }
}
