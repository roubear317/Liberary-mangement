﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libraryManagment.Core.Model
{
    public class UserBook
    {
        public string UserId { get; set; } 
        public ApplicationUser User { get; set; } 

        public int BookId { get; set; } 
        public Book Book { get; set; } 

        public DateTime BorrowDate { get; set; } 
        public DateTime? ReturnDate { get; set; }
    }
}
