using libraryManagment.Core.Model;
using libraryManagment.Core.Services;
using libraryManagment.EF.DTOS;
using LibraryManagment.EF.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace libraryManagment.api.Controllers
{
    //[Authorize(Roles = "User")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;

        public BookController(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddBook")]

        public async Task<ActionResult<BookDto>> AddBook(BookDto bookDto)
        {
            if (bookDto == null)  return BadRequest();

          var book  =await _bookRepo.FindBookAsync(bookDto.ISBN);


            if (book!=null) return BadRequest();

            var BookDB = new Book
            {
                Title = bookDto.Title,
                ISBN = bookDto.ISBN,
                Author = bookDto.Author,
                IsBorrowed = bookDto.IsBorrowed

            };

            await _bookRepo.AddBookAsync(BookDB);


            return Ok(bookDto);

        }


        [HttpGet("{Isbn}")]

        public async Task<ActionResult<BookDto>> GetBookByIsbnAsync(string Isbn)
        {

            var Book = await _bookRepo.FindBookAsync(Isbn);


                if (Book==null) return BadRequest();

            var BookDto = new BookDto
            {
                ISBN = Book.ISBN,
                Title = Book.Title,
                Author= Book.Author,
                IsBorrowed= Book.IsBorrowed
                };

            return BookDto;


        }



        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableBooks()
        {
            var books = await _bookRepo.GetAvailableBooksAsync();
            return Ok(books);
        }

        [HttpPost("borrow/{Isbn}")]
        public async Task<IActionResult> BorrowBook( string Isbn)
        {
            var result = await _bookRepo.BorrowBookAsync(Isbn);
            return Ok(new { Message = result });
        }

        [HttpPost("return/{Isbn}")]
        public async Task<IActionResult> ReturnBook( string Isbn)
        {
            var result = await _bookRepo.ReturnBookAsync(Isbn);
            return Ok(new { Message = result });
        }


    }
}
