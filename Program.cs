﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopMascot
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 form = new Form1();

            form.Show();

            //Application.Runではなく自分でループを作成
            while (form.Created)
            {
                form.MainLoop();
                Application.DoEvents();
            }
        }
    }
}
