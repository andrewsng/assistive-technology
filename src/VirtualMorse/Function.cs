using System;
using System.Collections.Generic;
using System.IO;

namespace VirtualMorse
{
    public static class Function
    {
        static Dictionary<string, string> morse_map = new Dictionary<string, string>() {
            // Letters
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
            {"--..", "z"},

            // Numbers
            {".----", "1"},
            {"..---", "2"},
            {"...--", "3"},
            {"....-", "4"},
            {".....", "5"},
            {"-....", "6"},
            {"--...", "7"},
            {"---..", "8"},
            {"----.", "9"},
            {"-----", "0"},

            // Special Characters
            {".-.-.-", "."},
            {"--..--", ","},
            {"..--..", "?"},
            {"-..-.", "/"},
            {".......", "!"},
            {".----.", "'"},
            {"-.-.-", ";"},
            {"---...", ":"},
            {"-....-", "-"},
            {"..--.-", "_"},
            {"-.--.-", "("},
            {"-.--..", ")"},

            // Custom Symbol (Email)
            {"..--", "@"}
        };

        public static string morseToText(string morse)
        {
            if (morse_map.ContainsKey(morse))
                return morse_map[morse];
            else
                return "";
        }

        public static void parseCommand(string command)
        {
            switch (command)
            {
                case "l":
                    Console.WriteLine("read last sentence");
                    break;
                case "g":
                    Console.WriteLine("checks email");
                    break;
                case "d":
                    Console.WriteLine("deletes email");
                    break;
                case "h":
                    Console.WriteLine("read email headers");
                    break;
                case "r":
                    Console.WriteLine("reads email");
                    break;
                case "y":
                    Console.WriteLine("reply to email");
                    break;
                case "n":
                    Console.WriteLine("adds email address nickname");
                    break;
                case "a":
                    Console.WriteLine("ties email address to nickname");
                    break;
                default:
                    Console.WriteLine("invalid command");
                    break;
            }
        }

        public static void addToFile(string directory, string file, string text)
        {

            using (StreamWriter writer = new StreamWriter(directory + file))
            {
                writer.WriteLine(text);
            }
        }
        public static List<string> readFullFile(string directory, string file)
        {
            List<string> return_string = new List<string>();
            using (StreamReader reader = new StreamReader(directory + file))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    return_string.Add(line);
                }
            }
            return return_string;
        }
    }
}
