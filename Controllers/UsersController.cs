using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseAccounting.Data;
using WarehouseAccounting.Models;

namespace WarehouseAccounting.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("get-all-users")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _db.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("get-user/{id:int}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("create-user")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("update-user/{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest("ID does not match");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.Username = updatedUser.Username;
            user.Password = updatedUser.Password;
            user.Role = updatedUser.Role;

            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("delete-user/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
