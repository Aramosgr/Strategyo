using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Helper
{
    public static int maxX = 12;
    public static int maxY = 7;
    //this is used for getting the x or y from blocks names. Example: 4-10 it will return 4 if before is true, or 10 otherwise
    public static string GetXorYFromString(this string text, string stopAt = "-", bool before=true)
    {
        if (!String.IsNullOrWhiteSpace(text))
        {
            int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

            if (charLocation > 0)
            {
                if(before)
                {
                    return text.Substring(0, charLocation);
                }
                else
                {
                    return text.Substring(charLocation+1);
                }
            }
        }
        return String.Empty;
    }
}
