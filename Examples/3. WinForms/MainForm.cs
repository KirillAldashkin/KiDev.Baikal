class MainForm : Form
{
    public MainForm()
    {
        Text = "Test \"KiDev.Baikal\"";
        Size = new(320, 240);
        var button = new Button() {
            Location = new(6, 6),
            Text = "Exit",
            Size = new(64, 21)
        };
        Controls.Add(button);
        button.Click += (_, _) => Close();
    }
}