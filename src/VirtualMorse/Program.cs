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
        public StateMachine stateMachine = new StateMachine();
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
            switch (input)
            {
                // COMMAND MODE
                case Switch.Switch1:  // Fall through
                case Switch.Switch9:
                    stateMachine.command();
                    break;

                // SHIFT [PRINT PAGE]
                case Switch.Switch2:
                    stateMachine.shift();
                    break;

                // SAVE [CLEAR DOCUMENT]
                case Switch.Switch3:
                    stateMachine.save();
                    break;

                // SPACE (ADD WORD)
                case Switch.Switch4:
                    stateMachine.space();
                    break;

                // DOT
                case Switch.Switch5:
                    stateMachine.dot();
                    break;

                // DASH
                case Switch.Switch6:
                    stateMachine.dash();
                    break;

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
                case Switch.Switch7:
                    stateMachine.enter();
                    break;

                // BACKSPACE
                case Switch.Switch8:
                    stateMachine.backspace();
                    break;
            }
            Console.WriteLine("current letter: '" + stateMachine.getCurrentLetter() + "'");
            Console.WriteLine("current word: '" + stateMachine.getCurrentWord() + "'");
            Console.WriteLine("current document: '" + stateMachine.getFile() + "'");
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