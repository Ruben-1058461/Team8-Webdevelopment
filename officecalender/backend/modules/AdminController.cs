using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private static List<Admin> Admins = new List<Admin>
    {
        new Admin { id = 1, user_name = "Thijs", password = "Admin1", email = "Test@mail.com" },
        new Admin { id = 2, user_name = "Jeroen", password = "Admin2", email = "Test2@mail.com"}
    };

    // GET: api/Admin
    [HttpGet]
    public ActionResult<IEnumerable<Admin>> GetAdmin()
    {
        return Ok(Admins);
    }

    // GET: api/Admin/1
    [HttpGet("{id}")]
    public ActionResult<Admin> GetAdmin(int id)
    {
        // Corrected to use Admins instead of _admin
        var admin = Admins.Find(a => a.user_name == Admins[id - 1].user_name);
        if (admin == null)
        {
            return NotFound();
        }
        return Ok(admin);
    }


}
