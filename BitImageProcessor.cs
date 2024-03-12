using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LPTtoUSB_Converter
{
    public class BitImageProcessor
    {
        private readonly byte[][] startSequences =
        {
            new byte[] { 0x1b, 0x4b },  // ESC K
            new byte[] { 0x1b, 0x4c },  // ESC L
            new byte[] { 0x1b, 0x59 },  // ESC Y
            new byte[] { 0x1b, 0x5a },  // ESC Z
            new byte[] { 0x1b, 0x2a }   // ESC * m (m = format byte, non relevant)
        };

        public int DotsAmount = 8;


        // Modify ProcessGraphicsFile method to take only one argument
        public Bitmap ProcessGraphicsFile(byte[] fileBytes)
        {
            try
            {


                int indexFirstSequence = FindFirstAny(fileBytes, startSequences, 0);
                if (indexFirstSequence != -1)
                {
                    ProcessSequence(fileBytes, indexFirstSequence);
                }

                // Call the method
                var result = calculatingLinesColumnsAmount(fileBytes, startSequences, indexFirstSequence);

                // Access the results
                int totalLinesSent = result.totalLinesSent;
                int totalColumnsSent = result.totalColumnsSent;


                // ... (remaining processing logic, similar to your button1_Click method)
                byte[,] matrix = CreateMatrix(fileBytes, totalColumnsSent, totalLinesSent);

                return CreateBitmap(matrix, DotsAmount);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return null;
            }
        }


        public static (int totalLinesSent, int totalColumnsSent) calculatingLinesColumnsAmount(byte[] fileBytes, byte[][] startSequences, int indexFirstSequence)
        {
            int totalColumnsSent = 0;
            int totalLinesSent = 0;

            for (int i = indexFirstSequence; i < fileBytes.Length;)
            {
                var currentSequence = startSequences.FirstOrDefault(sequence => FindFirst(fileBytes, sequence, i) == i);

                if (currentSequence == null)
                {
                    // Handle unknown start sequence or end loop when no more sequences found
                    break;
                }

                switch (currentSequence[0])
                {
                    case 0x1b when currentSequence[1] == 0x4b: // ESC K
                                                               // Process ESC K format
                        break;

                    case 0x1b when currentSequence[1] == 0x4c: // ESC L
                                                               // Process ESC L format
                        break;

                    case 0x1b when currentSequence[1] == 0x59: // ESC Y
                                                               // Process ESC Y format
                        break;

                    case 0x1b when currentSequence[1] == 0x5a: // ESC Z
                                                               // Process ESC Z format
                        break;

                    case 0x1b when currentSequence[1] == 0x2a: // ESC * m
                        i++; // Moves i over to skip m byte so byte1 would be the byte after m, and byte2 would be the 2nd. byte after m 
                             // Process ESC * format with mByte
                        break;

                    default:
                        // Handle unknown start sequence
                        break;
                }

                // Get the two bytes following the sequence
                byte byte1 = fileBytes[i + 2];
                byte byte2 = fileBytes[i + 3];

                // Calculate the number of bytes per line by command: ESC L byte1 byte2 => number of columns = byte1(dec) + 256(dec) * byte2(dec)
                int numberOfBytesPerLine = byte1 + 256 * byte2;

                // Next round starts from the next Esc L
                i = i + numberOfBytesPerLine;

                // Not including CR&LF characters, which appear at each end of the line as "Enter"
                // For safety - Choose the highest number of columns - not supposed to be necessary because the lines of a matrix should have all the same columns
                totalColumnsSent = Math.Max(totalColumnsSent, numberOfBytesPerLine);
            }

            // Find how many lines of graphics to be displayed sent from the computer
            totalLinesSent = FindAmount(fileBytes, startSequences);

            return (totalLinesSent, totalColumnsSent);
        }

        // Find the index where the sequence appears first
        static int FindFirst(byte[] source, byte[] pattern, int startIndex)
        {
            for (int i = startIndex; i < source.Length - pattern.Length; i++)
            {
                if (source[i] == pattern[0] && source[i + 1] == pattern[1])
                {
                    return i;
                }
            }

            return -1;
        }

        // Counts how many times any of the sequences appears
        static int FindAmount(byte[] source, byte[][] sequences)
        {
            int amount = 0;

            // Iterate through all the bytes in the file, stopping at the last possible index to start any sequence
            for (int i = 0; i < source.Length - 3;)
            {
                bool matchFound = false;

                // Check if any of the sequences match starting from the current index
                foreach (var sequence in sequences)
                {
                    bool match = true;

                    // Check if the sequence matches starting from the current index
                    for (int j = 0; j < sequence.Length; j++)
                    {
                        if (source[i + j] != sequence[j])
                        {
                            match = false;
                            break;
                        }
                    }

                    // If the sequence matches, increment the count and update the index
                    if (match)
                    {
                        amount++;
                        i += sequence.Length;
                        matchFound = true;
                        break;
                    }
                }

                // If no match is found, move to the next byte
                if (!matchFound)
                {
                    i++;
                }
            }

            return amount;
        }






        static int FindFirstAny(byte[] source, byte[][] patterns, int startIndex)
        {
            for (int i = startIndex; i <= source.Length; i++)
            {
                foreach (byte[] pattern in patterns)
                {
                    if (i <= source.Length - pattern.Length && source[i] == pattern[0] && source[i + 1] == pattern[1])
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        static void ProcessSequence(byte[] source, int startIndex)
        {
            byte[] currentSequence = new byte[2] { source[startIndex], source[startIndex + 1] };

            switch (currentSequence[0])
            {
                case 0x1b when currentSequence[1] == 0x4b: // ESC K
                                                           // Process ESC K format
                    break;

                case 0x1b when currentSequence[1] == 0x4c: // ESC L
                                                           // Process ESC L format
                    break;

                case 0x1b when currentSequence[1] == 0x59: // ESC Y
                                                           // Process ESC Y format
                    break;

                case 0x1b when currentSequence[1] == 0x5a: // ESC Z
                                                           // Process ESC Z format
                    break;

                case 0x1b when currentSequence[1] == 0x2a:
                    // Process ESC * format
                    break;

                // Add more cases as needed

                default:
                    // Handle unknown sequences
                    break;
            }
        }

        /* Copying all the hex data of the graphics to a matrix in size totalColumns x totalLines
    Modify the CreateMatrix method to handle different start sequences
 */
        private byte[,] CreateMatrix(byte[] fileBytes, int totalColumnsSent, int totalLinesSent)
        {
            byte[,] matrix = new byte[totalLinesSent, totalColumnsSent];

            for (int i = 0, currentLineSent = 0; i < fileBytes.Length && currentLineSent < totalLinesSent;)
            {
                int indexFirstSequence = -1;

                // Find the first occurrence of any start sequence
                foreach (var sequence in startSequences)
                {
                    indexFirstSequence = FindFirst(fileBytes, sequence, i);

                    if (indexFirstSequence != -1)
                        break;
                }

                if (indexFirstSequence == -1)
                {
                    // No more start sequences found, exit the loop
                    break;
                }

                // Inside the loop, determine the format and process accordingly
                var currentSequence = startSequences.First(sequence => FindFirst(fileBytes, sequence, indexFirstSequence) == indexFirstSequence);

                switch (currentSequence[0])
                {
                    case 0x1b when currentSequence[1] == 0x4b: // ESC K
                                                               // Process ESC K format
                        break;

                    case 0x1b when currentSequence[1] == 0x4c: // ESC L
                                                               // Process ESC L format
                        break;

                    case 0x1b when currentSequence[1] == 0x59: // ESC Y
                                                               // Process ESC Y format
                        break;

                    case 0x1b when currentSequence[1] == 0x5a: // ESC Z
                                                               // Process ESC Z format
                        break;

                    case 0x1b when currentSequence[1] == 0x2a: // ESC * (with additional format byte)
                        byte mByte = fileBytes[indexFirstSequence + 2];
                        if (mByte >= 0 && mByte <= 6)// 8 DotsAmount when m(decimal values) = 0, 1 , 2 ,3 ,4 ,5, 6
                        {
                            DotsAmount = 8;
                        }
                        else if (mByte == 32 || mByte == 33 || mByte == 38 || mByte == 39 || mByte == 40)//24 DotsAmount when m(decimal values) = 32, 33, 38 , 39, 40
                        {
                            DotsAmount = 24;
                        }
                        else
                        {
                            // Handle other cases or provide a default value for DotsAmount
                            DotsAmount = 8; // Default to 8 in case of other values
                        }
                        break;

                    default:
                        DotsAmount = 8;
                        // Handle unknown start sequence
                        break;


                }

                // Continue processing...
                byte byte1 = fileBytes[indexFirstSequence + currentSequence.Length];
                byte byte2 = fileBytes[indexFirstSequence + currentSequence.Length + 1];
                int numberOfBytesPerLine = byte1 + 256 * byte2;

                // Populate the matrix with bytes for the current line from fileBytes
                for (int j = 0; j < numberOfBytesPerLine && j < totalColumnsSent; j++)
                {
                    matrix[currentLineSent, j] = fileBytes[indexFirstSequence + currentSequence.Length + 2 + j];
                }

                // Move to the next line
                i = indexFirstSequence + currentSequence.Length + numberOfBytesPerLine + 4; // +4 = n1 + n2, +2 to skip CR&LF at the end of each line
                currentLineSent++;
            }

            return matrix;
        }

        // Creating the bit image that stores the pixels how they should be printed
        private Bitmap CreateBitmap(byte[,] matrix, int dotsAmount)
        {
            int totalRows = matrix.GetLength(0);
            int totalColumns = matrix.GetLength(1);
            int totalLines = totalRows * dotsAmount;

            Bitmap bitmap;

            if (dotsAmount == 24)
            {
                // For DotsAmount = 24, each column in the bitmap represents 3 bytes
                int bitmapColumns = totalColumns / 3;
                bitmap = new Bitmap(bitmapColumns, totalLines);
            }
            else
            {
                // For other DotsAmount values, each column in the bitmap represents 1 byte
                bitmap = new Bitmap(totalColumns, totalLines);
            }

            for (int i = 0; i < totalLines - dotsAmount; i = i + dotsAmount)
            {
                for (int j = 0; j < totalColumns; j++)
                {
                    byte value = matrix[i / dotsAmount, j];

                    for (int k = 0; k < dotsAmount; k++)
                    {
                        Color pixelColor = (value & (1 << (7 - k))) != 0 ? Color.Black : Color.White;
                        int pixelX = j;

                        // Adjust the pixelX for DotsAmount = 24
                        if (dotsAmount == 24)
                        {
                            pixelX = j + j / 3;
                        }

                        int pixelY = i + k;

                        if (pixelX < bitmap.Width && pixelY < bitmap.Height)
                        {
                            bitmap.SetPixel(pixelX, pixelY, pixelColor);
                        }
                    }
                }
            }

            return bitmap;
        }
    }
}
