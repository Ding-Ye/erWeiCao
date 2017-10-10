using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AdaMgr
{
    //实时绘图，温度报表
    public partial class Water : Form
    {
        private string sql = null;//定义sql语句

        //构造函数
        public Water()
        {
            InitializeComponent();
        }

        //点击温度报表窗口的查询按钮
          private void button_Click_1(object sender, EventArgs e)
        {
            DateTime startDate = dateTimePicker1.Value;
            DateTime endDate = dateTimePicker2.Value;
            TimeSpan dateDiff = endDate.Subtract(startDate);     //开始日期和结束两日期差

            if (dateDiff.Days <= 0)   //判断开始日期小于结束日期
            {
                MessageBox.Show("结束时间小于起始时间，请重新选择!");
                //MessageBox.Show(String.Format("{0:yyyy-MM-dd HH:mm:ss}", startDate));
                // MessageBox.Show(startDate.ToString("yyyy-MM-dd"));
                return;
            }

            string startDateFormat = startDate.ToString("yyyy-MM-dd");
            string strStartDate = startDate.ToString("yyyy-MM-dd");
            string strEndDate = endDate.ToString("yyyy-MM-dd");

            //从数据库中读取15路温度
            sql = "select AddDatetime,t1,t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12,t13 from water_data where AddDatetime between '" + strStartDate + " 00:00:00' and '" + strEndDate + " 00:00:00'";

            //sql = "select AddDatetime,t1,t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12,t13,t14,t15 from temp_data limit 50";

            List<TempInfo> list = new List<TempInfo>();
            try
            {
                using (MySqlCommand mysqlCommand = new MySqlCommand(sql, Login.mysqlConn))
                {
                    MySqlDataReader reader = mysqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        TempInfo mem = AddTempDataMember(reader);
                        list.Add(mem);
                    }
                    reader.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("DataBase Operate Failed: " + ex.Message);
            }
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = list;
        }
    

        private TempInfo AddTempDataMember(MySqlDataReader rd)
        {
            TempInfo mem = new TempInfo();
            mem.AddDatetime = Convert.ToDateTime(rd[0]);
            mem.t1 = rd[1].ToString();
            mem.t2 = rd[2].ToString();
            mem.t3 = rd[3].ToString();
            mem.t4 = rd[4].ToString();
            mem.t5 = rd[5].ToString();
            mem.t6 = rd[6].ToString();
            mem.t7 = rd[7].ToString();
            mem.t8 = rd[8].ToString();
            mem.t9 = rd[9].ToString();
            mem.t10 = rd[10].ToString();
            mem.t11 = rd[11].ToString();
            mem.t12 = rd[12].ToString();
            mem.t13 = rd[13].ToString();
            // mem.t14 = rd[14].ToString();
            // mem.t15 = rd[15].ToString();
            // MessageBox.Show(rd[13].ToString() + "\t"+rd[1].ToString()+ "\t"+rd[2].ToString());
            return mem;
        }

    }  
}
