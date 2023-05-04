//writing context
//implements the state machine for Virtual Morse
//implements the GUI for Virtual Morse
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VirtualMorse.States;
using VirtualMorse.Input;
using System.IO;
using System.Speech.Synthesis;

namespace VirtualMorse
{
    public class WritingContext
    {
        RichTextBox textBox;

        FunctionKeyInput functionKeys;
        ArduinoComms Board;

        //current state of the machine
        State state;

        //current letter and word
        public string currentMorse = "";
        public string currentWord = "";
        public char lastLetter = '\0';

        string textFileName;

        public WritingContext()
        {
            //create textbox
            textBox = new RichTextBox();
            textBox.Dock = DockStyle.Fill;
            textBox.AutoWordSelection = false;
            textBox.Font = new Font("Arial", 16, FontStyle.Regular);
            textBox.ForeColor = Color.Black;

            //set up function keys
            functionKeys = new FunctionKeyInput();
            functionKeys.KeyPressed += Handler_InputReceived;
            textBox.KeyDown += functionKeys.TextBox_KeyDown;

            //connect to the arduino board
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
        }

        //handles inputs from arduino and function keys
        //determines which state machine input to use
        private void Handler_InputReceived(object sender, SwitchInputEventArgs e)
        {
            if (Speech.isBlockingInputs())
            {
                return;
            }
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

        //move states
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

        public void setTextFile(string textFile)
        {
            textFileName = textFile;
        }

        //loads file into text document
        public void loadFromTextFile()
        {
            using (var reader = new StreamReader(Path.Combine(Program.fileDirectory, textFileName)))
            {
                setDocument(reader.ReadToEnd());
            }
        }

        //overrides the current document with a given string
        public void setDocument(string text)
        {
            textBox.Focus();
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.TextLength;
            textBox.SelectedText = text;
        }

        //adds word to document
        public void appendToDocument(string text)
        {
            textBox.Focus();
            textBox.SelectionStart = textBox.TextLength;
            textBox.SelectionLength = 0;
            textBox.SelectedText = text;
        }

        //deletes last character in the document
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

        //saves current document
        public void saveDocumentFile()
        {
            File.WriteAllText(Path.Combine(Program.fileDirectory, textFileName), getDocument());
        }
    }
}