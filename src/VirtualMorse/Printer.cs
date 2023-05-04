// static class Printer
// Contains function for printing a given string
// Printer name is obtained through environment variable PRINTER__NAME

using System;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace VirtualMorse
{
    public static class Printer
    {
        static Font printFont;
        static StringReader stringToPrint;

        // The PrintPage event is raised for each page to be printed.
        private static void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            ev.Graphics.DrawString(stringToPrint.ReadToEnd(), printFont, Brushes.Black,
                   ev.MarginBounds, new StringFormat());
        }

        // Print the given string
        public static void printString(string text)
        {
            stringToPrint = new StringReader(text);
            try
            {
                printFont = Program.textFont;
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.PrinterSettings.PrinterName = DotNetEnv.Env.GetString("PRINTER__NAME") ?? "";
                if (pd.PrinterSettings.IsValid)
                {
                    pd.Print();
                }
                else
                {
                    throw new InvalidPrinterException(pd.PrinterSettings);
                }
            }
            finally
            {
                stringToPrint.Close();
            }
        }
    }
}