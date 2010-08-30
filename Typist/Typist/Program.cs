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

            //if (string.IsNullOrEmpty(filePath))
                //filePath = @"C:\Documents and Settings\Adrian\Desktop\TypingPracticeTexts\SingleParagraph\European Wildcat.txt";
                //filePath = @"C:\Documents and Settings\Adrian\Desktop\TypingPracticeTexts\Wikipedia\Done\Aluminium.txt";
                //filePath = @"C:\Documents and Settings\Adrian\Desktop\TypingPracticeTexts\Wikipedia\Done\Honore de Balzac.txt";
                //filePath = @"C:\Users\Adrian\Samples\TypingPracticeTexts\Wikipedia\Done\Honore de Balzac.txt";
                //filePath = @"C:\Documents and Settings\Adrian\Desktop\TypingPracticeTexts\Code\C\Done\pretty.c.08.txt";
                //filePath = @"C:\Users\Adrian\Samples\TypingPracticeTexts\Code\C\Done\pretty.c.08.txt";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TypistForm(filePath));
        }
    }
}
