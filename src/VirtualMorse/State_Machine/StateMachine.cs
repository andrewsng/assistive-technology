using System;

public class StateMachine
{
	State typingState;
	State validateState;
	State backspaceState;
	State commandState;

	String current_letter;
	String current_word;
	public StateMachine()
	{
	}
}
