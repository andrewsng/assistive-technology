// abstract class State machine
// Base class for classes implementing a state machine State
// When implementing new states, make sure they extend from this base class
//   EX: public class NewState : State {}

using System;
using VirtualMorse.Input;

namespace VirtualMorse.States
{
    public abstract class State
    {
        // Gives all derived states a reference to a WritingContext
        protected WritingContext context;

        public State(WritingContext context)
        {
            this.context = context;
        }

        // All derived states must implement this function
        //   for responding to a Switch input
        // Typically, the derived states have a dictionary containing
        //   associations between switch values and functions, then
        //   'respond' calls the function associated with the input switch value.
        public abstract void respond(Switch input);
    }
}
