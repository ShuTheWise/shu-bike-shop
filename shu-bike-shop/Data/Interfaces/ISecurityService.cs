namespace shu_bike_shop
{
    public interface ISecurityService
    {
        User CurrentUser { get; }
    }
}