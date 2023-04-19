using System;

namespace VirtualMorse.States
{
    public class ConfirmationState : State
    {
        protected bool confirmation;
        State previousState;
        public ConfirmationState(WritingContext context) : base(context)
        {
        }

        void space()
        {
            confirmation = true;
            context.transitionToState(previousState);
        }

        void backspace()
        {
            confirmation = false;
            context.transitionToState(new CommandState(context));
        }
        public void speak(string message)
        {

        }
    }
}
