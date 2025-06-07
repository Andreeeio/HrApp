namespace HrApp.Domain.Entities;

public class ExellImport
{
    public Guid Id { get; set; }
    public Guid UploadedById { get; set; }
    public virtual User UploadedBy { get; set; } = default!;
    public string FilePath { get; set; } = default!;
    public DateOnly ImportDate { get; set; }
}
