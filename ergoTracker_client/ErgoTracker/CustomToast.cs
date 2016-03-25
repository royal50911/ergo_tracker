using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.Drawing;
using System.IO;

namespace ErgoTracker
{
    enum ToastAlertImageColors
    {
        RedAlert,
        GreeAlert,
        YellowAlert
    };

    class CustomToast
    {
        public static void CreateToast(string header, string message, ToastAlertImageColors color)
        {
            XmlDocument toast = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);
            XmlNodeList stringElements = toast.GetElementsByTagName("text");

            stringElements[0].AppendChild(toast.CreateTextNode("Ergo Tracker: " + header));
            stringElements[1].AppendChild(toast.CreateTextNode(message));

            string imagePath = "File:///";
            if (color == ToastAlertImageColors.RedAlert) imagePath += Path.GetFullPath("red_alert.png");
            if (color == ToastAlertImageColors.YellowAlert) imagePath += Path.GetFullPath("yellow_alert.png");
            if (color == ToastAlertImageColors.GreeAlert) imagePath += Path.GetFullPath("green_alert.png");

            XmlNodeList imageElements = toast.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

            ToastNotification toast_notif = new ToastNotification(toast);
            //toast_notif.Activated += ToastActivated;
            //toast_notif.Dismissed += ToastDismissed;
            //toast_notif.Failed += ToastFailed;

            ToastNotificationManager.CreateToastNotifier("ErgoTracker").Show(toast_notif);
        }

        /*
        private void ToastActivated(ToastNotification sender, object e)
        {

        }

        private void ToastDismissed(ToastNotification sender, ToastDismissedEventArgs e)
        {

        }

        private void ToastFailed(ToastNotification sender, ToastFailedEventArgs e)
        {

        }*/
    }
}
