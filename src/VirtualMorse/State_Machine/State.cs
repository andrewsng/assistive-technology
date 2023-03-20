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
        Console.WriteLine("dot not implemented correct");
    }

    public virtual void dash()
    {
        Console.WriteLine("dash not implemented correct");
    }

    public virtual void enter()
    {

    }

    public virtual void backspace()
    {

    }

}
