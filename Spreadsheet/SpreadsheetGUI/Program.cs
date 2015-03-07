using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SS
{
     class Program : ApplicationContext
    {
         // Instance variable used to keep track of how many spreadsheet windows are currently open. 
         private int spreadsheetWindows = 0;

         // Property for controlling the appContext 
         private static Program appContext;

         private Program()
         {
         }

         /// <summary>
         /// Getter method for creating appContext 
         /// </summary>
         /// <returns></returns>
         public static Program getAppContext()
         {
             if (appContext == null)
             {
                 appContext = new Program();
             }
             return appContext;
         }

         /// <summary>
         /// Method for keeping track of when the last spreadsheet window is closed so the program can stop executing. 
         /// </summary>
         /// <param name="form"></param>
         public void RunForm(Form form)
         {
             spreadsheetWindows++;

             form.FormClosed += (o, e) => { if (--spreadsheetWindows <= 0) ExitThread(); };

             form.Show();
         }

        /// <summary>
        /// Starts the execution of the form 
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Program appContext = Program.getAppContext();
            appContext.RunForm(new Form1());
            Application.Run(appContext);
        }
    }
}
