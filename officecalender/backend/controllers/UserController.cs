using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace officecalender.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Database _context;

        public UserController(Database context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            // Fetch all Users from the database
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/User/1
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            // Fetch a specific User by id from the database
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            // Validate the user object
            if (user == null)
            {
                return BadRequest("User data is null.");
            }

            // Optionally set default values
            user.is_admin = false;

            // Add the new User to the DbSet
            _context.Users.Add(user);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the created user
            return CreatedAtAction(nameof(GetUser), new { id = user.id }, user);
        }

        // PUT: api/User/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            // Validate the user object
            if (user == null || id != user.id)
            {
                return BadRequest();
            }

            // Update the User in the DbSet
            _context.Entry(user).State = EntityState.Modified;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("Data successfully updated.");
        }

        // DELETE: api/user/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Fetch the User by id
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Remove the User from the DbSet
            _context.Users.Remove(user);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("Data successfully deleted.");
        }
    }
}
