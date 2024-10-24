using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeCareWebApp.Entities;
using WeCareWebApp.Models;

namespace WeCareWebApp.Helpers
{
    public class MappingHelper : Profile
    {
        public MappingHelper()
        {
            CreateMap<Barber, BarberDto>()
                .ForMember(d => d.Partner, m => m.MapFrom(s => s.Partner.Name));

            CreateMap<Reservation, ReservationDto>()
                .ForMember(d => d.Details, m => m.MapFrom(s => s.Details))
                .ForMember(d => d.Partner, m => m.MapFrom(s => s.Partner.Name))
                .ForMember(d => d.ClientId, m => m.MapFrom(s => s.BusinessClientId))
                .ForMember(d => d.Client, m => m.MapFrom(s => s.BusinessClient.Name))
                .ForMember(d => d.ReservationStatus, m => m.MapFrom(s => s.ReservationStatus.Name))
                .ForMember(d => d.Details, m => m.MapFrom(s => s.Details));

            CreateMap<ReservationDetail, ReservationDetailDto>()
                .ForMember(d => d.Partner, m => m.MapFrom(s => s.PartnerService.Partner.Name))
                .ForMember(d => d.PartnerId, m => m.MapFrom(s => s.PartnerService.PartnerId))
                .ForMember(d => d.Barber, m => m.MapFrom(s => s.Barber.Names))
                .ForMember(d => d.Service, m => m.MapFrom(s => s.PartnerService.BusinessService.Name))
                .ForMember(d => d.ServiceId, m => m.MapFrom(s => s.PartnerService.BusinessServiceId));

            CreateMap<PartnerService, PartnerServiceDto>();

            CreateMap<User, UserCredentialsDto>()
                .ForMember(d => d.Role,m => m.MapFrom(s =>s.Role == null?"": s.Role.Name))
                .ForMember(d => d.Password, m => m.MapFrom(s => EncryptionHelper.Decrypt(s.Password)));

            
        }
    }
}
