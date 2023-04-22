using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
				Function.speak(context.currentWord);
                context.clearWord();
            }
			else
			{
				context.appendToDocument(" ");
				Console.WriteLine("SPACE added to file");
				Function.speak("Space.");
			}
		}

		void shift()
		{
            isCapitalized = !isCapitalized;
            Console.WriteLine("capitalization set to: " + isCapitalized);
			Function.speak("shift");
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

            string spokenMessage;
            if (nextLetter != '\0')
			{
                context.currentWord += nextLetter;
                context.lastLetter = nextLetter;
                context.clearMorse();
                Console.WriteLine("added letter: " + nextLetter);
				if (context.currentWord == "ttt")
				{
					context.clearWord();
					spokenMessage = context.getDocument();
					if (spokenMessage == "")
					{
						spokenMessage = "This page is blank.";
					}
                    Console.WriteLine("\"ttt\" entered - reading entire page");
                }
				else
                {
                    spokenMessage = nextLetter.ToString();
                    if (char.IsUpper(nextLetter))
                    {
                        spokenMessage = "Capital " + spokenMessage;
                    }
                }
			}
			else
			{
				context.clearMorse();
				Console.WriteLine("not a valid letter, try again");
				spokenMessage = "Try again";
			}
			Function.speak(spokenMessage);
		}

		void backspace()
		{
			if (context.currentWord.Length > 0)
			{
				context.currentWord = context.currentWord.Remove(context.currentWord.Length - 1, 1);
				Console.WriteLine("Delete");
				Function.speak("Delete.");
			}
			else
			{
				context.backspaceDocument();
				Console.WriteLine("Backspace");
				Function.speak("Backspace.");
			}
		}

		void save()
		{
			Console.WriteLine("save text doc as is");
			context.saveDocumentFile();
			Function.speak("Now saving.");
		}

		void command()
		{
			context.transitionToState(new CommandState(context));
            Console.WriteLine("move to command state");
			Function.speak("Command Level 1.");
        }
	}
}
