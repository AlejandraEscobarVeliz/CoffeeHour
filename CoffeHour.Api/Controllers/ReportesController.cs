// CoffeHour.Api/Controllers/ReportesController.cs
using CoffeeHour.Api.Responses;
using CoffeHour.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeHour.Api.Controllers
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

        /// <summary>
        /// Obtiene el reporte de ventas diarias (Caso de Uso 3).
        /// </summary>
        [HttpGet("ventas")]
        public async Task<IActionResult> GetVentasDiarias([FromQuery] DateTime fecha)
        {
            // ✅ Este método específico sí es async
            var reporte = await _unitOfWork.Pedidos.GetDailySalesReportAsync(fecha);

            if (reporte.OrdersCount == 0)
                return NotFound(new ApiResponse<string>(
                    $"No existen ventas registradas para {fecha:yyyy-MM-dd}", false));

            return Ok(new ApiResponse<object>(reporte));
        }
    }
}

