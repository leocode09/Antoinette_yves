using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeCareWebApp.Entities;
using WeCareWebApp.Helpers;
using WeCareWebApp.Models;
using WeCareWebApp.Services;

namespace WeCareWebApp.Controllers
{
    [Route("reservations")]
    public class ReservationController : Controller
    {
        private readonly IWeCareRepository _repo;

        public ReservationController(IWeCareRepository repo)
        {
            _repo = repo;
        }

        // GET Method endpoint to fetch all Reservation data from the underlying repository
        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> Get(string clientId)
        {
            var rDto = await _repo.GetClientReservations(clientId);

            return Ok(rDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rDto = await _repo.GetAllReservations();

            return Ok(rDto);
        }

        [HttpGet("serialNumber/{serialNumber}")]
        public async Task<IActionResult> GetserialNumber(string serialNumber)
        {
            var rDto = await _repo.GetSerialNumberReservations(serialNumber);

            return Ok(rDto);
        }

        // GET Method endpoint to fetch all Reservation data from the underlying repository
        [HttpGet("partner/{partnerId}")]
        public async Task<IActionResult> GetPartnerReservations(int partnerId)
        {
            var rDto = await _repo.GetPartnerReservations(partnerId);

            return Ok(rDto);
        }

        // GET Method endpoint to fetch a specific Reservation data from the underlying repository via the route
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation(string id)
        {
            var rDto = await _repo.GetOneReservation(id);

            return Ok(rDto);
        }


        // POST Method endpoint to register Reservation 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReservationInputDto inputDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("Provide all required information.");

                var sum = 0;
                inputDto.Details.ForEach(ix => sum += ix.Price);
                if (sum != inputDto.Amount)
                {
                    return BadRequest("The amount provided does not correspond with the listed service total amount.");
                }

                var maxId = await _repo.GetReservationMaxId();
                var id = (int.Parse(maxId.Substring(6, 5)) + 1).ToString().PadLeft(5, '0');
                var Code = maxId.Substring(0, 6);

                var addDto = new Reservation();
                addDto.BusinessClientId = inputDto.Phone;
                addDto.Amount = inputDto.Amount;
                addDto.Id = Code + id;
                addDto.Code = Code;
                addDto.ReservationDate = DateTime.Now;
                addDto.ReservationStatusId = 1;

                var partner = await _repo.GetPartnerByPartnerService(inputDto.Details.FirstOrDefault().PartnerServiceId);
                if (partner == null) return BadRequest("Service not found. Please try again later.");
                addDto.PartnerId = partner.Id;

                _repo.Add(addDto);
                await _repo.SaveChanges();

                var nbr = 0;
                foreach (var rd in inputDto.Details)
                {
                    nbr += 1;
                    var dd = new ReservationDetail();
                    dd.IsServed = false;
                    dd.PartnerServiceId = rd.PartnerServiceId;
                    dd.ReservationDate = rd.AppointmentTime;
                    dd.BarberId = rd.BarberId ?? 0;
                    dd.ReservationId = addDto.Id;
                    dd.Amount = rd.Price;
                    dd.Code = Code;
                    dd.Id = addDto.Id + nbr.ToString().PadLeft(2, '0');

                    _repo.Add(dd);
                    await _repo.SaveChanges();
                }

                return Ok(new { Message = "Saved successfully.", StatusCode = 200 });
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPut("served/{id}")]
        public async Task<IActionResult> IsServed(string id)
        {
            try
            {
                var rs = await _repo.GetReservationDetailById(id);
                if (rs == null) return NotFound("No record found.");

                rs.IsServed = true;
                await _repo.SaveChanges();

                return Ok(new { Message = "Updated", StatusCode = 200 });
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while processing your request.");
            }
        }

        // DELETE Method endpoint to Update a specific Reservation 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {

                // Calls the repository to get Reservation to be updated
                var deleteDto = await _repo.GetReservationById(id);

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

        [HttpPost("clientMovement")]
        public async Task<IActionResult> PostClientMovement([FromBody] ClientMovementInputDto inputDto)
        {
            try
            {
                var client = await _repo.GetBusinessClient(inputDto.ClientId);
                var clientId = "";
                if (client == null)
                {
                    clientId = null;
                }
                else
                {
                    clientId = client.Id;
                }

                var id = await _repo.GetMovementMaxId() + 1;
                var mvm = new ClientMovement();
                mvm.ClientId = clientId;
                mvm.CreatedOnDate = DateTime.Now;
                mvm.DestinationLatitude = inputDto.DestinationLatitude;
                mvm.DestinationLongitude = inputDto.DestinationLongitude;
                mvm.OriginLatitude = inputDto.OriginLatitude;
                mvm.OriginLongitude = inputDto.OriginLongitude;
                mvm.Id = id;

                _repo.Add(mvm);
                await _repo.SaveChanges();

                var a = new
                {
                    Message = "Successfull",
                    StatusCode = 200
                };

                return Ok(a);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class Role
{
    public DateTime CreatedOnDate { get; set; }
    // Other properties...
}

}

