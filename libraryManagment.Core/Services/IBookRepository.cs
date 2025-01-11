using libraryManagment.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libraryManagment.Core.Services
{
    public interface IBookRepository
    {

        Task<Book> FindBookAsync(string isbn);
         Task AddBookAsync(Book book);
        Task<List<Book>> GetAvailableBooksAsync();
        Task<List<Book>> GetBorrowedBooksAsync();
        Task<string> BorrowBookAsync(string isbn);
        Task<string> ReturnBookAsync(string isbn);



    }
}
