using System;
using System.Collections.Generic;
using System.IO;
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
        public static string fileDirectory;

        public Program()
        {
            context = new WritingContext();

            this.Text = programName + " " + programVersion;
            this.Size = new System.Drawing.Size(1024, 512);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.Controls.Add(context.getTextBox());

            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folderName = "Virtual Morse";
            fileDirectory = Path.Combine(myDocuments, folderName);
            Directory.CreateDirectory(fileDirectory);

            context.setTextFile("test.txt");
            try
            {
                context.loadFromTextFile();
            }
            catch (Exception ex)
            {
                string error = "Error reading file.";
                Console.WriteLine(error);
                Speech.speak(error);
                Console.WriteLine(ex.Message);
            }
        }

        [STAThread]
        public static void Main(string[] args)
        {
            Speech.speak(programName + " " + programVersion);
            Application.EnableVisualStyles();
            Application.Run(new Program());
        }
    }
}