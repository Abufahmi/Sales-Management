﻿using System.Globalization;

namespace Sales.Client.Helpers;

public class AppServices
{
    public static bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar");
    public static bool IsEnglish => CultureInfo.CurrentCulture.Name.Contains("en");
    public static string Direction => IsArabic ? "ar" : "en";
    public static string BaseApiAddress => "https://localhost:7063/api/";
    public static string BaseAddress => "https://localhost:7063/";
    public static string? Error { get; set; }
    public static string User => nameof(User);
    public static string Visor => nameof(Visor);
    public static string Admin => nameof(Admin);
    public static int ItemPerPage { get; set; }
}
