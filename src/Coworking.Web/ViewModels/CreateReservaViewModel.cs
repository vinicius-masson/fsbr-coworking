using System.ComponentModel.DataAnnotations;

namespace Coworking.Web.ViewModels
{
    public class CreateReservaViewModel
    {
        [Required(ErrorMessage = "Sala é obrigatória")]
        public Guid SalaId { get; set; }

        [Required(ErrorMessage = "Usuário é obrigatório")]
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "Data de início é obrigatória")]
        //[DataType(DataType.DateTime)]
        public DateTime DataInicioReserva { get; set; }

        [Required(ErrorMessage = "Data de término é obrigatória")]
        //[DataType(DataType.DateTime)]
        //[DateGreaterThan("DataInicio", ErrorMessage = "Data final deve ser após a data inicial")]
        public DateTime DataFimReserva { get; set; }
    }
}
