using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SS
{
     class Program : ApplicationContext
    {

         private int formCount = 0;

         private static Program appContext;

         private Program()
         {
         }

         public static Program getAppContext()
         {
             if (appContext == null)
             {
                 appContext = new Program();
             }
             return appContext;
         }

         public void RunForm(Form form)
         {
             formCount++;

             form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

             form.Show();
         }

        /// <summary>
        /// The main entry point for the application.
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
