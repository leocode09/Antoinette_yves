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
    [Route("partners")]
    public class PartnerController : Controller
    {
        private readonly IWeCareRepository _repo;

        public PartnerController(IWeCareRepository repo)
        {
            _repo = repo;
        }

        // GET Method endpoint to fetch all Partner data from the underlying repository
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rDto = await _repo.GetPartners();

            return Ok(rDto);
        }

        // GET Method endpoint to fetch a specific Partner data from the underlying repository via the route
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPartner(int id)
        {
            var rDto = await _repo.GetPartner(id);

            return Ok(rDto);
        }


        // POST Method endpoint to register Partner 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PartnerInputDto inputDto)
        {
            try
            {

                // Check if the model from the request body is valid. That is if the syntax, format and required information are valid
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = await _repo.GetUser(inputDto.Phone);
                if (user == null)
                {
                    user = new User();
                    user.CreatedOnDate = DateTime.Now;
                    user.IsActive = true;
                    user.Names = inputDto.Name;
                    user.Password = EncryptionHelper.Encrypt("123");
                    user.RoleId = 2;
                    user.Username = inputDto.Phone;

                    _repo.Add(user);
                }

                

                // Declaration of the object to be saved and population all the required information
                var addDto = new Partner();
                var id = await _repo.GetPartnerMaxId() + 1;
                addDto.Name = inputDto.Name;
                addDto.CreatedById = inputDto.UserId;
                addDto.Email = inputDto.Email;
                addDto.IsDeleted = false;
                addDto.Location = inputDto.Location;
                addDto.MomoCode = inputDto.MomoCode;
                addDto.Phone = inputDto.Phone;
                addDto.CreatedOnDate = DateTime.Now;
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

        // PUT Method endpoint to Update a specific Partner 
        [HttpPut("{id}")]
        public async Task<IActionResult> Post(int id, [FromBody] PartnerInputDto inputDto)
        {
            try
            {


                // Calls the repository to get Partner to be updated
                var updateDto = await _repo.GetPartnerById(id);

                // if the returned object is null a No found http response is returned
                if (updateDto == null) return NotFound("No record found.");

                if (!string.IsNullOrWhiteSpace(inputDto.Email)) updateDto.Email = inputDto.Email;
                if (!string.IsNullOrWhiteSpace(inputDto.Name)) updateDto.Name = inputDto.Name;
                if (!string.IsNullOrWhiteSpace(inputDto.Location)) updateDto.Location = inputDto.Location;
                if (!string.IsNullOrWhiteSpace(inputDto.MomoCode)) updateDto.MomoCode = inputDto.MomoCode;
                if (!string.IsNullOrWhiteSpace(inputDto.Phone)) updateDto.Phone = inputDto.Phone;

               

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

        // PUT Method endpoint to Update a specific Partner 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {


                // Calls the repository to get Partner to be updated
                var updateDto = await _repo.GetPartnerById(id);

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

        [HttpPut("updateLocation")]
        public async Task<IActionResult> UpdatePartnerLocationCoordinates([FromBody] PartnerLocationInputDto inputDto)
        {
            try
            {
                var prtn = await _repo.GetPartnerById(inputDto.PartnerId);
                if (prtn == null) return NotFound("No record found.");

                prtn.DestinationLatitude = inputDto.DestinationLatitude;
                prtn.DestinationLongitude = inputDto.DestinationLongitude;

                await _repo.SaveChanges();

                var a = new
                {
                    Message = "Updated successfully.",
                    StatusCode = 200
                };

                return Ok(a);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
