using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WSHAppDB.Model;
using WSHAppDB.Model.Entities;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;

namespace WSHAppAPI.Controllers
{
    //Alap kérések, amelyek minden entitásnál szükségesek (CRUD stb.)

    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseDataController<TEntity> : ControllerBase
        where TEntity : EntityBase
    {
        protected readonly ILogger _logger;
        protected readonly WSHDBContext _context;

        public BaseDataController(ILogger logger, WSHDBContext context)
        {
            this._logger = logger;
            this._context = context;
        }


        // GET: api/Entity
        [HttpGet("EntityCount")]
        public async Task<int> GetEntityCount()
        {
            return await _context.Set<TEntity>().CountAsync();
        }

        // GET: api/Entity
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> GetEntity()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }


        // GET: api/Entity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> GetEntity(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);

            if (entity == null)
            {
                _logger.LogInformation("NotFound");
                return NotFound();
            }

            return entity;
        }

        // PUT: api/Entity/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TEntity>> PutEntity(int id, TEntity entity)
        {
            if (id != entity.GetKey())
            {
                return BadRequest();
            }

            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return entity;
        }

        // POST: api/Entity
        [HttpPost]
        public virtual async Task<ActionResult<TEntity>> PostEntity(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntity", new { id = entity.GetKey() }, entity);
        }

        // DELETE: api/Entity/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntity(int id)
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

        private bool EntityExists(int id)
        {
            return _context.Set<TEntity>().Any(e => e.GetKey() == id);
        }

    }
}