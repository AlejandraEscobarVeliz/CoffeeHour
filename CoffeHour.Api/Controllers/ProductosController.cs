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
    /// Controlador encargado de gestionar los productos.
    /// </summary>
    [ApiController]
    [Route("api/coffee/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] ProductoQueryFilter filter)
        {
            // ✅ Sin await - Query() devuelve IQueryable
            var query = _unitOfWork.Productos.Query();

            // Aplicar filtros...
            if (!string.IsNullOrEmpty(filter.Categoria))
                query = query.Where(p => p.Categoria == filter.Categoria);

            if (!string.IsNullOrEmpty(filter.Estado))
                query = query.Where(p => p.Estado == filter.Estado);

            // Paginación
            var total = query.Count();
            var productos = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            var result = new
            {
                Data = _mapper.Map<IEnumerable<ProductoDTO>>(productos),
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
            var producto = await _unitOfWork.Productos.GetByIdAsync(id);
            if (producto is null)
                return NotFound(new ApiResponse<string>("Producto no encontrado", false));

            return Ok(new ApiResponse<ProductoDTO>(_mapper.Map<ProductoDTO>(producto)));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductoDTO dto)
        {
            var entity = _mapper.Map<Productos>(dto);
            await _unitOfWork.Productos.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.Id },
                new ApiResponse<ProductoDTO>(_mapper.Map<ProductoDTO>(entity)));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductoDTO dto)
        {
            if (id != dto.IdProducto)
                return BadRequest(new ApiResponse<string>("El ID no coincide", false));

            var existing = await _unitOfWork.Productos.GetByIdAsync(id);
            if (existing is null)
                return NotFound(new ApiResponse<string>("Producto no encontrado", false));

            _mapper.Map(dto, existing);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<ProductoDTO>(_mapper.Map<ProductoDTO>(existing)));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _unitOfWork.Productos.GetByIdAsync(id);
            if (producto is null)
                return NotFound(new ApiResponse<string>("Producto no encontrado", false));

            await _unitOfWork.Productos.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<string>("Producto eliminado correctamente"));
        }
    }
}



