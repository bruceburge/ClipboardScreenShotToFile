using GlobalHotkeys;
using System;
using System.Windows.Forms;

namespace ClipboardScreenShotToFile
{
    public partial class Form1 : Form
    {

        private GlobalHotkey ghk;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            var hotkeyInfo = HotkeyInfo.GetFromMessage(m);
            if (hotkeyInfo != null) HotkeyProc(hotkeyInfo);
            base.WndProc(ref m);
        }

        private void HotkeyProc(HotkeyInfo hotkeyInfo)
        {
            this.Hide();
            var screenShotFileName = DateTime.Now.ToFileTime().ToString() + ".png";
            using (var bmp = ScreenShotToBitmap.Screenshot())
            {
                // image processing
                bmp.Save(screenShotFileName, System.Drawing.Imaging.ImageFormat.Png);
            }
            screenShotLinkLabel.Text = screenShotFileName;
            screenShotLinkLabel.Show();
            this.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //register global hotkey 
            screenShotLinkLabel.Hide();
            try
            {
                ghk = new GlobalHotkey(Modifiers.Ctrl, Keys.Q, this, true);
            }
            catch (GlobalHotkeyException exc)
            {
                MessageBox.Show("could not register hot key details below\n" + exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ghk.Dispose();
        }

        private void ScreenShotLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(this.screenShotLinkLabel.Text);
        }
    }
}
