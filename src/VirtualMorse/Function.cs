using System;
using System.Collections.Generic;

public static class Function
{
    static Dictionary<string, string> morse_map = new Dictionary<string, string>() {
    {".-", "a"},
    {"-...", "b"},
    {"-.-.", "c"},
    {"-..", "d"},
    {".", "e"},
    {"..-.", "f"},
    {"--.", "g"},
    {"....", "h"},
    {"..", "i"},
    {".---", "j"},
    {"-.-", "k"},
    {".-..", "l"},
    {"--", "m"},
    {"-.", "n"},
    {"---", "o"},
    {".--.", "p"},
    {"--.-", "q"},
    {".-.", "r"},
    {"...", "s"},
    {"-", "t"},
    {"..-", "u"},
    {"...-", "v"},
    {".--", "w"},
    {"-..-", "x"},
    {"-.--", "y"},
    {"--..", "z"}
    };

    public static string morseToText(string morse)
    {
        if (morse_map.ContainsKey(morse))
            return morse_map[morse];
        else
            return "";
    }
}
