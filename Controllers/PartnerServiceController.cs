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
    [Route("partnerServices")]
    public class PartnerServiceController : Controller
    {
        private readonly IWeCareRepository _repo;

        public PartnerServiceController(IWeCareRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var services = await _repo.GetPartnerServiceDispDtos();
            //var ourservices = new List<OurServiceDto>();
            //var i = 1;
            //var srv = new OurServiceDto();
            //foreach(var s in services)
            //{
            //    srv.Row.Add(s);
               
            //    if(i == 2)
            //    {
            //        ourservices.Add(srv);
            //        i = 0;
            //        srv = new OurServiceDto();
            //    }
            //    i += 1;

            //}

            return Ok(services);
        }

        // GET Method endpoint to fetch all PartnerService data from the underlying repository
        [HttpGet("partner/{partnerId}")]
        public async Task<IActionResult> Get(int partnerId)
        {
            var rDto = await _repo.GetPartnerServices(partnerId);

            return Ok(rDto);
        }

        [HttpGet("service/{serviceId}")]
        public async Task<IActionResult> GetServicePartners(int serviceId)
        {
            var rDto = await _repo.GetServicePartners(serviceId);

            return Ok(rDto);
        }

        // GET Method endpoint to fetch a specific PartnerService data from the underlying repository via the route
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPartnerService(int id)
        {
            var rDto = await _repo.GetPartnerService(id);

            return Ok(rDto);
        }


        // POST Method endpoint to register PartnerService 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PartnerServiceInputDto inputDto)
        {
            try
            {

                // Check if the model from the request body is valid. That is if the syntax, format and required information are valid
                if (!ModelState.IsValid) return BadRequest("Provide all required information.");

                // Declaration of the object to be saved and population all the required information
                var addDto = new PartnerService();
                addDto.PartnerId = inputDto.PartnerId;
                addDto.BusinessServiceId = inputDto.BusinessServiceId;
                addDto.IsActive = true;
                addDto.Price = inputDto.Price;
                addDto.CreatedOnDate = DateTime.Now;
                addDto.Seats = inputDto.Seats;
                addDto.MinDuration = inputDto.MinDuration;
                addDto.Id = await _repo.GetPartnerServiceMaxId() + 1;


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

        // PUT Method endpoint to Update a specific PartnerService 
        [HttpPut("{id}")]
        public async Task<IActionResult> Post(int id, [FromBody] PartnerServiceInputDto inputDto)
        {
            try
            {


                // Calls the repository to get PartnerService to be updated
                var updateDto = await _repo.GetPartnerServiceById(id);

                // if the returned object is null a No found http response is returned
                if (updateDto == null) return NotFound("No record found.");

                updateDto.Price = inputDto.Price;
                updateDto.Seats = inputDto.Seats;
                updateDto.MinDuration = inputDto.MinDuration;

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

        // PUT Method endpoint to Update a specific PartnerService 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {


                // Calls the repository to get PartnerService to be updated
                var updateDto = await _repo.GetPartnerServiceById(id);

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

        [HttpPut("uploadServiceImage")]
        public async Task<IActionResult> PutPartnerServiceImage([FromForm] PartnerServiceImageInputDto inputDto)
        {
            try
            {
                var ptnImage = await _repo.GetPartnerServiceImage(inputDto.PartnerServiceId);
                if(ptnImage != null)
                {
                    _repo.Delete(ptnImage);
                    await _repo.SaveChanges();
                }

                var img = new PartnerServiceImage();
                img.Id = inputDto.PartnerServiceId;
                img.ImageData = Tools.FileUpload(inputDto.ImageData);

                _repo.Add(img);

                await _repo.SaveChanges();

                var a = new
                {
                    Message = "Uploaded successfully.",
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
