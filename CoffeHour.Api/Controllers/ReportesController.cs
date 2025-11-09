using CoffeeHour.Api.Responses;
using CoffeHour.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHour.Api.Controllers
{
    [ApiController]
    [Route("api/coffee/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("ventas")]
        public async Task<IActionResult> GetVentasDiarias([FromQuery] DateTime fecha)
        {
            var pedidos = await _unitOfWork.Pedidos.GetAllAsync();
            var pedidosFiltrados = pedidos
                .Where(p => p.Fecha.Date == fecha.Date && p.Estado.ToLower() == "entregado")
                .ToList();

            if (!pedidosFiltrados.Any())
                return NotFound(new ApiResponse<string>($"No existen ventas registradas para {fecha:yyyy-MM-dd}", false));

            var totalVentas = pedidosFiltrados.Sum(p => p.Total);
            var cantidadPedidos = pedidosFiltrados.Count;

            var reporte = new
            {
                fecha = fecha.ToString("yyyy-MM-dd"),
                totalVentas,
                cantidadPedidos,
                pedidos = pedidosFiltrados.Select(p => new
                {
                    p.Id,
                    Cliente = p.IdCliente,
                    p.Total,
                    p.Estado
                })
            };

            return Ok(new ApiResponse<object>(reporte));
        }
    }
}


