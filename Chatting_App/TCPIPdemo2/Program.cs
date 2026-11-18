namespace TCPIPdemo2
{
    internal static class Program
    {
        /// <summary>
        ///  This is where the server app starts. I apply the project's Windows Forms settings
        ///  (high DPI mode, default font and visual styles) and then open the server window.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
