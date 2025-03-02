using InventoryTrack.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InventoryTrack.Controllers
{
    [Route("api/products")]
    public class ProductsController : BaseController<Product>
    {
        public ProductsController(AppDbContext context) : base(context)
        {
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Update(Guid id, Product product)
        {
            var existingProduct = await _context.Product.FindAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            if (product.Price.HasValue)
            {
                existingProduct.Price = product.Price;
            }

            if (product.Count.HasValue)
            {
                existingProduct.Count = product.Count;
            }

            await _context.SaveChangesAsync();

            return Ok(existingProduct);
        }

        [HttpPatch("{id}/stock")]
        public async Task<ActionResult<Product>> SetCount(Guid id, int count)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.Count.HasValue)
            {
                product.Count = count;
            }

            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpGet("{id}/stock")]
        public async Task<ActionResult<int>> GetCount(Guid id, int quantity)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product.Count);
        }

        [HttpGet("{id}/history")]
        public async Task<ActionResult<string>> GetHistory(Guid id, int quantity)
        {
            // TO DO
            return Ok("История изменений");
        }

        // TO DO: возможно стоит подумать об универсальном способе импорта и экспорта
        [HttpPost("import")]
        public async Task<ActionResult<string>> Import()
        {
            // TO DO: необходимо реализовать функциб которая получает файл (для начала один тип)
            // далее необходимо добавить данные в систему
            // функционал важен так как на его основе можно проводить испорт с моб. приложения
            return Ok("Загрузили данные");
        }

        [HttpGet("export")]
        public async Task<ActionResult<string>> Export()
        {
            // TO DO: простой экпорт json или Excel
            return Ok("Выгрузили данные");
        }

        // TO DO: реализовать метод который будет фильтровать записи по field равные value
        [HttpGet("{id}filter")]
        public async Task<ActionResult<IEnumerable<Product>>> Filter([FromQuery] string field, [FromQuery] string value)
        {
            // Получаем свойство из класса Product
            PropertyInfo? property = typeof(Product).GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
            {
                return BadRequest($"Свойство '{field}' не найдено.");
            }

            // Преобразуем строковое значение в нужный тип
            object? convertedValue;
            try
            {
                convertedValue = Convert.ChangeType(value, property.PropertyType);
            }
            catch
            {
                return BadRequest($"Неверный формат значения '{value}' для свойства '{field}'.");
            }

            // Создаём динамический запрос
            var query = _context.Product.AsQueryable()
                .Where(p => EF.Property<object>(p, property.Name).Equals(convertedValue));

            var result = await query.ToListAsync();
            return Ok(result);

        }
    }
}