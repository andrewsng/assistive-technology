using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualMorse
{
    public class Program : Form
    {
        public WritingContext context;
        public static string programName = "Virtual Morse";
        public static string programVersion = "2023";

        public Program()
        {
            context = new WritingContext();

            this.Text = programName + " " + programVersion;
            this.Size = new System.Drawing.Size(1024, 512);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.Controls.Add(context.getTextBox());
        }

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(new Program());
        }
    }
}