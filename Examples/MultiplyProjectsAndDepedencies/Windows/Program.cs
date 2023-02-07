using System.Runtime.InteropServices;
using KiDev.Sample.Core;

new WindowsLauncher().Run();

class WindowsLauncher : Launcher
{
    protected override void ShowMessage(string title, string text) =>
        MessageBox(0, text, title, MessageBoxType.Button_Ok |
                                   MessageBoxType.Icon_Information |
                                   MessageBoxType.DefaultButton_1 |
                                   MessageBoxType.Modal_System);


    [DllImport("user32.dll")]
    private static extern int MessageBox(nint owner, string text, string caption, MessageBoxType type);
}

enum MessageBoxType : uint
{
    Button_AbortRetryIgnore = 0x00000002,
    Button_CancelTryContinue = 0x00000006,
    Button_Help = 0x00004000,
    Button_Ok = 0x00000000,
    Button_OkCancel = 0x00000001,
    Button_RetryCancel = 0x00000005,
    Button_YesNo = 0x00000004,
    Button_YesNoCancel = 0x00000003,
    
    Icon_Exclamation = 0x00000030,
    Icon_Warning = 0x00000030,
    Icon_Information = 0x00000040,
    Icon_Asterisk = 0x00000040,
    Icon_Question = 0x00000020,
    Icon_Stop = 0x00000010,
    Icon_Error = 0x00000010,
    Icon_Hand = 0x00000010,
    
    DefaultButton_1 = 0x00000000,
    DefaultButton_2 = 0x00000100,
    DefaultButton_3 = 0x00000200,
    DefaultButton_4 = 0x00000300,

    Modal_App = 0x00000000,
    Modal_System = 0x00001000,
    Modal_Task = 0x00002000,
    DefaultDesktopOnly = 0x00020000,
    Right = 0x00080000,
    RTL = 0x00100000,
    SetForeground = 0x00010000,
    TopMost = 0x00040000,
    ServiceNotification = 0x00200000
}