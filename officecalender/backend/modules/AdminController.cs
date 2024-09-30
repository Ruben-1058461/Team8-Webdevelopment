using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly Database _context;

    public AdminController(Database context)
    {
        _context = context;
    }

    // GET: api/Admin
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Admin>>> GetAdmin()
    {
        // Fetch all Admins from the database
        var admins = await _context.Admins.ToListAsync();
        return Ok(admins);
    }

    // GET: api/Admin/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Admin>> GetAdmin(int id)
    {
        // Fetch a specific Admin by id from the database
        var admin = await _context.Admins.FindAsync(id);
        if (admin == null)
        {
            return NotFound();
        }
        return Ok(admin);
    }


    // POST: api/Admin
    [HttpPost]
    public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
    {
        // Validate the admin object
        if (admin == null)
        {
            return BadRequest("Admin data is null.");
        }

        // Optionally set default values
        admin.is_admin = true;

        // Add the new Admin to the DbSet
        _context.Admins.Add(admin);

        // Save changes to the database
        await _context.SaveChangesAsync();

        // Return the created admin
        return CreatedAtAction(nameof(GetAdmin), new { id = admin.id }, admin);
    }
}
