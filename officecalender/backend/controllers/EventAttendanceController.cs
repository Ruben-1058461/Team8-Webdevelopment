using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace officecalender.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventAttendanceController : ControllerBase
    {
        private readonly Database _context;

        public EventAttendanceController(Database context)
        {
            _context = context;
        }

        // GET: api/EventAttendance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event_Attendance>>> GetEvent_Attendance()
        {
            // Fetch all Event_Attendances from the database
            var event_attendances = await _context.Event_Attendances.ToListAsync();
            return Ok(event_attendances);
        }

        // GET: api/Event_Attendance/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Event_Attendance>> GetEvent_Attendance(int id)
        {
            // Fetch a specific Event_Attendance by id from the database
            var event_attendance = await _context.Event_Attendances.FindAsync(id);
            if (event_attendance == null)
            {
                return NotFound();
            }
            return Ok(event_attendance);
        }


        // POST: api/Event_Attendance
        [HttpPost]
        public async Task<ActionResult<Event_Attendance>> PostEvent_Attendance(Event_Attendance event_attendance)
        {
            // Validate the attendance object
            if (event_attendance == null)
            {
                return BadRequest("Event Attendance data is null.");
            }

            // Add the new Attendance to the DbSet
            _context.Event_Attendances.Add(event_attendance);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the created attendance
            return CreatedAtAction(nameof(GetEvent_Attendance), new { id = event_attendance.id }, event_attendance);
        }

        // PUT: api/Event_Attendance/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent_Attendance(int id, Event_Attendance event_attendance)
        {
            // Validate the event_attendance object
            if (event_attendance == null || id != event_attendance.id)
            {
                return BadRequest();
            }

            // Update the Event_Attendance in the DbSet
            _context.Entry(event_attendance).State = EntityState.Modified;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("Data successfully updated.");
        }

        // DELETE: api/Event_Attendance/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent_Attendance(int id)
        {
            // Fetch the Event_Attendance by id
            var event_attendance = await _context.Event_Attendances.FindAsync(id);
            if (event_attendance == null)
            {
                return NotFound();
            }

            // Remove the Event_Attendance from the DbSet
            _context.Event_Attendances.Remove(event_attendance);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("Data successfully deleted.");
        }
    }
}