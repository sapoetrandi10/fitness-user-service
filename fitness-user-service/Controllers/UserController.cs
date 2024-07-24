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
        public IActionResult CreateUser([FromBody] ReqUserDto userReq)
        {
            try
            {
                if (userReq == null)
                    return BadRequest(new
                    {
                        status = "Failed",
                        message = "Requset not valid"
                    });

                var isUserExist = _userRep.GetUsers()
                    .Where(u => u.UserName.Trim().ToLower() == userReq.UserName.Trim().ToLower())
                    .FirstOrDefault();

                if (isUserExist != null)
                {
                    return Ok(new
                    {
                        status = "Success",
                        message = "User already exists",
                        data = isUserExist
                    });
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
                    return StatusCode(500, new
                    {
                        status = "Failed",
                        message = "Something went wrong while saving",
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "User Successfully created",
                    data = user
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] ReqUserDto userReq)
        {
            try
            {
                if (userReq == null)
                    return BadRequest(ModelState);

                var isUserExist = _userRep.GetUsers()
                        .Where(u => u.UserID == userId)
                        .FirstOrDefault();

                if (isUserExist == null)
                    return Ok(new
                    {
                        status = "Failed",
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
                    return StatusCode(500, new
                    {
                        status = "Failed",
                        message = "Something went wrong updating user"
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "User Successfully updated",
                    data = updatedUser
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            try
            {
                var isUserExist = _userRep.GetUsers()
                .Where(u => u.UserID == userId)
                .FirstOrDefault();

                if (isUserExist == null)
                    return Ok(new
                    {
                        status = "Success",
                        message = "User not found!"
                    });

                if (!_userRep.DeleteUser(isUserExist))
                {
                    return StatusCode(500, new
                    {
                        status = "Failed",
                        message = "Something went wrong deleting user!"
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "User Successfully Deleted"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var allUsers = _userRep.GetUsers();

                if (allUsers.Count <= 0)
                {
                    return Ok(new
                    {
                        status = "Success",
                        message = "User is empty!",
                        data = allUsers
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "All User Successfully fetched",
                    data = allUsers
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }

        [HttpGet("{userId}")]
        public IActionResult GetUser(int userId)
        {
            try
            {
                var user = _userRep.GetUser(userId);

                if (user == null)
                {
                    return Ok(new
                    {
                        status = "Success",
                        message = "User not found!",
                        data = user
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "User Successfully fetched",
                    data = user
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }
    }
}
