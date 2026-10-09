using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class History
{
    public long Id { get; set; }

    public string? Action { get; set; }

    public string? Guideline { get; set; }

    public double? Confidence { get; set; }

    public DateTime Timestamp { get; set; }
}
