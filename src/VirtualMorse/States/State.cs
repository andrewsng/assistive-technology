//base class State machine
//when implementing new states, make sure they extend from this base class
//EX: public class NewState : State {}
using System;
using VirtualMorse.Input;

namespace VirtualMorse.States
{
    public class State
    {
        protected WritingContext context;

        public State(WritingContext context)
        {
            this.context = context;
        }

        public virtual void respond(Switch input)
        {
        
        }
    }
}
