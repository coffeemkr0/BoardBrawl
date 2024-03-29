using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenCvSharp;




namespace OpenCVTest
{
    public class Program
    {
        public VideoCapture capture = new VideoCapture(0);

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            //DetectCardInVideo();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void DetectCardInVideo()
        {
            Console.WriteLine("DetectCardInVideo Method invoked.");

            VideoCapture capture = new VideoCapture(0);
            
            Mat frame = new Mat();
            Mat gray = new Mat();
            Mat corners = new Mat();
            List<Point2f> cardCorners = new List<Point2f>();
            List<Point> approxCorners = new List<Point>();

            while (true)
            {
                capture.Read(frame);
                Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

                Cv2.CornerHarris(gray, corners, 2, 3, 0.04);
                Cv2.Normalize(corners, corners, 0, 255, NormTypes.MinMax);
                Cv2.ConvertScaleAbs(corners, corners);

                Cv2.FindContours(corners, out var contours, out var hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                // Find the contour with the largest area
                double maxArea = -1;
                int maxContourIdx = -1;
                for (int i = 0; i < contours.Length; i++)
                {
                    double area = Cv2.ContourArea(contours[i]);
                    if (area > maxArea)
                    {
                        maxArea = area;
                        maxContourIdx = i;
                    }
                }

                // Approximate the corners of the card using the contour
                InputArray contourInput = InputArray.Create(contours[maxContourIdx]);
                Point[] approxCornersArray = new Point[contours.Length];

                Cv2.ApproxPolyDP(contourInput, OutputArray.Create(new Mat(approxCornersArray.Length, 1, MatType.CV_32SC2)), 0.02 * Cv2.ArcLength(contours[maxContourIdx], true), true);

                approxCorners = new List<Point>(approxCornersArray);


                // Check if the approximated contour has 4 corners and is convex
                if (approxCorners.Count() == 4 && Cv2.IsContourConvex(approxCorners))
                {
                    cardCorners = approxCorners.ConvertAll<Point2f>(p => new Point2f(p.X, p.Y));
                }

                // Draw the card corners on the frame
                if (cardCorners.Count() == 4)
                {
                    Cv2.Polylines(frame, new Point[][] { cardCorners.ConvertAll<Point>(p => new Point((int)p.X, (int)p.Y)).ToArray() }, true, Scalar.Green, 2);
                }

                Cv2.ImShow("frame", frame);

                if (Cv2.WaitKey(1) == 'q')
                {
                    break;
                }
            }

            capture.Release();
            Cv2.DestroyAllWindows();
        }


    }

    public class VideoController : Controller
    {
        private readonly VideoCapture _capture;

        public VideoController()
        {
            _capture = new VideoCapture(0);
        }

        //[HttpGet]
        //public ActionResult<byte[]> GetFrame()
        //{
        //    Mat frame = new Mat();
        //    _capture.Read(frame);

        //    //MemoryStream ms = new MemoryStream();

        //    if (!frame.Empty())
        //    {
        //        byte[] encodedImage = frame.ImEncode(".jpg", new[] { (int)ImwriteFlags.JpegQuality, 90 });
        //        //ms = new MemoryStream(encodedImage);
        //        return encodedImage;
        //    }
        //    else
        //    {
        //        //Send no camera image
        //        return null;
        //    }

        //    //Program.DetectCardInVideo();

        //}

        [HttpGet]
        public IActionResult GetFrame()
        {
            Mat frame = new Mat();
            _capture.Read(frame);

            if (frame.Empty())
            {
                // Send no camera image
                return new EmptyResult();
            }

            #region openCVTest
            //todo: figure out how to pass teh from into the class method instead.

            Mat gray = new Mat();
            Mat corners = new Mat();
            List<Point2f> cardCorners = new List<Point2f>();
            List<Point> approxCorners = new List<Point>();
            Mat edges = new Mat(frame.Size(), MatType.CV_8UC1);

            Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

            //Cv2.CornerHarris(gray, corners, 2, 3, 0.04);
            //Cv2.Normalize(corners, corners, 0, 255, NormTypes.MinMax);
            //Cv2.ConvertScaleAbs(corners, corners);

            Cv2.GaussianBlur(gray, gray, new Size(5, 5), 0);

            int threshold1 = 20;
            int threshold2 = 60;
            int apertureSize = 3;
            bool L2gradient = false;

            Cv2.Canny(gray, edges, threshold1, threshold2, apertureSize, L2gradient);

            Cv2.FindContours(corners, out var contours, out var hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // Find the contour with the largest area
            double maxArea = -1;
            int maxContourIdx = -1;
            for (int i = 0; i < contours.Length; i++)
            {
                double area = Cv2.ContourArea(contours[i]);
                if (area > maxArea)
                {
                    maxArea = area;
                    maxContourIdx = i;
                }
            }

            // Approximate the corners of the card using the contour
            InputArray contourInput = InputArray.Create(contours[maxContourIdx]);
            Point[] approxCornersArray = new Point[contours.Length];

            Cv2.ApproxPolyDP(contourInput, OutputArray.Create(new Mat(approxCornersArray.Length, 1, MatType.CV_32SC2)), 0.02 * Cv2.ArcLength(contours[maxContourIdx], true), true);

            approxCorners = new List<Point>(approxCornersArray);


            // Check if the approximated contour has 4 corners and is convex
            if (approxCorners.Count() == 4 && Cv2.IsContourConvex(approxCorners))
            {
                cardCorners = approxCorners.ConvertAll<Point2f>(p => new Point2f(p.X, p.Y));
            }

            // Draw the card corners on the frame
            if (cardCorners.Count() == 4)
            {
                Cv2.Polylines(frame, new Point[][] { cardCorners.ConvertAll<Point>(p => new Point((int)p.X, (int)p.Y)).ToArray() }, true, Scalar.Green, 2);
            }

            Cv2.ImShow("frame", frame);

            if (Cv2.WaitKey(1) == 'q')
            {
                //break;
            }

            #endregion


            byte[] encodedImage = frame.ImEncode(".jpg", new[] { (int)ImwriteFlags.JpegQuality, 70 });

            return File(encodedImage, "image/jpeg");
        }
    }
}
