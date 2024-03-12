using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LPTtoUSB_Converter
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button_FileDirectoryBrowse = new Button();
            comboBox_COMport = new ComboBox();
            label_Folder = new Label();
            textBox_FolderFiles = new TextBox();
            label_COMport = new Label();
            button_Set = new Button();
            label_ConnectionTitle = new Label();
            label_Title = new Label();
            notifyIcon_Convertor = new NotifyIcon(components);
            contextMenuStrip_notifyIcon = new ContextMenuStrip(components);
            toolStripMenuItem_endProgram = new ToolStripMenuItem();
            toolStripMenuItem_about = new ToolStripMenuItem();
            folderBrowserDialog_save = new FolderBrowserDialog();
            checkBox_OpenWordfile = new CheckBox();
            button_refreshCOM = new Button();
            tableLayoutPanel_Form = new TableLayoutPanel();
            panel_Title = new Panel();
            panel_OpenCheckox = new Panel();
            splitContainer_mainForm = new SplitContainer();
            tableLayoutPanel_Monitor = new TableLayoutPanel();
            richTextBox_Monitor = new RichTextBox();
            button_StartReading = new Button();
            button_PausePreview = new Button();
            button_Continue = new Button();
            button_StopReading = new Button();
            label_ConnectionStatus = new Label();
            contextMenuStrip_notifyIcon.SuspendLayout();
            tableLayoutPanel_Form.SuspendLayout();
            panel_Title.SuspendLayout();
            panel_OpenCheckox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer_mainForm).BeginInit();
            splitContainer_mainForm.Panel1.SuspendLayout();
            splitContainer_mainForm.Panel2.SuspendLayout();
            splitContainer_mainForm.SuspendLayout();
            tableLayoutPanel_Monitor.SuspendLayout();
            SuspendLayout();
            // 
            // button_FileDirectoryBrowse
            // 
            button_FileDirectoryBrowse.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button_FileDirectoryBrowse.Font = new Font("Microsoft Tai Le", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            button_FileDirectoryBrowse.Location = new Point(253, 80);
            button_FileDirectoryBrowse.Margin = new Padding(1);
            button_FileDirectoryBrowse.Name = "button_FileDirectoryBrowse";
            button_FileDirectoryBrowse.Size = new Size(30, 26);
            button_FileDirectoryBrowse.TabIndex = 1;
            button_FileDirectoryBrowse.Text = "...";
            button_FileDirectoryBrowse.UseVisualStyleBackColor = true;
            button_FileDirectoryBrowse.Click += button_FileDirectoryBrowse_Click;
            // 
            // comboBox_COMport
            // 
            tableLayoutPanel_Form.SetColumnSpan(comboBox_COMport, 3);
            comboBox_COMport.Dock = DockStyle.Fill;
            comboBox_COMport.Font = new Font("Microsoft Tai Le", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            comboBox_COMport.FormattingEnabled = true;
            comboBox_COMport.Location = new Point(61, 135);
            comboBox_COMport.Margin = new Padding(1);
            comboBox_COMport.Name = "comboBox_COMport";
            comboBox_COMport.Size = new Size(190, 27);
            comboBox_COMport.TabIndex = 2;
            // 
            // label_Folder
            // 
            label_Folder.AutoSize = true;
            label_Folder.Font = new Font("Microsoft Tai Le", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            label_Folder.Location = new Point(5, 80);
            label_Folder.Margin = new Padding(1);
            label_Folder.Name = "label_Folder";
            label_Folder.Size = new Size(54, 19);
            label_Folder.TabIndex = 3;
            label_Folder.Text = "Folder:";
            label_Folder.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox_FolderFiles
            // 
            textBox_FolderFiles.BackColor = SystemColors.HighlightText;
            tableLayoutPanel_Form.SetColumnSpan(textBox_FolderFiles, 3);
            textBox_FolderFiles.Dock = DockStyle.Fill;
            textBox_FolderFiles.Font = new Font("Microsoft Tai Le", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            textBox_FolderFiles.Location = new Point(61, 80);
            textBox_FolderFiles.Margin = new Padding(1);
            textBox_FolderFiles.Name = "textBox_FolderFiles";
            textBox_FolderFiles.Size = new Size(190, 27);
            textBox_FolderFiles.TabIndex = 4;
            // 
            // label_COMport
            // 
            label_COMport.AutoSize = true;
            label_COMport.Font = new Font("Microsoft Tai Le", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            label_COMport.Location = new Point(5, 135);
            label_COMport.Margin = new Padding(1);
            label_COMport.Name = "label_COMport";
            label_COMport.Size = new Size(45, 19);
            label_COMport.TabIndex = 5;
            label_COMport.Text = "COM:";
            label_COMport.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button_Set
            // 
            button_Set.AutoSize = true;
            button_Set.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel_Form.SetColumnSpan(button_Set, 3);
            button_Set.Dock = DockStyle.Fill;
            button_Set.Font = new Font("Microsoft Tai Le", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            button_Set.Location = new Point(64, 262);
            button_Set.Margin = new Padding(4);
            button_Set.Name = "button_Set";
            button_Set.Size = new Size(184, 34);
            button_Set.TabIndex = 6;
            button_Set.Text = "Set";
            button_Set.UseVisualStyleBackColor = true;
            button_Set.Click += button_Set_Click;
            // 
            // label_ConnectionTitle
            // 
            label_ConnectionTitle.AutoSize = true;
            label_ConnectionTitle.Dock = DockStyle.Fill;
            label_ConnectionTitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label_ConnectionTitle.Location = new Point(474, 1);
            label_ConnectionTitle.Margin = new Padding(1);
            label_ConnectionTitle.Name = "label_ConnectionTitle";
            label_ConnectionTitle.Size = new Size(61, 43);
            label_ConnectionTitle.TabIndex = 8;
            label_ConnectionTitle.Text = "Status:";
            label_ConnectionTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_Title
            // 
            label_Title.AllowDrop = true;
            label_Title.Anchor = AnchorStyles.None;
            label_Title.AutoSize = true;
            label_Title.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label_Title.Location = new Point(57, 20);
            label_Title.Margin = new Padding(4, 0, 4, 0);
            label_Title.Name = "label_Title";
            label_Title.Padding = new Padding(4);
            label_Title.Size = new Size(166, 29);
            label_Title.TabIndex = 0;
            label_Title.Text = "LPT to USB Settings";
            label_Title.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // notifyIcon_Convertor
            // 
            notifyIcon_Convertor.ContextMenuStrip = contextMenuStrip_notifyIcon;
            notifyIcon_Convertor.Icon = (Icon)resources.GetObject("notifyIcon_Convertor.Icon");
            notifyIcon_Convertor.Text = "notifyIcon_Tool";
            notifyIcon_Convertor.Visible = true;
            // 
            // contextMenuStrip_notifyIcon
            // 
            contextMenuStrip_notifyIcon.ImageScalingSize = new Size(20, 20);
            contextMenuStrip_notifyIcon.Items.AddRange(new ToolStripItem[] { toolStripMenuItem_endProgram, toolStripMenuItem_about });
            contextMenuStrip_notifyIcon.Name = "contextMenuStrip1";
            contextMenuStrip_notifyIcon.Size = new Size(144, 48);
            // 
            // toolStripMenuItem_endProgram
            // 
            toolStripMenuItem_endProgram.Name = "toolStripMenuItem_endProgram";
            toolStripMenuItem_endProgram.Size = new Size(143, 22);
            toolStripMenuItem_endProgram.Text = "End program";
            toolStripMenuItem_endProgram.Click += toolStripMenuItem_endProgram_Click;
            // 
            // toolStripMenuItem_about
            // 
            toolStripMenuItem_about.Name = "toolStripMenuItem_about";
            toolStripMenuItem_about.Size = new Size(143, 22);
            toolStripMenuItem_about.Text = "About";
            toolStripMenuItem_about.Click += toolStripMenuItem_about_Click;
            // 
            // checkBox_OpenWordfile
            // 
            checkBox_OpenWordfile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            checkBox_OpenWordfile.AutoSize = true;
            checkBox_OpenWordfile.Font = new Font("Microsoft Tai Le", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            checkBox_OpenWordfile.Location = new Point(46, 12);
            checkBox_OpenWordfile.Margin = new Padding(1);
            checkBox_OpenWordfile.Name = "checkBox_OpenWordfile";
            checkBox_OpenWordfile.Size = new Size(198, 23);
            checkBox_OpenWordfile.TabIndex = 9;
            checkBox_OpenWordfile.Text = "Open file after converting";
            checkBox_OpenWordfile.TextImageRelation = TextImageRelation.TextBeforeImage;
            checkBox_OpenWordfile.UseVisualStyleBackColor = true;
            // 
            // button_refreshCOM
            // 
            button_refreshCOM.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button_refreshCOM.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            button_refreshCOM.Image = Properties.Resources.Refresh_image1;
            button_refreshCOM.Location = new Point(253, 135);
            button_refreshCOM.Margin = new Padding(1);
            button_refreshCOM.Name = "button_refreshCOM";
            button_refreshCOM.Size = new Size(30, 27);
            button_refreshCOM.TabIndex = 12;
            button_refreshCOM.UseVisualStyleBackColor = true;
            button_refreshCOM.Click += button_refreshCOM_Click;
            // 
            // tableLayoutPanel_Form
            // 
            tableLayoutPanel_Form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel_Form.ColumnCount = 5;
            tableLayoutPanel_Form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 56F));
            tableLayoutPanel_Form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30.1671581F));
            tableLayoutPanel_Form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 39.6656837F));
            tableLayoutPanel_Form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30.1671581F));
            tableLayoutPanel_Form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 37F));
            tableLayoutPanel_Form.Controls.Add(comboBox_COMport, 1, 2);
            tableLayoutPanel_Form.Controls.Add(button_refreshCOM, 4, 2);
            tableLayoutPanel_Form.Controls.Add(panel_Title, 0, 0);
            tableLayoutPanel_Form.Controls.Add(textBox_FolderFiles, 1, 1);
            tableLayoutPanel_Form.Controls.Add(label_COMport, 0, 2);
            tableLayoutPanel_Form.Controls.Add(button_FileDirectoryBrowse, 4, 1);
            tableLayoutPanel_Form.Controls.Add(label_Folder, 0, 1);
            tableLayoutPanel_Form.Controls.Add(panel_OpenCheckox, 0, 3);
            tableLayoutPanel_Form.Controls.Add(button_Set, 1, 4);
            tableLayoutPanel_Form.Dock = DockStyle.Fill;
            tableLayoutPanel_Form.Location = new Point(0, 0);
            tableLayoutPanel_Form.Margin = new Padding(4);
            tableLayoutPanel_Form.Name = "tableLayoutPanel_Form";
            tableLayoutPanel_Form.Padding = new Padding(4);
            tableLayoutPanel_Form.RowCount = 6;
            tableLayoutPanel_Form.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tableLayoutPanel_Form.RowStyles.Add(new RowStyle(SizeType.Percent, 20.56403F));
            tableLayoutPanel_Form.RowStyles.Add(new RowStyle(SizeType.Percent, 24.34457F));
            tableLayoutPanel_Form.RowStyles.Add(new RowStyle(SizeType.Percent, 22.0973778F));
            tableLayoutPanel_Form.RowStyles.Add(new RowStyle(SizeType.Percent, 15.7303371F));
            tableLayoutPanel_Form.RowStyles.Add(new RowStyle(SizeType.Percent, 16.8539333F));
            tableLayoutPanel_Form.Size = new Size(295, 350);
            tableLayoutPanel_Form.TabIndex = 13;
            // 
            // panel_Title
            // 
            panel_Title.AutoSize = true;
            panel_Title.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel_Form.SetColumnSpan(panel_Title, 5);
            panel_Title.Controls.Add(label_Title);
            panel_Title.Dock = DockStyle.Fill;
            panel_Title.Location = new Point(7, 7);
            panel_Title.Name = "panel_Title";
            panel_Title.Size = new Size(281, 69);
            panel_Title.TabIndex = 0;
            // 
            // panel_OpenCheckox
            // 
            tableLayoutPanel_Form.SetColumnSpan(panel_OpenCheckox, 5);
            panel_OpenCheckox.Controls.Add(checkBox_OpenWordfile);
            panel_OpenCheckox.Dock = DockStyle.Fill;
            panel_OpenCheckox.Location = new Point(7, 202);
            panel_OpenCheckox.Name = "panel_OpenCheckox";
            panel_OpenCheckox.Size = new Size(281, 53);
            panel_OpenCheckox.TabIndex = 13;
            // 
            // splitContainer_mainForm
            // 
            splitContainer_mainForm.Dock = DockStyle.Fill;
            splitContainer_mainForm.Location = new Point(0, 0);
            splitContainer_mainForm.Name = "splitContainer_mainForm";
            // 
            // splitContainer_mainForm.Panel1
            // 
            splitContainer_mainForm.Panel1.Controls.Add(tableLayoutPanel_Form);
            // 
            // splitContainer_mainForm.Panel2
            // 
            splitContainer_mainForm.Panel2.Controls.Add(tableLayoutPanel_Monitor);
            splitContainer_mainForm.Size = new Size(940, 350);
            splitContainer_mainForm.SplitterDistance = 295;
            splitContainer_mainForm.TabIndex = 14;
            // 
            // tableLayoutPanel_Monitor
            // 
            tableLayoutPanel_Monitor.AutoSize = true;
            tableLayoutPanel_Monitor.ColumnCount = 6;
            tableLayoutPanel_Monitor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.1684866F));
            tableLayoutPanel_Monitor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15.9126368F));
            tableLayoutPanel_Monitor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.1170044F));
            tableLayoutPanel_Monitor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21.2168484F));
            tableLayoutPanel_Monitor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.9844F));
            tableLayoutPanel_Monitor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.22465F));
            tableLayoutPanel_Monitor.Controls.Add(richTextBox_Monitor, 0, 1);
            tableLayoutPanel_Monitor.Controls.Add(button_StartReading, 0, 0);
            tableLayoutPanel_Monitor.Controls.Add(button_PausePreview, 2, 0);
            tableLayoutPanel_Monitor.Controls.Add(button_Continue, 1, 0);
            tableLayoutPanel_Monitor.Controls.Add(button_StopReading, 3, 0);
            tableLayoutPanel_Monitor.Controls.Add(label_ConnectionTitle, 4, 0);
            tableLayoutPanel_Monitor.Controls.Add(label_ConnectionStatus, 5, 0);
            tableLayoutPanel_Monitor.Dock = DockStyle.Fill;
            tableLayoutPanel_Monitor.Location = new Point(0, 0);
            tableLayoutPanel_Monitor.Name = "tableLayoutPanel_Monitor";
            tableLayoutPanel_Monitor.RowCount = 2;
            tableLayoutPanel_Monitor.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel_Monitor.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel_Monitor.Size = new Size(641, 350);
            tableLayoutPanel_Monitor.TabIndex = 0;
            // 
            // richTextBox_Monitor
            // 
            richTextBox_Monitor.BackColor = SystemColors.ControlLightLight;
            richTextBox_Monitor.BorderStyle = BorderStyle.FixedSingle;
            tableLayoutPanel_Monitor.SetColumnSpan(richTextBox_Monitor, 6);
            richTextBox_Monitor.Dock = DockStyle.Fill;
            richTextBox_Monitor.Font = new Font("Courier New", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            richTextBox_Monitor.Location = new Point(4, 49);
            richTextBox_Monitor.Margin = new Padding(4);
            richTextBox_Monitor.Name = "richTextBox_Monitor";
            richTextBox_Monitor.ReadOnly = true;
            richTextBox_Monitor.Size = new Size(633, 297);
            richTextBox_Monitor.TabIndex = 2;
            richTextBox_Monitor.Text = "";
            // 
            // button_StartReading
            // 
            button_StartReading.AutoSize = true;
            button_StartReading.Dock = DockStyle.Fill;
            button_StartReading.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            button_StartReading.Location = new Point(4, 4);
            button_StartReading.Margin = new Padding(4);
            button_StartReading.Name = "button_StartReading";
            button_StartReading.Size = new Size(69, 37);
            button_StartReading.TabIndex = 0;
            button_StartReading.Text = "▶Start";
            button_StartReading.UseVisualStyleBackColor = true;
            button_StartReading.Click += button_StartReading_Click;
            // 
            // button_PausePreview
            // 
            button_PausePreview.AutoSize = true;
            button_PausePreview.Dock = DockStyle.Fill;
            button_PausePreview.Enabled = false;
            button_PausePreview.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            button_PausePreview.Location = new Point(182, 4);
            button_PausePreview.Margin = new Padding(4);
            button_PausePreview.Name = "button_PausePreview";
            button_PausePreview.Size = new Size(152, 37);
            button_PausePreview.TabIndex = 12;
            button_PausePreview.Text = "⏸Pause and Preview";
            button_PausePreview.UseVisualStyleBackColor = true;
            button_PausePreview.Click += button_PausePreview_Click;
            // 
            // button_Continue
            // 
            button_Continue.AutoSize = true;
            button_Continue.Dock = DockStyle.Fill;
            button_Continue.Enabled = false;
            button_Continue.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            button_Continue.Location = new Point(81, 4);
            button_Continue.Margin = new Padding(4);
            button_Continue.Name = "button_Continue";
            button_Continue.Size = new Size(93, 37);
            button_Continue.TabIndex = 13;
            button_Continue.Text = "▶Continue";
            button_Continue.UseVisualStyleBackColor = true;
            button_Continue.Click += button_Continue_Click;
            // 
            // button_StopReading
            // 
            button_StopReading.AutoSize = true;
            button_StopReading.Dock = DockStyle.Fill;
            button_StopReading.Enabled = false;
            button_StopReading.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            button_StopReading.Location = new Point(342, 4);
            button_StopReading.Margin = new Padding(4);
            button_StopReading.Name = "button_StopReading";
            button_StopReading.Size = new Size(127, 37);
            button_StopReading.TabIndex = 1;
            button_StopReading.Text = "⏹End and Save";
            button_StopReading.UseVisualStyleBackColor = true;
            button_StopReading.Click += button_StopReading_Click;
            // 
            // label_ConnectionStatus
            // 
            label_ConnectionStatus.AutoSize = true;
            label_ConnectionStatus.Dock = DockStyle.Fill;
            label_ConnectionStatus.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label_ConnectionStatus.ForeColor = SystemColors.ActiveBorder;
            label_ConnectionStatus.Location = new Point(537, 1);
            label_ConnectionStatus.Margin = new Padding(1);
            label_ConnectionStatus.Name = "label_ConnectionStatus";
            label_ConnectionStatus.Size = new Size(103, 43);
            label_ConnectionStatus.TabIndex = 7;
            label_ConnectionStatus.Text = "Disconnected";
            label_ConnectionStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(940, 350);
            Controls.Add(splitContainer_mainForm);
            Font = new Font("Microsoft Tai Le", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "LPT to USB Converter";
            Load += Form1_Load;
            contextMenuStrip_notifyIcon.ResumeLayout(false);
            tableLayoutPanel_Form.ResumeLayout(false);
            tableLayoutPanel_Form.PerformLayout();
            panel_Title.ResumeLayout(false);
            panel_Title.PerformLayout();
            panel_OpenCheckox.ResumeLayout(false);
            panel_OpenCheckox.PerformLayout();
            splitContainer_mainForm.Panel1.ResumeLayout(false);
            splitContainer_mainForm.Panel2.ResumeLayout(false);
            splitContainer_mainForm.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer_mainForm).EndInit();
            splitContainer_mainForm.ResumeLayout(false);
            tableLayoutPanel_Monitor.ResumeLayout(false);
            tableLayoutPanel_Monitor.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button button_FileDirectoryBrowse;
        private ComboBox comboBox_COMport;
        private Label label_Folder;
        private TextBox textBox_FolderFiles;
        private Label label_COMport;
        private Button button_Set;
        private Label label_ConnectionTitle;
        private Label label_Title;
        private NotifyIcon notifyIcon_Convertor;
        private FolderBrowserDialog folderBrowserDialog_save;
        private CheckBox checkBox_OpenWordfile;
        private ContextMenuStrip contextMenuStrip_notifyIcon;
        private ToolStripMenuItem toolStripMenuItem_endProgram;
        private ToolStripMenuItem toolStripMenuItem_about;
        private TableLayoutPanel tableLayoutPanel_Form;
        private Panel panel_Title;
        private Button button_refreshCOM;
        private SplitContainer splitContainer_mainForm;
        private TableLayoutPanel tableLayoutPanel_Monitor;
        private Button button_StartReading;
        private Button button_StopReading;
        private RichTextBox richTextBox_Monitor;
        private Button button_PausePreview;
        private Panel panel_OpenCheckox;
        private Button button_Continue;
        private Label label_ConnectionStatus;
    }
}