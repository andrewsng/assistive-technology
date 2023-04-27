using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
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
                { Switch.Switch2,  enterPunctuation("'") },
                { Switch.Switch3,  enterPunctuation("\"") },
                { Switch.Switch4,  enterPunctuation("!") },
                { Switch.Switch5,  enterPunctuation(".") },
                { Switch.Switch6,  enterPunctuation(",") },
                { Switch.Switch7,  enterPunctuation("?") },
                { Switch.Switch8,  enterPunctuation("-") },
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

        Action enterPunctuation(string punctuation)
        {
            return () =>
            {
                if (context.currentWord != "")
                {
                    context.currentWord += punctuation;
                }
                else
                {
                    context.appendToDocument(punctuation);
                }
                PromptBuilder spokenMessage = new PromptBuilder();
                spokenMessage.AppendTextWithHint(punctuation, SayAs.SpellOut);
                spokenMessage.AppendText(".");
                Function.speak(spokenMessage);
                context.transitionToState(new TypingState(context));
                Console.WriteLine("Move to typing state.");
            };
        }

        void command()
        {
            context.transitionToState(new TypingState(context));
            Console.WriteLine("Move to typing state.");
            Function.speak("Command off.");
        }
    }
}
