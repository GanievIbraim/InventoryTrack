using InventoryTrack.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TEntity> : ControllerBase where TEntity : class
    {
        protected readonly AppDbContext _context;

        public BaseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        // GET: api/[controller]/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> GetById(Guid id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }

        // POST: api/[controller]
        [HttpPost]
        public async Task<ActionResult<TEntity>> Create(TEntity entity)
        {
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(Guid))
            {
                var idValue = (Guid)idProperty.GetValue(entity);
                if (idValue == Guid.Empty)
                {
                    idProperty.SetValue(entity, Guid.NewGuid());
                }
            }

            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = idProperty?.GetValue(entity) }, entity);
        }

        // DELETE: api/[controller]/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}