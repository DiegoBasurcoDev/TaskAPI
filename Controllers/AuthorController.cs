using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using TaskAPI;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("Author")]
    public class AuthorController : ControllerBase
    {
        private readonly TaskDBContext _taskDbContext;
        public AuthorController(TaskDBContext taskDbContext)
        {
            _taskDbContext = taskDbContext;
        }

        [HttpGet]
        public List<Author> ListAll()
        {
            var author = _taskDbContext.Author.ToList();
            return author;
        }

        [HttpPost]
        public ActionResult Register(Author a)
        {
            try
            {
                _taskDbContext.Author.Add(a);
                _taskDbContext.SaveChanges();
                return Ok(new
                {
                    Status = 1,
                    Message = "Author registered",
                    Author = a
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Status = 0,
                    Message = "Failed to register Author",
                    Detail = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("jwt")]
        public IActionResult ValidToken(){

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();

            return Ok(new {
                AuthorID = claim[0].Value,
                Name = claim[1].Value,
                LastName = claim[2].Value,
                UserName = claim[3].Value
            });
        }
    }
}