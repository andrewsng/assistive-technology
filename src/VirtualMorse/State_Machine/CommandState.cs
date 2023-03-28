using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualMorse.State_Machine
{
    public class CommandState : TypingState
    {
        public CommandState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void shift()
        {
            Console.WriteLine("Print page");
        }

        public override void save()
        {
            Console.WriteLine("Clear document");
        }

        public override void space()
        {
        }

        public override void backspace()
        {
        }

        public override void enter()
        {
            string c = Function.morseToText(current_letter);
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
            stateMachine.setState(stateMachine.getTypingState());
        }
    }
}
