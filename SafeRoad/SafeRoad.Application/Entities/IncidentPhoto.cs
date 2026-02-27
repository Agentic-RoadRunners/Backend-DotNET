using System;
using System.Collections.Generic;
namespace SafeRoad.Core.Entities;

public class IncidentPhoto
{
    public int Id { get; set; }
    public Guid IncidentId { get; set; }
    public string BlobUrl { get; set; } = null!;

    public Incident Incident { get; set; } = null!;
}