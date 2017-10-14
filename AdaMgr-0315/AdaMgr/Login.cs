using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace AdaMgr
{
    public partial class Login : Form
    {
        
        public static MySqlConnection mysqlConn = null;
        public bool m_bIsInitFail = false;//数据库初始化失败
        //public MySqlCommand mysqlCommand = null;
        
        //构造函数
        public Login()
        {
            InitializeComponent();
            
            //连接数据库
            try
            {
                //连接数据库的命令
                mysqlConn = new MySqlConnection("server=localhost;user id=root;password='admin';database=waterlab");//连接本地数据
                mysqlConn.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Connecting to the Database Failed: "+ex.Message);
                m_bIsInitFail = true;
            }
            
           
        }
       
        //登录按钮
        private void btnLogin_Click(object sender, EventArgs e)
        {

            string userName = textBox1.Text;
            string userPwd = textBox2.Text;
            string msg;

            //判断用户是否输入为空或则没有输入
            if (string.IsNullOrEmpty(textBox1.Text)||string.IsNullOrEmpty(textBox2.Text))
            {
                //msgDiv1为验证码控件
                msgDiv1.MsgDivShow("账户或密码不能为空!",1);
                return ;
            }

            MySqlCommand mysqlCommand  = new MySqlCommand("use waterlab", mysqlConn);
            MySqlDataReader reader = mysqlCommand.ExecuteReader();
            mysqlCommand.Dispose();
            string mysqlStr = String.Format("select * from user where UserName='{0}' and Pwd='{1}'", userName, GetMD5(userPwd));
            mysqlCommand = new MySqlCommand(mysqlStr, mysqlConn);
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            
            reader = mysqlCommand.ExecuteReader();
        
            
            if (reader.HasRows)
            {
                msg = "登录成功!";
                msgDiv1.MsgDivShow(msg, 1, Bind);
            }
            else
            {
                msg = "账户或密码错误!";
                msgDiv1.MsgDivShow(msg, 2);
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            reader.Dispose();
            mysqlCommand.Dispose();
            
            //msgDiv1.MsgDivShow(null, 1, Bind);
        }
       
        //取消按钮
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (mysqlConn!=null && mysqlConn.State!=ConnectionState.Closed)
            {
                mysqlConn.Close();
                mysqlConn.Dispose();
            }
            this.Close();
        }
        //
        void Bind()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        //将密码加密
        private string GetMD5(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] buffer = Encoding.Default.GetBytes(str);
            byte[] md5buffer = md5.ComputeHash(buffer);

            string res = "";
            for (int i = 0; i < md5buffer.Length; i++)
            {
                res += md5buffer[i].ToString("x2");
            }
            return res;
        }

     


       
  
    }
}
