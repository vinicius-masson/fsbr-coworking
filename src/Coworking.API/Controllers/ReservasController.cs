using AutoMapper;
using Coworking.API.Validation;
using Coworking.Aplication.Commands.Reservas.CancelReserva;
using Coworking.Aplication.Commands.Reservas.CreateReserva;
using Coworking.Aplication.Commands.Reservas.UpdateReserva;
using Coworking.Aplication.Exceptions;
using Coworking.Aplication.Queries.Reservas.GetAllReservas;
using Coworking.Aplication.Queries.Reservas.GetReserva;
using Coworking.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coworking.API.Controllers
{
    [ApiController]
    [Route("api/reservas")]
    public class ReservasController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ReservasController> _logger;

        public ReservasController(IMediator mediator, IMapper mapper, ILogger<ReservasController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateReservaResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateReserva([FromBody] CreateReservaCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateReservaCommandValidator();
                var validationResult = await validator.ValidateAsync(command, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var response = await _mediator.Send(command, cancellationToken);

                return Created(string.Empty, new ApiResponseWithData<CreateReservaResponse>
                {
                    Success = true,
                    Message = "Reserva efetuada com sucesso",
                    Data = response
                });
            }
            catch (Exception ex) when (ex is BusinessException or NotFoundException)
            {
                _logger.LogError(ex, "Erro ao cancelar reserva");
                throw;
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateReservaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateReserva([FromRoute]Guid id, [FromBody]UpdateReservaCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ApiResponse { Success = false, Message = "ID inválido" });

                command.Id = id;

                var validator = new UpdateReservaCommandValidator();
                var validationResult = await validator.ValidateAsync(command, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var response = await _mediator.Send(command, cancellationToken);

                return Ok(new ApiResponseWithData<UpdateReservaResponse>
                {
                    Success = true,
                    Message = "Reserva atualizada com sucesso",
                    Data = response
                });
            }
            catch (Exception ex) when (ex is BusinessException or NotFoundException)
            {
                _logger.LogError(ex, "Erro ao cancelar reserva");
                throw;
            }
        }

        [HttpPatch("{id}/cancel")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelReserva([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var command = new CancelReservaCommand { Id = id };
                var validator = new CancelReservaCommandValidator();
                var validationResult = await validator.ValidateAsync(command, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                await _mediator.Send(command, cancellationToken);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Reserva cancelada com sucesso"
                });
            }
            catch (Exception ex) when (ex is BusinessException or NotFoundException)
            {
                _logger.LogError(ex, "Erro ao cancelar reserva");
                throw;
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetReservaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReserva([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var request = new GetReservaQuery { Id = id };
                var validator = new GetReservaQueryValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var response = await _mediator.Send(request, cancellationToken);

                return Ok(new ApiResponseWithData<GetReservaResponse>
                {
                    Success = true,
                    Message = "Reserva recuperada com sucesso",
                    Data = response
                });
            }
            catch (Exception ex) when (ex is BusinessException or NotFoundException)
            {
                _logger.LogError(ex, "Erro ao cancelar reserva");
                throw;
            }
           
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<GetReservaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllReservas([FromQuery] GetAllReservasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(request, cancellationToken);

                return Ok(new ApiResponseWithData<List<GetReservaResponse>>
                {
                    Success = true,
                    Message = "Reservas recuperadas com sucesso",
                    Data = response
                });
            }
            catch (Exception ex) when (ex is BusinessException or NotFoundException)
            {
                _logger.LogError(ex, "Erro ao cancelar reserva");
                throw;
            }
        }
    }
}
