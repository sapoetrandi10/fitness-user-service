using fitness_db.Interfaces;
using fitness_db.Models;
using fitness_user_service.Dto.Req;
using Microsoft.AspNetCore.Mvc;

namespace fitness_user_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRep;
        public UserController(IUserRepository userRepository)
        {
            _userRep = userRepository;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody]  ReqUserDto userReq)
        {
            try
            {
                if (userReq == null)
                    return BadRequest(ModelState);

                var isUserExist = _userRep.GetUsers()
                    .Where(u => u.UserName.Trim().ToLower() == userReq.UserName.Trim().ToLower())
                    .FirstOrDefault();

                if (isUserExist != null)
                {
                    ModelState.AddModelError("", "User already exists");
                    if (!ModelState.IsValid)
                    {
                        var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                        return BadRequest(new
                        {
                            status = "failed",
                            errors = errors
                        });
                    }
                }

                var user = new User
                {
                    UserName = userReq.UserName,
                    Age = userReq.Age,
                    Gender = userReq.Gender,
                    Weight = userReq.Weight,
                    Height = userReq.Height
                };

                if (!_userRep.CreateUser(user))
                {
                    ModelState.AddModelError("", "Something went wrong while savin");
                    return StatusCode(500, ModelState);
                }

                return Ok( new { 
                    status = "success",
                    message = "User Successfully created"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "failed",
                    message = e.Message
                });
                throw;
            }
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] ReqUserDto userReq)
        {
            if (userReq == null)
                return BadRequest(ModelState);

            var isUserExist = _userRep.GetUsers()
                    .Where(u => u.UserID == userId)
                    .FirstOrDefault();

            if (isUserExist == null)
                return NotFound(new
                {
                    status = "failed",
                    message = "User not found!"
                });

            
            isUserExist.UserID = userId;
            isUserExist.UserName = userReq.UserName;
            isUserExist.Age = userReq.Age;
            isUserExist.Gender = userReq.Gender;
            isUserExist.Weight = userReq.Height;
            isUserExist.Height = userReq.Weight;

            var updatedUser = _userRep.UpdateUser(isUserExist);
            if (updatedUser == null)
            {
                ModelState.AddModelError("", "Something went wrong updating user");
                return StatusCode(500, new {
                    status = "failed",
                    message = "Something went wrong updating user"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "User Successfully updated",
                data = updatedUser
            });
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            var isUserExist = _userRep.GetUsers()
                    .Where(u => u.UserID == userId)
                    .FirstOrDefault();

            if (isUserExist == null)
                return NotFound(new
                {
                    status = "failed",
                    message = "User not found!"
                });

            if (!_userRep.DeleteUser(isUserExist))
            {
                return BadRequest(new
                {
                    status = "failed",
                    message = "Something went wrong deleting user!"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "User Successfully Deleted"
            });
        }

        [HttpGet]
        public IActionResult GetUsers() {
            var allUsers = _userRep.GetUsers();

            if (allUsers.Count <= 0)
            {
                return NotFound(new
                {
                    status = "failed",
                    message = "User is empty!"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "All User Successfully fetched",
                data = allUsers
            });
        }

        [HttpGet("{userId}")]
        public IActionResult GetUser(int userId)
        {
            var user = _userRep.GetUser(userId);

            if (user == null)
            {
                return NotFound(new
                {
                    status = "failed",
                    message = "User not found!"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "User Successfully fetched",
                data = user
            });
        }
    }
}
