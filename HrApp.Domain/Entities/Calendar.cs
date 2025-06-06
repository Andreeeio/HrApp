﻿namespace HrApp.Domain.Entities;

public class Calendar
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = default!;
    public virtual CalendarEventCreator Creator { get; set; } = default!;
}
