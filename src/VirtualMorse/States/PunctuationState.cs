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
                { Switch.Switch1,  command },
                { Switch.Switch2,  enterPunctuation("'",  "apostrophe") },
                { Switch.Switch3,  enterPunctuation("\"", "quotation mark") },
                { Switch.Switch4,  enterPunctuation("!",  "exclamation mark") },
                { Switch.Switch5,  enterPunctuation(".",  "period") },
                { Switch.Switch6,  enterPunctuation(",",  "comma") },
                { Switch.Switch7,  enterPunctuation("?",  "question mark") },
                { Switch.Switch8,  enterPunctuation("-",  "hyphen") },
                { Switch.Switch9,  command },
                { Switch.Switch10, command },
            };
        }

        public override void respond(Switch input)
        {
            if (switchResponses.ContainsKey(input))
            {
                switchResponses[input]();
            }
        }

        Action enterPunctuation(string punctuation, string spokenMessage)
        {
            return () =>
            {
                context.appendToDocument(punctuation);
                Function.speak(spokenMessage);
                context.setState(context.getTypingState());
            };
        }

        void command()
        {
            Function.speak("Move to typing state");
            context.setState(context.getTypingState());
        }
    }
}
