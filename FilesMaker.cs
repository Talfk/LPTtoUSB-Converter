using LPTtoUSB_Converter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LPTtoUSB_Converter
{
    public class FilesMaker
    {
        private readonly string mainDirectory; // The base directory where temporary files will be created
        int nextNumber;
        static Process wordViewerProcess;

        public FilesMaker(string mainDirectory)
        {
            this.mainDirectory = mainDirectory;

            // Ensure the main directory exists
            if (!Directory.Exists(mainDirectory))
            {
                Directory.CreateDirectory(mainDirectory);
            }
        }

        public async void ProcessTextFile(byte[] rawData, List<DataBlock> dataBlocks, BitImageProcessor bitImageProcessor, bool isPreview, bool openatEnd)
        {
            // Check if dataBlocks is null or empty - for cases when user press start&stop withno information being recieved
            if (dataBlocks == null || dataBlocks.Count == 0)
            {
                // Handle the case where no information was received
                MessageBox.Show("No information received.");
                return; // No need to continue processing if no data is found - return to form1.cs
            }


            string uniqueFileName;

            // If it's a preview, generate a unique name based on the date
            if (isPreview)
            {
                // If it's not a preview, generate a unique filename using Guid
                uniqueFileName = "PreviewFile";

            }
            else
            {
                FindLatestDocumentNumber(); // You need to implement this method to find the next available document number
                nextNumber++;
                uniqueFileName = $"Document{nextNumber} - {DateTime.Now:dd-MM-yyyy-HH-mm-ss}";
            }

            // Create the "Raw Data" directory if it doesn't exist
            string centralRawDataDirectory = Path.Combine(mainDirectory, "Raw Data");
            if (!Directory.Exists(centralRawDataDirectory))
            {
                Directory.CreateDirectory(centralRawDataDirectory);
            }

            // Create a temporary directory inside the "Raw Data" directory for the current instance
            string tempDirectory = Path.Combine(centralRawDataDirectory, uniqueFileName);
            Directory.CreateDirectory(tempDirectory);

            // Save raw data to a file in the temporary directory
            string rawTextFilePath = Path.Combine(tempDirectory, "raw_data.txt");
            File.WriteAllBytes(rawTextFilePath, rawData);

            // Create directory for images
            string localImagesDirectory = Path.Combine(tempDirectory, "Images");
            Directory.CreateDirectory(localImagesDirectory);

            // Process the data blocks to create a text file with converted Code Page 437 content
            string blocksFilePath = Path.Combine(tempDirectory, $"{uniqueFileName}_blocks_data.txt");
            WriteBlocksDataToFile(dataBlocks, blocksFilePath);

            // Process and save images in the images directory
            int imageCount = 0;
            foreach (DataBlock block in dataBlocks)
            {
                if (block.Type == "Image")
                {
                    // Creating the images per separate block of image
                    Bitmap processedImageBitmap = bitImageProcessor.ProcessGraphicsFile(block.Data);
                    // Unique file name for each image + to be saved in the right directory
                    string imageFilePath = Path.Combine(localImagesDirectory, $"{uniqueFileName}_image{block.ImageCount}.png");
                    // Saves the bitmap variable to the correct location
                    processedImageBitmap.Save(imageFilePath);
                    // Clears the process that made the bitmap
                    processedImageBitmap.Dispose();
                    imageCount++;
                }
            }

            // Update the class-level variable with the local directory
            string imagesDirectory = localImagesDirectory;

            // Create Word document with the processed data
            string wordDocumentFilePath;
            if (isPreview)
            {
                wordDocumentFilePath = Path.Combine(tempDirectory, "Preview.docx");
            }
            else
            {
                wordDocumentFilePath = Path.Combine(mainDirectory, $"{uniqueFileName}.docx");
            }


            if (wordViewerProcess != null)
            {
                await Task.Delay(1000);
                wordViewerProcess.Kill();
            }

            CreateTextFile(wordDocumentFilePath, dataBlocks, bitImageProcessor, uniqueFileName, imagesDirectory);

            if (isPreview || openatEnd)
            {
                await Task.Delay(1000);
                wordViewerProcess = Process.Start(new ProcessStartInfo(wordDocumentFilePath) { UseShellExecute = true });

            }

        }




        private void FindLatestDocumentNumber()
        {
            string[] files = Directory.GetFiles(mainDirectory, "Document* - *.docx");

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);

                if (fileNameWithoutExtension.StartsWith("Document") && fileNameWithoutExtension.Contains(" - "))
                {
                    // Extracting the numeric part between "Document" and " - "
                    string numberPart = fileNameWithoutExtension.Split(new string[] { "Document", " - " }, StringSplitOptions.RemoveEmptyEntries)[0];

                    int number;
                    if (int.TryParse(numberPart, out number))
                    {
                        nextNumber = Math.Max(nextNumber, number);
                    }
                }
            }
        }



        private void WriteBlocksDataToFile(List<DataBlock> dataBlocks, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (DataBlock block in dataBlocks)
                {
                    if (block.Type == "Text")
                    {
                        writer.WriteLine(Encoding.UTF8.GetString(block.Data));
                    }
                    else if (block.Type == "Image")
                    {
                        writer.WriteLine($"[Image {block.ImageCount}]");
                    }
                }
            }
        }

        private async void CreateTextFile(string filePath, List<DataBlock> dataBlocks, BitImageProcessor bitImageProcessor, string uniqueFileName, string imagesDirectory)
        {
            try
            {
                // Word Document settings
                dynamic wordApp = Activator.CreateInstance(Type.GetTypeFromProgID("Word.Application"));
                dynamic wordDoc = wordApp.Documents.Add();

                string FontName = "Courier New";
                string FontSize = "11";

                foreach (dynamic section in wordDoc.Sections)
                {
                    section.PageSetup.LeftMargin = wordApp.CentimetersToPoints(0.8);
                    section.PageSetup.RightMargin = wordApp.CentimetersToPoints(0.5);
                    section.PageSetup.TopMargin = wordApp.CentimetersToPoints(0.75);
                    section.PageSetup.BottomMargin = wordApp.CentimetersToPoints(0.75);
                    section.PageSetup.Gutter = 0;
                    section.PageSetup.MirrorMargins = false;
                }

                // Inserting the text and images into the Word document
                dynamic range = wordDoc.Range(0, 0);
                foreach (DataBlock block in dataBlocks)
                {
                    if (block.Type == "Text")
                    {
                        Encoding sourceEncoding = Encoding.GetEncoding(437);
                        string content = sourceEncoding.GetString(block.Data);
                        range.InsertAfter(content);

                        range.Font.Name = FontName;
                        range.Font.Size = FontSize;
                        range.Font.Bold = false;
                        range.Font.Italic = false;
                        range.ParagraphFormat.SpaceBefore = 0;
                        range.ParagraphFormat.SpaceAfter = 0;
                        range.ParagraphFormat.Alignment = 0; // Left alignment, for cases when the default is to the right

                        range = wordDoc.Range(range.End, range.End);////????
                    }
                    else if (block.Type == "Image")
                    {
                        string imageFilePath = Path.Combine(imagesDirectory, $"{uniqueFileName}_image{block.ImageCount}.png");
                        dynamic inlineShape = wordDoc.InlineShapes.AddPicture(imageFilePath, Range: range);
                        range = wordDoc.Range(inlineShape.Range.End, inlineShape.Range.End);
                    }
                }

                // Save the Word document with the unique filename
                wordDoc.SaveAs2(filePath);
                wordDoc.Close();

                // Introduce a delay before quitting the Word application
                await Task.Delay(1000);



                // Quit Word application
                wordApp.Quit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}