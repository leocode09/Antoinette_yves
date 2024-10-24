using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextmagicRest;
using WeCareWebApp.Entities;
using WeCareWebApp.Helpers;
using WeCareWebApp.Models;
using WeCareWebApp.Services;

namespace WeCareWebApp.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IWeCareRepository _repo;

        public UserController(IWeCareRepository repo)
        {
            _repo = repo;
        }

        // GET Method endpoint to fetch all Users data from the underlying repository
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rDto = await _repo.GetUsers();

            return Ok(rDto);
        }

        // GET Method endpoint to fetch a specific Users data from the underlying repository via the route
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers(Guid id)
        {
            var rDto = await _repo.GetUser(id);

            return Ok(rDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                loginDto.Password = EncryptionHelper.Encrypt(loginDto.Password);
                var u = await _repo.GetUser(loginDto.Username, loginDto.Password);
                if (u == null) return Unauthorized("Incorrect username or password");

                var token = _repo.BuildToken(u);
                var a = new
                {
                    Token = token,
                    StatusCode = 200
                };
                return Ok(a);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // POST Method endpoint to register Users 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserInputDto inputDto)
        {
            try
            {

                // Check if the model from the request body is valid. That is if the syntax, format and required information are valid
                if (!ModelState.IsValid) return BadRequest("Provide all required information.");


                var addDto = new User();
                addDto.CreatedOnDate = DateTime.Now;
                addDto.IsActive = true;
                addDto.Names = inputDto.Names;
                addDto.Password = EncryptionHelper.Encrypt("123");
                addDto.RoleId = inputDto.RoleId;
                addDto.Username = inputDto.Username;
                addDto.IsDeleted = false;

                if(inputDto.RoleId == 2)
                {
                    var partnerDto = new Partner();
                    var id = await _repo.GetPartnerMaxId() + 1;
                    partnerDto.Name = inputDto.Names;
                    partnerDto.CreatedById = Guid.Parse("BA38B9EF-D197-48FA-95E2-08D89A8C1E88");
                    
                    partnerDto.IsDeleted = false;
                    partnerDto.CreatedOnDate = DateTime.Now;
                    partnerDto.User = addDto;

                    _repo.Add(partnerDto);
                }

                if(inputDto.RoleId == 3)
                {
                    if (!int.TryParse(inputDto.Username,out _))
                    {
                        return BadRequest("Username should be a valid phone number");
                    }

                    if(inputDto.Username.Length != 10)
                    {
                        return BadRequest("Username should be ten characters.");
                    }

                    var clientDto = new BusinessClient();
                    clientDto.Name = inputDto.Names;
                    clientDto.Id = inputDto.Username;
                    clientDto.CreatedOnDate = DateTime.Now;
                    _repo.Add(clientDto);
                }

                // Adding the above object to the repository connected to our database.
                _repo.Add(addDto);
                // Saves any changes within the repository to our connected database.
                await _repo.SaveChanges();

                // Declare an anonymous object for the message to be attached into the http response
                var a = new
                {
                    Message = "Saved successfully.",
                    StatusCode = 200
                };

                return Ok(a);
            }
            catch (Exception ex)
            {
                // Return the caught message in the http response.
                return BadRequest(ex.Message);
            }
        }

        // PUT Method endpoint to Update a specific Users 
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UserInputDto inputDto)
        {
            try
            {


                // Calls the repository to get Users to be updated
                var updateDto = await _repo.GetUserById(id);

                // if the returned object is null a No found http response is returned
                if (updateDto == null) return NotFound("No record found.");

                if (!string.IsNullOrWhiteSpace(inputDto.Names)) updateDto.Names = inputDto.Names;
                updateDto.RoleId = inputDto.RoleId;



                // Saves any changes within the repository to our connected database.
                await _repo.SaveChanges();

                // Declare an anonymous object for the message to be attached into the http response
                var a = new
                {
                    Message = "Updated successfully.",
                    StatusCode = 200
                };

                return Ok(a);
            }
            catch (Exception ex)
            {
                // Return the caught message in the http response.
                return BadRequest(ex.Message);
            }
        }

        // PUT Method endpoint to Update a specific Users 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {


                // Calls the repository to get Users to be updated
                var updateDto = await _repo.GetUserById(id);

                // if the returned object is null a No found http response is returned
                if (updateDto == null) return NotFound("No record found.");

                // Flag the object to deleted into the underlying repository.
                updateDto.IsActive = false;
               

                // Saves any changes within the repository to our connected database.
                await _repo.SaveChanges();

                // Declare an anonymous object for the message to be attached into the http response
                var a = new
                {
                    Message = "Deleted successfully.",
                    StatusCode = 200
                };

                return Ok(a);
            }
            catch (Exception ex)
            {
                // Return the caught message in the http response.
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("resetPassword/{userId}")]
        public async Task<IActionResult> ResertPassword(Guid userId)
        {
            try
            {
                var user = await _repo.GetUser(userId);
                if (user == null) return NotFound("No record found.");
                user.Password = EncryptionHelper.Encrypt("default123");
                await _repo.SaveChanges();

                var a = new
                {
                    Message = "Reset to default.",
                    StatusCode = 200
                };
                return Ok(a);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("changePassword/{userId}")]
        public async Task<IActionResult>ChangePassword(Guid userId,[FromBody] UserChangePasswordDto chgPassword)
        {
            try
            {
                var user = await _repo.GetUser(userId);
                if (user == null) return NotFound("No record found.");

                if(user.Password != EncryptionHelper.Encrypt(chgPassword.CurrentPassword))
                {
                    return Unauthorized("Wrong password.");
                }

                user.Password = EncryptionHelper.Encrypt(chgPassword.NewPassword);
                await _repo.SaveChanges();

                var a = new
                {
                    Message = "Updated successfully.",
                    StatusCode = 200
                };
                return Ok(a);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetUserCreds))]
        public async Task<IActionResult> GetUserCreds()
        {
            var rDto = await _repo.GetUserCredentials();

            return Ok(rDto);
        }


        [HttpGet("GetMessage/{phone}")]
        public async Task<IActionResult> SendSms(string phone)
        {
            try
            {
                var client = new Client("test", "my-api-key");
                var link = client.SendMessage("Hello from TextMagic API", $"{phone}");

                if (link.Success)
                {
                    return Ok($"Message ID {link.Id} has been successfully sent");
                }
                else
                {
                    return  BadRequest($"Message was not sent due to following exception: {link.ClientException.Message}");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
