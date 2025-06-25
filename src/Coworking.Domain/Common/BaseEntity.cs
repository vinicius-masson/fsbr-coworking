using Coworking.Common.Validation;

namespace Coworking.Domain.Common
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime DataCriacaoRegistro { get; set; }

        public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
        {
            return Validator.ValidateAsync(this);
        }
    }
}
