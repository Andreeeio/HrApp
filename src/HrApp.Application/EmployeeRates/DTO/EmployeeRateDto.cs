using HrApp.Domain.Entities;

namespace HrApp.Application.EmployeeRates.DTO;

public class EmployeeRateDto
{
    public DateOnly RateDate { get; set; }
    public int Rate { get; set; }
    public string Rater { get; set; } = string.Empty;
}
