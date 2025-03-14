using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    // Custom attribute to mark encrypted fields
    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptedAttribute : Attribute
    { }
}
