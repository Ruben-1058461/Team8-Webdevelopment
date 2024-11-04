public class Event
{
    public int id { get; set; }
    public string? title { get; set; }
    public string? date { get; set; }
    public string? start_time { get; set; }
    public string? end_time { get; set; }
    public string? location { get; set; }
    public bool admin_approval { get; set; } = true;

}