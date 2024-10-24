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
    [Route("barbers")]
    public class BarberController : Controller
    {
        private readonly IWeCareRepository _repo;

        public BarberController(IWeCareRepository repo)
        {
            _repo = repo;
        }

        // GET Method endpoint to fetch all Barber data from the underlying repository
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rDto = await _repo.GetBarbers();

            return Ok(rDto);
        }

        [HttpGet("partner/{partnerId}")]
        public async Task<IActionResult> GetPartnerBarbers(int partnerId)
        {
            var rDto = await _repo.GetBarbers(partnerId);

            return Ok(rDto);
        }

        // GET Method endpoint to fetch a specific Barber data from the underlying repository via the route
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBarber(int id)
        {
            var rDto = await _repo.GetBarber(id);

            return Ok(rDto);
        }


        // POST Method endpoint to register Barber 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BarberInputDto inputDto)
        {
            try
            {

                // Check if the model from the request body is valid. That is if the syntax, format and required information are valid
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = await _repo.GetUser(inputDto.Phone);
                if(user == null)
                {
                    user = new User();
                    user.CreatedOnDate = DateTime.Now;
                    user.IsActive = true;
                    user.Names = inputDto.Names;
                    user.Password = EncryptionHelper.Encrypt("123");
                    user.RoleId = 4;
                    user.Username = inputDto.Phone;

                    _repo.Add(user);

                }

                // Declaration of the object to be saved and population all the required information
                var addDto = new Barber();
                var id = await _repo.GetBarberMaxId() + 1;
                addDto.Names = inputDto.Names;
                addDto.CreatedById = inputDto.UserId;
                addDto.Email = inputDto.Email;
                addDto.IsDeleted = false;
                addDto.Phone = inputDto.Phone;
                addDto.CreatedOnDate = DateTime.Now;
                addDto.PartnerId = inputDto.PartnerId;
                addDto.User = user;
                addDto.Id = id;

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

        // PUT Method endpoint to Update a specific Barber 
        [HttpPut("{id}")]
        public async Task<IActionResult> Post(int id, [FromBody] BarberInputDto inputDto)
        {
            try
            {


                // Calls the repository to get Barber to be updated
                var updateDto = await _repo.GetBarberById(id);

                // if the returned object is null a No found http response is returned
                if (updateDto == null) return NotFound("No record found.");

                if (!string.IsNullOrWhiteSpace(inputDto.Email)) updateDto.Email = inputDto.Email;
                if (!string.IsNullOrWhiteSpace(inputDto.Names)) updateDto.Names = inputDto.Names;
                if (!string.IsNullOrWhiteSpace(inputDto.Phone)) updateDto.Phone = inputDto.Phone;
                if (inputDto.PartnerId != null && inputDto.PartnerId != 0) updateDto.PartnerId = inputDto.PartnerId;



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

        // PUT Method endpoint to Update a specific Barber 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {


                // Calls the repository to get Barber to be updated
                var updateDto = await _repo.GetBarberById(id);

                // if the returned object is null a No found http response is returned
                if (updateDto == null) return NotFound("No record found.");

                // Flag the object to deleted into the underlying repository.
                _repo.Delete(updateDto);

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
