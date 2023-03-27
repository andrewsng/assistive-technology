using System;
using VirtualMorse.State_Machine;

public class StateMachine
{
	State typingState;
	State commandState;

	State state;

	
	public StateMachine()
	{
		typingState = new TypingState(this);
        commandState = new CommandState(this);

        //set initial state
        state = typingState;
	}

	//Initialize States
	public void dot()
    {
		state.dot();
    }

	public void dash()
    {
		state.dash();
    }

	public void space()
    {
		state.space();
    }

	public void shift()
    {
		state.shift();
    }

	public void enter()
    {
		state.enter();
    }

	public void backspace()
    {
		state.backspace();
    }

	public void save()
    {
		state.save();
    }

	public void command()
    {
		state.command();
    }

	//fetching / setting functions for states
	public void setState(State state)
    {
		this.state = state;
    }

	public State getState()
    {
		return state;
    }

	public State getTypingState()
    {
		return typingState;
    }

	public State getCommandState()
    {
		return commandState;
    }

	public  string getCurrentWord()
	{
		return state.getCurrentWord();
	}

	public  string getCurrentLetter()
	{
		return state.getCurrentLetter();
	}

	public  string getFile()
	{
		return state.getFile();
	}



}
