using System;
using Library_Management_System.Data;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public BookController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

       
        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _appDbContext.Books.ToListAsync();
        }


        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _appDbContext.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }


        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(AddBookDto bookDto)
        {
            // 1. Convert DTO to actual Book Entity
            var bookEntity = new Book()
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Description = bookDto.Description
            };

            // 2. Add and Save
            _appDbContext.Books.Add(bookEntity);
            await _appDbContext.SaveChangesAsync();

            // 3. Return the result
            return CreatedAtAction(nameof(GetBook), new { id = bookEntity.Id }, bookEntity);
        }


        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, UpdateBookDto bookDto) // <--- Changed Type Here
        {
            // 1. Find the existing book
            var book = await _appDbContext.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            // 2. Update fields using the "Update" DTO
            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.Description = bookDto.Description;

            // 3. Save
            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_appDbContext.Books.Any(e => e.Id == id))
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



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _appDbContext.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _appDbContext.Books.Remove(book);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
