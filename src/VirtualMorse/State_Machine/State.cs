using System;

public class State
{
	public State()
	{
	}

	public virtual void shift()
    {

    }

	public virtual void command()
    {

    }

	public virtual void save()
    {

    }

    public virtual void space() 
    {  
    
    }

    public virtual void dot()
    {

    }

    public virtual void dash()
    {

    }

    public virtual void enter()
    {

    }

    public virtual void backspace()
    {

    }

    public virtual string getCurrentWord()
    {
        return "ERROR: GETCURRENTWORD NOT IMPLEMENTED IN CURRENT STATE";
    }

    public virtual string getCurrentLetter()
    {
        return "ERROR: GETCURRENTLETTERNOT IMPLEMENTED IN CURRENT STATE";
    }

    public virtual string getFile()
    {
        return "ERROR: GETFILE NOT IMPLEMENT IN CURRENT STATE";
    }

}
