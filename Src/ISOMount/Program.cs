using System;
using System.Windows.Forms;

namespace IsoMount
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new Main());
            }
            catch (Exception ex)
            {
                var errorForm = new Error(ex);
                errorForm.ShowDialog();
            }
        }
    }
}