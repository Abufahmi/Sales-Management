using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Context.Helpers
{
    public class LibraryService
    {
        public static string? User => nameof(User);
        public static string? Visor => nameof(Visor);
        public static string? Admin => nameof(Admin);

        public static string? Error { get; internal set; }
    }
}
