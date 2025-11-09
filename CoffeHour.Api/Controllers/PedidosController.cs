// CoffeHour.Api/Controllers/PedidosController.cs
using AutoMapper;
using CoffeeHour.Api.Responses;
using CoffeHour.Core.DTOs;
using CoffeHour.Core.Entities;
using CoffeHour.Core.Interfaces;
using CoffeHour.Core.QueryFilters;
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

        /// <summary>
        /// Obtiene todos los pedidos con filtros opcionales.
        /// </summary>
        [HttpGet]
        public IActionResult GetAll([FromQuery] PedidoQueryFilter filter)
        {
            // ✅ CAMBIO: GetAll() sin await
            var query = _unitOfWork.Pedidos.Query();

            // Aplicar filtros
            if (filter.IdCliente.HasValue)
                query = query.Where(p => p.IdCliente == filter.IdCliente.Value);

            if (filter.FechaInicio.HasValue)
                query = query.Where(p => p.Fecha >= filter.FechaInicio.Value);

            if (filter.FechaFin.HasValue)
                query = query.Where(p => p.Fecha <= filter.FechaFin.Value);

            if (!string.IsNullOrEmpty(filter.Estado))
                query = query.Where(p => p.Estado == filter.Estado);

            if (filter.MontoMin.HasValue)
                query = query.Where(p => p.Total >= filter.MontoMin.Value);

            if (filter.MontoMax.HasValue)
                query = query.Where(p => p.Total <= filter.MontoMax.Value);

            // Ordenamiento
            query = filter.OrderBy?.ToLower() switch
            {
                "fecha" => filter.OrderDesc
                    ? query.OrderByDescending(p => p.Fecha)
                    : query.OrderBy(p => p.Fecha),
                "total" => filter.OrderDesc
                    ? query.OrderByDescending(p => p.Total)
                    : query.OrderBy(p => p.Total),
                _ => query.OrderByDescending(p => p.Fecha) // Por defecto más recientes primero
            };

            // Paginación
            var total = query.Count();
            var pedidos = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            var result = new
            {
                Data = _mapper.Map<IEnumerable<PedidoDTO>>(pedidos),
                Pagination = new
                {
                    filter.PageNumber,
                    filter.PageSize,
                    TotalRecords = total,
                    TotalPages = (int)Math.Ceiling((double)total / filter.PageSize),
                    HasNextPage = filter.PageNumber < (int)Math.Ceiling((double)total / filter.PageSize),
                    HasPreviousPage = filter.PageNumber > 1
                }
            };

            return Ok(new ApiResponse<object>(result));
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
            await _unitOfWork.SaveChangesAsync(); // ✅ UnitOfWork guarda

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
            await _unitOfWork.SaveChangesAsync(); // ✅ UnitOfWork guarda

            return Ok(new ApiResponse<string>("Pedido eliminado correctamente"));
        }

        /// <summary>
        /// Crea un pedido completo con sus detalles (Caso de Uso 1).
        /// </summary>
        [HttpPost("completo")]
        public async Task<IActionResult> CreatePedidoCompleto([FromBody] PedidoDTO dto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var pedidoEntity = _mapper.Map<Pedidos>(dto);
                var detalles = dto.DetallesPedido.Select(d => _mapper.Map<DetallesPedido>(d)).ToList();

                var newId = await _unitOfWork.Pedidos.CreateOrderAsync(pedidoEntity, detalles);

                await _unitOfWork.CommitAsync();

                var created = await _unitOfWork.Pedidos.GetByIdAsync(newId);
                return StatusCode(201, new ApiResponse<PedidoDTO>(_mapper.Map<PedidoDTO>(created)));
            }
            catch (Core.Exceptions.BusinessException ex)
            {
                await _unitOfWork.RollbackAsync();
                return StatusCode(ex.StatusCode, new { Message = ex.Message, ErrorCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return StatusCode(500, new { Message = "Error interno del servidor", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Cambia el estado de un pedido (Caso de Uso 2).
        /// </summary>
        [HttpPut("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstadoPedido(int id, [FromBody] ChangeStatusDTO dto)
        {
            try
            {
                var ok = await _unitOfWork.Pedidos.ChangeOrderStatusAsync(id, dto.NuevoEstado);
                if (!ok)
                    return NotFound(new ApiResponse<string>("Pedido no encontrado", false));

                await _unitOfWork.SaveChangesAsync();

                var updated = await _unitOfWork.Pedidos.GetByIdAsync(id);
                return Ok(new ApiResponse<PedidoDTO>(_mapper.Map<PedidoDTO>(updated)));
            }
            catch (Core.Exceptions.BusinessException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
