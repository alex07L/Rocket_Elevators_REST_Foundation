using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Rocket_elevator_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        public AppDb Db { get; }

        public LeadController(AppDb db)
        {
            Db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetOne()
        {
            await Db.Connection.OpenAsync();
            var query = new LeadQuery(Db);
            var result = await query.getAllLead();
            await Db.Connection.CloseAsync();
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

    }
}