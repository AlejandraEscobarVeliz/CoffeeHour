using AutoMapper;
using CoffeeHour.Api.Responses;
using CoffeHour.Core.Entities;
using CoffeHour.Core.Interfaces;
using CoffeHour.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CoffeHour.Api.Controllers
{
    [ApiController]
    [Route("api/coffee/[controller]")]
    public class DetallesPedidoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DetallesPedidoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

         [HttpGet]
        public IActionResult GetAll()
        {
            // ✅ Sin await
            var detalles = _unitOfWork.Detalles.GetAll();
            return Ok(new ApiResponse<IEnumerable<DetallePedidoDTO>>(
                _mapper.Map<IEnumerable<DetallePedidoDTO>>(detalles)));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var detalle = await _unitOfWork.Detalles.GetByIdAsync(id);
            if (detalle is null)
                return NotFound(new ApiResponse<string>("Detalle no encontrado", false));

            return Ok(new ApiResponse<DetallePedidoDTO>(_mapper.Map<DetallePedidoDTO>(detalle)));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DetallePedidoDTO dto)
        {
            if (dto.Cantidad <= 0 || dto.Subtotal <= 0)
                return BadRequest(new ApiResponse<string>("La cantidad y el subtotal deben ser mayores que 0", false));

            var entity = _mapper.Map<DetallesPedido>(dto);
            await _unitOfWork.Detalles.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.Id },
                new ApiResponse<DetallePedidoDTO>(_mapper.Map<DetallePedidoDTO>(entity)));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var detalle = await _unitOfWork.Detalles.GetByIdAsync(id);
            if (detalle is null)
                return NotFound(new ApiResponse<string>("Detalle no encontrado", false));

            await _unitOfWork.Detalles.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<string>("Detalle eliminado correctamente"));
        }
    }
}

