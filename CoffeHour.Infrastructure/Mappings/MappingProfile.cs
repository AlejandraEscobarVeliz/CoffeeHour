using AutoMapper;
using CoffeHour.Core.DTOs;
using CoffeHour.Core.Entities;
using CoffeHour.Infrastructure.DTOs;

namespace CoffeHour.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Clientes, ClienteDTO>().ReverseMap();
            CreateMap<Productos, ProductoDTO>().ReverseMap();
            CreateMap<Pedidos, PedidoDTO>().ReverseMap();
            CreateMap<DetallesPedido, DetallePedidoDTO>().ReverseMap();
           
            //////
            CreateMap<ChangeStatusDTO, Pedidos>().ForMember(d => d.Estado, o => o.MapFrom(s => s.NuevoEstado));
            CreateMap<SalesReportDTO, SalesReportDTO>();
        }
    }
}

