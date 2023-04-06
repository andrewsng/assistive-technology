using System;

namespace VirtualMorse.States
{
    public class CommandState : TypingState
    {
        public CommandState(WritingContext context) : base(context)
        {
        }

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
            switch (commandLetter)
            {
                case "l":
                    Console.WriteLine("read last sentence");
                    break;
                case "g":
                    Console.WriteLine("checks email");
                    break;
                case "d":
                    Console.WriteLine("deletes email");
                    break;
                case "h":
                    Console.WriteLine("read email headers");
                    break;
                case "r":
                    Console.WriteLine("reads email");
                    break;
                case "y":
                    Console.WriteLine("reply to email");
                    break;
                case "n":
                    Console.WriteLine("adds email address nickname");
                    break;
                case "a":
                    Console.WriteLine("ties email address to nickname");
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
            moveToTypingState();
            speak("Command Off.");
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
