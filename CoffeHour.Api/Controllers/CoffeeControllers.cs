using AutoMapper;
using CoffeeHour.Api.Responses;
using CoffeHour.Core.DTOs;
using CoffeHour.Core.Entities;
using CoffeHour.Core.Interfaces;
using CoffeHour.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHour.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoffeeController : ControllerBase
    {
        private readonly IClienteRepository _clientes;
        private readonly IProductoRepository _productos;
        private readonly IPedidoRepository _pedidos;
        private readonly IDetallePedidoRepository _detalles;
        private readonly IMapper _mapper;

        public CoffeeController(
            IClienteRepository clientes,
            IProductoRepository productos,
            IPedidoRepository pedidos,
            IDetallePedidoRepository detalles,
            IMapper mapper)
        {
            _clientes = clientes;
            _productos = productos;
            _pedidos = pedidos;
            _detalles = detalles;
            _mapper = mapper;
        }

        // ---------- CLIENTES (CRUD simple) ----------
        [HttpGet("clientes")]
        public async Task<IActionResult> GetClientes()
        {
            var list = await _clientes.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ClienteDTO>>(_mapper.Map<IEnumerable<ClienteDTO>>(list)));
        }

        [HttpGet("clientes/{id:int}")]
        public async Task<IActionResult> GetClienteById(int id)
        {
            var cliente = await _clientes.GetByIdAsync(id);
            if (cliente is null) return NotFound(new ApiResponse<string>("Cliente no encontrado", false));
            return Ok(new ApiResponse<ClienteDTO>(_mapper.Map<ClienteDTO>(cliente)));
        }


        [HttpPost("clientes")]
        public async Task<IActionResult> PostCliente([FromBody] Clientes dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { errors });
            }

            await _clientes.AddAsync(dto);
            return CreatedAtAction(nameof(GetClienteById), new { id = dto.IdCliente }, dto);
        }
        
        [HttpPut("clientes/{id:int}")]
        public async Task<IActionResult> PutCliente(int id, [FromBody] ClienteDTO dto)
        {
            if (id != dto.IdCliente) return BadRequest(new ApiResponse<string>("Id no coincide", false));
            var existing = await _clientes.GetByIdAsync(id);
            if (existing is null) return NotFound(new ApiResponse<string>("Cliente no encontrado", false));
            _mapper.Map(dto, existing);
            await _clientes.UpdateAsync(existing);
            return Ok(new ApiResponse<ClienteDTO>(_mapper.Map<ClienteDTO>(existing)));
        }

        [HttpDelete("clientes/{id:int}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _clientes.GetByIdAsync(id);
            if (cliente is null) return NotFound(new ApiResponse<string>("Cliente no encontrado", false));
            await _clientes.DeleteAsync(id);
            return NoContent();
        }

        // ---------- PRODUCTOS ----------
        [HttpGet("productos")]
        public async Task<IActionResult> GetProductos()
        {
            var list = await _productos.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ProductoDTO>>(_mapper.Map<IEnumerable<ProductoDTO>>(list)));
        }

        [HttpPost("productos")]
        public async Task<IActionResult> PostProducto([FromBody] ProductoDTO dto)
        {
            var entity = _mapper.Map<Productos>(dto);
            await _productos.AddAsync(entity);
            return StatusCode(201, new ApiResponse<ProductoDTO>(_mapper.Map<ProductoDTO>(entity)));
        }

        [HttpPut("productos/{id:int}")]
        public async Task<IActionResult> PutProducto(int id, [FromBody] Productos dto)
        {
            if (id != dto.IdProducto) return BadRequest("Id no coincide.");
            var existing = await _productos.GetByIdAsync(id);
            if (existing is null) return NotFound();

            _mapper.Map(dto, existing);
            await _productos.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("productos/{id:int}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _productos.GetByIdAsync(id);
            if (producto is null) return NotFound();
            await _productos.DeleteAsync(id);
            return NoContent();
        }
        // ---------- PEDIDOS (CRUD + Casos de uso) ----------
        [HttpGet("pedidos")]
        public async Task<IActionResult> GetPedidos()
        {
            var list = await _pedidos.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<PedidoDTO>>(_mapper.Map<IEnumerable<PedidoDTO>>(list)));
        }

        [HttpGet("pedidos/{id:int}")]
        public async Task<IActionResult> GetPedidoById(int id)
        {
            var pedido = await _pedidos.GetByIdAsync(id);
            if (pedido is null) return NotFound(new ApiResponse<string>("Pedido no encontrado", false));
            return Ok(new ApiResponse<PedidoDTO>(_mapper.Map<PedidoDTO>(pedido)));
        }

        // Crear pedido simple (si lo necesitas)
        [HttpPost("pedidos")]
        public async Task<IActionResult> PostPedido([FromBody] PedidoDTO dto)
        {
            var entity = _mapper.Map<Pedidos>(dto);
            await _pedidos.AddAsync(entity);
            return StatusCode(201, new ApiResponse<PedidoDTO>(_mapper.Map<PedidoDTO>(entity)));
        }


        // ---------- Actualizar un pedido ----------
        [HttpPut("pedidos/{id:int}")]
        public async Task<IActionResult> PutPedido(int id, [FromBody] PedidoDTO dto)
        {
            if (id != dto.IdPedido)
                return BadRequest(new ApiResponse<string>("El ID del pedido no coincide.", false));

            var existing = await _pedidos.GetByIdAsync(id);
            if (existing is null)
                return NotFound(new ApiResponse<string>("Pedido no encontrado.", false));

            _mapper.Map(dto, existing);
            await _pedidos.UpdateAsync(existing);

            return Ok(new ApiResponse<PedidoDTO>(_mapper.Map<PedidoDTO>(existing)));
        }

        // ---------- Eliminar un pedido ----------
        [HttpDelete("pedidos/{id:int}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var existing = await _pedidos.GetByIdAsync(id);
            if (existing is null)
                return NotFound(new ApiResponse<string>("Pedido no encontrado.", false));

            await _pedidos.DeleteAsync(id);
            return NoContent(); 
        }




        // ===== CASO 1: Crear pedido completo con detalles =====
        // POST /api/coffee/pedidos/completo
        [HttpPost("pedidos/completo")]
        public async Task<IActionResult> CreatePedidoCompleto([FromBody] PedidoDTO dto)
        {
            try
            {
                var pedidoEntity = _mapper.Map<Pedidos>(dto);
                var detalles = dto.DetallesPedido.Select(d => _mapper.Map<DetallesPedido>(d)).ToList();
                var newId = await _pedidos.CreateOrderAsync(pedidoEntity, detalles);
                var created = await _pedidos.GetByIdAsync(newId);
                return StatusCode(201, new ApiResponse<PedidoDTO>(_mapper.Map<PedidoDTO>(created)));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message, false));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>("Error interno: " + ex.Message, false));
            }
        }

        // ===== CASO 2: Cambiar estado =====
        // PUT /api/coffee/pedidos/{id}/estado
        [HttpPut("pedidos/{id:int}/estado")]
        public async Task<IActionResult> CambiarEstadoPedido(int id, [FromBody] ChangeStatusDTO dto)
        {
            try
            {
                var ok = await _pedidos.ChangeOrderStatusAsync(id, dto.NuevoEstado);
                if (!ok) return NotFound(new ApiResponse<string>("Pedido no encontrado", false));
                var updated = await _pedidos.GetByIdAsync(id);
                return Ok(new ApiResponse<PedidoDTO>(_mapper.Map<PedidoDTO>(updated)));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message, false));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>("Error interno: " + ex.Message, false));
            }
        }

        // ===== CASO 3: Reporte de ventas diarias =====
        // GET /api/coffee/reportes/ventas?fecha=2025-10-07
        [HttpGet("reportes/ventas")]
        public async Task<IActionResult> GetReporteVentas([FromQuery] DateTime fecha)
        {
            try
            {
                var report = await _pedidos.GetDailySalesReportAsync(fecha);
                return Ok(new ApiResponse<SalesReportDTO>(report));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>("Error interno: " + ex.Message, false));
            }
        }

        // ---------- DETALLES DE PEDIDO (CRUD) ----------
        [HttpGet("detalles")]
        public async Task<IActionResult> GetDetalles()
        {
            var list = await _detalles.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<DetallePedidoDTO>>(_mapper.Map<IEnumerable<DetallePedidoDTO>>(list)));
        }

        [HttpPost("detalles")]
        public async Task<IActionResult> PostDetalle([FromBody] DetallesPedido dto)
        {
            await _detalles.AddAsync(dto);
            return CreatedAtAction(nameof(GetDetalles), new { id = dto.IdDetalle }, dto);
        }

        [HttpPut("detalles/{id:int}")]
        public async Task<IActionResult> PutDetalle(int id, [FromBody] DetallesPedido dto)
        {
            if (id != dto.IdDetalle) return BadRequest("Id no coincide.");
            var existing = await _detalles.GetByIdAsync(id);
            if (existing is null) return NotFound();

            _mapper.Map(dto, existing);
            await _detalles.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("detalles/{id:int}")]
        public async Task<IActionResult> DeleteDetalle(int id)
        {
            var detalle = await _detalles.GetByIdAsync(id);
            if (detalle is null) return NotFound();
            await _detalles.DeleteAsync(id);
            return NoContent();
        }
    }
}



