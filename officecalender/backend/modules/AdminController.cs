using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private static List<Admin> Admins = new List<Admin>
    {
        new Admin { user_name = "Thijs", password = "Admin1", email = "Test@mail.com" }
    };

    // GET: api/Admin
    [HttpGet]
    public ActionResult<IEnumerable<Admin>> GetAdmin()
    {
        return Ok(Admins);
    }

}
