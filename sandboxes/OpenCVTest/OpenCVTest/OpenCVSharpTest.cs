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
            Mat image = Cv2.ImRead(imagePath);
            
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

                            // Display auto-rotated cropped image
                            Cv2.ImShow("Auto-rotated Cropped Card Image", AutoOrientCroppedImage(cropped));

                            Cv2.WaitKey(0);
                        }

                        // TODO: Prepare and send the cropped image for recognition/classification

                    }
                }
            }
            
        }

        public void DetectCard_HoughLineTransform(string imagePath)
        {
            Mat image = Cv2.ImRead(imagePath);
            using (Mat gray = new Mat())
            {
                Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
                using (Mat blurred = new Mat())
                {
                    Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 0);

                    // Apply Hough Line Transform
                    LineSegmentPolar[] lines = Cv2.HoughLines(blurred, 1, Math.PI / 180, threshold: 150);

                    // Find the most prominent line using the longest length
                    LineSegmentPolar longestLine = lines.OrderByDescending(line => Math.Sqrt(line.Rho * line.Rho + line.Theta * line.Theta)).FirstOrDefault();

                    if (longestLine != null)
                    {
                        double angle = longestLine.Theta * 180 / Math.PI;
                                                
                        using (Mat edges = new Mat())
                        {
                            Cv2.Canny(blurred, edges, 75, 125, 7, false);

                            Point[][] contours;
                            HierarchyIndex[] hierarchy;

                            Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

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

                            Cv2.DrawContours(image, contours, maxAreaIdx, Scalar.Red, 2);

                            Rect boundingRect = Cv2.BoundingRect(contours[maxAreaIdx]);

                            using (Mat cropped = new Mat(image, boundingRect))
                            {
                                Cv2.ImShow("Card Detection Result", image);
                                Cv2.ImShow("Greyscale Result", gray);
                                Cv2.ImShow("Cropped Card Image", cropped);
                                Cv2.ImShow("Auto-rotated Cropped Card Image", AutoOrientCroppedImage(cropped));

                                Cv2.WaitKey(0);
                            }

                            // TODO: Prepare and send the cropped image for recognition/classification
                        }
                    }
                    else
                    {
                        // No prominent line found, display the original image
                        Cv2.ImShow("No Card Detection Result", image);
                        Cv2.WaitKey(0);
                    }
                }
            }
        }


        public void DetectCard_Laplacian(string imagePath)
        {
            // Load the image using OpenCVSharp4
            Mat image = Cv2.ImRead(imagePath);

            // Convert the image to grayscale
            using (Mat gray = new Mat())
            {
                Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

                // Apply Gaussian blur to reduce noise
                using (Mat blurred = new Mat())
                {
                    Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 0);

                    // Apply Laplacian edge detection
                    using (Mat edges = new Mat())
                    {
                        Cv2.Laplacian(blurred, edges, MatType.CV_8U, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);

                        // Find contours in the image
                        Point[][] contours;
                        HierarchyIndex[] hierarchy;
                        Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                        // Filter contours based on area and aspect ratio
                        const double minArea = 1000; // Minimum area of the card contour
                        const double minAspectRatio = 0.8; // Minimum aspect ratio of the card contour
                        const double maxAspectRatio = 1.2; // Maximum aspect ratio of the card contour

                        foreach (var contour in contours)
                        {
                            double area = Cv2.ContourArea(contour);
                            if (area < minArea)
                                continue;

                            Rect boundingRect = Cv2.BoundingRect(contour);
                            double aspectRatio = (double)boundingRect.Width / boundingRect.Height;
                            if (aspectRatio < minAspectRatio || aspectRatio > maxAspectRatio)
                                continue;

                            // Draw the card contour on the original image
                            Cv2.DrawContours(image, new[] { contour }, 0, Scalar.Red, 2);

                            // Display the cropped card image
                            using (Mat croppedCard = new Mat(image, boundingRect))
                            {
                                Cv2.ImShow("Cropped Card Image", croppedCard);
                                //Cv2.WaitKey(0);
                            }

                            // TODO: Process the cropped card image (e.g., perform recognition/classification)

                            // Break after detecting the first suitable card contour
                            break;
                        }

                        // Display the image with card contour(s)
                        Cv2.ImShow("Card Detection Result", image);
                        Cv2.WaitKey(0);
                    }
                }
            }
        }

        public void DetectCard_Sobel(string imagePath)
        {
            // Load the image using OpenCVSharp4
            Mat image = Cv2.ImRead(imagePath);

            // Convert the image to grayscale
            using (Mat gray = new Mat())
            {
                Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

                // Apply Gaussian blur to reduce noise
                using (Mat blurred = new Mat())
                {
                    Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 0);

                    // Apply Sobel edge detection
                    using (Mat edges = new Mat())
                    {
                        Cv2.Sobel(blurred, edges, MatType.CV_8U, 1, 1, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);

                        // Find contours in the image
                        Point[][] contours;
                        HierarchyIndex[] hierarchy;
                        Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                        // Filter contours based on area and aspect ratio
                        const double minArea = 1000; // Minimum area of the card contour
                        const double minAspectRatio = 0.8; // Minimum aspect ratio of the card contour
                        const double maxAspectRatio = 1.2; // Maximum aspect ratio of the card contour

                        foreach (var contour in contours)
                        {
                            double area = Cv2.ContourArea(contour);
                            if (area < minArea)
                                continue;

                            Rect boundingRect = Cv2.BoundingRect(contour);
                            double aspectRatio = (double)boundingRect.Width / boundingRect.Height;
                            if (aspectRatio < minAspectRatio || aspectRatio > maxAspectRatio)
                                continue;

                            // Draw the card contour on the original image
                            Cv2.DrawContours(image, new[] { contour }, 0, Scalar.Red, 2);

                            // Display the cropped card image
                            using (Mat croppedCard = new Mat(image, boundingRect))
                            {
                                Cv2.ImShow("Cropped Card Image", croppedCard);
                                Cv2.WaitKey(0);
                            }

                            // TODO: Process the cropped card image (e.g., perform recognition/classification)

                            // Break after detecting the first suitable card contour
                            break;
                        }

                        // Display the image with card contour(s)
                        Cv2.ImShow("Card Detection Result", image);
                        Cv2.WaitKey(0);
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
                                if (aspectRatioDiff < 0.15)
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

                                    // Display auto-rotated cropped image
                                    Cv2.ImShow("Auto-rotated Cropped Card Image", AutoOrientCroppedImage(cropped));

                                    Cv2.WaitKey(0);
                                }
                            }
                        }
                    }
                }
            }
        }

        public Mat AutoOrientCroppedImage(Mat croppedImage)
        {
            // TODO: Add parameters to define which way to orient an image

            // Convert the image to grayscale
            using (Mat gray = new Mat())
            {
                Cv2.CvtColor(croppedImage, gray, ColorConversionCodes.BGR2GRAY);

                // Detect edges in the image using the Canny algorithm
                using (Mat edges = new Mat())
                {
                    Cv2.Canny(gray, edges, 75, 125, 7, false);

                    // Detect lines in the image using the Hough line transform
                    LineSegmentPoint[] lines = Cv2.HoughLinesP(edges, 1, Math.PI / 180, 100, 100, 10);

                    // Find the angle of the most prominent line
                    double angle = 0;
                    int maxCount = 0;
                    foreach (var line in lines)
                    {
                        double xDiff = line.P2.X - line.P1.X;
                        double yDiff = line.P2.Y - line.P1.Y;
                        double lineAngle = Math.Atan2(yDiff, xDiff) * 180 / Math.PI;

                        // Count the number of lines with similar angles
                        int count = 0;
                        foreach (var otherLine in lines)
                        {
                            double otherXDiff = otherLine.P2.X - otherLine.P1.X;
                            double otherYDiff = otherLine.P2.Y - otherLine.P1.Y;
                            double otherLineAngle = Math.Atan2(otherYDiff, otherXDiff) * 180 / Math.PI;
                            double angleDiff = Math.Abs(lineAngle - otherLineAngle);
                            if (angleDiff < 10)
                            {
                                count++;
                            }
                        }

                        // If this line has more similar lines than any previous line, remember it as the most prominent line
                        if (count > maxCount)
                        {
                            maxCount = count;
                            angle = lineAngle;
                        }
                    }

                    // Rotate the image to align the most prominent line with the vertical axis
                    Mat rotationMatrix = Cv2.GetRotationMatrix2D(new Point2f(croppedImage.Width / 2f, croppedImage.Height / 2f), angle + 90, 1);

                    Mat rotatedImage = new Mat();
                
                    Cv2.WarpAffine(croppedImage, rotatedImage, rotationMatrix, croppedImage.Size());

                    //// Display the rotated image
                    //Cv2.ImShow("Rotated Card Image", rotatedImage);
                    //Cv2.WaitKey(0);

                    return(rotatedImage);
                
                }
            }
        }

    }

}