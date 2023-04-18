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

        public void speak(string message)
        {
            //context.speaker.SpeakAsyncCancelAll();
            context.speaker.SpeakAsync(message);
        }

        public void speakLetter(string letter)
        {
            string message = letter;
            if (Char.IsUpper(letter, 0))
            {
                message = "Capital " + letter;
            }
            speak(message);
        }

        public virtual void respond(Switch input)
        {
        
        }
    }
}
