using AutoMapper;
using Minimart.Core.Domain.Models;
using Minimart.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minimart.WebApi.Mapping
{
     
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Store, StoreResource>();


            CreateMap<StoreOpenDay, StoreOpenDaysResource>()
                .ForMember(x => x.From, opt => opt.MapFrom(src => src.From.ToString("HH:mm")))
                .ForMember(x => x.To, opt => opt.MapFrom(src => src.To.ToString("HH:mm")))
                .ForMember(x => x.WeekDay, opt => opt.MapFrom(src => Enum.GetName(typeof(EWeekDays), src.WeekDay)));


            CreateMap<Product, ProductResource>();

            //cart
            CreateMap<Cart, CartResource>();
            CreateMap<Store, CartStoreResource>();
            CreateMap<CartItem, CartItemResource>();
            CreateMap<Product, ProductItemResource>();
        }
    }
}
