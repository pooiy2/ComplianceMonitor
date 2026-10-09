using System;
using System.Collections.Generic;
using Backend.Enums;

namespace Backend.Models;

public partial class History
{
    public long Id { get; set; }

    public string? Action { get; set; }

    public string? Guideline { get; set; }

    public double? Confidence { get; set; }

    public DateTime Timestamp { get; set; }

    public Result? Result { get; set; }
}
