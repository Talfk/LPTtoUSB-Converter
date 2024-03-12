using System.Management;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.Configuration;
using LPTtoUSB_Converter;

namespace LPTtoUSB_Converter
{
    public partial class Form1 : Form
    {
        public string SELECTED_PortName;
        public string SELECTED_filesDirectory;


        SerialPort serialPort = new SerialPort();//serialPort is public so it would not be resetted each time it's approached and it's status can be followed

        //data transfer the settings between  GUI & XML file
        Data data = new Data();

        // Define a variable for your XML file name
        public string xmlFileName = "LPTtoUSB_settings.xml";


        // Get the path to the user's Documents folder
        public string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


        public MemoryStream memoryStream = new MemoryStream();



        //Set buffer size to read USB2.0 data, standart = 64
        const int bufferSize = 64;
        byte[] buffer = new byte[bufferSize];


        public Form1()
        {
            InitializeComponent();

            // Attach event handlers to the NotifyIcon control
            //notifyIcon_Convertor.MouseClick += notifyIcon_Convertor_MouseClick;
            notifyIcon_Convertor.DoubleClick += NotifyIcon_DoubleClick;

            // Set the tooltip text
            notifyIcon_Convertor.Text = "LPT to USB Converter";

            // Show the icon in the system tray
            notifyIcon_Convertor.Visible = true;

            // Hook the FormClosing event to handle minimizing to the system tray
            FormClosing += Form1_FormClosing;

        }


        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            // Handle double-click action (e.g., show the main form)
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Combine the Documents folder path and the XML file name
            string xmlFilePath;
            xmlFilePath = Path.Combine(documentsFolder, xmlFileName);

            if (File.Exists(xmlFilePath))//Checks if the data.xml file exists
            {
                //Updating the file directory and combobox of com port, from the Xml file
                data = XmlManager.XmlDataReader(xmlFilePath);

                if (Directory.Exists(data.Folder))
                {
                    textBox_FolderFiles.Text = data.Folder;
                }

                else//Uploading previously chosen folder
                {
                    SetDefaultFolderPath();
                }
                //Updating last com port data
                comboBox_COMport.Text = data.ComPort;
            }
            else
            {
                SetDefaultFolderPath();
            }

        }




        //Opening folder browsing option to select the folder for the saving of converted files
        private void button_FileDirectoryBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog_save.ShowDialog();//the user select desired path
            textBox_FolderFiles.Text = folderBrowserDialog_save.SelectedPath; //desired path is copied to the folder textBox
        }

        //Making a default folder to choose the files in path C:\Ducuments\LPT to USB files
        private void SetDefaultFolderPath()
        {
            // Determine the path to the "LPT to USB files" folder in the Documents directory
            string defaultFolderPath = Path.Combine(documentsFolder, "LPT to USB files");

            // Check if the folder exists, and create it if it doesn't
            if (!Directory.Exists(defaultFolderPath))
            {
                Directory.CreateDirectory(defaultFolderPath);
            }

            // Set the default folder path in the textBox_FileDirectory control
            textBox_FolderFiles.Text = defaultFolderPath;
        }



        //Updating the availble COM Ports
        private void PopulateComPortComboBox()
        {
            //Looking for COM Ports connected devices
            string query = "SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            using (var Searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0"))
            {
                foreach (var queryObj in searcher.Get())
                {
                    // Check if the device is a COM port.
                    if (queryObj["Caption"] != null && queryObj["Caption"].ToString().Contains("(COM"))
                    {
                        // Extract the COM port name.
                        var portName = queryObj["Caption"];

                        comboBox_COMport.Items.Add(portName);//Adds port name to list
                    }
                }
            }

        }


        public object XmlFileManager { get; set; }
        //Saving new data in XML file 
        private void Updating_Xml_File()
        {
            try
            {
                data.Folder = textBox_FolderFiles.Text;
                data.ComPort = comboBox_COMport.Text;


                // Combine the Documents folder path and the XML file name
                string xmlFilePath = Path.Combine(documentsFolder, xmlFileName);
                XmlManager.XmlDataWriter(data, xmlFilePath);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.ToString());
            }
        }


        //button_Set_Click saves the Folder and COM Port setting in the Xml file "data.xml"
        private void button_Set_Click(object sender, EventArgs e)
        {
            // Connecting_Convertor();
            Updating_Xml_File();
        }



        //Connecting the selected com port to the convertor
        public async void Connecting_Convertor()
        {
            //Checking that the folder directory exists
            if (!Directory.Exists(textBox_FolderFiles.Text))//if does'nt exist show a message 
            {
                MessageBox.Show("Folder does not exist. \nChoose another folder or make a new folder");
                return;// return to form
            }

            string selectedDevice = comboBox_COMport.Text;

            if (!string.IsNullOrEmpty(selectedDevice))
            {
                int startIndex = selectedDevice.LastIndexOf("(COM", StringComparison.OrdinalIgnoreCase);
                if (startIndex != -1)
                {
                    string selectedPortName = selectedDevice.Substring(startIndex);
                    selectedPortName = selectedPortName.Trim('(', ')');

                    string pdfDirectory = textBox_FolderFiles.Text; // Get the PDF directory from the text box

                    //Copy to for using in other methods
                    SELECTED_PortName = selectedPortName;
                    SELECTED_filesDirectory = pdfDirectory;


                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                    // Use async/await to read data in a non-blocking way
                    await Task.Run(() => ReaddataThread(selectedPortName, pdfDirectory));

                }

                else
                {
                    MessageBox.Show("The selected item does not contain a valid COM port.", "Invalid COM Port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Please select a COM port from the list.", "No COM Port Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void UpdateStatusLabel(string text)
        {
            if (label_ConnectionStatus.InvokeRequired)
            {
                // If not on the UI thread, invoke the method on the UI thread
                label_ConnectionStatus.Invoke(new Action(() => label_ConnectionStatus.Text = text));
                if (text == "Connected")
                    label_ConnectionStatus.ForeColor = Color.Green;
                else
                    label_ConnectionStatus.ForeColor = Color.Red;
            }
            else
            {
                // If already on the UI thread, directly update the control
                label_ConnectionStatus.Text = text;
                if (text == "Connected")
                    label_ConnectionStatus.ForeColor = Color.Green;
                else
                    label_ConnectionStatus.ForeColor = Color.Red;
            }
        }


        private void button_StartReading_Click(object sender, EventArgs e)
        {
            // Disable the Start button and enable the Stop button
            button_StartReading.Enabled = false;
            button_Continue.Enabled = false;
            button_PausePreview.Enabled = true;
            button_StopReading.Enabled = true;


            // Starting to listen to com port for Receiving
            Connecting_Convertor();

        }

        private void button_Continue_Click(object sender, EventArgs e)
        {
            button_StartReading.Enabled = false;
            button_Continue.Enabled = false;
            button_PausePreview.Enabled = true;
            button_StopReading.Enabled = true;

            // Resume to listen to com port for Receiving
            Connecting_Convertor();

        }



        private void button_PausePreview_Click(object sender, EventArgs e)
        {
            //En/Disabling the right buttons and change status accordingly
            button_StartReading.Enabled = false;//already disabled, but to make sure
            button_Continue.Enabled = true;
            button_PausePreview.Enabled = false;
            button_StopReading.Enabled = false;

            //Pausing data recienig
            //Closing the serial port
            serialPort.Close();
            // Update the UI label safely
            UpdateStatusLabel("Disonnected");


            bool isPreview = true;
            bool openatEnd = checkBox_OpenWordfile.Checked;
            SavingData(isPreview, openatEnd);//Making the word document from the data recieved untill now

            // Open the generated Word document

        }


        async private void button_StopReading_Click(object sender, EventArgs e)
        {
            // Enable the Start button and disable the Stop button
            button_StartReading.Enabled = true;
            button_Continue.Enabled = false;
            button_PausePreview.Enabled = false;
            button_StopReading.Enabled = false;


            //Closing the serial port
            serialPort.Close();
            // Update the UI label safely
            UpdateStatusLabel("Disonnected");

            richTextBox_Monitor.Clear();

            //Meaning no more data is recieved to this file
            bool isPreview = false;
            bool openatEnd = checkBox_OpenWordfile.Checked;
            SavingData(isPreview, openatEnd);

        }




        public void SavingData(bool isPreview, bool openatEnd)
        {
            // Register the code pages provider
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Specify the main directory where you want to save the information
            string mainDirectory = SELECTED_filesDirectory;

            // Create an instance of BitImageProcessor
            BitImageProcessor bitImageProcessor = new BitImageProcessor();

            // Create an instance of TextFileHandler
            FilesMaker textFileHandler = new FilesMaker(mainDirectory);

            // Convert memoryStream to byte array, memoryStream contains all the bytes sent from the old computer
            byte[] txtdata = memoryStream.ToArray();

            // Create an instance of DataBlockProcessor
            DataBlockProcessor dataBlockProcessor = new DataBlockProcessor();

            // Process the byte array through DataBlockProcessor to get a list of DataBlocks
            List<DataBlock> dataBlocks = dataBlockProcessor.ProcessDataBlocks(txtdata);



            // Process the text file using TextFileHandler with the extracted dataBlocks
            textFileHandler.ProcessTextFile(txtdata, dataBlocks, bitImageProcessor, isPreview, openatEnd);

            //Checking if the document has been done reciving, if not memryStream keeps recing data
            if (isPreview == false)
            {
                // Clear the memory stream for the next set of data
                memoryStream = new MemoryStream();
            }

        }




        //Thread is waiting for a file to be sent threogh the usb port,
        //then it receive all data and save it as a pcl file
        public void ReaddataThread(string portName, string pdfDirectory)
        {

            using (serialPort)
            {
                try
                {
                    //Opening serial port of selected COM port
                    if (serialPort.IsOpen == false)
                    {
                        serialPort.PortName = portName;
                        serialPort.BaudRate = 9600;
                        serialPort.Open();
                        // Update the UI label safely
                        UpdateStatusLabel("Connected");
                    }

                    int bytesRead;


                    while (true)
                    {
                        bytesRead = serialPort.Read(buffer, 0, buffer.Length);
                        if (bytesRead > 0)
                        {
                            memoryStream.Write(buffer, 0, bytesRead);

                            string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                            // Update the UI with the received data
                            UpdateRichTextBox(receivedData);

                            Thread.Sleep(100); // Add a small delay between each read operation

                        }
                    }



                }
                catch (Exception ex)
                {
                    // MessageBox.Show("Error " + ex.ToString());

                    // Update the UI label safely within the catch block
                    UpdateStatusLabel("Disconnected");

                    serialPort.Close();

                }
            }
        }


        private void UpdateRichTextBox(string data)
        {
            // Use Invoke to safely update UI from a different thread
            if (richTextBox_Monitor.InvokeRequired)
            {
                richTextBox_Monitor.Invoke(new Action(() => richTextBox_Monitor.AppendText(data)));
            }
            else
            {
                richTextBox_Monitor.AppendText(data);
            }
        }






        //So the user can hide the GUI also when it is working in the background
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prevent the form from being closed directly
            e.Cancel = true;

            // Minimize the form to the system tray, so the program would not stop when the user press "Form close"
            this.WindowState = FormWindowState.Minimized;//Minimize the form
            this.Hide();//Hide the form from the user. form can be opened throgh the notifyIcon

        }


        //Ending the program from notifyIcon
        private void toolStripMenuItem_endProgram_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void toolStripMenuItem_about_Click(object sender, EventArgs e)
        {
            // Get the path to the application's folder
            string appFolder = Application.StartupPath;

            // Combine the folder path with the "About.txt" file name
            string aboutFilePath = Path.Combine(appFolder, "LPTtoUSB About.txt");

            // Check if the file exists before attempting to open it
            if (File.Exists(aboutFilePath))
            {
                // Open the text file with the default text editor
                Process.Start(new ProcessStartInfo(aboutFilePath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show(aboutFilePath);
                // Display a message if the file doesn't exist
                MessageBox.Show("The 'About.txt' file could not be found in the application folder.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_refreshCOM_Click(object sender, EventArgs e)
        {
            comboBox_COMport.Text = "";
            comboBox_COMport.Items.Clear();// Clear the combo box
            PopulateComPortComboBox();//Filling the comboBox lisst with availble com ports
        }


    }
}
