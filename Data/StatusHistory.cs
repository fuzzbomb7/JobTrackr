using System;
using System.Collections.Generic;

namespace JobTrackrApi;

public partial class StatusHistory
{
    public int Id { get; set; }

    public string StatusId { get; set; } = null!;

    public DateTime Date { get; set; }

    public int ApplicationId { get; set; }

    public virtual JobApplication Application { get; set; } = null!;
}
