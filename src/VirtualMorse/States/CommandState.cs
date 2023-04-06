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
            Console.WriteLine("Clear document");
            moveToTypingState();
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
            string c = Function.morseToText(context.currentLetter);
            if (c != "")
            {
                Console.WriteLine("Valid letter");
                Function.parseCommand(c);
            }
            else
            {
                Console.WriteLine("Invalid letter");
                sayUnprogrammedError();
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
