using InventoryTrack.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryTrack.Controllers
{
    [Route("api/acounts")]
    public class AccountsController : BaseController<Account>
    {
        public AccountsController(AppDbContext context) : base(context)
        {
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Account>> Update(Guid id, Account account)
        {
            var existingAccount = await _context.Account.FindAsync(id);

            if (existingAccount == null)
            {
                return NotFound();
            }

            existingAccount.Address = account.Address;
            await _context.SaveChangesAsync();

            return Ok(existingAccount);
        }

    }
}
