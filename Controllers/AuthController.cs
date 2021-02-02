using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using TaskAPI;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtAuthenticationManager jwtAuthenticationManager;
        private readonly TaskDBContext _taskDbContext;
        public AuthController(IJwtAuthenticationManager jwtAuthenticationManager, TaskDBContext taskDbContext)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this._taskDbContext = taskDbContext;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "New Jersey", "New York" };
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("auth")]
        public IActionResult Authenticate([FromBody] Author a)
        {
            var authorLogin = _taskDbContext.Author.Where(au => au.UserName == a.UserName && au.Password == a.Password).ToList();
            if (authorLogin == null || authorLogin.Count == 0)
            {
                return Unauthorized();
            }
            else
            {
                //string cadena = authorLogin[0].Name + '|' + authorLogin[0].LastName + '|' + authorLogin[0].UserName + '|' + authorLogin[0].AuthorID;
                var token = jwtAuthenticationManager.Authenticate(authorLogin[0].AuthorID.ToString(), authorLogin[0].Name, authorLogin[0].LastName, authorLogin[0].UserName);
                //var token = jwtAuthenticationManager.Authenticate("test1", "password1");
                // if (token == null)
                //     return Unauthorized();
                return Ok(new
                {
                    status = 200,
                    //author = cadena,
                    token = token
                });
            }
        }
    }
}