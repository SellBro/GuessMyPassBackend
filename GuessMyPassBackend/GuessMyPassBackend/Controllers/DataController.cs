﻿using System;
using System.Collections.Generic;
using GuessMyPassBackend.Models;
using GuessMyPassBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace GuessMyPassBackend.Controllers
{
    [Produces("application/json")]
    [Route("data")]
    public class DataController : Controller
    {
        private readonly IDataRepository _datacontext;

        public DataController(IDataRepository datacontext)
        {
            _datacontext = datacontext;
        }


        [HttpPost]
        public ActionResult CreateData([FromBody] Data data)
        {
                Data newData;
                newData = _datacontext.CreateData(data, HttpContext.Request.Headers["Authorization"]); 

                return Ok(newData);
        }

        [HttpPut]
        public ActionResult UpdateData([FromBody] Data data)
        {

            Data updatedData = _datacontext.UpdateData(data);

            if(updatedData == null)
            {
                return BadRequest("Wrong data");
            }

            return Ok(updatedData);
        }


        // delete data by id
        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteDataById(string id)
        {
            string message = _datacontext.DeleteDataById(id, HttpContext.Request.Headers["Authorization"]);

            if (message == null)
            {
                return BadRequest("Data with this id doesn't exist");
            }

            return Ok(message);
        }

        // Get all data of user
        [HttpGet]
        public List<Data> Get()
        {
            return _datacontext.GetAllData(HttpContext.Request.Headers["Authorization"]);
        }

    }
}
