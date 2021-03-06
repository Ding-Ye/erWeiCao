﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using Advantech.Adam;
using Advantech.Common;

namespace Adam5013_17_18
{
    public partial class Form1 : Form
    {
        private int m_iCom, m_iAddr, m_iSlot, m_iChTotal;//COM号 从地址 控制槽 计数？？ 每个槽有效通道数
        private byte m_byRange, m_byFormat;
        private Adam5000Type m_Adam5000Type;
        private AdamCom adamCom;

        public Form1()
        {
            InitializeComponent();
            m_iCom = 3;		// using COM3  选择com号
            adamCom = new AdamCom(m_iCom);
            adamCom.Checksum = false; // disbale checksum
            
         
            m_iAddr = 1;	// the slave address is 1
            m_iSlot = 0;	// the slot index of the module 控制槽的位置
            m_Adam5000Type = Adam5000Type.Adam5017; 
                                                   
            m_iChTotal = AnalogInput.GetChannelTotal(m_Adam5000Type);

            if (adamCom.OpenComPort())
            {
                // set COM port state, 9600,N,8,1
                adamCom.SetComPortState(Baudrate.Baud_9600, Databits.Eight, Parity.None, Stopbits.One);
                // set COM port timeout
                adamCom.SetComPortTimeout(500, 500, 0, 500, 0);
            
                if (RefreshChannelRange())
                {

                    timer1.Enabled = true; // enable timer
                }
                else
                    adamCom.CloseComPort();
            }
            else
                MessageBox.Show("Failed to open COM port!", "Error");
        }

        /// <summary>
        /// 定时器2，每100ms控制m_iSlot的值从0到3变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (m_iSlot >= 0 && m_iSlot < 3)
            {
                m_iSlot++;
            }
            else
                m_iSlot = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)//定时器
        {
            RefreshAdam5017ChannelValue();
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



        private void RefreshAdam5017ChannelValue()//通道值更新
        {
            int iStart = m_iSlot * 8 + 1;//？？？
           // int[] iData;
            float[] fVal;
           // float fValue;
            string szFormat, szUnit;

            if (adamCom.AnalogInput(m_iAddr).GetValues(m_iSlot, m_iChTotal, out fVal))
            {
                // floating format
                szFormat = AnalogInput.GetFloatFormat(m_Adam5000Type, m_byRange);
                szUnit = AnalogInput.GetUnitName(m_Adam5000Type, m_byRange);//数据类型
                switch (m_iSlot)
                {
                    case 0:
                        {
                            txtAIValue0.Text = fVal[0].ToString(szFormat) + " " + szUnit;
                            txtAIValue1.Text = fVal[1].ToString(szFormat) + " " + szUnit;
                            txtAIValue2.Text = fVal[2].ToString(szFormat) + " " + szUnit;
                            txtAIValue3.Text = fVal[3].ToString(szFormat) + " " + szUnit;
                            txtAIValue4.Text = fVal[4].ToString(szFormat) + " " + szUnit;
                            txtAIValue5.Text = fVal[5].ToString(szFormat) + " " + szUnit;
                            txtAIValue6.Text = fVal[6].ToString(szFormat) + " " + szUnit;
                            txtAIValue7.Text = fVal[7].ToString(szFormat) + " " + szUnit;
                            break;
                        }

                    case 1:
                        {
                            txtAIIValue0.Text = fVal[0].ToString(szFormat) + " " + szUnit;
                            txtAIIValue1.Text = fVal[1].ToString(szFormat) + " " + szUnit;
                            txtAIIValue2.Text = fVal[2].ToString(szFormat) + " " + szUnit;
                            txtAIIValue3.Text = fVal[3].ToString(szFormat) + " " + szUnit;
                            txtAIIValue4.Text = fVal[4].ToString(szFormat) + " " + szUnit;
                            txtAIIValue5.Text = fVal[5].ToString(szFormat) + " " + szUnit;
                            txtAIIValue6.Text = fVal[6].ToString(szFormat) + " " + szUnit;
                            txtAIIValue7.Text = fVal[7].ToString(szFormat) + " " + szUnit;
                            break;
                        }

                    case 2:
                        {
                            txtAIIIValue0.Text = fVal[0].ToString(szFormat) + " " + szUnit;
                            txtAIIIValue1.Text = fVal[1].ToString(szFormat) + " " + szUnit;
                            txtAIIIValue2.Text = fVal[2].ToString(szFormat) + " " + szUnit;
                            txtAIIIValue3.Text = fVal[3].ToString(szFormat) + " " + szUnit;
                            txtAIIIValue4.Text = fVal[4].ToString(szFormat) + " " + szUnit;
                            txtAIIIValue5.Text = fVal[5].ToString(szFormat) + " " + szUnit;
                            txtAIIIValue6.Text = fVal[6].ToString(szFormat) + " " + szUnit;
                            txtAIIIValue7.Text = fVal[7].ToString(szFormat) + " " + szUnit;
                            break;
                        }

                    case 3:
                        {
                            txtAIVValue0.Text = fVal[0].ToString(szFormat) + " " + szUnit;
                            txtAIVValue1.Text = fVal[1].ToString(szFormat) + " " + szUnit;
                            txtAIVValue2.Text = fVal[2].ToString(szFormat) + " " + szUnit;
                            txtAIVValue3.Text = fVal[3].ToString(szFormat) + " " + szUnit;
                            txtAIVValue4.Text = fVal[4].ToString(szFormat) + " " + szUnit;
                            txtAIVValue5.Text = fVal[5].ToString(szFormat) + " " + szUnit;
                            txtAIVValue6.Text = fVal[6].ToString(szFormat) + " " + szUnit;
                            txtAIVValue7.Text = fVal[7].ToString(szFormat) + " " + szUnit;
                            break;
                        }
                }
            }
        }
    }
}
