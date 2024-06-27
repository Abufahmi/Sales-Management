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

        internal static string GeneratePassword(int stringLength)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            char[] chars = new char[stringLength];
            Random rd = new();
            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}
