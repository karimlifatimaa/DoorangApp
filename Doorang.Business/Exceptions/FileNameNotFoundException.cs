using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doorang.Business.Exceptions;

public class FileNameNotFoundException : Exception
{
    public string PropertyName { get; set; }
    public FileNameNotFoundException(string propertyname,string? message) : base(message)
    {
        PropertyName = propertyname;
    }
}
