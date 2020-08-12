﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Rocket_elevator_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatteryController : ControllerBase
    {
        public AppDb Db { get; }

        public BatteryController(AppDb db)
        {
            Db = db;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new BatteryQuery(Db);
            var result = await query.FindOneAsync(id);
            await Db.Connection.CloseAsync();
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> SetOne(int id, [FromBody]Battery body)
        {
            await Db.Connection.OpenAsync();
            var query = new BatteryQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.Status = body.Status;
            await result.UpdateAsync();
            await Db.Connection.CloseAsync();
            return new OkObjectResult(result);
        }

        [HttpPut("{id}/{status}")]
        public async Task<IActionResult> SetOne(int id, String status)
        {
            await Db.Connection.OpenAsync();
            var query = new BatteryQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
            {
                await Db.Connection.CloseAsync();
                return new NotFoundResult();
            }
            result.Status = status;
            await result.UpdateAsync();
            await Db.Connection.CloseAsync();
            return new OkObjectResult(result);
        }
    }
}