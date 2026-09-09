using AutoMapper;
using ShopHub.Application.DTOs.CatalogDTOs;
using ShopHub.Domain.Entities.Catalog;
using ShopHub.Domain.Entities.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // -- Catalog Mapping 
            CreateMap<Product, ProductDto>() // Src Product / Des ProductDto
                .ForMember(des => des.ProductType ,
                P => P.MapFrom(src => src.ProductType.Name));

            CreateMap<ProductType, ProductTypeDto>();
            CreateMap<ProductBrand, ProductBrandDto>();
        }
        
    }
}
