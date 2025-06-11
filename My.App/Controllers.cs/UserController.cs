using Microsoft.AspNetCore.Mvc;
using My.App.Dtos;
using My.App.Interfaces;


namespace My.App.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepo _userRepo;

    public UsersController(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
    {
        var users = await _userRepo.GetAllUserDtosAsync();
        return Ok(users);
    }



    // GET: api/users/1
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        try
        {
            var user = await _userRepo.GetUserDtoByIdAsync(id);
            return Ok(user);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }



    // GET: api/users/created?start=2024-01-01&end=2024-12-31
    [HttpGet("created")]
    public async Task<ActionResult<List<UserDto>>> GetUsersByCreatedAt([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        try
        {
            var users = await _userRepo.GetUserDtosByCreatedAtAsync(start, end);
            return Ok(users);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }



    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<UserDto>> InsertUser([FromBody] UserDto dto)
    {
        var newUser = await _userRepo.InsertUserDtoAsync(dto);
        return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
    }



    // PUT: api/users/5
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UserDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("ID mismatch.");
        }

        var updated = await _userRepo.UpdateUserDtoAsync(new UserDto
        {
            Id = dto.Id,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
        });

        return Ok(updated);
    }
}
