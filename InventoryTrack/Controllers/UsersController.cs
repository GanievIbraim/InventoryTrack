using InventoryTrack.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTrack.Controllers
{
    [Route("api/users")]
    public class UsersController : BaseController<User>
    {
        public UsersController(AppDbContext context) : base(context)
        {
        }
    }
}
