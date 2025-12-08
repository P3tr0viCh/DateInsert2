using System;
using System.Windows.Forms;

namespace DateInsert2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            if (AppOneInstance.Default.IsFirstInstance)
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Main());
                }
                finally
                {
                    AppOneInstance.Default.Release();
                }
            }
        }
    }
}