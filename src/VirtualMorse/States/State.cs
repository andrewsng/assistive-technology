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
        public void clearLetter()
        {
            context.currentLetter = "";
        }
        public void clearWord()
        {
            context.currentWord = "";
        }

        public virtual void respond(Switch input)
        {
        
        }

        public virtual void shift()
        {

        }

        public virtual void command()
        {

        }

        public virtual void save()
        {

        }

        public virtual void space()
        {

        }

        public virtual void dot()
        {

        }

        public virtual void dash()
        {

        }

        public virtual void enter()
        {

        }

        public virtual void backspace()
        {

        }
    }
}
