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
            var rsvId = "";
            try
            {

                // Check if the model from the request body is valid. That is if the syntax, format and required information are valid
                if (!ModelState.IsValid) return BadRequest("Provide all required information.");


                var sum = 0;

                // Checking if the total amoun on the listed services equals the amount provided in the header of the body request
                inputDto.Details.ForEach(ix => sum += ix.Price);
                if (sum != inputDto.Amount)
                {
                    return BadRequest("The amount provided does not correspond with the listed service total amount.");
                }


                // Gets the maximum id/code of the reservations
                var maxId = await _repo.GetReservationMaxId();
                var id = (int.Parse(maxId.Substring(6, 5)) + 1).ToString().PadLeft(5, '0');
                var Code = maxId.Substring(0, 6);

                // Initializing the reservation object to hold the data wich will be saved in the database
                var addDto = new Reservation();
                addDto.BusinessClientId = inputDto.Phone;
                addDto.Amount = inputDto.Amount;
                addDto.Id = Code + id;
                addDto.Code = Code;
                addDto.ReservationDate = DateTime.Now;
                addDto.ReservationStatusId = 1;


                rsvId = addDto.Id;

                // Getting the partner/ saloon in wich the reservation is being made to.
                var partner = await _repo.GetPartnerByPartnerService(inputDto.Details.FirstOrDefault().PartnerServiceId);
                if (partner == null) return BadRequest("Service not found. Please try again later.");
                addDto.PartnerId = partner.Id;

                // Adding the above object to the repository connected to our database.
                _repo.Add(addDto);
                await _repo.SaveChanges();
                var nbr = 0;






                //var rsvs = new List<ReservationDetail>();



                // Saving each listed service in the database one by one
                var barberId = 0;
                foreach (var rd in inputDto.Details)
                {
                    nbr += 1;

                    // initializing the reservation detail object to hold the details of the reservation made
                    var dd = new ReservationDetail();

                    // initializing the barberId to 0 in case no barber was selected in order to allow the system to provide one by default.
                    if (rd.BarberId == null) barberId = 0;
                    else barberId = rd.BarberId.GetValueOrDefault();


                    // Check if the service being reserved for is available at the provided time as well as the availabiliity of the barber at that same time.
                    var result = await _repo.IsSeatAvailable(rd.PartnerServiceId, rd.AppointmentTime, barberId);
                    var cd = result.Substring(0, 1);
                    var msg = result.Substring(2);

                    // if the slot is available a code with S is return and otherwise F is return with a specific message to notify what happened
                    if (cd == "F")
                    {

                        // if the slot is not available all the records saved related to this request is deleted.
                        var rsvs = await _repo.GetReservationDetails(addDto.Id);
                        if (rsvs != null)
                        {
                            _repo.Delete(rsvs);
                        }
                        _repo.Delete(addDto);
                        await _repo.SaveChanges();
                        return BadRequest(msg);
                    }

                    if (rd.BarberId == null || rd.BarberId == 0)
                    {
                        rd.BarberId = int.Parse(msg);
                    }

                    // Declaration of the object to be saved and population all the required information

                    dd.IsServed = false;
                    dd.PartnerServiceId = rd.PartnerServiceId;
                    dd.ReservationDate = rd.AppointmentTime;
                    dd.BarberId = rd.BarberId;
                    dd.ReservationId = addDto.Id;
                    dd.Amount = rd.Price;


                    dd.Code = Code;
                    dd.Id = addDto.Id + nbr.ToString().PadLeft(2, '0');

                    _repo.Add(dd);


                    await _repo.SaveChanges();
                }






                // Saves any changes within the repository to our connected database.


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
                var rsvs = await _repo.GetReservationDetails(rsvId);
                if (rsvs != null)
                {
                    _repo.Delete(rsvs);
                }

                var rsv = await _repo.GetReservationById(rsvId);
                if (rsv != null)
                {
                    _repo.Delete(rsv);
                }

                await _repo.SaveChanges();

                // Return the caught message in the http response.
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("clientReservation")]
        public async Task<IActionResult> ClientReservationPost([FromBody] ClientReservationInputDto inputDto)
        {
            try
            {

                // Check if the model from the request body is valid. That is if the syntax, format and required information are valid
                if (!ModelState.IsValid) return BadRequest("Provide all required information.");

                var addbDto = new BusinessClient();
                addbDto.Name = inputDto.Name;
                addbDto.Id = inputDto.Phone;

                if (!string.IsNullOrWhiteSpace(inputDto.SerialNumber)) addbDto.SerialNumber = inputDto.SerialNumber;

                addbDto.CreatedOnDate = DateTime.Now;

                var user = await _repo.GetUser(inputDto.Phone);
                if (user == null)
                {
                    user = new User();
                    user.CreatedOnDate = DateTime.Now;
                    user.IsActive = true;
                    user.Names = inputDto.Name;
                    user.Password = EncryptionHelper.Encrypt("123");
                    user.RoleId = 3;
                    user.Username = inputDto.Phone;

                    _repo.Add(user);

                }

                // Adding the above object to the repository connected to our database.
                _repo.Add(addbDto);

                // Saves any changes within the repository to our connected database.
                await _repo.SaveChanges();
                var rsvId = "";
                try
                {
                    var sum = 0;

                    // Checking if the total amoun on the listed services equals the amount provided in the header of the body request
                    inputDto.Details.ForEach(ix => sum += ix.Price);
                    if (sum != inputDto.Amount)
                    {
                        return BadRequest("The amount provided does not correspond with the listed service total amount.");
                    }


                    // Gets the maximum id/code of the reservations
                    var maxId = await _repo.GetReservationMaxId();
                    var id = (int.Parse(maxId.Substring(6, 5)) + 1).ToString().PadLeft(5, '0');
                    var Code = maxId.Substring(0, 6);

                    // Initializing the reservation object to hold the data wich will be saved in the database
                    var addDto = new Reservation();
                    addDto.BusinessClientId = inputDto.Phone;
                    addDto.Amount = inputDto.Amount;
                    addDto.Id = Code + id;
                    addDto.Code = Code;
                    addDto.ReservationDate = DateTime.Now;

                    rsvId = addDto.Id;

                    // Getting the partner/ saloon in wich the reservation is being made to.
                    var partner = await _repo.GetPartnerByPartnerService(inputDto.Details.FirstOrDefault().PartnerServiceId);
                    if (partner == null) return BadRequest("Service not found. Please try again later.");
                    addDto.PartnerId = partner.Id;

                    // Adding the above object to the repository connected to our database.
                    _repo.Add(addDto);
                    await _repo.SaveChanges();
                    var nbr = 0;






                    //var rsvs = new List<ReservationDetail>();



                    // Saving each listed service in the database one by one
                    var barberId = 0;
                    foreach (var rd in inputDto.Details)
                    {
                        nbr += 1;

                        // initializing the reservation detail object to hold the details of the reservation made
                        var dd = new ReservationDetail();

                        // initializing the barberId to 0 in case no barber was selected in order to allow the system to provide one by default.
                        if (rd.BarberId == null) barberId = 0;
                        else barberId = rd.BarberId.GetValueOrDefault();


                        // Check if the service being reserved for is available at the provided time as well as the availabiliity of the barber at that same time.
                        var result = await _repo.IsSeatAvailable(rd.PartnerServiceId, rd.AppointmentTime, barberId);
                        var cd = result.Substring(0, 1);
                        var msg = result.Substring(2);

                        // if the slot is available a code with S is return and otherwise F is return with a specific message to notify what happened
                        if (cd == "F")
                        {

                            // if the slot is not available all the records saved related to this request is deleted.
                            var rsvs = await _repo.GetReservationDetails(addDto.Id);
                            if (rsvs != null)
                            {
                                _repo.Delete(rsvs);
                            }
                            _repo.Delete(addDto);
                            await _repo.SaveChanges();
                            return BadRequest(msg);
                        }

                        if (rd.BarberId == null || rd.BarberId == 0)
                        {
                            rd.BarberId = int.Parse(msg);
                        }

                        // Declaration of the object to be saved and population all the required information

                        dd.IsServed = false;
                        dd.PartnerServiceId = rd.PartnerServiceId;
                        dd.ReservationDate = rd.AppointmentTime;
                        dd.BarberId = rd.BarberId;
                        dd.ReservationId = addDto.Id;
                        dd.Amount = rd.Price;


                        dd.Code = Code;
                        dd.Id = addDto.Id + nbr.ToString().PadLeft(2, '0');

                        _repo.Add(dd);


                        await _repo.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    var rsvs = await _repo.GetReservationDetails(rsvId);
                    if (rsvs != null)
                    {
                        _repo.Delete(rsvs);
                    }

                    var rsv = await _repo.GetReservationById(rsvId);
                    if (rsv != null)
                    {
                        _repo.Delete(rsv);
                    }

                    await _repo.SaveChanges();

                    return BadRequest(ex.Message);
                }


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

        //// PUT Method endpoint to Update a specific Reservation 
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(string id, [FromBody] ReservationInputDto inputDto)
        //{
        //    try
        //    {


        //        // Calls the repository to get Reservation to be updated
        //        var updateDto = await _repo.GetReservationById(id);

        //        // if the returned object is null a No found http response is returned
        //        if (updateDto == null) return NotFound("No record found.");

        //        updateDto.PartnerServiceId = inputDto.PartnerServiceId;
        //        updateDto.ReservationDate = inputDto.AppointmentTime;


        //        // Saves any changes within the repository to our connected database.
        //        await _repo.SaveChanges();

        //        // Declare an anonymous object for the message to be attached into the http response
        //        var a = new
        //        {
        //            Message = "Updated successfully.",
        //            StatusCode = 200
        //        };

        //        return Ok(a);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Return the caught message in the http response.
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPut("served/{id}")]
        public async Task<IActionResult> IsServed(string id)
        {
            try
            {
                var rs = await _repo.GetReservationDetailById(id);
                if (rs == null) return NotFound("No record found.");

                rs.IsServed = true;

                await _repo.SaveChanges();

                var a = new
                {
                    Message = "Updated",
                    StatusCode = 200
                };

                return Ok(a);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

        [HttpPost(nameof(RequestMomoPay))]
        public async Task<IActionResult> RequestMomoPay([FromBody] CheckoutInputDto inputDto)
        {
            try
            {
                var rsv = await _repo.GetReservationById(inputDto.ReservationId);
                if (rsv.ReservationStatusId != 1) return BadRequest("Allready processed.");

                var transaction = new PayTransaction();
                transaction.Amount = inputDto.Amount;
                transaction.Phone = inputDto.Phone;
                transaction.ReservationId = rsv.Id;
                transaction.TransactionDate = DateTime.Now;
                transaction.TransactionStatusId = 1;

                _repo.Add(transaction);
                await _repo.SaveChanges();

                var a = new
                {
                    Message = "Request sent. Dial *182*7# to complete payment",
                    StatusCode = 200
                };

                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest("Request Failed. Please try again!");
            }
        }

        [HttpGet(nameof(GetUUID))]
        public IActionResult GetUUID()
        {
            var uuId = Guid.NewGuid();

            return Ok(uuId);
        }

        [HttpGet(nameof(GetAuthorizationFromKeys))]
        public IActionResult GetAuthorizationFromKeys()
        {
            var tkn = "8db3b260-f367-4713-a666-25d889967dbb:ac6711572b0145cabce42211e8a415d6";

            var bs64 = Encoding.UTF8.GetBytes(tkn);

            var key = Convert.ToBase64String(bs64);

            var authValue = "Basic " + key;

            return Ok(authValue);
        }

        [HttpGet(nameof(GetToken))]
        public async Task<IActionResult> GetToken()
        {
            //API User: API Key
            var tkn = "8db3b260-f367-4713-a666-25d889967dbb:ac6711572b0145cabce42211e8a415d6";

            var bs64 = Encoding.UTF8.GetBytes(tkn);

            var key = Convert.ToBase64String(bs64);

            var authValue = "Basic " + key;

            var body = new
            {


            };


            var client = new RestClient("https://mtndeveloperapi.portal.mtn.co.rw");
            var request = new RestRequest("/collection/token/", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Host", "mtndeveloperapi.portal.mtn.co.rw");
            request.AddHeader("Authorization", "Basic " + key);
            request.AddHeader("Ocp-Apim-Subscription-Key", "93e8f4bb519b4bdb9e44d06f044d641e");
            request.AddJsonBody(body);
            request.UseNewtonsoftJson();

            var response = await client.ExecuteAsync(request);

            return new ContentResult()
            {
                Content = response.Content,
                ContentType = response.ContentType,
                StatusCode = ((int)response.StatusCode)
            };
        }

        [HttpGet(nameof(TransactionFinished) + "/{xReferenceId}")]
        public IActionResult TransactionFinished(Guid xReferenceId)
        {
            return Ok(xReferenceId);
        }

        [HttpPost(nameof(RequestMomoPayment))]
        public async Task<IActionResult> RequestMomoPayment([FromBody] CheckoutInputDto inputDto)
        {
            try
            {


                var tkn = "8db3b260-f367-4713-a666-25d889967dbb:ac6711572b0145cabce42211e8a415d6";

                var bs64 = Encoding.UTF8.GetBytes(tkn);

                var key = Convert.ToBase64String(bs64);


                var client = new RestClient("https://mtndeveloperapi.portal.mtn.co.rw");
                var request = new RestRequest("/collection/token/", Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("Host", "mtndeveloperapi.portal.mtn.co.rw");
                request.AddHeader("Authorization", "Basic " + key);
                request.AddHeader("Ocp-Apim-Subscription-Key", "93e8f4bb519b4bdb9e44d06f044d641e");
               
                request.UseNewtonsoftJson();

                var rsp = await client.ExecuteAsync(request);

                if (rsp.IsSuccessful)
                {
                    var tokenString = JsonConvert.DeserializeObject<TokenModelGetDto>(rsp.Content);
                    var token = tokenString.Access_token;
                    var payerObj = new
                    {
                        partyIdType = "MSISDN",
                        partyId = inputDto.Phone
                    };

                    var body = new
                    {
                        amount = inputDto.Amount.ToString(),
                        currency = "RWF",
                        externalId = inputDto.ReservationId,
                        payer = payerObj,
                        payerMessage = "Testing message",
                        payeeNote = "Test message"
                    };

                    var x_ReferenceId = Guid.NewGuid().ToString();
                    var ckient = new RestClient("https://mtndeveloperapi.portal.mtn.co.rw/");
                    var rekuest = new RestRequest("collection/v1_0/requesttopay", Method.POST);
                    rekuest.AddHeader("Content-Type", "application/json");
                    rekuest.AddHeader("Accept", "*/*");
                    rekuest.AddHeader("Host", "mtndeveloperapi.portal.mtn.co.rw");
                    rekuest.AddHeader("Authorization", "Bearer " + token);
                    rekuest.AddHeader("Ocp-Apim-Subscription-Key", "93e8f4bb519b4bdb9e44d06f044d641e");
                    rekuest.AddHeader("X-Reference-Id", x_ReferenceId);
                    rekuest.AddHeader("X-Target-Environment", "mtnrwanda");
                    rekuest.AddJsonBody(body);
                    rekuest.UseNewtonsoftJson();

                    var response = await ckient.ExecuteAsync(rekuest);
                    var statusCode = ((int)response.StatusCode);
                    var content = response.Content;
                    var contentType = string.IsNullOrEmpty(response.ContentType) ? "text/plain" : response.ContentType;

                    if (statusCode >= 200 && statusCode <= 204)
                    {
                        var rsv = await _repo.GetReservationById(inputDto.ReservationId);
                        if (rsv.ReservationStatusId != 1) return BadRequest("Allready processed.");

                        var transaction = new PayTransaction();
                        transaction.Amount = inputDto.Amount;
                        transaction.Phone = inputDto.Phone;
                        transaction.ReservationId = rsv.Id;
                        transaction.TransactionDate = DateTime.Now;
                        transaction.TransactionStatusId = 1;
                        transaction.X_ReferenceId = x_ReferenceId;

                        _repo.Add(transaction);
                        await _repo.SaveChanges();

                        dynamic rspData = new ExpandoObject();
                        rspData.data = content;
                        rspData.x_ReferenceId = x_ReferenceId;

                        var rspDataJson = JsonConvert.SerializeObject(rspData);
                        contentType = "application/json";

                        return new ContentResult()
                        {
                            ContentType = contentType,
                            Content = rspDataJson,
                            StatusCode = statusCode
                        };
                    }
                    else
                    {
                        return new ContentResult()
                        {
                            ContentType = contentType,
                            Content = content,
                            StatusCode = statusCode
                        };
                    }


                    

                }
                else
                {
                    var statusCode = ((int)rsp.StatusCode);
                    var content = rsp.Content;
                    var contentType = string.IsNullOrEmpty(rsp.ContentType) ? "text/plain" : rsp.ContentType;

                    return new ContentResult()
                    {
                        ContentType = contentType,
                        Content = content,
                        StatusCode = statusCode
                    };
                }



                //var rsv = await _repo.GetReservationById(inputDto.ReservationId);
                //if (rsv.ReservationStatusId != 1) return BadRequest("Allready processed.");

                //var transaction = new PayTransaction();
                //transaction.Amount = inputDto.Amount;
                //transaction.Phone = inputDto.Phone;
                //transaction.ReservationId = rsv.Id;
                //transaction.TransactionDate = DateTime.Now;
                //transaction.TransactionStatusId = 1;

                //_repo.Add(transaction);
                //await _repo.SaveChanges();

                //var a = new
                //{
                //    Message = "Request sent. Dial *182*7# to complete payment",
                //    StatusCode = 200
                //};

                //return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest("Request Failed. Please try again!");
            }
        }

        [HttpGet(nameof(CheckPaymentStatus)+ "/{referenceId}")]
        public async Task<IActionResult> CheckPaymentStatus(string referenceId)
        {
            try
            {


                var tkn = "8db3b260-f367-4713-a666-25d889967dbb:ac6711572b0145cabce42211e8a415d6";

                var bs64 = Encoding.UTF8.GetBytes(tkn);

                var key = Convert.ToBase64String(bs64);


                var client = new RestClient("https://mtndeveloperapi.portal.mtn.co.rw");
                var request = new RestRequest("/collection/token/", Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("Host", "mtndeveloperapi.portal.mtn.co.rw");
                request.AddHeader("Authorization", "Basic " + key);
                request.AddHeader("Ocp-Apim-Subscription-Key", "93e8f4bb519b4bdb9e44d06f044d641e");

                request.UseNewtonsoftJson();

                var rsp = await client.ExecuteAsync(request);

                var tokenString = JsonConvert.DeserializeObject<TokenModelGetDto>(rsp.Content);
                var token = tokenString.Access_token;

                var x_ReferenceId = referenceId;
                var ckient = new RestClient("https://mtndeveloperapi.portal.mtn.co.rw");
                var rekuest = new RestRequest($"/collection/v1_0/requesttopay/{x_ReferenceId}", Method.GET);
                rekuest.AddHeader("Content-Type", "application/json");
                rekuest.AddHeader("Accept", "*/*");
                rekuest.AddHeader("Host", "mtndeveloperapi.portal.mtn.co.rw");
                rekuest.AddHeader("Authorization", "Bearer " + token);
                rekuest.AddHeader("Ocp-Apim-Subscription-Key", "93e8f4bb519b4bdb9e44d06f044d641e");
               // rekuest.AddParameter("X-Reference-Id", x_ReferenceId);
                rekuest.AddHeader("X-Target-Environment", "mtnrwanda");
                rekuest.UseNewtonsoftJson();

                var response = await ckient.ExecuteAsync(rekuest);
                var statusCode = ((int)response.StatusCode);
                var content = response.Content;
                var contentType = string.IsNullOrEmpty(response.ContentType) ? "text/plain" : response.ContentType;

                return new ContentResult()
                {
                    ContentType = contentType,
                    Content = content,
                    StatusCode = statusCode
                };

            }
            catch(Exception ex)
            {
                return BadRequest("Request Failed. Please try again!");
            }
        }
    }

    public class Role
{
    public DateTime CreatedOnDate { get; set; }
    // Other properties...
}

}
