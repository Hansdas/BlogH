using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Blog.Common
{
   public class RegexExtensions
    {
        private static Regex Regex(string pattern)
        {
            return new Regex(pattern);
        }
        public static void RegexReplace(string pattern,string input,string replace)
        {
            Regex(pattern).Replace(input, replace);
        }
    }
}
