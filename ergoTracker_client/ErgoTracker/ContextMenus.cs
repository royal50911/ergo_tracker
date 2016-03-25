using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErgoTracker
{
    class ContextMenus
    {
        MyKinect kinect;
        ServerRequestHandler requestHandler;

        public ContextMenuStrip Create(MyKinect _kinect, ServerRequestHandler _requestHandler)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;
            kinect = _kinect;
            requestHandler = _requestHandler;
            /*
             * This tool strip menu item will allow you to choose
             * what mode you want to be in. "Tutorial" or "Capturing"
             */
            item = new ToolStripMenuItem();
            item.Text = "Modes";
            item.Click += new EventHandler(Modes_Click);
            menu.Items.Add(item);

            /*
             * This tool strip menu itme will allow you to 
             */
            item = new ToolStripMenuItem();
            item.Text = "Review Personal Data";
            item.Click += new EventHandler(Data_Review_Click);
            menu.Items.Add(item);

            /*
             * Track current position.
             */
            item = new ToolStripMenuItem();
            item.Text = "View Current Posture";
            item.Click += new EventHandler(Open_Kinect_View_Click);
            menu.Items.Add(item);

            // separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            /*
             * This tool strip menu item will allow you to log off
             * and exit the program.
             */
            item = new ToolStripMenuItem();
            item.Text = "Log off";
            item.Click += new EventHandler(Log_Off_Click);
            menu.Items.Add(item);
            return menu;
        }

        void Modes_Click(object sender, EventArgs e)
        {
            // do nothing for now
            var modes_form = new ModesForm();
            modes_form.Show();
        }

        void Data_Review_Click(object sender, EventArgs e)
        {
            // do nothing for now
            string url = "http://maxubi.herokuapp.com/login";
            System.Diagnostics.Process.Start(url);
        }

        void Open_Kinect_View_Click(object sender, EventArgs e)
        {
            var kinect_form = new KinectForm(kinect.getSensor(), requestHandler);
            kinect_form.Show();
        }

        void Log_Off_Click(object sender, EventArgs e)
        {
            // just exit program for now.
            Application.Exit();
        }
    }
}
