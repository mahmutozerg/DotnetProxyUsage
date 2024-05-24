namespace WebScrapping.Core.Models;

public class UserScrappingRequest:Base
{
    public string RequestedUrl { get; set; } = string.Empty;
    public List<AppUser> Users { get; set; } = new();
}