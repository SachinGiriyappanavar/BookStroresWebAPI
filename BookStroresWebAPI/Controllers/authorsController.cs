using BookStroresWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStroresWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class authorsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Author> Get()
        {
            using(var context = new BookStoresDBContext())
            {
                //return context.Authors.ToList();

                return context.Authors.Where(a => a.AuthorId == 1).ToList();
            }
        }
    }
}
