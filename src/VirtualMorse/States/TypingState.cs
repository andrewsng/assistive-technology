using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VirtualMorse.Input;

namespace VirtualMorse.States
{
	public class TypingState : State
	{
		string lastLetter = "";
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

        public override void dot()
		{
			Console.WriteLine("storing dot");
			addDot();
		}

		public override void dash()
		{
			Console.WriteLine("storing dash");
			addDash();
		}

		public override void space()
		{
			if (context.currentWord != "")
			{
				context.appendToDocument(context.currentWord);
				Console.WriteLine("added word to file: " + context.currentWord);
				speak(context.currentWord);
                clearWord();
            }
			else
			{
				context.appendToDocument(" ");
				Console.WriteLine("SPACE added to file");
				speak("Space.");
			}
		}

		public override void shift()
		{
			toggleCapitalized();
			Console.WriteLine("capitalization set to: " + isCapitalized);
			speak("shift");
		}

		public override void enter()
		{
			string spokenMessage;
			string letter;
			if (context.currentLetter == "" && lastLetter != "")
			{
				letter = lastLetter;
			}
			else
			{
				letter = Function.morseToText(context.currentLetter);
				if (isCapitalized)
				{
					letter = letter.ToUpper();
					isCapitalized = false;
				}
			}

			if (letter != "")
			{
				addLetterToWord(letter);
				Console.WriteLine("added letter: " + letter);
				if (context.currentWord == "ttt")
				{
					clearWord();
					spokenMessage = context.getDocument();
					if (spokenMessage == "")
					{
						spokenMessage = "This page is blank.";
					}
                    Console.WriteLine("\"ttt\" entered - reading entire page");
                }
				else
                {
                    spokenMessage = letter;
                    if (Char.IsUpper(letter, 0))
                    {
                        spokenMessage = "Capital " + spokenMessage;
                    }
                }
			}
			else
			{
				clearLetter();
				Console.WriteLine("not a valid letter, try again");
				spokenMessage = "Try again";
			}
			speak(spokenMessage);
		}

		public override void backspace()
		{
			if (context.currentWord.Length > 0)
			{
				context.currentWord = context.currentWord.Remove(context.currentWord.Length - 1, 1);
				Console.WriteLine("Delete");
				speak("Delete.");
			}
			else
			{
				context.backspaceDocument();
				Console.WriteLine("Backspace");
				speak("Backspace.");
			}
		}

		public override void save()
		{
			Console.WriteLine("save text doc as is");
			context.saveDocumentFile();
			speak("Now saving.");
		}

		public override void command()
		{
			context.setState(context.getCommandState());
            Console.WriteLine("move to command state");
			speak("Command On.");
        }

		//helper functions
		public void addDot()
		{
			context.currentLetter += '.';
		}

		public void addDash()
		{
			context.currentLetter += '-';
		}

		public void addLetterToWord(string c)
		{
			context.currentWord += c;
			lastLetter = c;
			clearLetter();
        }

		public void toggleCapitalized()
		{
			isCapitalized = !isCapitalized;
		}
	}
}
