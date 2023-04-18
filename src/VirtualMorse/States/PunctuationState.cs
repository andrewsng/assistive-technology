using System;
using System.Collections.Generic;
using VirtualMorse.Input;

namespace VirtualMorse.States
{
    public class PunctuationState : State
    {
        Dictionary<Switch, Action> switchResponses;

        public PunctuationState(WritingContext context) : base(context)
        {
            switchResponses = new Dictionary<Switch, Action>(){
                { Switch.Switch1,  enter },
                { Switch.Switch2,  shift },
                { Switch.Switch3,  save },
                { Switch.Switch4,  space },
                { Switch.Switch5,  dot },
                { Switch.Switch6,  dash },
                { Switch.Switch7,  enter },
                { Switch.Switch8,  backspace },
                { Switch.Switch9,  command },
                { Switch.Switch10, command },
            };
        }

        public override void respond(Switch input)
        {
            switchResponses[input]();
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
