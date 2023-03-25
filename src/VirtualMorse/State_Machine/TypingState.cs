using System;

public class TypingState : State
{
	StateMachine stateMachine;

	string current_letter = "";
	string current_word = "";
	string text_file = "";
	bool is_Capitalized = false;

	//state functions
	public TypingState(StateMachine stateMachine)
	{
		this.stateMachine = stateMachine;
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
		if(current_word != "")
        {
			text_file += current_word;
			Console.WriteLine("added word to file: " + current_word);
			clearWord();
        }
        else
        {
			text_file += " ";
			Console.WriteLine("SPACE added to file");
        }
	}

    public override void shift()
    {
		toggleCapitalized();
		Console.WriteLine("capitalization set to: " + is_Capitalized);
	}

    public override void enter()
    {
		string c = Function.morseToText(current_letter);
		if (c != "")
        {
			if (is_Capitalized)
			{
				c = c.ToUpper();
				is_Capitalized = false;
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

    public override void backspace()
    {
	
		if (current_word.Length > 0)
		{
			current_word = current_word.Remove(current_word.Length - 1, 1);
			clearLetter();
			Console.WriteLine("Deleting last letter of current word");
		}
		else if (text_file.Length > 0)
		{
			text_file = text_file.Remove(text_file.Length - 1, 1);
			string[] words = text_file.Split(' ');
			current_word = words[words.Length - 1];
			text_file = text_file.Remove(text_file.Length - current_word.Length, current_word.Length);
			Console.WriteLine("Backspace");
		}
	}

    public override void save()
    {
		Console.WriteLine("save text doc as is");
		Console.WriteLine("says 'now saving'");
    }

    public override void command()
    {
		Console.WriteLine("move to command state");
		//stateMachine.setState(stateMachine.getCommandState());
    }

	public override string getCurrentWord()
	{
		return current_word;
	}

	public override string getCurrentLetter()
	{
		return current_letter;
	}

	public override string getFile()
	{
		return text_file;
	}

	//helper functions
	public void addDot()
	{
		current_letter += '.';
	}

	public void addDash()
	{
		current_letter += '-';
	}

	public void clearLetter()
	{
		current_letter = "";
	}

	public void addLetterToWord(string c)
	{
		current_word += c;
		current_letter = "";
	}
	public void clearWord()
	{
		current_word = "";
	}

	//retrival / setting functions for member variables


	public void toggleCapitalized()
	{
		is_Capitalized = !is_Capitalized;
	}
}
