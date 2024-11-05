using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace officecalender.backend.Controllers
{
[LoggedIn]

[Route("api/[controller]")]
[ApiController]
public class SignUpEventController : ControllerBase
{

     private readonly string _connectionString = "Data Source=database.db"; 



    [HttpPost("SignUpUser")]  // POST api/SignUpEvent/SignUpUser
    public async Task<IActionResult> SignUp([FromBody] PostEvent postevent)
    {
        if (postevent == null || postevent.Id <= 0)
        {
            return BadRequest("Invalid event ID.");
        }
        return Ok(postevent.Id);
    }

    [HttpPost("GetUserId")]  // POST api/SignUpEvent/GetUserId
    public async Task<IActionResult> PostId([FromBody] IdRequest request)
    {
        if (request == null || request.Id <= 0)
        {
            return BadRequest("Invalid ID.");
        }
        return Ok(request.Id);
    }
   

     [HttpPost("exampleUserId")]


 [HttpPost("SignUpEventUser")] //Post Adds user to the event
public async Task<IActionResult> addUserToEvent([FromBody] EventAttendanceRequest request)
{
    if (request == null || request.EventId <= 0)
    {
        return BadRequest("Invalid request data.");
    }

    // Check if the user is logged in by retrieving UserEmail and UserId from session
    var userEmail = HttpContext.Session.GetString("UserEmail");
    if (string.IsNullOrEmpty(userEmail))
    {
        return Unauthorized("Session does not contain UserEmail; user might not be logged in.");
    }

    int? userId = HttpContext.Session.GetInt32("UserId");
    if (!userId.HasValue)
    {
        return Unauthorized("User ID not found in session.");
    }

    try
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Check if the event exists if it doesnt, stops
            bool eventExists;
            using (var checkEventCommand = connection.CreateCommand())
            {
                checkEventCommand.CommandText = @"
                    SELECT COUNT(*)
                    FROM event_data
                    WHERE id = $eventId;";
                checkEventCommand.Parameters.AddWithValue("$eventId", request.EventId);

                eventExists = (long)await checkEventCommand.ExecuteScalarAsync() > 0;
            }

            if (!eventExists)
            {
                return NotFound("Event not found.");
            }

            // Check if user is signed up doesnt add them
            bool userAlreadySignedUp;
            using (var checkSignupCommand = connection.CreateCommand())
            {
                checkSignupCommand.CommandText = @"
                    SELECT COUNT(*)
                    FROM event_attendance_data
                    WHERE user_id = $userId AND event_id = $eventId;";
                checkSignupCommand.Parameters.AddWithValue("$userId", userId);
                checkSignupCommand.Parameters.AddWithValue("$eventId", request.EventId);

                userAlreadySignedUp = (long)await checkSignupCommand.ExecuteScalarAsync() > 0;
            }

            if (userAlreadySignedUp)
            {
                return Conflict("User is already signed up for this event.");
            }

            // Add user to the event
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    INSERT INTO event_attendance_data (user_id, event_id, rating, feedback)
                    VALUES ($userId, $eventId, $rating, $feedback);";

                command.Parameters.AddWithValue("$userId", userId); 
                command.Parameters.AddWithValue("$eventId", request.EventId);
                command.Parameters.AddWithValue("$rating", request.Rating ?? (object)DBNull.Value); 
                command.Parameters.AddWithValue("$feedback", request.Feedback ?? (object)DBNull.Value);

                var result = await command.ExecuteNonQueryAsync();

                if (result > 0)
                {
                    return Ok("User added to the event successfully.");
                }
                else
                {
                    return StatusCode(500, "Failed to add user to the event.");
                }
            }
        }
    }
    catch (System.Exception ex)
    {
        return Problem("An error occurred while adding the user to the event: " + ex.Message);
    }
}

[HttpDelete("deleteUserSignup/{eventId}")]  // DELETE deletes user from event
public async Task<IActionResult> DeleteUserSignupAsync(int eventId)
{
    // Check if user is logged in by retrieving UserEmail and UserId from session
    var userEmail = HttpContext.Session.GetString("UserEmail");
    if (string.IsNullOrEmpty(userEmail))
    {
        return Unauthorized("Session does not contain UserEmail; user might not be logged in.");
    }

    int? userId = HttpContext.Session.GetInt32("UserId");
    if (!userId.HasValue)
    {
        return Unauthorized("User ID not found in session.");
    }

    try
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Check if the user is signed up for the specified event
            bool signupExists;
            using (var checkSignupCommand = connection.CreateCommand())
            {
                checkSignupCommand.CommandText = @"
                    SELECT COUNT(*)
                    FROM event_attendance_data
                    WHERE user_id = $userId AND event_id = $eventId;";
                checkSignupCommand.Parameters.AddWithValue("$userId", userId);
                checkSignupCommand.Parameters.AddWithValue("$eventId", eventId);

                signupExists = (long)await checkSignupCommand.ExecuteScalarAsync() > 0;
            }

            if (!signupExists)
            {
                return NotFound("Signup for this event not found.");
            }

            // Delete the signup
            using (var deleteCommand = connection.CreateCommand())
            {
                deleteCommand.CommandText = @"
                    DELETE FROM event_attendance_data
                    WHERE user_id = $userId AND event_id = $eventId;";
                deleteCommand.Parameters.AddWithValue("$userId", userId);
                deleteCommand.Parameters.AddWithValue("$eventId", eventId);

                var result = await deleteCommand.ExecuteNonQueryAsync();

                if (result > 0)
                {
                    return Ok("Signup removed successfully.");
                }
                else
                {
                    return StatusCode(500, "Failed to remove signup.");
                }
            }
        }
    }
    catch (System.Exception ex)
    {
        return Problem("An error occurred while removing the signup: " + ex.Message);
    }
}

public class PostEvent
{
    public int Id { get; set; }
}

public class IdRequest
{
    public int Id { get; set; }
}

public class EventAttendanceRequest
{
    public int EventId { get; set; } // Event ID 
    public int? Rating { get; set; } // rating
    public string Feedback { get; set; } // feedback
}
}
}