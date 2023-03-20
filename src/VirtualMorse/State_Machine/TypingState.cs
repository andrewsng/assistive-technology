using System;

public class TypingState : State
{
	StateMachine stateMachine;
	public TypingState(StateMachine stateMachine)
	{
		this.stateMachine = stateMachine;
	}

	public override void dot()
    {
		Console.WriteLine("storing dot");
		stateMachine.addDot();
		Console.WriteLine();
	}

	public override void dash()
    {
		Console.WriteLine("storing dash");
		Console.WriteLine("if TTT is entered: read entire page and clear TTT");
		stateMachine.addDash();
		Console.WriteLine();
	}

    public override void space()
    {
		Console.WriteLine("read current word: " + stateMachine.getCurrentWord());
		Console.WriteLine("add current word to page");
		Console.WriteLine("if no current word, add space and say 'Space'");
		stateMachine.clearWord();
		Console.WriteLine();
	}

    public override void shift()
    {
		Console.WriteLine("toggle capitalization");
		stateMachine.setIsCapitalized(true);
		Console.WriteLine("say shift");
		Console.WriteLine();
	}

    public override void enter()
    {
		Console.WriteLine("move to validate");
		//stateMachine.setState(stateMachine.getValidateState());
		stateMachine.addLetterToWord();
		Console.WriteLine();
	}

    public override void backspace()
    {
		Console.WriteLine("move to Backspace");
		//stateMachine.setState(stateMachine.getBackspaceState());
		Console.WriteLine();
	}

    public override void save()
    {
		Console.WriteLine("save text doc as is");
		Console.WriteLine("says 'now saving'");
		Console.WriteLine();
    }

    public override void command()
    {
		Console.WriteLine("move to command state");
		//stateMachine.setState(stateMachine.getCommandState());
    }
}
