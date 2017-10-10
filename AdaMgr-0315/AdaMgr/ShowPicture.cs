using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AdaMgr
{
    public partial class ShowPicture : Form
    {
        public ShowPicture()
        {
            InitializeComponent();

            if (0 == FrmMain.GetCRLIPISCamera().SaveImage())
            {
               // MessageBox.Show("IPISCamera SaveImage Sucess!");
                Image pic = Image.FromFile("temp_image.jpg");
                this.pictureBox1.Image = System.Drawing.Bitmap.FromFile("temp_image.jpg");
                pic.Dispose();//资源释放
            }
            else
            {
                MessageBox.Show("IPISCamera SaveImage Failed!");
            }
        }
/// <summary>
/// 当关闭图像窗口，释放相关资源
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void ShowPicture_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.pictureBox1.Image.Dispose(); //资源释放
            this.pictureBox1.Dispose(); //资源释放
            this.Dispose(); //资源释放
        }
    }
}
