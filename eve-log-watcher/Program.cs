using System;
using System.Windows.Forms;

namespace eve_log_watcher
{
    internal static class Program
    {
        [STAThread]
        private static void Main() {
            DbHelper.EnsureDatatabaseExists();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += (sender, args) => {
                if (DbHelper.HasDataContext) {
                    DbHelper.DataContext.Dispose();
                }
            };
            Application.Run(new FormMain());
        }
    }
}
