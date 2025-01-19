using libraryManagment.Core.Model;
using libraryManagment.EF.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libraryManagment.Core.Services
{
    public  class BookService : IBookService
    {

        private readonly IBookRepository _bookRepo;

        public BookService(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }

        public async Task<ResponseModel<BookDto>> BorrowBook(string isbn)
        {
            var response = new ResponseModel<BookDto>();

            if (string.IsNullOrEmpty(isbn))
            {
                response.Errors.Add("ISBN is required.");
                return response;
            }

            var result = await _bookRepo.BorrowBookAsync(isbn);
            if (result == "Book not found")
            {
                response.Errors.Add("Book not found.");
                return response;
            }

            if (result == "The book is already borrowed")
            {
                response.Errors.Add(result);
                return response;
            }

            var book = await _bookRepo.FindBookAsync(isbn); 
            if (book == null)
            {
                response.Errors.Add("Book not found.");
                return response;
            }

           
            response.Data = new BookDto
            {
                ISBN = book.ISBN,
                Title = book.Title,
                Author = book.Author,
                IsBorrowed = true
            };
            return response;
        }

        public async Task<ResponseModel<BookDto>> ReturnBook(string isbn)
        {
            var response = new ResponseModel<BookDto>();

            if (string.IsNullOrEmpty(isbn))
            {
                response.Errors.Add("ISBN is required.");
                return response;
            }

            var result = await _bookRepo.ReturnBookAsync(isbn);
            if (result == "Book not found" || result == "The book was not borrowed")
            {
                response.Errors.Add(result);
                return response;
            }

            response.Data = new BookDto { ISBN = isbn, IsBorrowed = false };
            return response;
        }

        public async Task<ResponseModel<BookDto>> GetBookByIsbnAsync(string isbn)
        {
            var response = new ResponseModel<BookDto>();

            if (string.IsNullOrEmpty(isbn))
            {
                response.Errors.Add("ISBN is required.");
                return response;
            }

            var book = await _bookRepo.FindBookAsync(isbn);
            if (book == null)
            {
                response.Errors.Add("Book not found.");
                return response;
            }

            response.Data = new BookDto
            {
                ISBN = book.ISBN,
                Title = book.Title,
                Author = book.Author,
                IsBorrowed = book.IsBorrowed
            };
            return response;
        }

        public async Task<ResponseModel<List<BookDto>>> GetAvailableBooks()
        {
            var response = new ResponseModel<List<BookDto>>();

            var books = await _bookRepo.GetAvailableBooksAsync();
            if (books == null || !books.Any())
            {
                response.Errors.Add("No available books found.");
                return response;
            }

            response.Data = books.Select(book => new BookDto
            {
                ISBN = book.ISBN,
                Title = book.Title,
                Author = book.Author,
                IsBorrowed = book.IsBorrowed
            }).ToList();
            return response;
        }

        public async Task<ResponseModel<BookDto>> AddBook(BookDto bookDto)
        {
            var response = new ResponseModel<BookDto>();

            if (string.IsNullOrEmpty(bookDto.ISBN))
            {
                response.Errors.Add("ISBN is required.");
                return response;
            }

            var existingBook = await _bookRepo.FindBookAsync(bookDto.ISBN);
            if (existingBook != null)
            {
                response.Errors.Add("Book already exists.");
                return response;
            }

            var book = new Book
            {
                Title = bookDto.Title,
                ISBN = bookDto.ISBN,
                Author = bookDto.Author,
                IsBorrowed = bookDto.IsBorrowed
            };

            await _bookRepo.AddBookAsync(book);
            response.Data = bookDto; 
            return response;
        }


    }
}
