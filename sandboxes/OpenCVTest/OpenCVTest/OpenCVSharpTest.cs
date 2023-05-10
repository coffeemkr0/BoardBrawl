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
                            //
                            // TODO: If user clicks on the image and the area includes the coordinates where the user clicked, it should be the area used for recognition
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

                            // Get the cropping area
                            Rect boundingRect = Cv2.BoundingRect(contours[maxAreaIdx]);

                            // Crop the image to the bounding rectangle
                            using (Mat cropped = new Mat(image, boundingRect))
                            {
                                // Display the result
                                Cv2.ImShow("Card Detection Result", image);

                                // Display the greyscale version
                                Cv2.ImShow("Greyscale Result", gray);

                                // Display the cropped image
                                Cv2.ImShow("Cropped Card Image", cropped);
                                Cv2.WaitKey(0);
                            }

                            // TODO: Prepare and send the cropped image for recognition

                        }
                    }
                }
            }
        }

        public void DetectCardShapes(string imagePath)
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
                            Cv2.Canny(blurred, edges, 75, 125, 7, false);

                            // Find contours in the image
                            Point[][] contours;
                            HierarchyIndex[] hierarchy;
                            Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                            // Find all the card-shaped contours
                            double aspectRatio = 2.5 / 3.5;
                            List<Rect> cardRects = new List<Rect>();
                            foreach (var contour in contours)
                            {
                                double area = Cv2.ContourArea(contour);
                                if (area < 750) // Ignore small contours
                                    continue;

                                // Find the bounding rectangle of the contour
                                var rect = Cv2.BoundingRect(contour);

                                // Calculate the aspect ratio of the rectangle
                                double rectAspectRatio = (double)rect.Width / rect.Height;

                                // Calculate the difference between the aspect ratios
                                double aspectRatioDiff = Math.Abs(aspectRatio - rectAspectRatio);

                                // If the difference is small enough, we assume the contour is a card
                                if (aspectRatioDiff < 0.2)
                                {
                                    cardRects.Add(rect);
                                }
                            }

                            // Crop the image to each of the card-shaped contours and display the result
                            foreach (var cardRect in cardRects)
                            {
                                // Draw the contour on the image for visualization
                                Cv2.Rectangle(image, cardRect, Scalar.Red, 2);

                                // Crop the image to the bounding rectangle
                                using (Mat cropped = new Mat(image, cardRect))
                                {
                                    // Display the result
                                    Cv2.ImShow("Card Detection Result", image);

                                    // Display the greyscale version
                                    Cv2.ImShow("Greyscale Result", gray);

                                    // Display the cropped image
                                    Cv2.ImShow("Cropped Card Image", cropped);
                                    Cv2.WaitKey(0);
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}