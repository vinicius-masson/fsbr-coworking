using AutoMapper;
using Coworking.Aplication.Commands.Reservas.CreateReserva;
using Coworking.Aplication.Commands.Reservas.UpdateReserva;
using Coworking.Aplication.Queries.Reservas.GetReserva;
using Coworking.Domain.Entities;

namespace Coworking.Aplication.Profiles
{
    public class ReservaProfile : Profile
    {
        public ReservaProfile()
        {
            CreateMap<CreateReservaCommand, Reserva>();
            CreateMap<Reserva, CreateReservaResponse>();

            CreateMap<UpdateReservaCommand, Reserva>();

            CreateMap<Reserva, GetReservaResponse>()
                .ForMember(dest => dest.UsuarioNome, opt => opt.MapFrom(src => src.Usuario.Nome))
                .ForMember(dest => dest.SalaDescricao, opt => opt.MapFrom(src => src.Sala.Descricao))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
