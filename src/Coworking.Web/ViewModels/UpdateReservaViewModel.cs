using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Coworking.Web.ViewModels
{
    public class UpdateReservaViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public Guid SalaId { get; set; }

        [Required]
        public Guid UsuarioId { get; set; }

        [Required]
        public DateTime DataInicioReserva { get; set; }

        [Required]
        public DateTime DataFimReserva { get; set; }

        [JsonIgnore]
        public List<SelectListItem> Salas { get; set; }

        [JsonIgnore]
        public List<SelectListItem> Usuarios { get; set; }
    }
}
