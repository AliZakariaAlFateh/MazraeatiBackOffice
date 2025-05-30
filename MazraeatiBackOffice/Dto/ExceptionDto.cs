using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Dto
{
    public class ExceptionDto
    {
        public string MethodName { get; set; }
        public Exception exception { get; set; }
    }
}
