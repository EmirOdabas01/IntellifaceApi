using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.BLL.DTOs
{
    public class ReadDto<T>
    {
      public int Id { get; set; }
      public T Data { get; set; }
    }
}
