// class Program
// Derived from base class Form
// Contains the Main function where the program starts.
// Creates a window containing a text box, and initializes 
//   a WritingContext with Arduino and function key inputs.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualMorse.Input;

namespace VirtualMorse
{
    public class Program : Form
    {
        public WritingContext context;
        public static string programName = "Virtual Morse";
        public static string programVersion = "2023";
        public static string fileDirectory;
        public static Font textFont = new Font("Arial", 16, FontStyle.Regular);

        public Program()
        {
            // Initialize text box
            RichTextBox textBox = new RichTextBox();
            textBox.Dock = DockStyle.Fill;
            textBox.AutoWordSelection = false;
            textBox.Font = textFont;
            textBox.ForeColor = Color.Black;

            // Initialize Form (Window)
            this.Text = programName + " " + programVersion;
            this.Size = new System.Drawing.Size(1024, 512);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.Controls.Add(textBox);

            // Create writing context
            context = new WritingContext(textBox);

            // Add input sources to writing context
            context.addInputSource(new FunctionKeyInput(context.getTextBox()));
            try
            {
                context.addInputSource(new ArduinoComms(context.getTextBox()));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening serial port");
                Console.WriteLine(ex.Message);
            }

            // Initialize Virtual Morse folder in Windows Documents
            string folderName = "Virtual Morse";
            initializeDirectory(folderName);

            // Load from existing text file
            string fileName = "document.txt";
            loadFromTextFile(fileName);
        }

        [STAThread]
        public static void Main(string[] args)
        {
            DotNetEnv.Env.TraversePath().Load();
            Speech.speakFully(programName + " " + programVersion);
            Application.EnableVisualStyles();
            Application.Run(new Program());
        }

        void initializeDirectory(string folderName)
        {
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fileDirectory = Path.Combine(myDocuments, folderName);
            Directory.CreateDirectory(fileDirectory);
        }

        void loadFromTextFile(string textFile)
        {
            context.setTextFile(textFile);
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
    }
}