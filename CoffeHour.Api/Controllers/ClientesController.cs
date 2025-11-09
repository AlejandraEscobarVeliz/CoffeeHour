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
    /// Controlador encargado de gestionar los clientes del sistema.
    /// </summary>
    /// <remarks>
    /// Caso de uso 1: Registrar Cliente con validaciones.
    /// </remarks>
    [ApiController]
    [Route("api/coffee/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los clientes con soporte de paginación y búsqueda.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryFilter filter)
        {
            var clientes = await _unitOfWork.Clientes.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                clientes = clientes.Where(c => c.Nombre.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase));

            int total = clientes.Count();
            var paged = clientes
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            var result = new
            {
                Data = _mapper.Map<IEnumerable<ClienteDTO>>(paged),
                Pagination = new
                {
                    filter.PageNumber,
                    filter.PageSize,
                    TotalRecords = total,
                    TotalPages = (int)Math.Ceiling((double)total / filter.PageSize)
                }
            };

            return Ok(new ApiResponse<object>(result));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (cliente is null)
                return NotFound(new ApiResponse<string>("Cliente no encontrado", false));

            return Ok(new ApiResponse<ClienteDTO>(_mapper.Map<ClienteDTO>(cliente)));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClienteDTO dto)
        {
            var entity = _mapper.Map<Clientes>(dto);
            await _unitOfWork.Clientes.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.Id },
                new ApiResponse<ClienteDTO>(_mapper.Map<ClienteDTO>(entity)));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClienteDTO dto)
        {
            if (id != dto.IdCliente)
                return BadRequest(new ApiResponse<string>("El Id no coincide", false));

            var existing = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (existing is null)
                return NotFound(new ApiResponse<string>("Cliente no encontrado", false));

            _mapper.Map(dto, existing);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<ClienteDTO>(_mapper.Map<ClienteDTO>(existing)));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (cliente is null)
                return NotFound(new ApiResponse<string>("Cliente no encontrado", false));

            await _unitOfWork.Clientes.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<string>("Cliente eliminado correctamente"));
        }
    }
}

