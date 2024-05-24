namespace WebScrapping.Core.Models;

public class ScrappingResults:Base
{
    public UserScrappingRequest ScrappingRequest { get; set; } = new();

    public string Directory { get; set; }
    
}