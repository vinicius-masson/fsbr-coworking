using Coworking.Common.Validation;

namespace Coworking.Common.Response
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<ValidationErrorDetail> Errors { get; set; } = Array.Empty<ValidationErrorDetail>();
    }
}
