using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.BLL.DTOs
{
    public class LoginDto
    {
        public string EMail { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
