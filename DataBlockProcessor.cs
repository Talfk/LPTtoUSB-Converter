using System;
using System.Collections.Generic;
using System.Linq;

namespace LPTtoUSB_Converter
{
    public class DataBlock
    {
        public int Order { get; set; }
        public int ImageCount { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }
    }

    public class DataBlockProcessor
    {
        private readonly byte[] imageStartPatternBytes = { 0x1b, 0x41, 0x08, 0x1b, 0x32, 0x1b, 0x41, 0x08 };
        private readonly byte[] imageEndPatternBytes = { 0x1b, 0x41, 0x0c, 0x1b, 0x32 };

        public List<DataBlock> ProcessDataBlocks(byte[] fileContents)
        {
            List<DataBlock> dataBlocks = new List<DataBlock>();
            int order = 1;//To save the appearance order of all blocks
            int imageCount = 1;//To save the order of all images
            int currentIndex = 0;//To follow the index in the raw file

            while (currentIndex < fileContents.Length)
            {
                int start = currentIndex;

                // Check for conditions that indicate the start of an image block
                if (IsImageBlockStart(fileContents, currentIndex))
                {
                    int endOfImageBlock = FindEndOfImageBlock(fileContents, currentIndex);

                    // Extract the image data block
                    byte[] imageData = fileContents.Skip(start).Take(endOfImageBlock + imageEndPatternBytes.Length - start).ToArray();

                    // Add the image data block to the list
                    dataBlocks.Add(new DataBlock
                    {
                        Order = order++,
                        Type = "Image",
                        Data = imageData,
                        ImageCount = imageCount++
                    }); 

                    // Move to the next position after processing the image block
                    currentIndex = endOfImageBlock;
                }
                else
                {
                    // Non-image data block
                    currentIndex = FindPatternIndex(fileContents, start);

                    // Extract the non-image data block
                    byte[] nonImageData = fileContents.Skip(start).Take(currentIndex - start).ToArray();
                    nonImageData = TransformTextBlock(nonImageData); //Deletes any ESC comands and replaces Lf with CR & LF

                    // Add the non-image data block to the list
                    dataBlocks.Add(new DataBlock
                    {
                        Order = order++,
                        Type = "Text",
                        Data = nonImageData
                    });
                }
            }

            return dataBlocks;
        }


        //Modify the text to be representable = Deletes any ESC comands and replaces Lf with CR&LF 
        static byte[] TransformTextBlock(byte[] textBlock)
        {
            // Apply transformations for each block of text
            List<byte> transformedText = new List<byte>();

            for (int i = 0; i < textBlock.Length; i++)
            {
                //whenevere there is a byte to delete, it just does not get inserted to the new block of text
                switch (textBlock[i])
                {
                    case 0x1b://current byte = ESC 0x27
                        i++; // Skip the next byte on the text block
                        if (textBlock[i] == 0x78 && textBlock[i + 1] != 0x1b && i < textBlock.Length)//after ESC x need also to delete the next byte(if its is not ESC)
                        {

                            i++;
                        }
                        break;

                    case 0x12:// DC2 0x12
                        // Do nothing for DC2, effectively deleting it
                        break;

                    //case 0x0a: // LF
                    //           // Replace standalone LF (0x0a) with 0x0d 0x0a only if CR does not precede it
                    //    if (i == 0 || textBlock[i - 1] != 0x0d)
                    //    {
                    //        transformedText.Add(0x0d);
                    //        transformedText.Add(0x0a);
                    //    }
                    //    break;

                    //case 0x0d: // CR
                    //           // Replace standalone CR (0x0d) with 0x0d 0x0a only if LF does not follow it
                    //    if (i == textBlock.Length - 1 || textBlock[i + 1] != 0x0a)
                    //    {
                    //        transformedText.Add(0x0d);
                    //        transformedText.Add(0x0a);
                    //    }
                    //    break;


                    default:
                        // Preserve other bytes
                        transformedText.Add(textBlock[i]);
                        break;
                }
            }

            return transformedText.ToArray();
        }

        private bool IsImageBlockStart(byte[] source, int index)
        {
            // Check for conditions that indicate the start of an image block
            if (IsMatch(source, index, imageStartPatternBytes))
            {
                return true;
            }
            else if (source[index] == 0x1b)
            {
                // Check for additional conditions, e.g., specific bit image commands
                switch (source[index + 1])
                {
                    case 0x4b: // ESC K
                    case 0x4c: // ESC L
                    case 0x59: // ESC Y
                    case 0x5a: // ESC Z
                        int byte1 = source[index + 2];
                        int byte2 = source[index + 3];
                        int numberOfBytes = byte1 + 256 * byte2 + 2; // Calculate the correct number of bytes per line
                        int endForSpecificCommand = index + 4 + numberOfBytes;//+4=lenthg of ESC K/L/Y/Z n1 n2,+2 = CR&LF end of line

                        if (endForSpecificCommand <= source.Length)
                        {
                            return true;
                        }
                        else
                        {
                            // Handle the case where the end of the image block exceeds the file length
                            return false;
                        }

                    case 0x2a: // ESC * m
                        int mByte = source[index + 2];
                        byte1 = source[index + 3];
                        byte2 = source[index + 4];
                        int numberOfBytesForStarM = byte1 + 256 * byte2 + 2; // Calculate the correct number of bytes per line
                        int endForStarM = index + 5 + numberOfBytesForStarM;

                        if (endForStarM <= source.Length)
                        {
                            return true;
                        }
                        else
                        {
                            // Handle the case where the end of the image block exceeds the file length
                            return false;
                        }

                    default:
                        // Handle unknown start sequence
                        return false;
                }
            }

            return false;
        }

   

        private int FindEndOfImageBlock(byte[] source, int startIndex)
        {
            while (startIndex < source.Length)
            {
                // Check for conditions that indicate the end of an image block
                if (IsMatch(source, startIndex, imageEndPatternBytes))
                {
                    return startIndex;
                }
                else if (source[startIndex] == 0x1b)
                {
                    // Check for additional conditions, e.g., specific bit image commands
                    switch (source[startIndex + 1])
                    {
                        case 0x4b: // ESC K
                        case 0x4c: // ESC L
                        case 0x59: // ESC Y
                        case 0x5a: // ESC Z
                            int byte1 = source[startIndex + 2];
                            int byte2 = source[startIndex + 3];
                            int numberOfBytes = byte1 + 256 * byte2 ; // Calculate the correct number of bytes per line
                            int endForSpecificCommand = startIndex + 4 + numberOfBytes;
                            

                            // Check for the absence of bit image commands after numberOfBytes
                            bool bitImageCommandFound = false;
                            int i = endForSpecificCommand;
                            if (source[i] == 0x0d & source[i + 1] != 0x0a)//Checking if the line ends only with CR(there some cases that it is)
                                i++;//skips to the next line to check, but only over CR
                            else if (source[i] == 0x0d & source[i + 1] == 0x0a)//meaning the line end with CR&LF
                                i = i + 2;
                            if (source[i] == 0x1b && (source[i + 1] == 0x4b || source[i + 1] == 0x4c || source[i + 1] == 0x59 || source[i + 1] == 0x5a || source[i + 1] == 0x2a))
                                {
                                    bitImageCommandFound = true;
                                    break;
                                }
                            
                            if (!bitImageCommandFound)
                            {
                                return endForSpecificCommand;
                            }
                            else
                            {
                                // Continue searching if a bit image command is found
                                startIndex = endForSpecificCommand;
                            }
                            break;

                        case 0x2a: // ESC * m
                            int mByte = source[startIndex + 2];
                            byte1 = source[startIndex + 3];
                            byte2 = source[startIndex + 4];
                            int numberOfBytesForStarM = byte1 + 256 * byte2 ; // Calculate the correct number of bytes per line
                            int endForStarM = startIndex + 5 + numberOfBytesForStarM+2;//109+5+640+2

                            // Check for the absence of bit image commands after numberOfBytesForStarM
                            bool bitImageCommandFoundForStarM = false;
                            int j = endForStarM;               
                            if (source[j] == 0x0d & source[j + 1] != 0x0a)//Checking if the line ends only with CR(there some cases that it is)
                                j++;//skips to the next line to check, but only over CR
                            else if (source[j] == 0x0d & source[j + 1] == 0x0a)//meaning the line end with CR&LF
                                j = j + 2;
                            if (source[j] == 0x1b && (source[j + 1] == 0x4b || source[j + 1] == 0x4c || source[j + 1] == 0x59 || source[j + 1] == 0x5a|| source[j + 1] == 0x2a))
                                {
                                    bitImageCommandFoundForStarM = true;
                                    break;
                                }
                            

                            if (!bitImageCommandFoundForStarM)
                            {
                                return endForStarM;
                            }
                            else
                            {
                                // Continue searching if a bit image command is found
                                startIndex = endForStarM;
                            }
                            break;

                        default:
                            // Handle unknown start sequence
                            break;
                    }
                }

                startIndex++;
            }

            // If the loop completes without finding the pattern, 
            // return the length of the source array to indicate the end of the content
            return source.Length;
        }



        private bool IsMatch(byte[] source, int index, byte[] pattern)
        {
            if (index < 0 || index + pattern.Length > source.Length)
            {
                return false;
            }

            for (int i = 0; i < pattern.Length; i++)
            {
                if (source[index + i] != pattern[i])
                {
                    return false;
                }
            }
            return true;
        }

        private int FindPatternIndex(byte[] source, int startIndex)
        {
            while (startIndex < source.Length)
            {
                // Check for conditions that indicate the start of an image block
                if (IsMatch(source, startIndex, imageStartPatternBytes))
                {
                    return startIndex;
                }
                else if (source[startIndex] == 0x1b)
                {
                    // Check for additional conditions, e.g., specific bit image commands
                    switch (source[startIndex + 1])
                    {
                        case 0x4b: // ESC K
                        case 0x4c: // ESC L
                        case 0x59: // ESC Y
                        case 0x5a: // ESC Z
                            int byte1 = source[startIndex + 2];
                            int byte2 = source[startIndex + 3];
                            int numberOfBytes = byte1 + 256 * byte2 + 2; // Calculate the correct number of bytes per line
                            int endForSpecificCommand = startIndex + 4 + numberOfBytes;

                            if (endForSpecificCommand <= source.Length)
                            {
                                return startIndex;
                            }
                            else
                            {
                                // Handle the case where the end of the image block exceeds the file length
                                return source.Length;
                            }

                        case 0x2a: // ESC * m
                            int mByte = source[startIndex + 2];
                            byte1 = source[startIndex + 3];
                            byte2 = source[startIndex + 4];
                            int numberOfBytesForStarM = byte1 + 256 * byte2+2; // Calculate the correct number of bytes per line
                            int endForStarM = startIndex + 5 + numberOfBytesForStarM ;//

                            if (endForStarM <= source.Length)
                            {
                                return startIndex;
                            }
                            else
                            {
                                // Handle the case where the end of the image block exceeds the file length
                                return source.Length;
                            }

                        default:
                            // Handle unknown start sequence
                            break;
                    }
                }

                startIndex++;
            }

            // If the loop completes without finding the pattern, 
            // return the length of the source array to indicate the end of the content
            return source.Length;
        }

    }
}
