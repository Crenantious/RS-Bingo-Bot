using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSBingo_Framework.Exceptions;

public class UnpermittedURLException : RSBingoException
{
    public UnpermittedURLException(string? message)
        : base(message) { }
}
