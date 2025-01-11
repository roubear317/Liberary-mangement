using libraryManagment.Core.Model;
using libraryManagment.Core.Services;
using libraryManagment.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagment.EF.Repos
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbcontext _context;

        public BookRepository(AppDbcontext context)
        {
            _context = context;
        }

       
        public async Task<Book> FindBookAsync(string isbn)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);

           
            return book;

        }
        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task<string> BorrowBookAsync(string isbn)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.ISBN == isbn);
            if (book == null)
            {
                return "Book not found";
            }

            if (book.IsBorrowed)
            {
                return "The book is already borrowed";
            }

            book.IsBorrowed = true;
            await _context.SaveChangesAsync();
            return "Book is borrowed";
        }

        public async Task<List<Book>> GetAvailableBooksAsync()
        {
            return await _context.Books.Where(x => !x.IsBorrowed).ToListAsync();
        }

        public async Task<List<Book>> GetBorrowedBooksAsync()
        {
            return await _context.Books.Where(x => x.IsBorrowed).ToListAsync();
        }

        public async Task<string> ReturnBookAsync(string isbn)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.ISBN == isbn);
            if (book == null)
            {
                return "Book not found";
            }

            if (!book.IsBorrowed)
            {
                return "The book was not borrowed";
            }

            book.IsBorrowed = false;
            await _context.SaveChangesAsync();
            return "Book has been returned";
        }

       
    }
}
