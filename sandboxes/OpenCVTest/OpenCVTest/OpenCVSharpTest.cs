using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace CardDetectionExample
{
    class CardDetector
    {
        public void DetectCard(string imagePath)
        {
            // Load the image using OpenCVSharp4
            using (Mat image = Cv2.ImRead(imagePath))
            {
                // Convert the image to grayscale
                using (Mat gray = new Mat())
                {
                    Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

                    // Blur the image to reduce noise
                    using (Mat blurred = new Mat())
                    {
                        Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 0);

                        // Detect edges in the image using the Canny algorithm
                        using (Mat edges = new Mat())
                        {
                            // 3rd parameter - Lower Edge Threshold (default 50)
                            // 4th parameter - Upper Edge Threshold (default 200)
                            // 5th parameter - Aperture (default 3)
                            Cv2.Canny(blurred, edges, 75, 125, 7, false);

                            // Find contours in the image
                            Point[][] contours;
                            HierarchyIndex[] hierarchy;

                            // changing the approximation parameter can affect the detection of edges
                            Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                            //Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxTC89KCOS);


                            // Find the contour with the largest area, which is likely the card
                            double maxArea = 0;
                            int maxAreaIdx = -1;
                            for (int i = 0; i < contours.Length; i++)
                            {
                                double area = Cv2.ContourArea(contours[i]);
                                if (area > maxArea)
                                {
                                    maxArea = area;
                                    maxAreaIdx = i;
                                }
                            }

                            // Draw the contour on the image for visualization
                            Cv2.DrawContours(image, contours, maxAreaIdx, Scalar.Red, 2);

                            // Display the result
                            Cv2.ImShow("Card Detection Result", image);
                            Cv2.WaitKey(0);
                        }
                    }
                }
            }
        }
    }
}