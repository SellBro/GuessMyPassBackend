﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using GuessMyPassBackend.Models;
using GuessMyPassBackend.Contexts;

namespace GuessMyPassBackend.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("user")]
    public class UsersController : Controller
    {
        private readonly IUserContext _userContext;

        public UsersController(IUserContext userContext)
        {
            _userContext = userContext;
        }


        // /user/test
        [HttpGet]
        [Route("test")]
        public ActionResult Login()
        {
            return Ok(new { message = "Izi dla menia. Ludshiu v mire za rabotoi" });
        }

        
        // /user/login
        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] AuthenticateRequest userFromRequest)
        {
            AuthedUserResponse user = null;

            user = _userContext.Login(userFromRequest.Email, userFromRequest.Password);

            if (user == null)
            {

                return BadRequest( new { error = "Wrong email or password" });
            }

            return Ok(user);
        }


        // /user/register
        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] User user)
        {

            string message = _userContext.CreateUser(user);

            if (!message.Equals("User was created"))
            {
                return BadRequest(new { error = message });
            }

            return Ok( new { message });
        }


        // /user/options/password
        [HttpPut]
        [Route("options/password")]
        public ActionResult UpdatePassword([FromBody] UserOptionsRequest requestBody)
        {

            string message = _userContext.UpdatePassword(requestBody, HttpContext.Request.Headers["Authorization"]);

            if(message == null)
            {
                return BadRequest(new { error = "Wrong password provided" });
            }

            return Ok(new { message });

        }

        // /user/options/username
        [HttpPut]
        [Route("options/username")]
        public ActionResult UpdateUsername([FromBody] UserOptionsRequest requestBody)
        {

            string message = _userContext.UpdateUsername(requestBody, HttpContext.Request.Headers["Authorization"]);

            if (message == null || message.Equals("User with same username already exists"))
            {
                return (message == null) ? BadRequest(new { error = "Wrong username" }) : StatusCode(409, new { message });
            }

            return Ok(new { message });

        }

    }
}
