namespace HrApp.Application.Interfaces;

public interface IIpAddressService
{
    public string GetUserIpAddress();
    public string GetUserAgent();
}
