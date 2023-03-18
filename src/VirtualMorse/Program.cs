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

        private bool commandMode = false;

        public ProgramForm()
        {
            fKeys.KeyPressed += handler.DeviceInputReceived;
            handler.InputReceived += Handler_InputReceived;


            textBox = new RichTextBox();
            textBox.Dock = DockStyle.Fill;
            textBox.SelectionFont = new Font("Arial", 12, FontStyle.Regular);
            textBox.SelectionColor = Color.Black;
            textBox.AutoWordSelection = false;
            textBox.KeyDown += fKeys.TextBox_KeyDown;

            this.Text = versionStr;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.Controls.Add(textBox);
        }

        public void Handler_InputReceived(object sender, SwitchInputEventArgs e)
        {
            Switch input = e.switchInput;
            Console.WriteLine($"Received input in form - {input}");
            if (input == Switch.Switch1 ||  input == Switch.Switch9)
            {
                commandMode = true;
            }
            else if (input == Switch.Switch2)
            {
                if (commandMode)
                {
                    // PRINT PAGE
                    commandMode = false;
                }
                else
                {
                    // SHIFT
                }
            }
            else if (input == Switch.Switch3)
            {
                if (commandMode)
                {
                    // CLEAR DOCUMENT
                    commandMode = false;
                }
                else
                {
                    // SAVE
                }
            }
            else if (input == Switch.Switch4)
            {
                // SPACE & ADD WORD
            }
            else if (input == Switch.Switch5)
            {
                // DOT
            }
            else if (input == Switch.Switch6)
            {
                // DASH
            }
            else if (input == Switch.Switch7)
            {
                // ENTER LETTER
                //
                // If Command Mode:
                //   'l' -> Reads last sentence.
                //   'g' -> Checks email.
                //   'd' -> Deletes email.
                //   'h' -> Reads email headers.
                //   'r' -> Reads email.
                //   'y' -> Replies to email. (Not working in current)
                //   'n' -> Adds email address nickname.
                //   'a' -> Ties email address to nickname.
            }
            else if (input == Switch.Switch8)
            {
                // BACKSPACE
            }
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