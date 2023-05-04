//confirmation state
//used to confirm certain actions
//space is confirm, backspace is cancel
using System;
using System.Collections.Generic;
using VirtualMorse.Input;

namespace VirtualMorse.States
{
    public class ConfirmationState : State
    {
        Action actionToConfirm;
        static string instructions = "Press space to confirm or backspace to cancel.";

        Dictionary<Switch, Action> switchResponses;

        public ConfirmationState(WritingContext context, Action actionToConfirm) : base(context)
        {
            this.actionToConfirm = actionToConfirm;

            switchResponses = new Dictionary<Switch, Action>(){
                { Switch.Switch4,  confirm },
                { Switch.Switch8,  cancel },
            };

            Speech.speak(instructions);
        }

        public override void respond(Switch input)
        {
            if (switchResponses.ContainsKey(input))
            {
                switchResponses[input]();
            }
            else
            {
                Speech.speak("Invalid entry. " + instructions);
            }
        }

        void confirm()
        {
            Speech.speakFully("Confirmed.");
            actionToConfirm();
            context.transitionToState(new TypingState(context));
            Console.WriteLine("Move to typing state.");
        }

        void cancel()
        {
            Speech.speakFully("Cancelled.");
            context.transitionToState(new TypingState(context));
            Console.WriteLine("Move to typing state.");
        }
    }
}
