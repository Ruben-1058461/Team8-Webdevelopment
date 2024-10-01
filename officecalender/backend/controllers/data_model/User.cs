public class User
{
    public required int id { get; set; }
    public required string first_name { get; set; }

    public required string last_name { get; set; }
    public required string password { get; set; }
    public required string email { get; set; }
    public required string recurring_days { get; set; }
    public bool is_admin { get; set; } = false;
}