using System;

namespace VirtualMorse.States
{
    public class PunctuationState : State
    {
        protected WritingContext context;
        public PunctuationState(WritingContext context)
        {
            this.context = context;
        }

        public override void shift()
        {
            context.appendToDocument("'");
            speak("apostrophe");
            context.setState(context.getTypingState());
        }

        public override void command()
        {
            speak("Move to typing state");
            context.setState(context.getTypingState());
        }

        public override void save()
        {
            context.appendToDocument("\"");
            speak("quotation mark");
            context.setState(context.getTypingState());
        }

        public override void space()
        {
            context.appendToDocument("!");
            speak("exclamation mark");
            context.setState(context.getTypingState());
        }

        public override void dot()
        {
            context.appendToDocument(".");
            speak("period");
            context.setState(context.getTypingState());
        }

        public override void dash()
        {
            context.appendToDocument(",");
            speak("comma");
            context.setState(context.getTypingState());
        }

        public override void enter()
        {
            context.appendToDocument("?");
            speak("question mark");
            context.setState(context.getTypingState());

        }

        public override void backspace()
        {
            context.appendToDocument("-");
            speak("hyphen");
            context.setState(context.getTypingState());
        }
        public void speak(string message)
        {
            context.speaker.SpeakAsyncCancelAll();
            context.speaker.SpeakAsync(message);
        }
    }
}
