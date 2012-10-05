using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FlyingClub.Common
{
    public static class StringExtensions
    {
        public static string ToFriendlyString(this string source)
        {
            return Regex.Replace(source, "(?!^)([A-Z])", " $1");
        }
    }
}
