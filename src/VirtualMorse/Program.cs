using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System.Drawing;

namespace VirtualMorse
{
    public class ProgramForm : Form
    {
        public FunctionKeyInput fKeys = new FunctionKeyInput();
        public InputHandler handler = new InputHandler();
        public RichTextBox textBox;
        static String versionStr = "Virtual Morse 2023";

        public ProgramForm()
        {
            fKeys.KeyPressed += handler.DeviceInputReceived;
            handler.InputReceived += Handler_InputReceived;


            textBox = new RichTextBox();
            textBox.Dock = DockStyle.Fill;
            textBox.SelectionFont = new Font("Arial", 12, FontStyle.Regular);
            textBox.SelectionColor = Color.Black;
            textBox.KeyDown += fKeys.TextBox_KeyDown;

            this.Text = versionStr;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Add(textBox);
        }

        public void Handler_InputReceived(object sender, EventArgs e)
        {
            Console.WriteLine("Received input in form");
        }

        [STAThread]
        public static void Main(string[] args)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
            synth.Speak(versionStr);

            Application.EnableVisualStyles();
            Application.Run(new ProgramForm());
        }
    }
}