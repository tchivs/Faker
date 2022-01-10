namespace Faker
{
    public enum PortNumberModel
    {
        IsSystem,
        IsUser,
        IsDynamic
    }
    public interface IInternetProvider : IProvider<string>
    {
        string Email();
        string HostName();
        string UserName();
        string HttpMethod();
        string MacAddress();
        string UriExtension();
        int PortNumber(PortNumberModel? model=null);
    }
}