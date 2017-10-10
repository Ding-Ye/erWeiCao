using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AdaMgr
{ 
    static class Program
    {       
        /// 应用程序的主入口点
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //创建登录窗口
            Login f1 = new Login();
            //判断初始化是否成功，是否显示
            if ((!f1.m_bIsInitFail) && (f1.ShowDialog() == DialogResult.OK))
            {

                Application.Run(new FrmMain());
            }
        }
    }
}
