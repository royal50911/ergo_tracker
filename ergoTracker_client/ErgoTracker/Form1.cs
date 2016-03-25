using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mshtml;

namespace ErgoTracker
{
    public partial class Form1 : Form
    {
        MyKinect myKinect;

        public Form1(MyKinect kinect)
        {
            InitializeComponent();
            myKinect = kinect;
        }

        private void maxUbi_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (Maxubi.Url.ToString().StartsWith("http://maxubi.herokuapp.com/loggedin_home"))
            {
                InitializeApplicationComponents();
            }
            else
            {
                ApplicationInformation appInfo = ApplicationInformation.Instance;
                appInfo.setUsername("");
                appInfo.setPassword("");
            }
        }

        private void maxUbi_DocumentNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            HtmlDocument doc = Maxubi.Document;
            HtmlElement username = doc.GetElementById("username");
            HtmlElement password = doc.GetElementById("password");
            string id = username.GetAttribute("value");
            string pw = password.GetAttribute("value");

            ApplicationInformation appInfo = ApplicationInformation.Instance;
            appInfo.setUsername(id);
            appInfo.setPassword(pw);
        }

        private void InitializeApplicationComponents()
        {
            if (!myKinect.InitializeKinectSensor(true, true, true)) Application.Exit();
            ApplicationInformation appInfo = ApplicationInformation.Instance;

            appInfo.setDiagnosticMode(true);
            appInfo.setTrainingMode(false);

            MessageBox.Show("Kinect is ready!", "Application now running!");
            this.Close();
        }

    }
}
