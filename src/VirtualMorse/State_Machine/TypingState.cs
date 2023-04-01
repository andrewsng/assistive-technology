﻿using System;
using System.IO;

public class TypingState : State
{
	protected StateMachine stateMachine;

    string lastLetter = "";
    
	bool isCapitalized = false;
	string directory;
	string file = "test.txt";



	//state functions
	public TypingState(StateMachine stateMachine)
	{
		this.stateMachine = stateMachine;

		directory = AppDomain.CurrentDomain.BaseDirectory;
		directory = directory.Replace("bin\\Debug\\", "Text_documents\\");
		stateMachine.currentDocument = Function.readFullFile(directory, file)[0];
		string[] words = stateMachine.currentDocument.Split(' ');
		stateMachine.currentWord = words[words.Length - 1];
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
		if(stateMachine.currentWord != "")
        {
			stateMachine.currentDocument += stateMachine.currentWord;
			Console.WriteLine("added word to file: " + stateMachine.currentWord);
			string file = "test.txt";
			using (StreamWriter writer = new StreamWriter(directory + file))
			{
				writer.WriteLine(directory);
				writer.WriteLine("it do");
			}
			clearWord();
        }
        else
        {
			stateMachine.currentDocument += " ";
			Console.WriteLine("SPACE added to file");
        }
		Function.addToFile(directory, file, stateMachine.currentDocument);
	}

    public override void shift()
    {
		toggleCapitalized();
		Console.WriteLine("capitalization set to: " + isCapitalized);
	}

    public override void enter()
    {
		if (stateMachine.currentLetter == "" && lastLetter != "")
		{
            addLetterToWord(lastLetter);
            Console.WriteLine("added letter: " + lastLetter);
        }
		else
        {
            string c = Function.morseToText(stateMachine.currentLetter);
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
		if (stateMachine.currentLetter.Length > 0)
		{
			clearLetter();
            Console.WriteLine("Clearing morse symbols");
        }
		else if (stateMachine.currentWord.Length > 0)
		{
			stateMachine.currentWord = stateMachine.currentWord.Remove(stateMachine.currentWord.Length - 1, 1);
			Console.WriteLine("Delete");
		}
		else if (stateMachine.currentDocument.Length > 0)
		{
			stateMachine.currentDocument = stateMachine.currentDocument.Remove(stateMachine.currentDocument.Length - 1, 1);
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
		stateMachine.setState(stateMachine.getCommandState());
    }

	public override string getCurrentWord()
	{
		return stateMachine.currentWord;
	}

	public override string getCurrentLetter()
	{
		return stateMachine.currentLetter;
	}

	public override string getFile()
	{
		return stateMachine.currentDocument;
	}

	//helper functions
	public void addDot()
	{
		stateMachine.currentLetter += '.';
	}

	public void addDash()
	{
		stateMachine.currentLetter += '-';
	}

	public void clearLetter()
	{
		stateMachine.currentLetter = "";
	}

	public void addLetterToWord(string c)
	{
		stateMachine.currentWord += c;
		lastLetter = c;
		clearLetter();
	}
	public void clearWord()
	{
		stateMachine.currentWord = "";
	}

	//retrival / setting functions for member variables


	public void toggleCapitalized()
	{
		isCapitalized = !isCapitalized;
	}
}
