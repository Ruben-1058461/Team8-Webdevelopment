using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace officecalender.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly Database _context;

        public AttendanceController(Database context)
        {
            _context = context;
        }

        // GET: api/Attendance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendance()
        {
            // Fetch all Attendances from the database
            var attendances = await _context.Attendances.ToListAsync();
            return Ok(attendances);
        }

        // GET: api/Attendance/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> GetAttendance(int id)
        {
            // Fetch a specific Attendance by id from the database
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            return Ok(attendance);
        }


        // POST: api/Attendance
        [HttpPost]
        public async Task<ActionResult<Attendance>> PostAttendance(Attendance attendance)
        {
            // Validate the attendance object
            if (attendance == null)
            {
                return BadRequest("Attendance data is null.");
            }

            // Add the new Attendance to the DbSet
            _context.Attendances.Add(attendance);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the created attendance
            return CreatedAtAction(nameof(GetAttendance), new { id = attendance.id }, attendance);
        }

        // PUT: api/Attendance/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttendance(int id, Attendance attendance)
        {
            // Validate the attendance object
            if (attendance == null || id != attendance.id)
            {
                return BadRequest();
            }

            // Update the Attendance in the DbSet
            _context.Entry(attendance).State = EntityState.Modified;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("Data successfully updated.");
        }

        // DELETE: api/Attendance/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            // Fetch the Attendance by id
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }

            // Remove the Attendance from the DbSet
            _context.Attendances.Remove(attendance);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("Data successfully deleted.");
        }
    }
}