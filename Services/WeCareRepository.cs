using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WeCareWebApp.EF;
using WeCareWebApp.Entities;
using WeCareWebApp.Helpers;
using WeCareWebApp.Models;

namespace WeCareWebApp.Services
{
    public class WeCareRepository : IWeCareRepository
    {
        #region Base

        /// Inject DbContext in repository
        /// SmartHrDataContext and IConfiguration initialization
        ///

        private readonly WeCareDbContext _context;
        private readonly IMapper _mapper;

        public WeCareRepository(WeCareDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Saves the changes made on the context to update record in the database
        /// </summary>
        /// <returns></returns>
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Adding an object of type T to the DbContext
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Add<T>(T entity) where T : class
        {
            _context.Add<T>(entity);
        }


        /// <summary>
        /// Adding a list of objects of Type To the DbContext as Range
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Add<T>(List<T> entity) where T : class
        {
            _context.AddRange(entity);
        }

        /// <summary>
        /// Deleting a single object or a list of a given object from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove<T>(entity);
        }


        /// <summary>
        /// Remove a list of objects of type T from the database context wich will result in a deletion
        /// of those from the databse when save changes is called
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Delete<T>(List<T> entity) where T : class
        {
            _context.RemoveRange(entity);
        }


        /// <summary>
        /// Update a single object or a list of given object being tracked
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Update<T>(T entity) where T : class
        {
            _context.Update<T>(entity);
        }

        /// <summary>
        /// Updating a range or a list of object of Type T being tracked and will take effect whe save changes is called
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Update<T>(List<T> entity) where T : class
        {
            _context.UpdateRange(entity);
        }

        /// <summary>
        /// Fetch data from database of any given Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<object> Get<T>() where T : class
        {
            var d = _context.Set<T>().AsQueryable();

            foreach (var property in _context.Model.FindEntityType(typeof(T)).GetNavigations())
            {
                d = d.Include(property.Name);
            }

            return await d.ToListAsync<T>();
        }


        #endregion Base

        #region Business Client

        // Method to get all BusinessClient record from the database
        public async Task<List<BusinessClient>> GetBusinessClients()
        {
            return await _context.BusinessClients.ToListAsync();
        }

        // Method to get a specific record from the database by its identifier
        public async Task<BusinessClient> GetBusinessClient(string id)
        {
            return await _context.BusinessClients.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<BusinessClient> GetBusinessClientBySerialNumber(string serialNumber)
        {
            return await _context.BusinessClients.FirstOrDefaultAsync(i => i.SerialNumber == serialNumber);
        }

        #endregion

        #region Barber

        public async Task<IEnumerable<BarberDto>> GetBarbers() => await _mapper.ProjectTo<BarberDto>(_context.Barbers.AsQueryable()).ToListAsync();
        public async Task<IEnumerable<BarberDto>> GetBarbers(int partnerId) => await _mapper.ProjectTo<BarberDto>(_context.Barbers.Where(ix => ix.PartnerId == partnerId).AsQueryable()).ToListAsync();
        public async Task<BarberDto> GetBarber(int id) => await _mapper.ProjectTo<BarberDto>(_context.Barbers.Where(ix => ix.Id == id).AsQueryable()).FirstOrDefaultAsync();
        public async Task<Barber> GetBarberById(int id) => await _context.Barbers.FirstOrDefaultAsync(ix => ix.Id == id);
        public async Task<int> GetBarberMaxId()
        {
            try
            {
                return await _context.Barbers.MaxAsync(i => i.Id);
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region Business Service

        // Method to get all BusinessService record from the database
        public async Task<List<BusinessService>> GetBusinessServices()
        {
            return await _context.BusinessServices.ToListAsync();
        }

        // Method to get a specific record from the database by its identifier
        public async Task<BusinessService> GetBusinessService(int id)
        {
            return await _context.BusinessServices.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<int> GetBusinessServiceMaxId()
        {
            try
            {
                return await _context.BusinessServices.MaxAsync(i => i.Id);
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region Partner

        public async Task<List<PartnerDto>> GetPartners()
        {
            return await _context.PartnerDtos.FromSqlRaw("SELECT * FROM [Bzn].[GetPartners]()").ToListAsync();
        }

        public async Task<PartnerDto> GetPartner(int id)
        {
            return await _context.PartnerDtos.FromSqlRaw("SELECT * FROM [Bzn].[GetPartner]({0})", id).FirstOrDefaultAsync();
        }

        public async Task<Partner> GetPartnerById(int id)
        {
            return await _context.Partners.FirstOrDefaultAsync(i => i.Id == id);
        }


        public async Task<Partner> GetPartnerByPartnerService(int id)
        {
            return await _context.PartnerServices.Where(ix => ix.Id == id).Select(ix => ix.Partner).FirstOrDefaultAsync();
        }

        public async Task<int> GetPartnerMaxId()
        {
            try
            {
                return await _context.Partners.MaxAsync(i => i.Id);
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region Partner Service

        public async Task<List<PartnerServiceDispDto>> GetPartnerServiceDispDtos()
        {
            return await _context.PartnerServiceDispDtos.FromSqlRaw("SELECT * FROM [Bzn].[GetOurServices]()").ToListAsync();
        }

        public async Task<List<PartnerServiceDto>> GetPartnerServices(int partnerId)
        {
            return await _context.PartnerServiceDtos.FromSqlRaw("SELECT * FROM [Bzn].GetPartnerServices({0})", partnerId).ToListAsync();
        }

        public async Task<List<PartnerServiceDto>> GetServicePartners(int serviceId)
        {
            return await _context.PartnerServiceDtos.FromSqlRaw("SELECT * FROM  [Bzn].GetServicePartners({0})", serviceId).ToListAsync();
        }

        public async Task<PartnerServiceDto> GetPartnerService(int id)
        {
            return await _context.PartnerServiceDtos.FromSqlRaw("SELECT * FROM [Bzn].GetPartnerService({0})", id).FirstOrDefaultAsync();
        }
       

        public async Task<PartnerService> GetPartnerServiceById(int id)
        {
            return await _context.PartnerServices.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<int> GetPartnerServiceMaxId()
        {
            try
            {
                return await _context.PartnerServices.MaxAsync(i => i.Id);
            }
            catch
            {
                return 0;
            }
        }

        public async Task<PartnerServiceImage> GetPartnerServiceImage(int id)
        {
            return await _context.PartnerServiceImages.FirstOrDefaultAsync(ix => ix.Id == id);
        }

        #endregion

        #region Reservation


        public async Task<string> IsSeatAvailable(int partnerServiceId,DateTime reservationDate,int barberId)
        {
            var r = await _context.ScalarValueDtos.FromSqlRaw("exec [Bzn].[CheckIfSeatAvailable] {0},{1},{2}",partnerServiceId,reservationDate,barberId).ToListAsync();

            return r.FirstOrDefault().Result;
        }

        public async Task<List<ReservationDto>> GetReservations(string clientId)
        {
            return await _context.ReservationDtos.FromSqlRaw("SELECT * FROM [Bzn].[GetClientReservations]({0})", clientId).ToListAsync();
        }

        public async Task<List<ReservationDto>> GetClientReservations(string clientId)
        {
            return await _mapper.ProjectTo<ReservationDto>(_context.Reservations.Where(ix => ix.BusinessClientId == clientId).AsQueryable()).ToListAsync();
        }

        public async Task<List<ReservationDto>> GetReservations()
        {
            return await _context.ReservationDtos.FromSqlRaw("SELECT * FROM [Bzn].[GetReservations]()").ToListAsync();
        }

        public async Task<List<ReservationDto>> GetAllReservations()
        {
            return await _mapper.ProjectTo<ReservationDto>(_context.Reservations.AsQueryable()).ToListAsync();
        }

        public async Task<List<ReservationDto>> GetReservationBySerialNumber(string serialNumber)
        {
            return await _context.ReservationDtos.FromSqlRaw("SELECT * FROM [Bzn].[GetClientSerialNumberReservations]({0})", serialNumber).ToListAsync();
        }

        public async Task<List<ReservationDto>> GetSerialNumberReservations(string serialNumber)
        {
            return await _mapper.ProjectTo<ReservationDto>(_context.Reservations.Where(ix => ix.BusinessClient.SerialNumber == serialNumber).AsQueryable()).ToListAsync();
        }

        public async Task<List<ReservationDto>> GetReservations(int partnerId)
        {
            return await _context.ReservationDtos.FromSqlRaw("SELECT * FROM [Bzn].[GetPartnerReservations]({0})", partnerId).ToListAsync();
        }

        public async Task<List<ReservationDto>> GetPartnerReservations(int partnerId)
        {
            return await _mapper.ProjectTo<ReservationDto>(_context.Reservations.Where(ix => ix.PartnerId == partnerId).AsQueryable()).ToListAsync();
        }

        public async Task<ReservationDto> GetReservation(string id)
        {
            return await _context.ReservationDtos.FromSqlRaw("SELECT * FROM [Bzn].[GetReservation]({0})", id).FirstOrDefaultAsync();
        }

        public async Task<ReservationDto> GetOneReservation(string id)
        {
            return await _mapper.ProjectTo<ReservationDto>(_context.Reservations.Where(ix => ix.Id == id).AsQueryable()).FirstOrDefaultAsync();
        }

        public async Task<Reservation> GetReservationById(string id)
        {
            return await _context.Reservations.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<ReservationDetail> GetReservationDetailById(string id)
        {
            return await _context.ReservationDetails.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<ReservationDetail>> GetReservationDetails(string reservationId)
        {
            return await _context.ReservationDetails.Where(ix => ix.ReservationId == reservationId).ToListAsync();
        }

        public async Task<string> GetReservationMaxId()
        {
            var y = DateTime.Now.Year.ToString().Substring(2, 2);
            var m = DateTime.Now.Month.ToString().PadLeft(2, '0');
            var d = DateTime.Now.Day.ToString().PadLeft(2, '0');
            var code = y + m + d;
            try
            {
                var r = await _context.Reservations.Where(i => i.Code == code).MaxAsync(k => k.Id);
                if (string.IsNullOrWhiteSpace(r)) return code + "00000";

                return r;
            }
            catch
            {
                return code + "00000";
            }
        }

        #endregion

        #region Movement

        public async Task<int> GetMovementMaxId()
        {
            try
            {
                return await _context.ClientMovements.MaxAsync(ix => ix.Id);
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region Role

        public async Task<List<Role>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRole(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<int> GetRoleMaxId()
        {
            try
            {
                return await _context.Roles.MaxAsync(i => i.Id);
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region User

        public async Task<List<UserDto>> GetUsers()
        {
            return await _context.UserDtos.FromSqlRaw("SELECT * FROM [Security].[GetUsers]()").ToListAsync();
        }

        public async Task<UserDto> GetUser(Guid id)
        {
            return await _context.UserDtos.FromSqlRaw("SELECT * FROM [Security].[GetUser]({0})", id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<UserAuthenticationDto> GetUser(string username,string password)
        {
            return await _context.UserAuthenticationDtos.FromSqlRaw("SELECT * FROM [Security].AuthenticateUser({0},{1})", username, password).FirstOrDefaultAsync();
        }

        public async Task<User> GetUser(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<List<UserCredentialsDto>> GetUserCredentials()
            => await _mapper.ProjectTo<UserCredentialsDto>(_context.Users.AsQueryable()).ToListAsync();

        public async Task<bool> DoesUsernameExists(string username) => await _context.Users.AnyAsync(ix => ix.Username == username);

        public string BuildToken(UserAuthenticationDto user)
        {
            // adding claims, custom claims and identity claims.
            var claims = new[] {
        new Claim("Role",user.RoleId.ToString()),
        //new Claim("Role",user.RoleId),
        new Claim("UserId", user.Id.ToString()),
        new Claim("Names",user.Names),
        new Claim("RoleName",user.Role),
        new Claim("PartnerId",user.PartnerId.ToString()),
        new Claim("ClientId",user.ClientId),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Connection.JwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Connection.JwtIssuer,
              Connection.JwtIssuer,
              claims,
              notBefore: DateTime.Now,
              expires: DateTime.Now.AddHours(3),
              signingCredentials: creds
              );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
