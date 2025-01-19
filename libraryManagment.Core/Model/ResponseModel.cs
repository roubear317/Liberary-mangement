using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libraryManagment.Core.Model
{
    public class ResponseModel<T>
    {

        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public ResponseModel()
        {
            Errors = new List<string>();
        }

        public bool IsSuccess => Errors.Count == 0;



    }
}
