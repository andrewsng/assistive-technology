using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VirtualMorse.States;
using VirtualMorse.Input;

namespace VirtualMorse
{
    public class WritingContext
    {
        RichTextBox textBox;

        FunctionKeyInput functionKeys;
        ArduinoComms Board;

        State state;

        public string currentMorse = "";
        public string currentWord = "";
        public char lastLetter = '\0';

        string directory;
        string file = "test.txt";

        public WritingContext()
        {
            textBox = new RichTextBox();
            textBox.Dock = DockStyle.Fill;
            textBox.AutoWordSelection = false;
            textBox.Font = new Font("Arial", 16, FontStyle.Regular);
            textBox.ForeColor = Color.Black;

            functionKeys = new FunctionKeyInput();
            functionKeys.KeyPressed += Handler_InputReceived;
            textBox.KeyDown += functionKeys.TextBox_KeyDown;

            try
            {
                Board = new ArduinoComms(textBox);
                Board.buttonPressed += Handler_InputReceived;
            }
            catch
            {
                Console.WriteLine("Error opening serial port");
                // TODO: Throw here or not?
            }

            state = new TypingState(this);

            directory = AppDomain.CurrentDomain.BaseDirectory;
            directory = directory.Replace("bin\\Debug\\", "Text_documents\\");
            List<string> fileContents = Function.readFullFile(directory, file);
            if (fileContents.Count > 0)
            {
                setDocument((Function.readFullFile(directory, file))[0]);
            }
        }

        private void Handler_InputReceived(object sender, SwitchInputEventArgs e)
        {
            state.respond(e.switchInput);
            Console.WriteLine("current letter: '" + getCurrentMorse() + "'");
            Console.WriteLine("current word: '" + getCurrentWord() + "'");
            Console.WriteLine("current document: '" + getDocument() + "'");
            Console.WriteLine();
        }

        //fetching / setting functions

        public RichTextBox getTextBox()
        {
            return textBox;
        }

        public void transitionToState(State state)
        {
            this.state = state;
        }

        public string getCurrentWord()
        {
            return currentWord;
        }

        public string getCurrentMorse()
        {
            return currentMorse;
        }

        public string getDocument()
        {
            return textBox.Text;
        }

        public void clearMorse()
        {
            currentMorse = "";
        }

        public void clearWord()
        {
            currentWord = "";
        }

        public void setDocument(string text)
        {
            textBox.Focus();
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.TextLength;
            textBox.SelectedText = text;
        }

        public void appendToDocument(string text)
        {
            textBox.Focus();
            textBox.SelectionStart = textBox.TextLength;
            textBox.SelectionLength = 0;
            textBox.SelectedText = text;
        }

        public void backspaceDocument()
        {
            if (textBox.TextLength > 0)
            {
                textBox.Focus();
                textBox.SelectionStart = textBox.TextLength - 1;
                textBox.SelectionLength = 1;
                textBox.SelectedText = "";
            }
        }

        public void saveDocumentFile()
        {
            Function.addToFile(directory, file, getDocument());
        }
    }
}