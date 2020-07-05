using System;
using System.Collections.Generic;
using System.Text;

namespace TrendyolLinkConverter.Core.Extensions
{
   public static class StringExtensions
    {

        public static string ConvertFromTurkishCharacters(this string input)
        {
            input= input.Replace('ö', 'o');
            input = input.Replace('ç', 'c');
            input = input.Replace('ş', 's');
            input = input.Replace('ı', 'i');
            input = input.Replace('ğ', 'g');
            input = input.Replace('ü', 'u');
            return input;
        }

    }
}
