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
