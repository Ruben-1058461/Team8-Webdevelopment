public class Event
{
    public required int id { get; set; }
    public required string title { get; set; }
    public required string date { get; set; }
    public required string start_time { get; set; }
    public required string end_time { get; set; }
    public required string location { get; set; }
    public bool admin_approval { get; set; } = true;

}