namespace M3U8_GETTER
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.startDownload = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.DownloadProgress = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.videoTypeMP4 = new System.Windows.Forms.RadioButton();
            this.videoTypeMKV = new System.Windows.Forms.RadioButton();
            this.videoTypeTS = new System.Windows.Forms.RadioButton();
            this.downloadPath = new System.Windows.Forms.TextBox();
            this.changePathBtn = new System.Windows.Forms.Button();
            this.openPathBtn = new System.Windows.Forms.Button();
            this.exitApp = new System.Windows.Forms.Button();
            this.changeFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.makeVideo = new System.Windows.Forms.Button();
            this.videoTypeFLV = new System.Windows.Forms.RadioButton();
            this.url = new System.Windows.Forms.TextBox();
            this.regFileTypeBtn = new System.Windows.Forms.Button();
            this.chkAutoMakeDir = new System.Windows.Forms.CheckBox();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // logBox
            // 
            this.logBox.HideSelection = false;
            this.logBox.Location = new System.Drawing.Point(311, 12);
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(535, 290);
            this.logBox.TabIndex = 0;
            this.logBox.Text = "";
            // 
            // startDownload
            // 
            this.startDownload.Location = new System.Drawing.Point(11, 271);
            this.startDownload.Name = "startDownload";
            this.startDownload.Size = new System.Drawing.Size(85, 56);
            this.startDownload.TabIndex = 2;
            this.startDownload.Text = "开始下载";
            this.startDownload.UseVisualStyleBackColor = true;
            this.startDownload.Click += new System.EventHandler(this.startDownload_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(11, 110);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(293, 12);
            this.infoLabel.TabIndex = 4;
            this.infoLabel.Text = "请在上方粘贴m3u8文件网址，然后点击\"开始下载\"按钮";
            // 
            // DownloadProgress
            // 
            this.DownloadProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DownloadProgress.Location = new System.Drawing.Point(0, 336);
            this.DownloadProgress.Margin = new System.Windows.Forms.Padding(0);
            this.DownloadProgress.Name = "DownloadProgress";
            this.DownloadProgress.Size = new System.Drawing.Size(857, 12);
            this.DownloadProgress.Step = 1;
            this.DownloadProgress.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "生成的文件格式：";
            // 
            // videoTypeMP4
            // 
            this.videoTypeMP4.AutoSize = true;
            this.videoTypeMP4.Checked = true;
            this.videoTypeMP4.Location = new System.Drawing.Point(117, 136);
            this.videoTypeMP4.Name = "videoTypeMP4";
            this.videoTypeMP4.Size = new System.Drawing.Size(41, 16);
            this.videoTypeMP4.TabIndex = 7;
            this.videoTypeMP4.TabStop = true;
            this.videoTypeMP4.Text = "MP4";
            this.videoTypeMP4.UseVisualStyleBackColor = true;
            // 
            // videoTypeMKV
            // 
            this.videoTypeMKV.AutoSize = true;
            this.videoTypeMKV.Location = new System.Drawing.Point(167, 136);
            this.videoTypeMKV.Name = "videoTypeMKV";
            this.videoTypeMKV.Size = new System.Drawing.Size(41, 16);
            this.videoTypeMKV.TabIndex = 8;
            this.videoTypeMKV.Text = "MKV";
            this.videoTypeMKV.UseVisualStyleBackColor = true;
            // 
            // videoTypeTS
            // 
            this.videoTypeTS.AutoSize = true;
            this.videoTypeTS.Location = new System.Drawing.Point(218, 136);
            this.videoTypeTS.Name = "videoTypeTS";
            this.videoTypeTS.Size = new System.Drawing.Size(35, 16);
            this.videoTypeTS.TabIndex = 9;
            this.videoTypeTS.Text = "TS";
            this.videoTypeTS.UseVisualStyleBackColor = true;
            // 
            // downloadPath
            // 
            this.downloadPath.Location = new System.Drawing.Point(11, 163);
            this.downloadPath.Name = "downloadPath";
            this.downloadPath.Size = new System.Drawing.Size(291, 21);
            this.downloadPath.TabIndex = 10;
            // 
            // changePathBtn
            // 
            this.changePathBtn.Location = new System.Drawing.Point(11, 193);
            this.changePathBtn.Name = "changePathBtn";
            this.changePathBtn.Size = new System.Drawing.Size(140, 28);
            this.changePathBtn.TabIndex = 11;
            this.changePathBtn.Text = "更改下载路径";
            this.changePathBtn.UseVisualStyleBackColor = true;
            this.changePathBtn.Click += new System.EventHandler(this.changePathBtn_Click);
            // 
            // openPathBtn
            // 
            this.openPathBtn.Location = new System.Drawing.Point(164, 193);
            this.openPathBtn.Name = "openPathBtn";
            this.openPathBtn.Size = new System.Drawing.Size(140, 28);
            this.openPathBtn.TabIndex = 12;
            this.openPathBtn.Text = "打开下载目录";
            this.openPathBtn.UseVisualStyleBackColor = true;
            this.openPathBtn.Click += new System.EventHandler(this.openPathBtn_Click);
            // 
            // exitApp
            // 
            this.exitApp.Location = new System.Drawing.Point(219, 271);
            this.exitApp.Name = "exitApp";
            this.exitApp.Size = new System.Drawing.Size(85, 56);
            this.exitApp.TabIndex = 13;
            this.exitApp.Text = "退出";
            this.exitApp.UseVisualStyleBackColor = true;
            this.exitApp.Click += new System.EventHandler(this.exitApp_Click);
            // 
            // makeVideo
            // 
            this.makeVideo.Location = new System.Drawing.Point(117, 271);
            this.makeVideo.Name = "makeVideo";
            this.makeVideo.Size = new System.Drawing.Size(85, 56);
            this.makeVideo.TabIndex = 14;
            this.makeVideo.Text = "直接处理视频";
            this.makeVideo.UseVisualStyleBackColor = true;
            this.makeVideo.Click += new System.EventHandler(this.makeVideo_Click);
            // 
            // videoTypeFLV
            // 
            this.videoTypeFLV.AutoSize = true;
            this.videoTypeFLV.Location = new System.Drawing.Point(263, 136);
            this.videoTypeFLV.Name = "videoTypeFLV";
            this.videoTypeFLV.Size = new System.Drawing.Size(41, 16);
            this.videoTypeFLV.TabIndex = 15;
            this.videoTypeFLV.Text = "FLV";
            this.videoTypeFLV.UseVisualStyleBackColor = true;
            // 
            // url
            // 
            this.url.AllowDrop = true;
            this.url.Location = new System.Drawing.Point(11, 28);
            this.url.Multiline = true;
            this.url.Name = "url";
            this.url.Size = new System.Drawing.Size(290, 74);
            this.url.TabIndex = 16;
            this.url.TextChanged += new System.EventHandler(this.url_TextChanged);
            this.url.DragDrop += new System.Windows.Forms.DragEventHandler(this.url_DragDrop);
            this.url.DragEnter += new System.Windows.Forms.DragEventHandler(this.url_DragEnter);
            this.url.DragOver += new System.Windows.Forms.DragEventHandler(this.url_DragOver);
            this.url.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.url_KeyPress);
            // 
            // regFileTypeBtn
            // 
            this.regFileTypeBtn.Location = new System.Drawing.Point(164, 233);
            this.regFileTypeBtn.Name = "regFileTypeBtn";
            this.regFileTypeBtn.Size = new System.Drawing.Size(140, 28);
            this.regFileTypeBtn.TabIndex = 17;
            this.regFileTypeBtn.Text = "注册关联文件";
            this.regFileTypeBtn.UseVisualStyleBackColor = true;
            this.regFileTypeBtn.Click += new System.EventHandler(this.regFileTypeBtn_Click);
            // 
            // chkAutoMakeDir
            // 
            this.chkAutoMakeDir.AutoSize = true;
            this.chkAutoMakeDir.Location = new System.Drawing.Point(11, 240);
            this.chkAutoMakeDir.Name = "chkAutoMakeDir";
            this.chkAutoMakeDir.Size = new System.Drawing.Size(132, 16);
            this.chkAutoMakeDir.TabIndex = 18;
            this.chkAutoMakeDir.Text = "自动递增创建文件夹";
            this.chkAutoMakeDir.UseVisualStyleBackColor = true;
            // 
            // linkLabel
            // 
            this.linkLabel.AutoSize = true;
            this.linkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.linkLabel.Location = new System.Drawing.Point(12, 9);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(221, 12);
            this.linkLabel.TabIndex = 19;
            this.linkLabel.TabStop = true;
            this.linkLabel.Text = "wujinhai(940390@qq.com) 点击检查更新";
            this.linkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 348);
            this.Controls.Add(this.linkLabel);
            this.Controls.Add(this.chkAutoMakeDir);
            this.Controls.Add(this.regFileTypeBtn);
            this.Controls.Add(this.url);
            this.Controls.Add(this.videoTypeFLV);
            this.Controls.Add(this.makeVideo);
            this.Controls.Add(this.exitApp);
            this.Controls.Add(this.openPathBtn);
            this.Controls.Add(this.changePathBtn);
            this.Controls.Add(this.downloadPath);
            this.Controls.Add(this.videoTypeTS);
            this.Controls.Add(this.videoTypeMKV);
            this.Controls.Add(this.videoTypeMP4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DownloadProgress);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.startDownload);
            this.Controls.Add(this.logBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "M3U8 GETTER";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox logBox;
        private System.Windows.Forms.Button startDownload;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.ProgressBar DownloadProgress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton videoTypeMP4;
        private System.Windows.Forms.RadioButton videoTypeMKV;
        private System.Windows.Forms.RadioButton videoTypeTS;
        private System.Windows.Forms.TextBox downloadPath;
        private System.Windows.Forms.Button changePathBtn;
        private System.Windows.Forms.Button openPathBtn;
        private System.Windows.Forms.Button exitApp;
        private System.Windows.Forms.FolderBrowserDialog changeFolderDialog;
        private System.Windows.Forms.Button makeVideo;
        private System.Windows.Forms.RadioButton videoTypeFLV;
        private System.Windows.Forms.TextBox url;
        private System.Windows.Forms.Button regFileTypeBtn;
        private System.Windows.Forms.CheckBox chkAutoMakeDir;
        private System.Windows.Forms.LinkLabel linkLabel;
    }
}

