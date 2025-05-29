using AutoMapper;
using DotNetAPI.Data;
using DotNetAPI.Dtos;
using DotNetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    IUserRepository _userRepository;
    IMapper _mapper;

    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserToAddDto, User>();
        }));
    }

    [HttpGet("TestConnection")]



    [HttpGet("getUsers")]
    // public IActionResult Test()
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _userRepository.GetUsers();
        return users;
    }

    [HttpGet("getSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        return _userRepository.GetSingleUser(userId);
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        User? userDb = _userRepository.GetSingleUser(user.UserId);
        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to Update User");
        }
        throw new Exception("Failed to Get User");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        User userDb = _mapper.Map<User>(user);
        
        _userRepository.AddEntity<User>(userDb);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Failed to Add User");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _userRepository.GetSingleUser(userId);
        if (userDb != null)
        {
            _userRepository.RemoveEntity<User>(userDb);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to Delete User");
        }
        throw new Exception("Failed to Get User");
    }

}
