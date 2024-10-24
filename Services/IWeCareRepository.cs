using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeCareWebApp.Entities;
using WeCareWebApp.Models;

namespace WeCareWebApp.Services
{
    public interface IWeCareRepository
    {
        #region Base

        /// <summary>
        /// Saves the changes made on the context to update record in the database
        /// </summary>
        /// <returns></returns>
        Task SaveChanges();

        /// <summary>
        /// Generic Add to context for creation or addition of a new record to a given class represented by T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Add<T>(T entity) where T : class;


        /// <summary>
        /// Adding a list of objects of Type To the DbContext as Range
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Add<T>(List<T> entity) where T : class;

        /// <summary>
        /// Deleting a single object or a list of a given object from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Delete<T>(T entity) where T : class;


        /// <summary>
        /// Remove a list of objects of type T from the database context wich will result in a deletion
        /// of those from the databse when save changes is called
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Delete<T>(List<T> entity) where T : class;

        /// <summary>
        /// Update a single object or a list of given object being tracked
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Update<T>(T entity) where T : class;

        /// <summary>
        /// Updating a range or a list of object of Type T being tracked and will take effect whe save changes is called
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Update<T>(List<T> entity) where T : class;

        /// <summary>
        /// Fetch data from database of any given Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<object> Get<T>() where T : class;




        #endregion Base

        #region Business Client

        // Method to get all BusinessClient record from the database
        Task<List<BusinessClient>> GetBusinessClients();

        // Method to get a specific record from the database by its identifier
        Task<BusinessClient> GetBusinessClient(string id);

        Task<BusinessClient> GetBusinessClientBySerialNumber(string serialNumber);

        #endregion

        #region Business Service

        // Method to get all BusinessService record from the database
        Task<List<BusinessService>> GetBusinessServices();

        // Method to get a specific record from the database by its identifier
        Task<BusinessService> GetBusinessService(int id);

        // Method to get the max record identifier
        Task<int> GetBusinessServiceMaxId();

        #endregion

        #region Barber

        Task<IEnumerable<BarberDto>> GetBarbers();
        Task<IEnumerable<BarberDto>> GetBarbers(int partnerId);
        Task<BarberDto> GetBarber(int id);
        Task<Barber> GetBarberById(int id);
        Task<int> GetBarberMaxId();

        #endregion

        #region Partner

        Task<List<PartnerDto>> GetPartners();

        Task<PartnerDto> GetPartner(int id);

        Task<Partner> GetPartnerById(int id);

        Task<Partner> GetPartnerByPartnerService(int id);

        Task<int> GetPartnerMaxId();

        #endregion

        #region Partner Service

        Task<List<PartnerServiceDispDto>> GetPartnerServiceDispDtos();
        Task<List<PartnerServiceDto>> GetPartnerServices(int partnerId);

        Task<PartnerServiceDto> GetPartnerService(int id);

        Task<List<PartnerServiceDto>> GetServicePartners(int serviceId);

        Task<PartnerService> GetPartnerServiceById(int id);

        Task<int> GetPartnerServiceMaxId();

        Task<PartnerServiceImage> GetPartnerServiceImage(int id);


        #endregion

        #region Reservation

        Task<string> IsSeatAvailable(int partnerServiceId, DateTime reservationDate,int barberId);
        Task<List<ReservationDto>> GetReservations(string clientId);
        Task<List<ReservationDto>> GetClientReservations(string clientId);
        Task<List<ReservationDto>> GetReservations();
        Task<List<ReservationDto>> GetAllReservations();
        Task<List<ReservationDto>> GetReservationBySerialNumber(string serialNumber);
        Task<List<ReservationDto>> GetSerialNumberReservations(string serialNumber);
        Task<List<ReservationDto>> GetReservations(int partnerId);
        Task<List<ReservationDto>> GetPartnerReservations(int partnerId);

        Task<ReservationDto> GetReservation(string id);
        Task<ReservationDto> GetOneReservation(string id);

        Task<Reservation> GetReservationById(string id);

        Task<ReservationDetail> GetReservationDetailById(string id);

        Task<string> GetReservationMaxId();


        Task<List<ReservationDetail>> GetReservationDetails(string reservationId);

        #endregion

        #region Movement

        Task<int> GetMovementMaxId();

        #endregion

        #region Role

        Task<List<Role>> GetRoles();

        Task<Role> GetRole(int id);

        Task<int> GetRoleMaxId();

        #endregion

        #region User

        Task<List<UserDto>> GetUsers();

        Task<UserDto> GetUser(Guid id);

        Task<User> GetUserById(Guid id);

        Task<User> GetUser(string username);

        Task<bool> DoesUsernameExists(string username);

        Task<UserAuthenticationDto> GetUser(string username, string password);

        Task<List<UserCredentialsDto>> GetUserCredentials();

        string BuildToken(UserAuthenticationDto user);

        #endregion
    }
}
