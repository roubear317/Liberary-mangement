using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libraryManagment.Core.Services;

using System.Threading.Tasks;
using libraryManagment.EF.DTOS;
using libraryManagment.Core.Model;

namespace libraryManagment.Core.Services
{
    public interface IBookService
    {

        Task<ResponseModel<BookDto>> AddBook(BookDto bookDto);

        Task<ResponseModel<BookDto>> GetBookByIsbnAsync(string isbn);

        Task<ResponseModel<List<BookDto>>> GetAvailableBooks();

        Task<ResponseModel<BookDto>> BorrowBook(string isbn);

        Task<ResponseModel<BookDto>> ReturnBook(string isbn);
    }
}
