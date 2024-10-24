using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeCareWebApp.Entities;
using WeCareWebApp.Helpers;
using WeCareWebApp.Models;
using WeCareWebApp.Services;

namespace WeCareWebApp.Controllers
{
    [Route("businessClients")]
    public class BusinessClientController : Controller
    {
        private readonly IWeCareRepository _repo;

        public BusinessClientController(IWeCareRepository repo)
        {
            _repo = repo;
        }

        // GET Method endpoint to fetch all BusinessClient data from the underlying repository
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rDto = await _repo.GetBusinessClients();

            return Ok(rDto);
        }

        // GET Method endpoint to fetch a specific BusinessClient data from the underlying repository via the route
        [HttpGet("{id}")] 
        public async Task<IActionResult> GetBusinessClient(string id)
        {
            var rDto = await _repo.GetBusinessClient(id);

            return Ok(rDto);
        }

        [HttpGet("serialNumber/{serialNumber}")]
        public async Task<IActionResult> GetBusinessClientBySerialNumber(string serialNumber)
        {
            var rDto = await _repo.GetBusinessClientBySerialNumber(serialNumber);
            if(rDto == null)
            {
                return NotFound("Not found");
            }

            else
            {
                var a = new
                {
                    Phone = rDto.Id,
                    SerialNumber = rDto.SerialNumber
                };

                return Ok(a);
            }

        }


        // POST Method endpoint to register BusinessClient 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]BusinessClientInputDto inputDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(inputDto.Name)) inputDto.Name = inputDto.Phone;
                

                // Check if the model from the request body is valid. That is if the syntax, format and required information are valid
                if (!ModelState.IsValid) return BadRequest("Provide all required information.");

                // Declaration of the object to be saved and population all the required information
                var addDto = new BusinessClient();
                addDto.Name = inputDto.Name;
                addDto.Id = inputDto.Phone;
                if(!string.IsNullOrWhiteSpace(inputDto.SerialNumber))addDto.SerialNumber = inputDto.SerialNumber;

                addDto.CreatedOnDate = DateTime.Now;

                var user = await _repo.GetUser(inputDto.Phone);
                if(user == null)
                {
                    inputDto.Username = string.IsNullOrWhiteSpace(inputDto.Username) ? inputDto.Phone : inputDto.Username;
                    var usernameExists = await _repo.DoesUsernameExists(inputDto.Username);
                    if (usernameExists) return BadRequest("Username taken.");
                    user = new User();
                    user.CreatedOnDate = DateTime.Now;
                    user.IsActive = true;
                    user.Names = inputDto.Name;
                    user.Password = EncryptionHelper.Encrypt(inputDto.Password);
                    user.RoleId = 3;
                    user.Username = inputDto.Username;

                    _repo.Add(user);
                }

                addDto.User = user;
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
            }catch(Exception ex)
            {
                // Return the caught message in the http response.
                return BadRequest(ex.Message);
            }
        }

        // PUT Method endpoint to Update a specific BusinessClient 
        [HttpPut("{id}")]
        public async Task<IActionResult> Post(string id,[FromBody] BusinessClientInputDto inputDto)
        {
            try
            {


                // Calls the repository to get BusinessClient to be updated
                var updateDto = await _repo.GetBusinessClient(id);

                // if the returned object is null a No found http response is returned
                if (updateDto == null) return NotFound("No record found.");

                if(!string.IsNullOrWhiteSpace(inputDto.Name))updateDto.Name = inputDto.Name;
               

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

        // DELETE Method endpoint to Update a specific BusinessClient 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {


                // Calls the repository to get BusinessClient to be updated
                var deleteDto = await _repo.GetBusinessClient(id);

                // if the returned object is null a No found http response is returned
                if (deleteDto == null) return NotFound("No record found.");

                // Flag the object to deleted into the underlying repository.
                _repo.Delete(deleteDto);

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
    }
}
