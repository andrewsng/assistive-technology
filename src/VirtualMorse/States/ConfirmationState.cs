using System;
using System.Collections.Generic;
using VirtualMorse.Input;

namespace VirtualMorse.States
{
    public class ConfirmationState : State
    {
        Action actionToConfirm;

        Dictionary<Switch, Action> switchResponses;

        public ConfirmationState(WritingContext context, Action actionToConfirm) : base(context)
        {
            this.actionToConfirm = actionToConfirm;

            switchResponses = new Dictionary<Switch, Action>(){
                { Switch.Switch4,  confirm },
                { Switch.Switch8,  cancel },
            };
        }

        public override void respond(Switch input)
        {
            if (switchResponses.ContainsKey(input))
            {
                switchResponses[input]();
            }
        }

        void confirm()
        {
            actionToConfirm();
            context.transitionToState(new TypingState(context));
            Console.WriteLine("Move to typing state.");
        }

        void cancel()
        {
            context.transitionToState(new TypingState(context));
            Console.WriteLine("Move to typing state.");
            Function.speak("Operation cancelled.");
        }
    }
}
