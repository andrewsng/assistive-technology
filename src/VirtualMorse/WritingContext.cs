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
        List<InputSource> inputSources;
        State state;

        public string currentMorse = "";
        public string currentWord = "";
        public char lastLetter = '\0';

        string textFileName;

        public WritingContext(RichTextBox textBox)
        {
            this.textBox = textBox;

            inputSources = new List<InputSource>();

            state = new TypingState(this);
        }

        void Handler_InputReceived(object sender, SwitchInputEventArgs e)
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

        public void addInputSource(InputSource inputSource)
        {
            inputSource.switchActivated += Handler_InputReceived;
            inputSources.Add(inputSource);
        }

        public void setTextFile(string textFile)
        {
            textFileName = textFile;
        }

        public string getTextFile()
        {
            return textFileName;
        }

        public void loadFromTextFile()
        {
            using (var reader = new StreamReader(Path.Combine(Program.fileDirectory, textFileName)))
            {
                setDocument(reader.ReadToEnd());
            }
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

        public void saveDocumentFile(string filePath)
        {
            Console.WriteLine($"Saving document to {filePath}");
            File.WriteAllText(filePath, getDocument());
        }
    }
}