using System;

public class StateMachine
{
	State typingState;
	State validateState;
	State backspaceState;
	State commandState;

	State state;

	String current_letter;
	String current_word;
	public StateMachine()
	{
		typingState = new TypingState(this);

		//set initial state
		state = typingState;
	}

	public void dot()
    {
		state.dot();
    }

	public void dash()
    {
		state.dash();
    }
}
