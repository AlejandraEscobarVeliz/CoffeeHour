using AutoMapper;
using CoffeeHour.Api.Responses;
using CoffeHour.Core.DTOs;
using CoffeHour.Core.Entities;
using CoffeHour.Core.Interfaces;
using CoffeHour.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CoffeHour.Api.Controllers
{
    /// <summary>
    /// Controlador encargado de la gestión de pedidos.
    /// </summary>
    [ApiController]
    [Route("api/coffee/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PedidosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pedidos = await _unitOfWork.Pedidos.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<PedidoDTO>>(_mapper.Map<IEnumerable<PedidoDTO>>(pedidos)));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pedido = await _unitOfWork.Pedidos.GetByIdAsync(id);
            if (pedido is null)
                return NotFound(new ApiResponse<string>("Pedido no encontrado", false));

            return Ok(new ApiResponse<PedidoDTO>(_mapper.Map<PedidoDTO>(pedido)));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PedidoDTO dto)
        {
            if (dto.IdCliente == null || dto.Total <= 0)
                return BadRequest(new ApiResponse<string>("El pedido debe tener cliente y total mayor a 0", false));

            var entity = _mapper.Map<Pedidos>(dto);
            await _unitOfWork.Pedidos.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.Id },
                new ApiResponse<PedidoDTO>(_mapper.Map<PedidoDTO>(entity)));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await _unitOfWork.Pedidos.GetByIdAsync(id);
            if (pedido is null)
                return NotFound(new ApiResponse<string>("Pedido no encontrado", false));

            await _unitOfWork.Pedidos.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<string>("Pedido eliminado correctamente"));
        }
    }
}
