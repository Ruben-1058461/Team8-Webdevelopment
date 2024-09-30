public class Admin
{
    public required int id { get; set; }
    public required string user_name { get; set; }
    public required string password { get; set; }
    public required string email { get; set; }
    public bool is_admin { get; set; } = true;
}