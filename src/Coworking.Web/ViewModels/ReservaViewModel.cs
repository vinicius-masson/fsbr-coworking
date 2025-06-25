using System.ComponentModel.DataAnnotations;

namespace Coworking.Web.ViewModels
{
    public class ReservaViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Data/Hora Início")]
        public DateTime DataInicio { get; set; }

        [Required]
        [Display(Name = "Data/Hora Fim")]
        public DateTime DataFim { get; set; }

        [Required]
        [Display(Name = "Sala")]
        public Guid SalaId { get; set; }

        [Required]
        [Display(Name = "Usuário")]
        public Guid UsuarioId { get; set; }

        public string UsuarioNome { get; set; } = string.Empty;
        public string SalaDescricao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
