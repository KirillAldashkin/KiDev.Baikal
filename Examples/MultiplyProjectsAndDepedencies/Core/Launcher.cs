namespace KiDev.Sample.Core;

public abstract class Launcher
{
	public void Run()
	{
		Console.WriteLine("Hello from console!");
		ShowMessage("Message box", "Hello there ^-^");
	}

	protected abstract void ShowMessage(string title, string text);
}