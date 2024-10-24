using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeCareWebApp.Entities;
using WeCareWebApp.Models;
using WeCareWebApp.Services;

namespace WeCareWebApp.Controllers
{
    [Route("roles")]
    public class RoleController : Controller
    {
        private readonly IWeCareRepository _repo;

        public RoleController(IWeCareRepository repo)
        {
            _repo = repo;
        }

        // GET Method endpoint to fetch all Role data from the underlying repository
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rDto = await _repo.GetRoles();

            return Ok(rDto);
        }

        // GET Method endpoint to fetch a specific Role data from the underlying repository via the route
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            var rDto = await _repo.GetRole(id);

            return Ok(rDto);
        }


        // POST Method endpoint to register Role 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReferenceInputDto inputDto)
        {
            try
            {

                // Check if the model from the request body is valid. That is if the syntax, format and required information are valid
                if (!ModelState.IsValid) return BadRequest("Provide all required information.");

                // Declaration of the object to be saved and population all the required information
                var addDto = new Role();
                addDto.Id = await _repo.GetRoleMaxId()+ 1;
                addDto.Name = inputDto.Name;


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

        // PUT Method endpoint to Update a specific Role 
        [HttpPut("{id}")]
        public async Task<IActionResult> Post(int id, [FromBody] ReferenceInputDto inputDto)
        {
            try
            {


                // Calls the repository to get Role to be updated
                var updateDto = await _repo.GetRole(id);

                // if the returned object is null a No found http response is returned
                if (updateDto == null) return NotFound("No record found.");

                if (!string.IsNullOrWhiteSpace(inputDto.Name)) updateDto.Name = inputDto.Name;


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

        // DELETE Method endpoint to Update a specific Role 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {


                // Calls the repository to get Role to be updated
                var deleteDto = await _repo.GetRole(id);

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
