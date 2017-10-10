using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Advantech.Adam;//Advanttch Controls
using Advantech.Common;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using ZedGraph;//负压计柱状图
using CLRIPISImage;

namespace AdaMgr
{

    public partial class FrmMain : Form
    {
        //定义全局温度泛型集合
        public List<System.Windows.Forms.Label> listTempLabel = new List<System.Windows.Forms.Label>();

        // 定义全局含水率泛型集合
        public List<System.Windows.Forms.Label> listTdrLabel = new List<System.Windows.Forms.Label>();

        //定义全局基质势泛型集合
        public List<System.Windows.Forms.Label> listMpLabel = new List<System.Windows.Forms.Label>();

        public double[] tempValue = new double[13];//定义长度为15的float型数组存储温度
        public float[] tdrValue = new float[13];//定义长度为13的float型数组存储基质势
        
        public float[] waterValue=new float[13];

        public int[] depressimeterValue = new int[10];

        public AdamCom adamCom;//Adam 的com口

        private int m_iAddr = 1;//模块地址，adamCom.AnalogInput(m_iAddr).GetValues(nSlot, nChTotal, out fVal)
        private int m_iSlot = 0;
        private byte m_byRange, m_byFormat;
        private bool m_bAscendButtom = false;//上升按钮控制返回值

        private bool m_bDescButtom = false;  //下降按钮控制返回值

        private Random ran = new Random();

        public static List<double>[] realTimeValue = new List<double>[13];//定义一个温度的15路的集合，用于15路温度再次分配集合

        private int timeCount = 0;//计时器

        private string sqlTemp = "insert into temp_data(t1,t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12,t13,AddDatetime) value({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},now())";

        private string sqlwater = "insert into water_data(t1,t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12,t13,AddDatetime) value({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},now())";
       
        private string sqltdr = "insert into tdr_data(t1,t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12,t13,AddDatetime) value({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},now())";
       
        private string sqlWaterLevel = "insert into waterlevel_data(Val,AddDatetime) value({0}, now())";

        private string sql = null;

        private Thread waterLevelThread = null;

        static Socket serverSocket;


        volatile int realTimeWaterValue = 0;  //实时的水位值，volatile作用：被设计用来修饰被不同线程访问和修改的变量,作为指令关键字，确保本条指令不会因编译器的优化而省略，且要求每次直接读值.

        private int setWaterValue = 0;

        private const int HG_LEVEL_READING_COUNT = 10;//负压计读数计数

        const double kMotorSpeed = 1000 / 11.5;//控制水位的速度的常量

        //一定要声明成局部变量以保持对Timer的引用，否则会被垃圾收集器回收！
        private System.Threading.Timer threadTimer1 = null;//局部变量Timer，供水位上升时计时

        private System.Threading.Timer threadTimer2 = null;//局部变量Timer，供水位下降时计时


        public FrmMain()
        {
            InitializeComponent();//初始化，系统自带

            Control.CheckForIllegalCrossThreadCalls = false;//允许跨线程调用,访问窗口中的控件

           // AdamMeterFromInit();//温度和函数率初始化

            WaterLevelControl();//创建水位控制新线程

            AdamOpenCom(); //打开端口

            panel1.Height = 344 - (int)((344 / 3000.0) * realTimeWaterValue);//水位值显示的高度
        }

        //温度,含水率“标尺”的初始化
        public void AdamMeterFromInit()
        {

            //15路 含水率 lablel3--label17
            listTempLabel.Add(label3);
            listTempLabel.Add(label4);
            listTempLabel.Add(label5);
            listTempLabel.Add(label6);
            listTempLabel.Add(label7);
            listTempLabel.Add(label8);
            listTempLabel.Add(label9);
            listTempLabel.Add(label10);


            //温度label19--label33
            listTdrLabel.Add(label19);
            listTdrLabel.Add(label20);
            listTdrLabel.Add(label21);
            listTdrLabel.Add(label22);
            listTdrLabel.Add(label23);
            listTdrLabel.Add(label24);
            listTdrLabel.Add(label25);
            listTdrLabel.Add(label26);
            listTdrLabel.Add(label29);
            listTdrLabel.Add(label30);
            listTdrLabel.Add(label31);


            //基质势label
            listMpLabel.Add(label44);
            listMpLabel.Add(label43);
            listMpLabel.Add(label42);
            listMpLabel.Add(label41);
            listMpLabel.Add(label40);
            listMpLabel.Add(label39);
            listMpLabel.Add(label38);
            listMpLabel.Add(label37);
            listMpLabel.Add(label36);
            listMpLabel.Add(label32);
            listMpLabel.Add(label33);


            for (int i = 0; i < listTempLabel.Count; i++)     //cout为集合中包含的元素数
            {
                //double temp = ran.Next(6, 12) + 0.01 * ran.Next(0, 100);
                // string str = temp >= 10 ? String.Format("{0:F}", temp) : "0" + String.Format("{0:F}", temp);
                // listTempLabel[i].Text = (ran.Next(6, 12) + 0.01 * ran.Next(0, 100)).ToString("00.00");

                listTempLabel[i].Text = 0.ToString("00.00");//设置温度label的显示格式

                //listTempLabel[i].DisplayLedOnColor = System.Drawing.Color.Green;
            }
            //3.9

            for (int i = 0; i < listTdrLabel.Count; i++)
            {
                //listTdrLabel[i].Text = (ran.Next(20, 30) + 0.01 * ran.Next(0, 100)).ToString();

                //随机设置含水率label的数值，产生50.00—69.99之间的随机数,//含水率要改
                //listTdrLabel[i].Text = String.Format("{0:F}", ran.Next(50, 70) + 0.01 * ran.Next(0, 100));

                listTdrLabel[i].Text = 0.ToString("00.00");
                //listTempLabel[i].DisplayLedOnColor = System.Drawing.Color.Green;
            }

            for (int i = 0; i < listMpLabel.Count; i++)     //cout为集合中包含的元素数
            {
                //double temp = ran.Next(6, 12) + 0.01 * ran.Next(0, 100);
                // string str = temp >= 10 ? String.Format("{0:F}", temp) : "0" + String.Format("{0:F}", temp);
                // listTempLabel[i].Text = (ran.Next(6, 12) + 0.01 * ran.Next(0, 100)).ToString("00.00");

                listMpLabel[i].Text = 0.ToString("00.00");//设置温度label的显示格式

                //listTempLabel[i].DisplayLedOnColor = System.Drawing.Color.Green;
            }

            //提前分配15路温度集合的空间集合，15路的集合的，每个元素再次定义为list类型

            for (int i = 0; i < realTimeValue.Length; i++)
            {
                realTimeValue[i] = new List<double>();

            }

            /*
            for (int i = 0; i < realTimeValue.Length; i++)
            {
                for (int j = 0; j < 60; j++ )
                {
                    realTimeValue[i].Add((float)(ran.Next(10, 20) + 0.01 * ran.Next(0, 100)));
                }
            }
            */

            // 调试阶段，只取第4,9,12路作为实时绘图温度
            for (int j = 0; j < 2; j++)
            {
                realTimeValue[4].Add((double)0);
                realTimeValue[9].Add((double)0.5);
                realTimeValue[12].Add((double)1);
            }
            /*          for (int j = 0; j < 2; j++)
                      {
                          realTimeValue[4].Add((float)(ran.Next(6, 8) + 0.01 * ran.Next(0, 100)));
                          realTimeValue[9].Add((float)(ran.Next(8, 10) + 0.01 * ran.Next(0, 100)));
                          realTimeValue[12].Add((float)(ran.Next(10, 12) + 0.01 * ran.Next(0, 100)));
                      }
          */
        }

        //创建水位控制模块新线程，外网控制
        private void WaterLevelControl()
        {

            textBox1.Text = Convert.ToString(0);//水位控制的输入文本框1

            int nWaterlabPort = 2222; //定义端口为2222

            IPAddress waterlabIP = IPAddress.Parse("127.0.0.1");//定义本机地址127.0.0.1为waterlabIP

            //socket通信，进行监听，三次握手，创建连接
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(waterlabIP, nWaterlabPort));
            serverSocket.Listen(10);

            waterLevelThread = new Thread(WaterLevelControlMethod);  //创建水位控制新线程

            waterLevelThread.IsBackground = true;//允许线程后台运行

            waterLevelThread.Priority = ThreadPriority.Highest;//线程是高等级

            waterLevelThread.Start();
        }

        // 水位控制方法
        void WaterLevelControlMethod()
        {

            while (true)
            {
                byte[] res = new byte[128];//为socket通信接受数据分配空间
                try
                {
                    Socket clientSocket = serverSocket.Accept();//接受socket

                    int iRet = clientSocket.Receive(res);//接受数据res的字节数

                    if (0 == iRet)
                    {
                        continue;//如果是0，继续回到while循环
                    }

                    //byte decodeRes = Encoding.UTF8.GetBytes(res,0,iRet);

                    string str = Encoding.UTF8.GetString(res, 0, iRet);//将res转换为C#中的string
                    byte[] buffer = BitConverter.GetBytes(realTimeWaterValue);//将int实时水位值转换为buffer数组

                    if ("request" == str)//判断如果接受到网页端的指令是request
                    {
                        //byte[] buffer = Encoding.UTF8.GetBytes(lbDigitalMeter1.Value);

                        clientSocket.Send(buffer);//判断为request，请求读取数据，发送实时水位给网页端
                        //MessageBox.Show(str);
                    }
                    else
                    {
                        // string strHeight = str.Substring(1)

                        int nHeihht = Convert.ToInt32(str);//请求不是request，则为控制水位，接收需要达到的水位值int类型

                        //MessageBox.Show(strHeight);

                        this.textBox1.Text = str;
                        clientSocket.Send(buffer);//发送实时水位给网页端

                        if (!((nHeihht >= 0) && (nHeihht <= 3000)))
                        {
                            MessageBox.Show("水位不在0-3000以内,忽略此次请求!");
                            continue;
                        }

                        if (realTimeWaterValue < nHeihht)
                        {
                            //this.lbButton4.PerformClick();
                            //this.lbButton4.lbButton4_Click(this, null);

                            int nMotorTime = (int)(((nHeihht - realTimeWaterValue) / kMotorSpeed) * 1000);//经过反复计算得到的控制水位时间公式

                            this.lbLed1.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;//上升指示灯亮，命名空间：LBSoft.IndustrialCtrls.Leds，类名：LBled


                            threadTimer1 = new System.Threading.Timer(new System.Threading.TimerCallback(this.ThreadTimer1), this, nMotorTime, -1);//一个计时器，到达时间调用方法this.ThreadTimer1,结束上升的计时器
                            //  Timer构造函数参数说明：
                            //  Callback：一个 TimerCallback 委托，表示要执行的方法。
                            //  State：一个包含回调方法要使用的信息的对象，或者为空引用（Visual Basic 中为 Nothing）。
                            //  dueTime：调用 callback 之前延迟的时间量（以毫秒为单位）。指定 Timeout.Infinite 以防止计时器开始计时。指定零 (0) 以立即启动计时器。
                            //  Period：调用 callback 的时间间隔（以毫秒为单位）。指定 Timeout.Infinite 可以禁用定期终止。


                            this.Adam5069SetValue(0, 0, true);//第一个0为槽数，第二个0位通道数
                            // this.timer3.Interval = nMotorTime;
                            // this.timer3.Enabled = true;
                        }

                        else if (realTimeWaterValue > nHeihht)
                        {
                            this.lbLed2.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;

                            int nMotorTime = (int)(((realTimeWaterValue - nHeihht) / kMotorSpeed) * 1000);

                            threadTimer2 = new System.Threading.Timer(new System.Threading.TimerCallback(this.ThreadTimer2), null, nMotorTime, -1);

                            this.Adam5069SetValue(0, 1, true);

                            //this.timer4.Interval = nMotorTime;
                            //this.timer4.Enabled = true;
                        }

                    }

                }
                catch (Exception ex)
                {
                    serverSocket.Close();
                    MessageBox.Show(ex.Message);
                    break;
                }
            }
        }

        //计时器，到达时间时结束上升
        private void ThreadTimer1(object status)
        {
            //Adam5069SetValue(iSlot, 0, m_bAscendButtom);            
            this.Adam5069SetValue(0, 0, false);
            this.lbLed1.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;

            if (null != threadTimer1)
            {
                threadTimer1.Dispose();
            }
        }

        //计时器，到达时间时结束下降
        private void ThreadTimer2(object status)
        {
            //Adam5069SetValue(iSlot, 0, m_bAscendButtom); 

            this.Adam5069SetValue(0, 1, false);
            this.lbLed2.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;

            if (null != threadTimer2)
            {
                threadTimer2.Dispose();
            }
        }
        //Adamf赋值
        private void Adam5069SetValue(int iSlot, int iCh, bool val)
        {
            bool bRet;
            bRet = adamCom.DigitalOutput(m_iAddr).SetValue(iSlot, iCh, val == true);
            if (!bRet)
                MessageBox.Show("Set digital " + iCh + " Ch output failed!", "Error");
     
        }

        //timer1做的所有事!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private void timer1_Tick(object sender, EventArgs e)
        {

            // this.Adam5017GetWaterLevel();//获取水位值，并显示到图中
           // Adam5017GetVlote();
            Adam5017GetVlote();
            if (m_iSlot >= 0 && m_iSlot < 3)
            {
                m_iSlot++;
            }
            else
                m_iSlot = 0;
        }

        //5017获取实时水位值
        //public void Adam5017GetWaterLevel()
        //{
        //    /*
        //     * Get channel values when the data format is set to engineering unit or ohms(ADAM-5013 on ADAM-5000). 
        //   格式：public bool GetValues(
        //           int i_iSlot,
        //           int i_iChannelTotal,
        //           out float[] o_fValues
        //       );
        //     */

        //    float[] fVal;

        //    int nSlot = 2; //Slot index.(槽指数) The value is between 0~7 

        //    int nChTotal = 8;//The total of the channels. The value is between 1~8

        //    //???
        //    if (adamCom.AnalogInput(m_iAddr).GetValues(nSlot, nChTotal, out fVal))//成功返回true，否则false
        //    {

        //        lbDigitalMeter0.Value = (int)(1000 * fVal[0] - 1000);//获取实时的水位高度

        //        //水位测量器显示值赋值
        //        realTimeWaterValue = (int)lbDigitalMeter0.Value; //lbDigitalMeter0.Value = realTimeWaterValue; 

        //        panel1.Height = 344 - (int)((344 / 3000.0) * realTimeWaterValue);//用白色覆盖绿色，panel1水位显示

        //    }
        //    else
        //    {
        //        timer1.Enabled = false;
        //        MessageBox.Show("Water Level Failed to get!");
        //    }
        //}

        //向数据库存入15路温度值
        private void DBInsertTemp()
        {
            sql = String.Format(sqlTemp, tempValue[0], tempValue[1], tempValue[2], tempValue[3], tempValue[4], tempValue[5],
                tempValue[6], tempValue[7], tempValue[8], tempValue[9], tempValue[10], tempValue[11], tempValue[12]);
            try
            {

                using (MySqlCommand mysqlCommand = new MySqlCommand(sql, Login.mysqlConn))
                {
                    mysqlCommand.ExecuteNonQuery();//sql语句的执行
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("DataBase Operate Failed: " + ex.Message);
            }
        }

        //向数据库存入水的高度值
        private void DBInsertWater() 
        {
            sql = String.Format(sqlwater, waterValue[0], waterValue[1], waterValue[2], waterValue[3], waterValue[4], waterValue[5],
                  waterValue[6], waterValue[7], waterValue[8], waterValue[9], waterValue[10], waterValue[11], waterValue[12]);
            try
            {

                using (MySqlCommand mysqlCommand = new MySqlCommand(sql, Login.mysqlConn))
                {
                    mysqlCommand.ExecuteNonQuery();//sql语句的执行
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("DataBase Operate Failed: " + ex.Message);
            }
        }

        //向数据库存入ttr值
        private void DBInserttdr()
        {
            sql = String.Format(sqltdr, tdrValue[0], tdrValue[1], tdrValue[2], tdrValue[3], tdrValue[4], tdrValue[5],
                tdrValue[6], tdrValue[7], tdrValue[8], tdrValue[9], tdrValue[10], tdrValue[11], tdrValue[12]);
            try
            {

                using (MySqlCommand mysqlCommand = new MySqlCommand(sql, Login.mysqlConn))
                {
                    mysqlCommand.ExecuteNonQuery();//sql语句的执行
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("DataBase Operate Failed: " + ex.Message);
            }
        }

        //向数据库存入水位值
        private void DBInsertWaterLevel()
        {
            sql = String.Format(sqlWaterLevel, this.realTimeWaterValue);
            try
            {
                using (MySqlCommand mysqlCommand = new MySqlCommand(sql, Login.mysqlConn))
                {
                    mysqlCommand.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("DataBase Operate Failed: " + ex.Message);
            }

        }


        

        //通过com1口，连接advantech
        //public void Adam5013GetTemp()
        //{
        //    //设置com1口
        //    int m_iCom = 8;

        //    //int m_iAddr = 1;
        //    // int m_iSlot = 3;

        //    adamCom = new AdamCom(m_iCom);

        //    adamCom.Checksum = false; // disbale checksum; Get/Set Checksum status when module runs on ASCII protocol. 

        //    //int m_iChTotal = AnalogInput.GetChannelTotal(Adam5000Type.Adam5013);

        //    if (adamCom.OpenComPort())   //OpenComPort,打开COM端口和从系统获得的串行通讯端口设置,True if port opened successfully.
        //    {
        //        // set COM port state, 9600,N,8,1

        //        adamCom.SetComPortState(Baudrate.Baud_9600, Databits.Eight, Parity.None, Stopbits.One);

        //        // set COM port timeout

        //        adamCom.SetComPortTimeout(500, 500, 0, 500, 0);

        //        timer1.Enabled = true;

        //        //timer1.Enabled = false;

        //    }
        //    else
        //    {
        //        MessageBox.Show("Failed to open COM port!", "Error");
        //        timer1.Enabled = false;
        //    }

        //}

        //实时绘图
        private void lbButton1_Click(object sender, EventArgs e)
        {
            SingleTemp singleTemp = new SingleTemp();
            singleTemp.ShowDialog();
            return;
        }

        //温度报表按钮实时绘图
        private void lbButton2_Click(object sender, EventArgs e)
        {
            MutlipleTemp mutlipleTemp = new MutlipleTemp();
            mutlipleTemp.Show();
        }


        //温度报表按钮
        private void lbButton2_Click_1(object sender, EventArgs e)
        {
            MutlipleTemp mutlipleTemp = new MutlipleTemp();
            mutlipleTemp.Show();

        }

        //tdr报表绘图
        private void lbButton20_Click_(object sender, EventArgs e)
        {
            tdr tdr = new tdr();
            tdr.Show();
        }

        //水高度报表绘图
        private void lbButton90_Click(object sender, EventArgs e)
        {
            Water water = new Water();
            water.Show();
        }


        //控制水位上升按钮
        private void lbButton4_Click(object sender, EventArgs e)
        {
            int iSlot = 4;
            if (m_bAscendButtom == false)
            {
                m_bAscendButtom = true;
                lbLed1.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;//lbLed1--上升指示灯
                Adam5069SetValue(iSlot, 0, m_bAscendButtom);
            }
            else
            {
                m_bAscendButtom = false;
                lbLed1.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
                Adam5069SetValue(iSlot, 0, m_bAscendButtom);
            }

        }

        //控制水位下降按钮
        private void lbButton5_Click(object sender, EventArgs e)
        {
            int iSlot = 4;
            if (m_bDescButtom == false)
            {
                m_bDescButtom = true;
                lbLed2.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;
                Adam5069SetValue(iSlot, 1, m_bDescButtom);

            }
            else
            {
                m_bDescButtom = false;
                lbLed2.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
                Adam5069SetValue(iSlot, 1, m_bDescButtom);
            }

        }

        //水位控制部分的确定按钮
        private void lbButton8_Click(object sender, EventArgs e)
        {
            setWaterValue = Convert.ToInt32(textBox1.Text);

            if (!((setWaterValue >= 0) && (setWaterValue <= 3000)))
            {
                MessageBox.Show("水位不在0-3000以内,请重新设置!");
                textBox1.Clear();
                return;
            }

            if (realTimeWaterValue < setWaterValue)
            {

                //this.lbButton4.PerformClick();
                //this.lbButton4.lbButton4_Click(this, null);

                int nMotorTime = (int)(((setWaterValue - realTimeWaterValue) / kMotorSpeed) * 1000);
                this.lbLed1.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;
                threadTimer1 = new System.Threading.Timer(new System.Threading.TimerCallback(this.ThreadTimer1), this, nMotorTime, -1);

                this.Adam5069SetValue(0, 0, true);
                // this.timer3.Interval = nMotorTime;
                // this.timer3.Enabled = true;

            }
            else if (realTimeWaterValue > setWaterValue)
            {
                this.lbLed2.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;
                int nMotorTime = (int)(((realTimeWaterValue - setWaterValue) / kMotorSpeed) * 1000);
                threadTimer2 = new System.Threading.Timer(new System.Threading.TimerCallback(this.ThreadTimer2), null, nMotorTime, -1);

                this.Adam5069SetValue(0, 1, true);

                //this.timer4.Interval = nMotorTime;
                //this.timer4.Enabled = true;
            }
            else
            {
                MessageBox.Show("设定值无效，请重新设置!");
            }
        }


        //水位高度设置条
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            setWaterValue = trackBar2.Value * 100;//滑动条的比例为1:100
            textBox1.Text = setWaterValue.ToString();

            //panel1.Height = 344 - (int)((344 / 3000.0) * lbDigitalMeter1.Value);
        }

        //Timer2的作用：给实时绘图的温度值realTimeValue[i]每一分钟添加一个实时温度值tempValue[i]
        private void timer2_Tick(object sender, EventArgs e)
        {
            //realTimeValue.Length
            for (int i = 0; i < realTimeValue.Length; i++)
            {
                realTimeValue[i].Add(tempValue[i]);//给实时绘图的温度值realTimeValue[i]每一分钟添加一个实时温度值tempValue[i]

                realTimeValue[i].RemoveAt(0);//去除下标为0的实时绘图的温度值，以保证长度固定


            }

        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }





        public void AdamOpenCom()
        {
            int m_iCom = 3;		// using COM3  选择com号
            adamCom = new AdamCom(m_iCom);
            adamCom.Checksum = false; // disbale checksum


           //int m_iSlot = 0;	// the slot index of the module 控制槽的位置
            if (adamCom.OpenComPort())
            {
                adamCom.SetComPortState(Baudrate.Baud_9600, Databits.Eight, Parity.None, Stopbits.One);
                adamCom.SetComPortTimeout(500, 500, 0, 500, 0);
                timer1.Enabled = true; // enable timer
            }
            else
                MessageBox.Show("Failed to open COM port!", "Error");
        }

        private bool RefreshChannelRange()//通道范围刷新
        {
            bool bRet;
            byte byIntegration;
            bRet = adamCom.AnalogInput(m_iAddr).GetRangeIntegrationDataFormat(m_iSlot, out m_byRange, out byIntegration, out m_byFormat);
            if (!bRet)
                MessageBox.Show("Get range failed!", "Error");
            return bRet;
        }



        private void Adam5017GetVlote()//通道值更新
        {
            float[] fVal;

            string szFormat;

            if (adamCom.AnalogInput(m_iAddr).GetValues(m_iSlot, 8, out fVal))
            {
                szFormat = AnalogInput.GetFloatFormat(Adam5000Type.Adam5017, m_byRange);

                switch (m_iSlot)
                {
                    case 0:
                        {
                            label3.Text = fVal[0].ToString(szFormat);
                            label4.Text = fVal[1].ToString(szFormat);
                            label5.Text = fVal[2].ToString(szFormat);
                            label6.Text = fVal[3].ToString(szFormat);
                            label7.Text = fVal[4].ToString(szFormat);
                            label8.Text = fVal[5].ToString(szFormat);
                            label9.Text = fVal[6].ToString(szFormat);
                            label10.Text = fVal[7].ToString(szFormat); 
                            break;
                        }

                    case 1:
                        {
                            label19.Text = fVal[0].ToString(szFormat);
                            label20.Text = fVal[1].ToString(szFormat);
                            label21.Text = fVal[2].ToString(szFormat);
                            label22.Text = fVal[3].ToString(szFormat);
                            label23.Text = fVal[4].ToString(szFormat);
                            label24.Text = fVal[5].ToString(szFormat);
                            label25.Text = fVal[6].ToString(szFormat);
                            label26.Text = fVal[7].ToString(szFormat);
                            break;
                        }

                    case 2:
                        {
                            label44.Text = fVal[0].ToString(szFormat) ;
                            label43.Text = fVal[1].ToString(szFormat) ;
                            label42.Text = fVal[2].ToString(szFormat) ;
                            label41.Text = fVal[3].ToString(szFormat) ;
                            label40.Text = fVal[4].ToString(szFormat) ;
                            label39.Text = fVal[5].ToString(szFormat) ;
                            label38.Text = fVal[6].ToString(szFormat) ;
                            label37.Text = fVal[7].ToString(szFormat) ;
                            break;
                        }

                    case 3:
                        {
                            label33.Text = fVal[0].ToString(szFormat) ;
                            label32.Text = fVal[1].ToString(szFormat) ;
                            label30.Text = fVal[2].ToString(szFormat) ;
                            label31.Text = fVal[3].ToString(szFormat) ;
                            label36.Text = fVal[4].ToString(szFormat) ;
                            label46.Text = fVal[5].ToString(szFormat) ;
                            label47.Text = fVal[6].ToString(szFormat) ;
                            label48.Text = fVal[7].ToString(szFormat) ;
                            break;
                        }
                }
            }
        }
    }
}
