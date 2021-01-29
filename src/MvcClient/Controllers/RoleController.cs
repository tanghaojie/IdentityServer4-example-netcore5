using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcClient.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "BobRole")]
        public IActionResult BobRole()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
            return View();
        }

        [Authorize(Roles = "AliceRole")]
        public IActionResult AliceRole()
        {
            return View();
        }
    }
}
