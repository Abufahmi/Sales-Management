﻿using System.Globalization;

namespace Sales.Client.Helpers
{
    public class AppServices
    {
        public static bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar");
        public static bool IsEnglish => CultureInfo.CurrentCulture.Name.Contains("en");

        public static string Direction => IsArabic ? "ar" : "en";
    }
}