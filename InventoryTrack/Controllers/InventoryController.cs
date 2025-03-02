using InventoryTrack.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryTrack.Controllers
{
    [Route("api/inventory")]
    public class InventoryController : BaseController<Inventory>
    {
        public InventoryController(AppDbContext context) : base(context)
        {
        }

        // Добавление товара в инвентаризацию (сканирование)
        [HttpPost("sessions/{sessionId}/scan")]
        public async Task<ActionResult<string>> ScanProduct(Guid sessionId)
        {
            return Ok("Товар отсканирован");
        }

        // Просмотр текущего состояния инвентаризации
        [HttpGet("sessions/{sessionId}/status")]
        public async Task<ActionResult<string>> GetInventoryStatus(Guid sessionId)
        {
            return Ok("Список отскнаированных товаров");
        }
    }
}
