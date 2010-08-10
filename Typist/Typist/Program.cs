using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Typist
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string filePath = args.Length > 0 ? args[0] : "";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TypistForm(filePath));
        }
    }
}
