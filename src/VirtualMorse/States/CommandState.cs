using System;

namespace VirtualMorse.States
{
    public class CommandState : TypingState
    {
        public CommandState(WritingContext context) : base(context)
        {
        }

        static string nickname = "";

        public override void shift()
        {
            Console.WriteLine("Print page");
            moveToTypingState();
        }

        public override void save()
        {
            context.setDocument("");
            moveToTypingState();
            Console.WriteLine("Clear document");
            speak("Document cleared.");
        }

        public override void space()
        {
            moveToTypingState();
            sayUnprogrammedError();
        }

        public override void backspace()
        {
            moveToTypingState();
            sayUnprogrammedError();
        }

        public override void enter()
        {
            string commandLetter = Function.morseToText(context.currentLetter);
            string address = "";
            string contents = "";
            string index = "";
            switch (commandLetter)
            {
                case "l":
                    Console.WriteLine("read last sentence");
                    string sentence = Function.getLastSentence(context.getDocument());
                    Console.WriteLine(sentence);
                    speak(sentence);
                    break;
                case "g":
                    Console.WriteLine("checks email");
                    break;
                case "d":
                    Console.WriteLine("deletes email");
                    index = context.getCurrentWord();
                    break;
                case "h":
                    Console.WriteLine("read email headers");
                    index = context.getCurrentWord();
                    break;
                case "r":
                    Console.WriteLine("reads email");
                    index = context.getCurrentWord();
                    break;
                case "e":
                    Console.WriteLine("create/send email");
                    address = context.getCurrentWord();
                    contents = context.getDocument();
                    break;
                case "y":
                    Console.WriteLine("reply to email");
                    index = context.getCurrentWord();
                    contents = context.getDocument();
                    break;
                case "n":
                    Console.WriteLine("adds email address nickname");
                    nickname = context.getCurrentWord();
                    break;
                case "a":
                    Console.WriteLine("ties email address to nickname");
                    address = context.getCurrentWord();
                    break;
                default:
                    Console.WriteLine("invalid command");
                    sayUnprogrammedError();
                    break;
            }
            moveToTypingState();
        }

        public override void command()
        {
            context.setState(context.getPunctuationState());
            speak("move to punctuation state.");
        }

        // Helper functions

        void moveToTypingState()
        {
            clearLetter();
            context.setState(context.getTypingState());
            Console.WriteLine("move to typing state");
        }

        void sayUnprogrammedError()
        {
            speak("That command is not programmed.");
        }
    }
}
