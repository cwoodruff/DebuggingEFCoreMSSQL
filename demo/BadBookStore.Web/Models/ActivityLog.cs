using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class ActivityLog
{
    public long ActivityId { get; set; }

    public string? HappenedAt { get; set; }

    public string? Actor { get; set; }

    public string? Action { get; set; }

    public string? SubjectType { get; set; }

    public string? SubjectKey { get; set; }

    public string? Payload { get; set; }
}