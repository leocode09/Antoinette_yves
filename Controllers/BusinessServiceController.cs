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
    [Route("businessServices")]
    public class BusinessServiceController : Controller
    {
        private readonly IWeCareRepository _repo;

        public BusinessServiceController(IWeCareRepository repo)
        {
            _repo = repo;
        }

        // GET Method endpoint to fetch all BusinessService data from the underlying repository
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rDto = await _repo.GetBusinessServices();

            return Ok(rDto);
        }

        // GET Method endpoint to fetch a specific BusinessService data from the underlying repository via the route
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusinessService(int id)
        {
            var rDto = await _repo.GetBusinessService(id);

            return Ok(rDto);
        }


        // POST Method endpoint to register BusinessService 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReferenceInputDto inputDto)
        {
            try
            {

                // Check if the model from the request body is valid. That is if the syntax, format and required information are valid
                if (!ModelState.IsValid) return BadRequest("Provide all required information.");

                // Declaration of the object to be saved and population all the required information
                var addDto = new BusinessService();
                addDto.Id = await _repo.GetBusinessServiceMaxId() + 1;
                addDto.Name = inputDto.Name;
                addDto.CreatedOnDate = DateTime.Now;
               

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

        // PUT Method endpoint to Update a specific BusinessService 
        [HttpPut("{id}")]
        public async Task<IActionResult> Post(int id, [FromBody] ReferenceInputDto inputDto)
        {
            try
            {


                // Calls the repository to get BusinessService to be updated
                var updateDto = await _repo.GetBusinessService(id);

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

        // PUT Method endpoint to Update a specific BusinessService 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {


                // Calls the repository to get BusinessService to be updated
                var updateDto = await _repo.GetBusinessService(id);

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