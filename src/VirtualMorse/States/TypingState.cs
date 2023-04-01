using System;
using System.IO;

public class TypingState : State
{
	protected WritingContext context;

    string lastLetter = "";
	bool isCapitalized = false;

	//state functions
	public TypingState(WritingContext context)
	{
		this.context = context;
	}

	public override void dot()
    {
		Console.WriteLine("storing dot");
		addDot();
	}

	public override void dash()
    {
		Console.WriteLine("storing dash, if TTT is entered: read entire page and clear TTT");
		addDash();
	}

    public override void space()
    {
		if(context.currentWord != "")
        {
			context.appendToDocument(context.currentWord);
			clearWord();
            Console.WriteLine("added word to file: " + context.currentWord);
        }
        else
        {
            context.appendToDocument(" ");
            Console.WriteLine("SPACE added to file");
        }
	}

    public override void shift()
    {
		toggleCapitalized();
		Console.WriteLine("capitalization set to: " + isCapitalized);
	}

    public override void enter()
    {
		if (context.currentLetter == "" && lastLetter != "")
		{
            addLetterToWord(lastLetter);
            Console.WriteLine("added letter: " + lastLetter);
        }
		else
        {
            string c = Function.morseToText(context.currentLetter);
            if (c != "")
            {
                if (isCapitalized)
                {
                    c = c.ToUpper();
                    isCapitalized = false;
                }
                addLetterToWord(c);
                Console.WriteLine("added letter: " + c);
            }
            else
            {
                clearLetter();
                Console.WriteLine("not a valid letter, try again");
            }
        }
	}

    public override void backspace()
    {
		if (context.currentLetter.Length > 0)
		{
			clearLetter();
            Console.WriteLine("Clearing morse symbols");
        }
		else if (context.currentWord.Length > 0)
		{
			context.currentWord = context.currentWord.Remove(context.currentWord.Length - 1, 1);
			Console.WriteLine("Delete");
		}
		else
		{
			context.backspaceDocument();
			Console.WriteLine("Backspace");
		}
	}

    public override void save()
    {
		Console.WriteLine("save text doc as is");
		Console.WriteLine("says 'now saving'");
		context.saveDocumentFile();
    }

    public override void command()
    {
		Console.WriteLine("move to command state");
		context.setState(context.getCommandState());
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

	public void clearLetter()
	{
		context.currentLetter = "";
	}

	public void addLetterToWord(string c)
	{
		context.currentWord += c;
		lastLetter = c;
		clearLetter();
	}
	public void clearWord()
	{
		context.currentWord = "";
	}

	public void toggleCapitalized()
	{
		isCapitalized = !isCapitalized;
	}
}
