using System.ComponentModel.DataAnnotations;

namespace WebScrapping.Core.Models;

public class AppUser:Base
{
    [EmailAddress]
    public string EMail { get; set; } = string.Empty;
    public List<UserScrappingRequest> UserScrappingRequests { get; set; } = new List<UserScrappingRequest>();
    
    
}