using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using VirtualMorse.Input;

namespace VirtualMorse.States
{
	public class TypingState : State
    {
        bool isCapitalized = false;

        Dictionary<Switch, Action> switchResponses;

		//state functions
		public TypingState(WritingContext context) : base(context)
		{
			switchResponses = new Dictionary<Switch, Action>(){
				{ Switch.Switch1,  command },
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
            if (switchResponses.ContainsKey(input))
            {
                switchResponses[input]();
            }
        }

        void dot()
		{
			Console.WriteLine("storing dot");
            context.currentMorse += '.';
        }

		void dash()
		{
			Console.WriteLine("storing dash");
            context.currentMorse += '-';
        }

		void space()
		{
			if (context.currentWord != "")
			{
				context.appendToDocument(context.currentWord);
				Console.WriteLine("added word to file: " + context.currentWord);
				Speech.speak(context.currentWord);
                context.clearWord();
            }
			else
			{
				context.appendToDocument(" ");
				Console.WriteLine("SPACE added to file");
				Speech.speak("Space.");
			}
		}

		void shift()
		{
			if (isCapitalized)
			{
                Speech.speak("Shift cancelled.");
            }
			else
			{
				Speech.speak("Shift.");
			}
            isCapitalized = !isCapitalized;
            Console.WriteLine("capitalization set to: " + isCapitalized);
		}

		void enter()
		{
			string morseString = context.currentMorse;
            char nextLetter;
            if (morseString == "" && context.lastLetter != '\0')
			{
                nextLetter = context.lastLetter;
			}
			else
			{
				nextLetter = Function.morseToText(morseString);
				if (isCapitalized)
				{
					nextLetter = char.ToUpper(nextLetter);
					isCapitalized = false;
				}
			}

            PromptBuilder spokenMessage = new PromptBuilder();
            if (nextLetter != '\0')
			{
                context.currentWord += nextLetter;
                context.lastLetter = nextLetter;
                context.clearMorse();
                Console.WriteLine("added letter: " + nextLetter);
				if (context.currentWord == "ttt")
				{
					context.clearWord();
					string document = context.getDocument();
					spokenMessage.AppendText((document != "") ? document : "This page is blank.");
                    Console.WriteLine("\"ttt\" entered - reading entire page");
                }
				else
                {
					if (char.IsUpper(nextLetter))
                    {
                        spokenMessage.AppendText("Capital ");
                    }
                    spokenMessage.AppendTextWithHint(nextLetter.ToString(), SayAs.SpellOut);
                    spokenMessage.AppendText(".");
                }
            }
			else
			{
				context.clearMorse();
				Console.WriteLine("not a valid letter, try again");
				spokenMessage.AppendText("Try again");
            }
            Speech.speak(spokenMessage);
		}

		void backspace()
		{
			if (context.currentWord.Length > 0)
			{
				context.currentWord = context.currentWord.Remove(context.currentWord.Length - 1, 1);
				Console.WriteLine("Delete");
				Speech.speak("Delete.");
			}
			else
			{
				context.backspaceDocument();
				Console.WriteLine("Backspace");
				Speech.speak("Backspace.");
			}
		}

		void save()
        {
            Speech.speak("Now saving.");
            Console.WriteLine("save text doc as is");
			try
            {
                context.saveDocumentFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving file");
                Speech.speak("Error saving file");
                Console.WriteLine(ex.Message);
            }
		}

		void command()
		{
			context.transitionToState(new CommandState(context));
            Console.WriteLine("move to command state");
			Speech.speak("Command Mode.");
        }
	}
}
