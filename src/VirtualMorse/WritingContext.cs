using System;
using System.Collections.Generic;
using System.Drawing;
using System.Speech.Synthesis;
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

        public SpeechSynthesizer speaker;

        State typingState;
        State commandState;
        State punctuationState;
        State ConfirmationState;

        State currentState;

        public string currentLetter = "";
        public string currentWord = "";

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

            Board = new ArduinoComms(textBox);
            Board.buttonPressed += Handler_InputReceived;

            speaker = new SpeechSynthesizer();
            speaker.SetOutputToDefaultAudioDevice();
            speaker.SpeakAsync(Program.programName + " " + Program.programVersion);

            typingState = new TypingState(this);
            commandState = new CommandState(this);
            currentState = typingState;  //set initial state

            directory = AppDomain.CurrentDomain.BaseDirectory;
            directory = directory.Replace("bin\\Debug\\", "Text_documents\\");
            List<string> fileContents = Function.readFullFile(directory, file);
            if (fileContents.Count > 0)
            {
                setDocument((Function.readFullFile(directory, file))[0]);
            }

            punctuationState = new PunctuationState(this);
            ConfirmationState = new ConfirmationState(this, this.currentState);
        }

        private void Handler_InputReceived(object sender, SwitchInputEventArgs e)
        {
            Switch input = e.switchInput;
            switch (input)
            {
                case Switch.Switch1:  // Fall through
                case Switch.Switch9:
                    currentState.command();
                    break;
                case Switch.Switch2:
                    currentState.shift();
                    break;
                case Switch.Switch3:
                    currentState.save();
                    break;
                case Switch.Switch4:
                    currentState.space();
                    break;
                case Switch.Switch5:
                    currentState.dot();
                    break;
                case Switch.Switch6:
                    currentState.dash();
                    break;
                case Switch.Switch7:
                    currentState.enter();
                    break;
                case Switch.Switch8:
                    currentState.backspace();
                    break;
            }
            Console.WriteLine("current letter: '" + getCurrentLetter() + "'");
            Console.WriteLine("current word: '" + getCurrentWord() + "'");
            Console.WriteLine("current document: '" + getDocument() + "'");
            Console.WriteLine();
        }

        //fetching / setting functions

        public RichTextBox getTextBox()
        {
            return textBox;
        }

        public void setState(State state)
        {
            this.currentState = state;
        }


        public State getState()
        {
            return currentState;
        }

        public State getTypingState()
        {
            return typingState;
        }

        public State getCommandState()
        {
            return commandState;
        }

        public State getPunctuationState()
        {
            return punctuationState;
        }

        public string getCurrentWord()
        {
            return currentWord;
        }

        public string getCurrentLetter()
        {
            return currentLetter;
        }

        public string getDocument()
        {
            return textBox.Text;
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