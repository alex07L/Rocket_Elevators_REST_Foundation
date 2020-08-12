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
    public class InterventionController : ControllerBase
    {

        public AppDb Db { get; }

        public InterventionController(AppDb db)
        {
            Db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Getlist()
        {
            await Db.Connection.OpenAsync();
            var query = new InterventionQuery(Db);
            var result = await query.Async();
            await Db.Connection.CloseAsync();
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        [HttpPut("{id}/{status}")]
        public async Task<IActionResult> SetOne(int id, String status)
        {
            await Db.Connection.OpenAsync();
            var query = new InterventionQuery(Db);
            var result = await query.First(id);
            if (result is null)
            {
                await Db.Connection.CloseAsync();
                return new NotFoundResult();
            }
            result.Status = status;
            await result.UpdateStatus();
            result = await query.First(id);
            await Db.Connection.CloseAsync();
            return new OkObjectResult(result);
        }


    }
}