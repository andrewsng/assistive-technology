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
		Console.WriteLine("*");
    }

	public override void dash()
    {
		Console.WriteLine("-");
    }
}
