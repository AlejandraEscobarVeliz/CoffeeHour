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
        public async Task<IActionResult> GetAll()
        {
            var productos = await _unitOfWork.Productos.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ProductoDTO>>(_mapper.Map<IEnumerable<ProductoDTO>>(productos)));
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



