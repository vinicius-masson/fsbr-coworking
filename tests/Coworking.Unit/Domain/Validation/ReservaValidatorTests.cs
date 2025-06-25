using Coworking.Domain.Validation;
using Coworking.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;

namespace Coworking.Unit.Domain.Validation
{
    public class ReservaValidatorTests
    {
        private readonly ReservaValidator _validator;

        public ReservaValidatorTests()
        {
            _validator = new ReservaValidator();
        }

        [Fact(DisplayName = "Valid reservation should pass all validation rules")]
        public void Given_ValidReservation_When_Validated_Then_ShoulNotHaveErrors()
        {
            //Arrange
            var reserva = ReservaTestData.GenerateValidReservation();

            //Act
            var result = _validator.TestValidate(reserva);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
