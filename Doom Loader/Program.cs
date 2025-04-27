namespace Doom_Loader
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    if (arg == "--info" || arg == "-i")
                    {
                        About about = new()
                        {
                            StartPosition = FormStartPosition.CenterScreen
                        };
                        Application.Run(about);
                        Environment.Exit(0);
                    }
                }
            }
            Application.Run(new Main());
        }
    }
}