using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doorang.Business.Exceptions;

public class FileContentException : Exception
{
    public string PropertyName { get; set; }
    public FileContentException(string propertName,string? message) : base(message)
    {
        PropertyName = propertName;
    }
}
