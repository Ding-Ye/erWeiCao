using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using MySql.Data.MySqlClient;

namespace AdaMgr
{
    //实时绘图
    public partial class SingleTemp : Form
    {
        int sequence = 0;
        Random ran = new Random();
        //SmplPBForm sm = new SmplPBForm();

        PointPairList list1 = new PointPairList();
        PointPairList list2 = new PointPairList();
        PointPairList list3 = new PointPairList();
        PointPairList list4 = new PointPairList();

    
        
        LineItem myCurve1;
        LineItem myCurve2;
        LineItem myCurve3;
        LineItem myCurve4;

    
        //构造函数
        public SingleTemp()
        {
            InitializeComponent();
            RealTimeDraw();
        }

        public SingleTemp(int seq)
        {
            InitializeComponent();
            RealTimeDraw();
        }

        public void RealTimeDraw()
        {
            this.zedGraphControl1.Visible = true;//控件1-实时绘图可见
            this.zedGraphControl2.Visible = false;//控件2-历史绘图不可见

            if (myCurve1 != null)//判断myCurve1是不是为空
            {
                myCurve1.Clear();
                myCurve3.Clear();
                myCurve4.Clear();
       

            }

            string str = "温度实时变化图";
            this.zedGraphControl1.GraphPane.Title.Text = str;

            this.zedGraphControl1.GraphPane.XAxis.Title.Text = "时间";
            this.zedGraphControl1.GraphPane.YAxis.Title.Text = "温度";

            this.zedGraphControl1.GraphPane.XAxis.MajorGrid.IsVisible = true;
            this.zedGraphControl1.GraphPane.YAxis.MajorGrid.IsVisible = true;
            // this.zedGraphControl1.GraphPane.XAxis.MajorGrid.Color = Color.LightGray;
             // this.zedGraphControl1.GraphPane.YAxis.MajorGrid.Color = Color.LightGray;
            this.zedGraphControl1.GraphPane.XAxis.Type = ZedGraph.AxisType.DateAsOrdinal;//X轴以时间为单位

  /*          for (int i = 0; i < FrmMain.realTimeValue[4].Count; i++)
            {
                double x = (double)new XDate(DateTime.Now.AddMinutes(-(60 - 1 - i)));//60分钟
                //double y = ran.Next(20, 30);
                double y1 = FrmMain.realTimeValue[4][i];//取第5,10,15三路温度
                double y2 = FrmMain.realTimeValue[9][i];
                double y3 = FrmMain.realTimeValue[12][i];

                //double y = 0;

                list1.Add(x, y1);//给list1,3,4分别进行赋值
                list3.Add(x, y2);
                list4.Add(x, y3);

            }
*/
       
            for (int i = 0; i < FrmMain.realTimeValue[4].Count; i++)
            {
            
               double x = (double)new XDate(DateTime.Now.AddMinutes(-(60 - 1 - i)));//60分钟
              
                //double y = ran.Next(20, 30);
                double y1 = FrmMain.realTimeValue[4][i];//取第5,10,15三路温度
                double y2 = FrmMain.realTimeValue[9][i];
                double y3 = FrmMain.realTimeValue[12][i];

                //double y = 0;

                list1.Add(x, y1);//给list1,3,4分别进行赋值
                list3.Add(x, y2);
                list4.Add(x, y3);
              
            }

            int dt = DateTime.Now.Minute;
          // Console.WriteLine(dt);
           // MessageBox.Show(dt.ToString());

            myCurve1 = zedGraphControl1.GraphPane.AddCurve("第四路", list1, Color.Red, SymbolType.Diamond);
            myCurve1.Line.Width = 2.0F;
            myCurve3 = zedGraphControl1.GraphPane.AddCurve("第九路", list3, Color.Green, SymbolType.Circle);
            myCurve3.Line.Width = 2.0F;
            myCurve4 = zedGraphControl1.GraphPane.AddCurve("第十二路", list4, Color.Blue, SymbolType.Star);
            myCurve4.Line.Width = 2.0F;

            //刷新
            this.zedGraphControl1.AxisChange();
            this.zedGraphControl1.Refresh();
        }

        public void HistoryDraw()
        {
            if (myCurve2 != null)
            {
                myCurve2.Clear();
            }
            DateTime startDate = dateTimePicker1.Value;
            DateTime endDate = dateTimePicker2.Value;
            TimeSpan dateDiff = endDate.Subtract(startDate);
            if (dateDiff.Days < 0)
            {
                MessageBox.Show("结束时间小于起始时间，请重新选择!");
                //MessageBox.Show(String.Format("{0:yyyy-MM-dd HH:mm:ss}", startDate));
               // MessageBox.Show(startDate.ToString("yyyy-MM-dd"));
                RealTimeDraw();
                return;
            }
            else if (dateDiff.Days == 0)
            {

            }
            else
            {

            }

            //string startDateFormat = String.Format("{0:yyyy-MM-dd HH:mm:ss}", startDate);
           // string endDateFormat = String.Format("{0:yyyy-MM-dd HH:mm:ss}", startDate);

            string startDateFormat = startDate.ToString("yyyy-MM-dd");
            string endDateFormat = endDate.ToString("yyyy-MM-dd");
            //MessageBox.Show(startDateFormat);
            //MessageBox.Show(endDateFormat);
            string sql = "select tt.t"+sequence+" from (select * from temp_data where AddDatetime between '"+startDateFormat+" 12:00:00' and '"
            +endDateFormat+" 12:03:00') as tt where tt.Adddatetime like '% 12:00:%'";
            //MessageBox.Show(sql);
            List<float> temp_list = new List<float>();
            try
            {
                using (MySqlCommand mysqlCommand = new MySqlCommand(sql, Login.mysqlConn))
                {
                    MySqlDataReader reader = mysqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        temp_list.Add(reader.GetFloat(0));
                        //Console.WriteLine(reader.GetFloat(0));
                    }
                    reader.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("DataBase Query Failed: " + ex.Message);
            }
            //this.zedGraphControl1.Visible = true;
            //this.zedGraphControl2.Visible = false;
            string str = "第" + sequence + "路温度历史变化图";
            this.zedGraphControl2.GraphPane.Title.Text = str;

            this.zedGraphControl2.GraphPane.XAxis.Title.Text = "时间";

            this.zedGraphControl2.GraphPane.YAxis.Title.Text = "温度";
            this.zedGraphControl2.GraphPane.XAxis.MajorGrid.IsVisible = true;
            this.zedGraphControl2.GraphPane.YAxis.MajorGrid.IsVisible = true;

            this.zedGraphControl2.GraphPane.XAxis.Type = ZedGraph.AxisType.Date;
            this.zedGraphControl2.GraphPane.XAxis.Scale.Format = "yyyy-MM-dd";
            //this.zedGraphControl2.GraphPane.XAxis.Scale.MajorStep = 1;
            this.zedGraphControl2.GraphPane.XAxis.Scale.FontSpec.Angle = 45;


            for (int i = 0; i < temp_list.Count; i++)
            {
                double x = (double)new XDate(startDate.Year, startDate.Month, startDate.Day + i);
                double y = (double)temp_list[i];
                list2.Add(x, y);
            }
            /*
            for (int i = 0; i < dateDiff.Days; i++)
            {
                double x = (double)new XDate(startDate.Year, startDate.Month, startDate.Day + i);
                // double x = (double)new XDate(1991, 5, i+28);
                double y = ran.Next(20, 30);
                //double y = 0;
                list2.Add(x, y);
            }
            */
            myCurve2 = zedGraphControl2.GraphPane.AddCurve("", list2, Color.Red, SymbolType.None);
            myCurve2.Line.Width = 2.0F;
            this.zedGraphControl2.AxisChange();
            this.zedGraphControl2.Refresh();
        }   

        private void timer1_Tick(object sender, EventArgs e)
        {
            zedGraphControl1.GraphPane.XAxis.Scale.MaxAuto = true;
           double x = (double)new XDate(DateTime.Now.AddMinutes(0));
           //string st2 = sm.open();
           //string[] a = sm.cut(st2);
         
          //  double y1 = FrmMain.realTimeValue[4][FrmMain.realTimeValue[4].Count-1];
            //double y2 = FrmMain.realTimeValue[9][FrmMain.realTimeValue[9].Count - 1];
           // double y3 = FrmMain.realTimeValue[12][FrmMain.realTimeValue[12].Count - 1];

           //double y1 = Convert.ToInt32(a[16]);
           //double y2 = Convert.ToInt32(a[21]);
           //double y3 = Convert.ToInt32(a[24]);


            //list1.Add(x, y1);
            //list3.Add(x, y2);
            //list4.Add(x, y3);

            if (list1.Count >= 60)
            {
                list1.RemoveAt(0);
            }
            if (list3.Count >= 60)
            {
                list3.RemoveAt(0);
            } 
            if (list4.Count >= 60)
            {
                list4.RemoveAt(0);
            }
            
            this.zedGraphControl1.AxisChange();

            this.zedGraphControl1.Refresh();
        }

        
        //实时绘图
        private void button1_Click(object sender, EventArgs e)
        {
            this.zedGraphControl1.Visible = true;
            this.zedGraphControl2.Visible = false;
        }
        
        //历史绘图
        private void button2_Click(object sender, EventArgs e)
        {
            this.zedGraphControl1.Visible = false;
            this.zedGraphControl2.Visible = true;
            this.HistoryDraw();

        }

        //关闭绘图
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
