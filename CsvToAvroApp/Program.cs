﻿using System;
using System.Windows.Forms;
using NLog;

namespace CsvToAvroApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CsvToAvroForm());
        
        }
    }
}
