using System;

namespace VirtualMorse.States
{
    public class ConfirmationState : State
    {
        protected WritingContext context;

        protected bool confirmation;
        State previousState;
        public ConfirmationState(WritingContext context, State previousState)
        {
            this.context = context;
            this.previousState = previousState;
        }

        public override void shift()
        {

        }

        public override void command()
        {

        }

        public override void save()
        {

        }

        public override void space()
        {
            confirmation = true;
            context.setState(previousState);
        }

        public override void dot()
        {

        }

        public override void dash()
        {

        }

        public override void enter()
        {


        }

        public override void backspace()
        {
            confirmation = false;
            context.setState(context.getCommandState());
        }
        public void speak(string message)
        {

        }
    }
}
