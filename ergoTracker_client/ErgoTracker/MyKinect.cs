using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErgoTracker
{
    public class MyKinect
    {
        private KinectSensor myKinect;
        private ServerRequestHandler requestHandler;
        int counter = 0;
        int totalDataCounter = 0;
        string data = "";
        bool trackingUser = false;

        public MyKinect(ServerRequestHandler _requestHandler)
        {
            this.requestHandler = _requestHandler;
            this.requestHandler.ReceivedScoreData += HandleScoreReceived;
        }

        public Boolean InitializeKinectSensor(bool depthSensorActive, bool colorSensorActive, bool skeletonSensorActive)
        {
            if (KinectSensor.KinectSensors.Count == 0)
            {
                MessageBox.Show("No Kinect devices detected!", "Camera View");
                return false;
            }

            try
            {
                myKinect = KinectSensor.KinectSensors[0];
                if (depthSensorActive) myKinect.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                if (colorSensorActive) myKinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                if (skeletonSensorActive) myKinect.SkeletonStream.Enable();

                myKinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(myKinect_AllFramesReady);
                myKinect.Start();
            }
            catch
            {
                MessageBox.Show("Kinect Initialization Failed");
                return false;
            }

            return true;
        }

        private void myKinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            // do event handling of when all frames are ready here
            SkeletonFrame frame = e.OpenSkeletonFrame();

            if (frame == null) return;

            Skeleton[] skeletons = new Skeleton[frame.SkeletonArrayLength];
            frame.CopySkeletonDataTo(skeletons);
            Skeleton skeletonToUse = null;

            float depth = float.MaxValue;
            foreach (Skeleton s in skeletons)
            {
                if (s.TrackingState == SkeletonTrackingState.Tracked)
                {
                    if (s.Position.Z < depth && s.Position.Z != 0)
                    {
                        depth = s.Position.Z;
                        skeletonToUse = s;
                    }
                }
            }

            if (skeletonToUse != null)
            {
                if (!trackingUser)
                {
                    trackingUser = true;
                    CustomToast.CreateToast("You are now being tracked by the Kinect.", "Diagnosis will begin now. If at any point we lose track of you we will notify you.", ToastAlertImageColors.GreeAlert);
                }

                ++counter;
                //string jsonStr = JsonConverter.createKinectDataString("", skeletonToUse);
                //Console.WriteLine(jsonStr);
                if (totalDataCounter == 0)
                    data = JsonConverter.createKinectDataString(ApplicationInformation.Instance.getUsername());
                if (counter == 6)
                {
                    counter = 0;
                    totalDataCounter++;
                    data = JsonConverter.writeFrameData(skeletonToUse, data);

                    // should be flushing to the server about once a minute
                    if (totalDataCounter == 50/*150*/)
                    {
                        data = JsonConverter.closeJsonStringObject(data);
                        // flush data to the server here!
                        if (!ApplicationInformation.Instance.isTrainingModeOn())
                            requestHandler.postKinectData(data);
                        totalDataCounter = 0;
                    }
                }
            }
            else
            {
                if (trackingUser)
                {
                    trackingUser = false;
                    CustomToast.CreateToast("You are not being tracked by the Kinect!", "Please open the Kinect Viewer to get yourself position to be tracked by the Kinect.", ToastAlertImageColors.RedAlert);
                }
            }

            frame.Dispose();
        }

        private void HandleScoreReceived(object sender, EventArgs e)
        {
            string jsonData = ((DataEventHandlerArgs)e).Data;
            KinectData data = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<KinectData>(jsonData);
            Console.WriteLine(data.score);
            float score = data.score;
            if (score > 30 && score < 60) CustomToast.CreateToast("Warning!", "Your posture is not too healthy!", ToastAlertImageColors.YellowAlert);
            else if (score <= 30) CustomToast.CreateToast("Critical!", "Your posture is in the danger zone! Please fix!", ToastAlertImageColors.RedAlert);
        }

        public KinectSensor getSensor()
        {
            return this.myKinect;
        }
    }

    public class KinectData
    {
        public ErrorObject error { get; set; }
        public float score { get; set; }
        public string avg_score { get; set; }
    }

    public class ErrorObject
    {
        public string message { get; set; }
        public string code { get; set; }
    }
}
