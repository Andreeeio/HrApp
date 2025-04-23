namespace HrApp.Domain.Entities;

public class ExellImport
{
    public Guid Id { get; set; }
    public Guid UploadedBy { get; set; }
    public virtual User User { get; set; } = default!;
    public string FilePath { get; set; } = default!;
    public DateOnly ImportDate { get; set; }
}
