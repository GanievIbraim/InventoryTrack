using InventoryTrack.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTrack.Controllers
{
    [Route("api/contacts")]
    public class ContactsController : BaseController<Contact>
    {
        public ContactsController(AppDbContext context) : base(context)
        {
        }
    }
}
