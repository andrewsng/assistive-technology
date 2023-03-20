using System;

public class StateMachine
{
	State typingState;
	State validateState;
	State backspaceState;
	State commandState;

	State state;

	string current_letter;
	string current_word;
	bool is_Capitalized = false;
	public StateMachine()
	{
		typingState = new TypingState(this);

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

	public State getValidateState()
    {
		return validateState;
    }

	public State getBackspaceState()
    {
		return backspaceState;
    }

	public State getCommandState()
    {
		return commandState;
    }


	//retrival / setting functions for member variables
	public string getCurrentLetter()
    {
		return current_letter;
    }
	public string getCurrentWord()
	{
		return current_word;
	}

	public void setIsCapitalized(bool boolean)
    {
		is_Capitalized = boolean;
    }


	//helper functions
	public void addDot()
    {
		current_letter += '*';
    }

	public void addDash()
    {
		current_letter += '-';
    }

	public void clearLetter()
    {
		current_letter = "";
    }

	public void addLetterToWord()
    {
		current_word += current_letter + " ";
		current_letter = "";
    }
	public void clearWord()
    {
		current_word = "";
    }
}
