using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace WorkoutTracker.Converters;

public class WorkoutIconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string type = value as string ?? "Jiné";

        // Vylepšené SVG cesty ve stylu Font Awesome
        return type switch
        {
            "Posilovna" => Geometry.Parse("M22.67,11H21V7a1,1,0,0,0-1-1H18V4a1,1,0,0,0-1-1H15a1,1,0,0,0-1,1V6H10V4A1,1,0,0,0,9,3H7A1,1,0,0,0,6,4V6H4A1,1,0,0,0,3,7v4H1.33a1.33,1.33,0,0,0,0,2.67H3v4a1,1,0,0,0,1,1H6v2a1,1,0,0,0,1,1H9a1,1,0,0,0,1-1V18h4v2a1,1,0,0,0,1,1h2a1,1,0,0,0,1-1V18h2a1,1,0,0,0,1-1V13h1.67a1.33,1.33,0,0,0,0-2.67Z"),
            "Běh" => Geometry.Parse("M21,11c-.34,0-.65.15-.84.41l-1.9,2.66A2,2,0,0,1,16.63,15H14.1l1.53,2.56a2,2,0,0,1-3.44,2.06L9,14.24V11a2,2,0,0,1,2-2h1.41l1.3-1.63a2,2,0,1,1,3.12,2.5L15.35,11ZM12,6a2,2,0,1,1-2-2A2,2,0,0,1,12,6ZM7.13,10.65,5.19,12.59a2,2,0,1,0,2.83,2.83l2.25-2.25A2,2,0,0,0,10.1,10.1L8.5,8.5a2,2,0,0,0-2.83,0,2,2,0,0,0,0,2.83Z"),
            "Cyklistika" => Geometry.Parse("M19,14a3,3,0,1,0,3,3A3,3,0,0,0,19,14Zm0,4.5a1.5,1.5,0,1,1,1.5-1.5A1.5,1.5,0,0,1,19,18.5ZM12.5,9,11,6,8,5V7L9.5,8,7.3,11.3A3,3,0,0,0,5,14a3,3,0,1,0,3,3,3,3,0,0,0-.4-1.5l1.6-2.4L11,15l2-4,2,3h4v-2H16l-2-3ZM5,20a1.5,1.5,0,1,1,1.5-1.5A1.5,1.5,0,0,1,5,20Z"),
            "Jóga" => Geometry.Parse("M12,8a2,2,0,1,0-2-2A2,2,0,0,0,12,8Zm8,5a1,1,0,0,0-1-1H16.1l-1.8-3.6A3,3,0,0,0,11.7,7H11a3,3,0,0,0-2.6,1.4L6.1,12H4a1,1,0,0,0,0,2H6.4l2-4h7.2l2,4H20a1,1,0,0,0,0-2Zm-3,5H7a1,1,0,0,0,0,2H17a1,1,0,0,0,0-2Z"),
            "Plavání" => Geometry.Parse("M21.5,13.5c-1,0-1.5.5-2.5.5s-1.5-.5-2.5-.5-1.5.5-2.5.5-1.5-.5-2.5-.5-1.5.5-2.5.5-1.5-.5-2.5-.5S4.5,13.5,3.5,13.5a1,1,0,0,0,0,2c1,0,1.5-.5,2.5-.5s1.5.5,2.5.5,1.5-.5,2.5-.5,1.5.5,2.5.5,1.5-.5,2.5-.5,1.5.5,2.5.5,1.5-.5,2.5-.5a1,1,0,0,0,0-2ZM12,11a3,3,0,1,0-3-3A3,3,0,0,0,12,11Z"),
            _ => Geometry.Parse("M12,21.35l-1.45-1.32C5.4,15.36,2,12.28,2,8.5A5.45,5.45,0,0,1,7.5,3,5.91,5.91,0,0,1,12,5.09,5.91,5.91,0,0,1,16.5,3,5.45,5.45,0,0,1,22,8.5c0,3.78-3.4,6.86-8.55,11.54Z")
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
