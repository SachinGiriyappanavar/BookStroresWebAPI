using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStroresWebAPI.Models;

namespace BookStroresWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly BookStoresDBContext _context;

        public PublishersController(BookStoresDBContext context)
        {
            _context = context;
        }

        // GET: api/Publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
        {
            return await _context.Publishers.ToListAsync();
        }

        // GET: api/Publishers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisher(int id)
        {
            //var publisher = await _context.Publishers.FindAsync(id);

            var publisher = _context.Publishers
                                            .Include(pub => pub.Books)
                                                .ThenInclude(book => book.Sales)
                                            .Include(pub => pub.Users)
                                                .ThenInclude(user => user.Role)
                                            .Where(pub => pub.PubId == id).FirstOrDefault();

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }

        [HttpGet("PostPublisherDetails/")]
        public async Task<ActionResult<Publisher>> PostPublisherDetails()
        {
            //var publisher = await _context.Publishers.FindAsync(id);

            var publisher = new Publisher();

            publisher.PublisherName = "Harper & Brothers";
            publisher.City = "New York City";
            publisher.State = "NY";
            publisher.Country = "USA";


            var book1 = new Book();

            book1.Title = "Good night Moon - 1";
            book1.PublishedDate = DateTime.Now;

            var book2 = new Book();

            book2.Title = "Good night Moon - 1";
            book2.PublishedDate = DateTime.Now;

            publisher.Books.Add(book1);
            publisher.Books.Add(book2);

            _context.Publishers.Add(publisher);
            _context.SaveChanges();
            
            var publishers = _context.Publishers
                                            .Include(pub => pub.Books)
                                                .ThenInclude(book => book.Sales)
                                            .Include(pub => pub.Users)
                                                .ThenInclude(user => user.Role)
                                            .Where(pub => pub.PubId == publisher.PubId).FirstOrDefault();

            if (publishers == null)
            {
                return NotFound();
            }

            return publishers;
        }

        // PUT: api/Publishers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, Publisher publisher)
        {
            if (id != publisher.PubId)
            {
                return BadRequest();
            }

            _context.Entry(publisher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Publishers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Publisher>> PostPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPublisher", new { id = publisher.PubId }, publisher);
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Publisher>> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return publisher;
        }

        private bool PublisherExists(int id)
        {
            return _context.Publishers.Any(e => e.PubId == id);
        }
    }
}
