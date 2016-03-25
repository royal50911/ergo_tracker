using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErgoTracker
{
    public partial class KinectForm : Form
    {
        KinectSensor myKinect;
        ServerRequestHandler requestHandler;
        bool sendOnNextCallback = false;

        public KinectForm(KinectSensor kinect, ServerRequestHandler _requestHandler)
        {
            InitializeComponent();
            myKinect = kinect;
            requestHandler = _requestHandler;
            requestHandler.ReceivedScoreData += HandleScoreReceived;
        }

        private void KinectForm_Load(object sender, EventArgs e)
        {
            if (!myKinect.IsRunning)
            {
                myKinect = KinectSensor.KinectSensors[0];
                myKinect.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                myKinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                myKinect.SkeletonStream.Enable();

                myKinect.Start();
            }

            myKinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(myKinect_AllFramesReady);
        }

        Bitmap _bitmap;

        void myKinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            myKinect_SensorDepthFrameReady(e);
            kinectVideoBox.Image = _bitmap;
        }

        void myKinect_SensorDepthFrameReady(AllFramesReadyEventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                using (var frame = e.OpenColorImageFrame())
                {
                    //_bitmap = CreateBitmapFromDepthFrame(frame);
                    _bitmap = CreateBitmapFromColorImage(frame);
                    using (var skel_frame = e.OpenSkeletonFrame())
                        DrawSkeleton(skel_frame, _bitmap);
                }
            }
        }

        private void DrawSkeleton(SkeletonFrame frame, Bitmap bitmap)
        {
            if (frame != null)
            {
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
                    SkeletonPoint sloc = skeletonToUse.Joints[JointType.Head].Position;
                    ColorImagePoint cloc = myKinect.CoordinateMapper.MapSkeletonPointToColorPoint(sloc, ColorImageFormat.RgbResolution640x480Fps30);
                    markAtPoint(cloc, bitmap);
                    DrawSkeleton(skeletonToUse, bitmap);
                    DrawInformation(skeletonToUse, bitmap);

                    if (sendOnNextCallback)
                    {
                        string data = JsonConverter.createKinectDataString(ApplicationInformation.Instance.getUsername());
                        data = JsonConverter.writeFrameData(skeletonToUse, data);
                        data = JsonConverter.closeJsonStringObject(data);
                        // flush data to the server here!
                        if (!ApplicationInformation.Instance.isTrainingModeOn())
                            requestHandler.postKinectData(data);

                        sendOnNextCallback = false;
                    }
                }

            }
        }

        private void DrawInformation(Skeleton s, Bitmap bitmap)
        {
            if (bitmap == null) return;


            Joint head = s.Joints[JointType.Head];
            Double[] headcoordinate = getJointCoordinate(head);
            Joint shouldercenter = s.Joints[JointType.ShoulderCenter];
            Double[] shouldercentercoordinate = getJointCoordinate(shouldercenter);
            Joint shoulderleft = s.Joints[JointType.ShoulderLeft];
            Double[] shoulderleftcoordinate = getJointCoordinate(shoulderleft);
            Joint shoulderright = s.Joints[JointType.ShoulderRight];
            Double[] shoulderrightcoordinate = getJointCoordinate(shoulderright);
            Joint spine = s.Joints[JointType.Spine];
            Double[] spinecoordinate = getJointCoordinate(spine);
            Joint hipcenter = s.Joints[JointType.HipCenter];
            Double[] hipcentercoordinate = getJointCoordinate(hipcenter);
            Joint hipleft = s.Joints[JointType.HipLeft];
            Double[] hipleftcoordinate = getJointCoordinate(hipleft);
            Joint hipright = s.Joints[JointType.HipRight];
            Double[] hiprightcoordinate = getJointCoordinate(hipright);


            Graphics g = Graphics.FromImage(bitmap);
            DisplayCoordinate(headcoordinate, shouldercentercoordinate, shoulderleftcoordinate, shoulderrightcoordinate,
                spinecoordinate, hipcentercoordinate, hipleftcoordinate, hiprightcoordinate, g);
        }

        private void DrawSkeleton(Skeleton s, Bitmap bitmap)
        {
            if (bitmap == null) return;

            Graphics g = Graphics.FromImage(bitmap);
            // draw head to shoulder center
            DrawBone(JointType.Head, JointType.ShoulderCenter, s, g);
            // draw shoulder center to shoulder left
            DrawBone(JointType.ShoulderCenter, JointType.ShoulderLeft, s, g);
            // draw shoulder center to shoulder right
            DrawBone(JointType.ShoulderCenter, JointType.ShoulderRight, s, g);
            // draw shoulder left to elbow left
            DrawBone(JointType.ShoulderLeft, JointType.ElbowLeft, s, g);
            // draw shoulder right to elbow right
            DrawBone(JointType.ShoulderRight, JointType.ElbowRight, s, g);
            // draw shoulder center to spine
            DrawBone(JointType.ShoulderCenter, JointType.Spine, s, g);
            // draw spine to hip center
            DrawBone(JointType.Spine, JointType.HipCenter, s, g);
            // draw hip center to hip left
            DrawBone(JointType.HipCenter, JointType.HipLeft, s, g);
            // draw hip center to hip right
            DrawBone(JointType.HipCenter, JointType.HipRight, s, g);
        }


        private Bitmap CreateBitmapFromDepthFrame(DepthImageFrame frame)
        {
            if (frame != null)
            {
                var bitmapImage = new Bitmap(frame.Width, frame.Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
                var g = Graphics.FromImage(bitmapImage);
                g.Clear(Color.FromArgb(0, 34, 68));

                var _pixelData = new short[frame.PixelDataLength];
                frame.CopyPixelDataTo(_pixelData);
                System.Drawing.Imaging.BitmapData bmapdata = bitmapImage.LockBits(new Rectangle(0, 0, frame.Width, frame.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmapImage.PixelFormat);
                IntPtr ptr = bmapdata.Scan0;
                Marshal.Copy(_pixelData, 0, ptr, frame.Width * frame.Height);
                bitmapImage.UnlockBits(bmapdata);

                return bitmapImage;
            }

            return null;
        }

        private Bitmap CreateBitmapFromColorImage(ColorImageFrame frame)
        {
            if (frame != null)
            {
                byte[] pixelData = new byte[frame.PixelDataLength];
                frame.CopyPixelDataTo(pixelData);
                Bitmap bmap = new Bitmap(
                    frame.Width,
                    frame.Height,
                    PixelFormat.Format32bppRgb);
                BitmapData bmapdata = bmap.LockBits(new Rectangle(0, 0, frame.Width, frame.Height), ImageLockMode.WriteOnly, bmap.PixelFormat);
                IntPtr ptr = bmapdata.Scan0;
                Marshal.Copy(pixelData, 0, ptr, frame.PixelDataLength);
                bmap.UnlockBits(bmapdata);
                return bmap;
            }
            return null;
        }

        private void markAtPoint(ColorImagePoint p, Bitmap b)
        {
            if (b == null) return;
            Graphics g = Graphics.FromImage(b);
            g.DrawEllipse(Pens.Red, new Rectangle(p.X - 20, p.Y - 20, 40, 40));
        }

        private Point GetJoint(JointType j, Skeleton s)
        {
            SkeletonPoint sloc = s.Joints[j].Position;
            ColorImagePoint cloc = myKinect.CoordinateMapper.MapSkeletonPointToColorPoint(sloc, ColorImageFormat.RgbResolution640x480Fps30);
            return new Point(cloc.X, cloc.Y);
        }

        private void DrawBone(JointType j1, JointType j2, Skeleton s, Graphics g)
        {
            Point p1 = GetJoint(j1, s);
            Point p2 = GetJoint(j2, s);

            Pen drawingPen = new Pen(Color.LimeGreen, 5);
            try
            {
                g.DrawLine(drawingPen, p1, p2);
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry("Error Occurred in drawing skeleton.", e.ToString());
            }
        }

        private void DisplayCoordinate(Double[] h, Double[] s, Double[] sl, Double[] sr,
    Double[] sp, Double[] hp, Double[] hpl, Double[] hpr, Graphics g)
        {

            g.DrawString("Joint Information:",
             new Font("Arial", 10, FontStyle.Regular), Brushes.GreenYellow, 10, 30);

            g.DrawString("head: " + ", " + string.Format("{0:0.0000}", h[0])
                + ", " + string.Format("{0:0.0000}", h[1]) + ", " + string.Format("{0:0.0000}", h[2]),
             new Font("Arial", 10, FontStyle.Regular), Brushes.GreenYellow, 10, 50);

            g.DrawString("shoulder center: " + ", " + string.Format("{0:0.0000}", s[0])
                + ", " + string.Format("{0:0.0000}", s[1]) + ", " + string.Format("{0:0.0000}", s[2]),
             new Font("Arial", 10, FontStyle.Regular), Brushes.GreenYellow, 10, 70);

            g.DrawString("shoulder left: " + ", " + string.Format("{0:0.0000}", sl[0])
                + ", " + string.Format("{0:0.0000}", sl[1]) + ", " + string.Format("{0:0.0000}", sl[2]),
             new Font("Arial", 10, FontStyle.Regular), Brushes.GreenYellow, 10, 90);

            g.DrawString("shoulder right: " + ", " + string.Format("{0:0.0000}", sr[0])
                + ", " + string.Format("{0:0.0000}", sr[1]) + ", " + string.Format("{0:0.0000}", sr[2]),
             new Font("Arial", 10, FontStyle.Regular), Brushes.GreenYellow, 10, 110);

            g.DrawString("spine: " + ", " + string.Format("{0:0.0000}", sp[0])
                + ", " + string.Format("{0:0.0000}", sp[1]) + ", " + string.Format("{0:0.0000}", sp[2]),
             new Font("Arial", 10, FontStyle.Regular), Brushes.GreenYellow, 10, 130);

            g.DrawString("Hip center: " + ", " + string.Format("{0:0.0000}", hp[0])
                + ", " + string.Format("{0:0.0000}", hp[1]) + ", " + string.Format("{0:0.0000}", hp[2]),
             new Font("Arial", 10, FontStyle.Regular), Brushes.GreenYellow, 10, 150);

            g.DrawString("hip left: " + ", " + string.Format("{0:0.0000}", hpl[0])
                + ", " + string.Format("{0:0.0000}", hpl[1]) + ", " + string.Format("{0:0.0000}", hpl[2]),
             new Font("Arial", 10, FontStyle.Regular), Brushes.GreenYellow, 10, 170);

            g.DrawString("hip right: " + ", " + string.Format("{0:0.0000}", hpr[0])
                + ", " + string.Format("{0:0.0000}", hpr[1]) + ", " + string.Format("{0:0.0000}", hpr[2]),
             new Font("Arial", 10, FontStyle.Regular), Brushes.GreenYellow, 10, 190);

        }

        private Double[] getJointCoordinate(Joint j)
        {

            Double X = j.Position.X;
            Double Y = j.Position.Y;
            Double Z = j.Position.Z;
            Double[] coordinate = new Double[] { X, Y, Z };
            return coordinate;
        }

        private void HandleScoreReceived(object sender, EventArgs e)
        {
            string jsonData = ((DataEventHandlerArgs)e).Data;
            KinectData data = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<KinectData>(jsonData);
            //Console.WriteLine(data.score);
            float score = data.score;

            scorelabel.Text = "" + score;
            if (score >= 60) scorelabel.ForeColor = Color.DarkGreen;
            else if (score > 30 && score < 60) scorelabel.ForeColor = Color.DarkOrange;
            else scorelabel.ForeColor = Color.DarkRed;
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            sendOnNextCallback = true;
        }
    }
}
