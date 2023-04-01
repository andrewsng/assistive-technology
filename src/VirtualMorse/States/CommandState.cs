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
            command();
        }

        public override void save()
        {
            Console.WriteLine("Clear document");
            command();
        }

        public override void space()
        {
        }

        public override void backspace()
        {
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
            }
            command(); // clears letter, returns to typing state
        }

        public override void command()
        {
            Console.WriteLine("move to typing state");
            clearLetter();
            context.setState(context.getTypingState());
        }
    }
}
