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
            context.setState(previousState);
        }

        void backspace()
        {
            confirmation = false;
            context.setState(context.getCommandState());
        }
        public void speak(string message)
        {

        }
    }
}
