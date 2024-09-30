using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace officecalender.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly Database _context;

        public EventController(Database context)
        {
            _context = context;
        }

        // GET: api/Event
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvent()
        {
            // Fetch all Events from the database
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        // GET: api/Event/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            // Fetch a specific Admin by id from the database
            var eventdata = await _context.Events.FindAsync(id);
            if (eventdata == null)
            {
                return NotFound();
            }
            return Ok(eventdata);
        }


        // POST: api/Event
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event eventdata)
        {
            // Validate the event object
            if (eventdata == null)
            {
                return BadRequest("Admin data is null.");
            }

            // Add the new Event to the DbSet
            _context.Events.Add(eventdata);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the created event
            return CreatedAtAction(nameof(GetEvent), new
            {
                id = eventdata.id
            }, eventdata);
        }

        // PUT: api/Event/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event eventdata)
        {
            // Validate the admin object
            if (eventdata == null || id != eventdata.id)
            {
                return BadRequest();
            }

            // Update the Admin in the DbSet
            _context.Entry(eventdata).State = EntityState.Modified;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("Data successfully updated.");
        }

        // DELETE: api/Event/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            // Fetch the Event by id
            var eventdata = await _context.Events.FindAsync(id);
            if (eventdata == null)
            {
                return NotFound();
            }

            // Remove the Event from the DbSet
            _context.Events.Remove(eventdata);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("Data successfully deleted.");
        }
    }
}