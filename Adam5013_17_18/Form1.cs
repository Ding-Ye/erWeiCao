using System;
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
        private int m_iCom, m_iAddr, m_iSlot, m_iCount, m_iChTotal;
        private bool m_bStart;
        private byte m_byRange, m_byFormat;
        private string m_szIP;
        private Adam5000Type m_Adam5000Type;
        private AdamCom adamCom;
        private AdamSocket adamSocket;


        public Form1()
        {
            InitializeComponent();

             // set to true for module on ADAM-5000; set to false for module on ADAM-5000/TCP
     
            m_iCom = 3;		// using COM2
            adamCom = new AdamCom(m_iCom);
            adamCom.Checksum = false; // disbale checksum
            
         
            m_iAddr = 1;	// the slave address is 1
            m_iSlot = 0;	// the slot index of the module 控制槽的位置
            m_iCount = 0;	// the counting start from 0
            m_bStart = false;
            //m_Adam5000Type = Adam5000Type.Adam5013; // the sample is for ADAM-5013
            m_Adam5000Type = Adam5000Type.Adam5017; // the sample is for ADAM-5017
                                                    // m_Adam5000Type = Adam5000Type.Adam5018; // the sample is for ADAM-5018

            m_iChTotal = AnalogInput.GetChannelTotal(m_Adam5000Type);

            txtModule.Text = m_Adam5000Type.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_bStart)
            {
                timer1.Enabled = false; // disable timer
                adamCom.CloseComPort(); // close the COM port
         
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (m_bStart) // was started
            {
                m_bStart = false;
                timer1.Enabled = false;
                adamCom.CloseComPort();
              
                buttonStart.Text = "Start";
            }
            else
            {
                if (adamCom.OpenComPort())
                {
                    // set COM port state, 9600,N,8,1
                    adamCom.SetComPortState(Baudrate.Baud_9600, Databits.Eight, Parity.None, Stopbits.One);
                    // set COM port timeout
                    adamCom.SetComPortTimeout(500, 500, 0, 500, 0);
                    m_iCount = 0; // reset the reading counter
                                  //
                    if (RefreshChannelRange())
                    {
                        RefreshChannelEnable();
                        //
                        timer1.Enabled = true; // enable timer
                        buttonStart.Text = "Stop";
                        m_bStart = true; // starting flag
                    }
                    else
                        adamCom.CloseComPort();
                }
                else
                    MessageBox.Show("Failed to open COM port!", "Error");
                
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_iCount++;
            txtReadCount.Text = "Polling " + m_iCount.ToString() + " times...";
            RefreshAdam5017ChannelValue();

        }

        private void RefreshChannelEnable()
        {
            bool bRet;
            bool[] bEnabled;

           
            bRet = adamCom.AnalogInput(m_iAddr).GetChannelEnabled(m_iSlot, m_iChTotal, out bEnabled);
           
            if (bRet)
            {
                if (m_iChTotal > 0)
                    chkboxCh0.Checked = bEnabled[0];
                if (m_iChTotal > 1)
                    chkboxCh1.Checked = bEnabled[1];
                if (m_iChTotal > 2)
                    chkboxCh2.Checked = bEnabled[2];
                if (m_iChTotal > 3)
                    chkboxCh3.Checked = bEnabled[3];
                if (m_iChTotal > 4)
                    chkboxCh4.Checked = bEnabled[4];
                if (m_iChTotal > 5)
                    chkboxCh5.Checked = bEnabled[5];
                if (m_iChTotal > 6)
                    chkboxCh6.Checked = bEnabled[6];
                if (m_iChTotal > 7)
                    chkboxCh7.Checked = bEnabled[7];
                txtAIValue0.Text = "";
                txtAIValue1.Text = "";
                txtAIValue2.Text = "";
                txtAIValue3.Text = "";
                txtAIValue4.Text = "";
                txtAIValue5.Text = "";
                txtAIValue6.Text = "";
                txtAIValue7.Text = "";
            }
            else
                MessageBox.Show("GetChannelEnabled() failed", "Error");
        }

        private bool RefreshChannelRange()
        {
            bool bRet;
            byte byIntegration;
            bRet = adamCom.AnalogInput(m_iAddr).GetRangeIntegrationDataFormat(m_iSlot, out m_byRange, out byIntegration, out m_byFormat);
            if (!bRet)
                MessageBox.Show("Get range failed!", "Error");
            return bRet;
        }



        private void RefreshAdam5017ChannelValue()
        {
            int iStart = m_iSlot * 8 + 1;
           // int[] iData;
            float[] fVal;
           // float fValue;
            string szFormat, szUnit;

            if (adamCom.AnalogInput(m_iAddr).GetValues(m_iSlot, m_iChTotal, out fVal))
            {
                // floating format
                szFormat = AnalogInput.GetFloatFormat(m_Adam5000Type, m_byRange);
                szUnit = AnalogInput.GetUnitName(m_Adam5000Type, m_byRange);
                //
                if (chkboxCh0.Checked)
                    txtAIValue0.Text = fVal[0].ToString(szFormat) + " " + szUnit;
                if (chkboxCh1.Checked)
                    txtAIValue1.Text = fVal[1].ToString(szFormat) + " " + szUnit;
                if (chkboxCh2.Checked)
                    txtAIValue2.Text = fVal[2].ToString(szFormat) + " " + szUnit;
                if (chkboxCh3.Checked)
                    txtAIValue3.Text = fVal[3].ToString(szFormat) + " " + szUnit;
                if (chkboxCh4.Checked)
                    txtAIValue4.Text = fVal[4].ToString(szFormat) + " " + szUnit;
                if (chkboxCh5.Checked)
                    txtAIValue5.Text = fVal[5].ToString(szFormat) + " " + szUnit;
                if (chkboxCh6.Checked)
                    txtAIValue6.Text = fVal[6].ToString(szFormat) + " " + szUnit;
                if (chkboxCh7.Checked)
                    txtAIValue7.Text = fVal[7].ToString(szFormat) + " " + szUnit;
            }
            else
            {
                if (chkboxCh0.Checked)
                    txtAIValue0.Text = "Failed to get!";
                if (chkboxCh1.Checked)
                    txtAIValue1.Text = "Failed to get!";
                if (chkboxCh2.Checked)
                    txtAIValue2.Text = "Failed to get!";
                if (chkboxCh3.Checked)
                    txtAIValue3.Text = "Failed to get!";
                if (chkboxCh4.Checked)
                    txtAIValue4.Text = "Failed to get!";
                if (chkboxCh5.Checked)
                    txtAIValue5.Text = "Failed to get!";
                if (chkboxCh6.Checked)
                    txtAIValue6.Text = "Failed to get!";
                if (chkboxCh7.Checked)
                    txtAIValue7.Text = "Failed to get!";
            }
            
        }
    }
}
