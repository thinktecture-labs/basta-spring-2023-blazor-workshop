namespace WorkshopConfTool.Shared.Models;

public class ConferenceDetails
{
    public Guid ID { get; set; }

    public string Title { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string? Url { get; set; }
}
