using M3U8_GETTER.classes;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.DownloadProgress = new System.Windows.Forms.ProgressBar();
            this.changeFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tctlMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnAddToList = new System.Windows.Forms.Button();
            this.tctlList = new System.Windows.Forms.TabControl();
            this.tpM3u8FileList = new System.Windows.Forms.TabPage();
            this.lvwM3U8List = new M3U8_GETTER.ListViewNF();
            this.clnListId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnListShortname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnListFilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnListUrl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnListState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnVideoState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ctmsM3u8List = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmitClearFinished = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmitMakeVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmitResetStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmitOpenM3u8Folder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmitOpenVideoFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmitCopyM3u8Url = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmitDeleteSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmitClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tpCurrentDownList = new System.Windows.Forms.TabPage();
            this.lvwStatusList = new M3U8_GETTER.ListViewNF();
            this.clnDownId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnDownFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnDownFilesize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnDownDownLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnDownPercent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnDownSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnDownState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStartDownload = new System.Windows.Forms.Button();
            this.rtxtLogs = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnResetFfmpegCommand = new System.Windows.Forms.Button();
            this.txtFfmpegCommand = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtM3u8ListDownMax = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOpenPath = new System.Windows.Forms.Button();
            this.btnChangePath = new System.Windows.Forms.Button();
            this.txtDownloadPath = new System.Windows.Forms.TextBox();
            this.btnRegFileType = new System.Windows.Forms.Button();
            this.txtFileThreadMax = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtM3u8FileDownMax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkDeleteTmp = new System.Windows.Forms.CheckBox();
            this.btnOpenTmpPath = new System.Windows.Forms.Button();
            this.btnChangeTmpPath = new System.Windows.Forms.Button();
            this.txtTempPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbtnVideoTypeMP4 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.timrRefresh = new System.Windows.Forms.Timer(this.components);
            this.tctlMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tctlList.SuspendLayout();
            this.tpM3u8FileList.SuspendLayout();
            this.ctmsM3u8List.SuspendLayout();
            this.tpCurrentDownList.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // DownloadProgress
            // 
            this.DownloadProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DownloadProgress.Location = new System.Drawing.Point(0, 554);
            this.DownloadProgress.Margin = new System.Windows.Forms.Padding(0);
            this.DownloadProgress.Name = "DownloadProgress";
            this.DownloadProgress.Size = new System.Drawing.Size(861, 12);
            this.DownloadProgress.Step = 1;
            this.DownloadProgress.TabIndex = 5;
            // 
            // tctlMain
            // 
            this.tctlMain.Controls.Add(this.tabPage1);
            this.tctlMain.Controls.Add(this.tabPage2);
            this.tctlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tctlMain.Location = new System.Drawing.Point(0, 0);
            this.tctlMain.Name = "tctlMain";
            this.tctlMain.SelectedIndex = 0;
            this.tctlMain.Size = new System.Drawing.Size(861, 554);
            this.tctlMain.TabIndex = 33;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnAddToList);
            this.tabPage1.Controls.Add(this.tctlList);
            this.tabPage1.Controls.Add(this.txtUrl);
            this.tabPage1.Controls.Add(this.infoLabel);
            this.tabPage1.Controls.Add(this.btnStop);
            this.tabPage1.Controls.Add(this.btnStartDownload);
            this.tabPage1.Controls.Add(this.rtxtLogs);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(853, 528);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "下载";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnAddToList
            // 
            this.btnAddToList.Location = new System.Drawing.Point(429, 38);
            this.btnAddToList.Name = "btnAddToList";
            this.btnAddToList.Size = new System.Drawing.Size(144, 25);
            this.btnAddToList.TabIndex = 42;
            this.btnAddToList.Text = "添加到待下载列表";
            this.btnAddToList.UseVisualStyleBackColor = true;
            this.btnAddToList.Click += new System.EventHandler(this.BtnAddToList_Click);
            // 
            // tctlList
            // 
            this.tctlList.Controls.Add(this.tpM3u8FileList);
            this.tctlList.Controls.Add(this.tpCurrentDownList);
            this.tctlList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tctlList.Location = new System.Drawing.Point(3, 96);
            this.tctlList.Name = "tctlList";
            this.tctlList.SelectedIndex = 0;
            this.tctlList.ShowToolTips = true;
            this.tctlList.Size = new System.Drawing.Size(847, 316);
            this.tctlList.TabIndex = 41;
            // 
            // tpM3u8FileList
            // 
            this.tpM3u8FileList.Controls.Add(this.lvwM3U8List);
            this.tpM3u8FileList.Location = new System.Drawing.Point(4, 22);
            this.tpM3u8FileList.Name = "tpM3u8FileList";
            this.tpM3u8FileList.Padding = new System.Windows.Forms.Padding(3);
            this.tpM3u8FileList.Size = new System.Drawing.Size(839, 290);
            this.tpM3u8FileList.TabIndex = 0;
            this.tpM3u8FileList.Text = "M3U8 待下载列表";
            this.tpM3u8FileList.UseVisualStyleBackColor = true;
            // 
            // lvwM3U8List
            // 
            this.lvwM3U8List.AllowDrop = true;
            this.lvwM3U8List.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clnListId,
            this.clnListShortname,
            this.clnListFilePath,
            this.clnListUrl,
            this.clnListState,
            this.clnVideoState});
            this.lvwM3U8List.ContextMenuStrip = this.ctmsM3u8List;
            this.lvwM3U8List.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwM3U8List.FullRowSelect = true;
            this.lvwM3U8List.GridLines = true;
            this.lvwM3U8List.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwM3U8List.HideSelection = false;
            this.lvwM3U8List.Location = new System.Drawing.Point(3, 3);
            this.lvwM3U8List.Name = "lvwM3U8List";
            this.lvwM3U8List.ShowItemToolTips = true;
            this.lvwM3U8List.Size = new System.Drawing.Size(833, 284);
            this.lvwM3U8List.TabIndex = 35;
            this.lvwM3U8List.UseCompatibleStateImageBehavior = false;
            this.lvwM3U8List.View = System.Windows.Forms.View.Details;
            this.lvwM3U8List.VirtualMode = true;
            this.lvwM3U8List.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lvwM3U8List_RetrieveVirtualItem);
            this.lvwM3U8List.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvM3U8List_DragDrop);
            this.lvwM3U8List.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvM3U8List_DragEnter);
            this.lvwM3U8List.DragOver += new System.Windows.Forms.DragEventHandler(this.lvM3U8List_DragOver);
            // 
            // clnListId
            // 
            this.clnListId.Text = "序号";
            this.clnListId.Width = 45;
            // 
            // clnListShortname
            // 
            this.clnListShortname.Text = "文件名";
            this.clnListShortname.Width = 199;
            // 
            // clnListFilePath
            // 
            this.clnListFilePath.Text = "最终视频目录";
            this.clnListFilePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clnListFilePath.Width = 212;
            // 
            // clnListUrl
            // 
            this.clnListUrl.Text = "m3u8源地址";
            this.clnListUrl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clnListUrl.Width = 193;
            // 
            // clnListState
            // 
            this.clnListState.Text = "状态";
            this.clnListState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clnListState.Width = 95;
            // 
            // clnVideoState
            // 
            this.clnVideoState.Text = "视频状态";
            // 
            // ctmsM3u8List
            // 
            this.ctmsM3u8List.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmitClearFinished,
            this.tsmitMakeVideo,
            this.toolStripSeparator3,
            this.tsmitResetStatus,
            this.toolStripSeparator2,
            this.tsmitOpenM3u8Folder,
            this.tsmitOpenVideoFolder,
            this.tsmitCopyM3u8Url,
            this.toolStripSeparator1,
            this.tsmitDeleteSelect,
            this.tsmitClearAll});
            this.ctmsM3u8List.Name = "ctmsM3u8List";
            this.ctmsM3u8List.Size = new System.Drawing.Size(220, 198);
            // 
            // tsmitClearFinished
            // 
            this.tsmitClearFinished.Name = "tsmitClearFinished";
            this.tsmitClearFinished.Size = new System.Drawing.Size(219, 22);
            this.tsmitClearFinished.Text = "清除已完成的项目";
            this.tsmitClearFinished.Click += new System.EventHandler(this.tsmitClearFinished_Click);
            // 
            // tsmitMakeVideo
            // 
            this.tsmitMakeVideo.Name = "tsmitMakeVideo";
            this.tsmitMakeVideo.Size = new System.Drawing.Size(219, 22);
            this.tsmitMakeVideo.Text = "手动处理视频";
            this.tsmitMakeVideo.Click += new System.EventHandler(this.tsmitMakeVideo_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(216, 6);
            // 
            // tsmitResetStatus
            // 
            this.tsmitResetStatus.Name = "tsmitResetStatus";
            this.tsmitResetStatus.Size = new System.Drawing.Size(219, 22);
            this.tsmitResetStatus.Text = "重置状态";
            this.tsmitResetStatus.Click += new System.EventHandler(this.tsmitResetStatus_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(216, 6);
            // 
            // tsmitOpenM3u8Folder
            // 
            this.tsmitOpenM3u8Folder.Name = "tsmitOpenM3u8Folder";
            this.tsmitOpenM3u8Folder.Size = new System.Drawing.Size(219, 22);
            this.tsmitOpenM3u8Folder.Text = "打开M3U8文件所在文件夹";
            this.tsmitOpenM3u8Folder.Click += new System.EventHandler(this.tsmitOpenM3u8Folder_Click);
            // 
            // tsmitOpenVideoFolder
            // 
            this.tsmitOpenVideoFolder.Name = "tsmitOpenVideoFolder";
            this.tsmitOpenVideoFolder.Size = new System.Drawing.Size(219, 22);
            this.tsmitOpenVideoFolder.Text = "打开视频所在文件夹";
            this.tsmitOpenVideoFolder.Click += new System.EventHandler(this.tsmitOpenVideoFolder_Click);
            // 
            // tsmitCopyM3u8Url
            // 
            this.tsmitCopyM3u8Url.Name = "tsmitCopyM3u8Url";
            this.tsmitCopyM3u8Url.Size = new System.Drawing.Size(219, 22);
            this.tsmitCopyM3u8Url.Text = "复制 M3U8 源地址";
            this.tsmitCopyM3u8Url.Click += new System.EventHandler(this.tsmitCopyM3u8Url_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(216, 6);
            // 
            // tsmitDeleteSelect
            // 
            this.tsmitDeleteSelect.Name = "tsmitDeleteSelect";
            this.tsmitDeleteSelect.Size = new System.Drawing.Size(219, 22);
            this.tsmitDeleteSelect.Text = "删除选中的项目";
            this.tsmitDeleteSelect.Click += new System.EventHandler(this.tsmitDeleteSelect_Click);
            // 
            // tsmitClearAll
            // 
            this.tsmitClearAll.Name = "tsmitClearAll";
            this.tsmitClearAll.Size = new System.Drawing.Size(219, 22);
            this.tsmitClearAll.Text = "删除所有项目";
            this.tsmitClearAll.Click += new System.EventHandler(this.tsmitClearAll_Click);
            // 
            // tpCurrentDownList
            // 
            this.tpCurrentDownList.Controls.Add(this.lvwStatusList);
            this.tpCurrentDownList.Location = new System.Drawing.Point(4, 22);
            this.tpCurrentDownList.Name = "tpCurrentDownList";
            this.tpCurrentDownList.Padding = new System.Windows.Forms.Padding(3);
            this.tpCurrentDownList.Size = new System.Drawing.Size(839, 290);
            this.tpCurrentDownList.TabIndex = 1;
            this.tpCurrentDownList.Text = "当前下载队列";
            this.tpCurrentDownList.UseVisualStyleBackColor = true;
            // 
            // lvwStatusList
            // 
            this.lvwStatusList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clnDownId,
            this.clnDownFilename,
            this.clnDownFilesize,
            this.clnDownDownLength,
            this.clnDownPercent,
            this.clnDownSpeed,
            this.clnDownState});
            this.lvwStatusList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwStatusList.FullRowSelect = true;
            this.lvwStatusList.GridLines = true;
            this.lvwStatusList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwStatusList.HideSelection = false;
            this.lvwStatusList.Location = new System.Drawing.Point(3, 3);
            this.lvwStatusList.MultiSelect = false;
            this.lvwStatusList.Name = "lvwStatusList";
            this.lvwStatusList.ShowItemToolTips = true;
            this.lvwStatusList.Size = new System.Drawing.Size(833, 284);
            this.lvwStatusList.TabIndex = 34;
            this.lvwStatusList.UseCompatibleStateImageBehavior = false;
            this.lvwStatusList.View = System.Windows.Forms.View.Details;
            this.lvwStatusList.VirtualMode = true;
            this.lvwStatusList.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lvwStatusList_RetrieveVirtualItem);
            // 
            // clnDownId
            // 
            this.clnDownId.Text = "序号";
            this.clnDownId.Width = 54;
            // 
            // clnDownFilename
            // 
            this.clnDownFilename.Text = "文件名";
            this.clnDownFilename.Width = 334;
            // 
            // clnDownFilesize
            // 
            this.clnDownFilesize.Text = "总大小";
            this.clnDownFilesize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clnDownFilesize.Width = 72;
            // 
            // clnDownDownLength
            // 
            this.clnDownDownLength.Text = "已下载";
            this.clnDownDownLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clnDownDownLength.Width = 74;
            // 
            // clnDownPercent
            // 
            this.clnDownPercent.Text = "进度";
            this.clnDownPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clnDownPercent.Width = 56;
            // 
            // clnDownSpeed
            // 
            this.clnDownSpeed.Text = "用时";
            this.clnDownSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clnDownSpeed.Width = 56;
            // 
            // clnDownState
            // 
            this.clnDownState.Text = "状态";
            this.clnDownState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clnDownState.Width = 160;
            // 
            // txtUrl
            // 
            this.txtUrl.AllowDrop = true;
            this.txtUrl.Location = new System.Drawing.Point(6, 6);
            this.txtUrl.Multiline = true;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(415, 57);
            this.txtUrl.TabIndex = 40;
            this.txtUrl.Click += new System.EventHandler(this.txtUrl_Click);
            this.txtUrl.TextChanged += new System.EventHandler(this.url_TextChanged);
            this.txtUrl.DragDrop += new System.Windows.Forms.DragEventHandler(this.url_DragDrop);
            this.txtUrl.DragEnter += new System.Windows.Forms.DragEventHandler(this.url_DragEnter);
            this.txtUrl.DragOver += new System.Windows.Forms.DragEventHandler(this.url_DragOver);
            this.txtUrl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.url_KeyPress);
            // 
            // infoLabel
            // 
            this.infoLabel.Location = new System.Drawing.Point(427, 9);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(164, 30);
            this.infoLabel.TabIndex = 39;
            this.infoLabel.Text = "请在左边粘贴m3u8文件网址，并点击“添加到待下载列表”";
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(731, 18);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(112, 36);
            this.btnStop.TabIndex = 35;
            this.btnStop.Text = "停止下载";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStartDownload
            // 
            this.btnStartDownload.Location = new System.Drawing.Point(608, 18);
            this.btnStartDownload.Name = "btnStartDownload";
            this.btnStartDownload.Size = new System.Drawing.Size(105, 36);
            this.btnStartDownload.TabIndex = 34;
            this.btnStartDownload.Text = "开始下载";
            this.btnStartDownload.UseVisualStyleBackColor = true;
            this.btnStartDownload.Click += new System.EventHandler(this.btnStartDownload_Click);
            // 
            // rtxtLogs
            // 
            this.rtxtLogs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtxtLogs.HideSelection = false;
            this.rtxtLogs.Location = new System.Drawing.Point(3, 412);
            this.rtxtLogs.Name = "rtxtLogs";
            this.rtxtLogs.ReadOnly = true;
            this.rtxtLogs.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtxtLogs.Size = new System.Drawing.Size(847, 113);
            this.rtxtLogs.TabIndex = 1;
            this.rtxtLogs.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnResetFfmpegCommand);
            this.tabPage2.Controls.Add(this.txtFfmpegCommand);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.txtM3u8ListDownMax);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.btnOpenPath);
            this.tabPage2.Controls.Add(this.btnChangePath);
            this.tabPage2.Controls.Add(this.txtDownloadPath);
            this.tabPage2.Controls.Add(this.btnRegFileType);
            this.tabPage2.Controls.Add(this.txtFileThreadMax);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.txtM3u8FileDownMax);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.chkDeleteTmp);
            this.tabPage2.Controls.Add(this.btnOpenTmpPath);
            this.tabPage2.Controls.Add(this.btnChangeTmpPath);
            this.tabPage2.Controls.Add(this.txtTempPath);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.rbtnVideoTypeMP4);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.linkLabel);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(853, 528);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnResetFfmpegCommand
            // 
            this.btnResetFfmpegCommand.Location = new System.Drawing.Point(784, 233);
            this.btnResetFfmpegCommand.Name = "btnResetFfmpegCommand";
            this.btnResetFfmpegCommand.Size = new System.Drawing.Size(57, 28);
            this.btnResetFfmpegCommand.TabIndex = 49;
            this.btnResetFfmpegCommand.Text = "重置";
            this.btnResetFfmpegCommand.UseVisualStyleBackColor = true;
            this.btnResetFfmpegCommand.Click += new System.EventHandler(this.btnResetFfmpegCommand_Click);
            // 
            // txtFfmpegCommand
            // 
            this.txtFfmpegCommand.Location = new System.Drawing.Point(126, 238);
            this.txtFfmpegCommand.Name = "txtFfmpegCommand";
            this.txtFfmpegCommand.Size = new System.Drawing.Size(646, 21);
            this.txtFfmpegCommand.TabIndex = 48;
            this.txtFfmpegCommand.Text = "-allowed_extensions ALL -protocol_whitelist \"file,http,crypto,tcp\" -i \"{{m3u8file" +
    "name}}\" -vcodec copy -acodec copy -absf aac_adtstoasc \"{{mp4filename}}\"";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 241);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 12);
            this.label9.TabIndex = 47;
            this.label9.Text = "ffmpeg 转换命令：";
            // 
            // txtM3u8ListDownMax
            // 
            this.txtM3u8ListDownMax.Location = new System.Drawing.Point(778, 205);
            this.txtM3u8ListDownMax.Name = "txtM3u8ListDownMax";
            this.txtM3u8ListDownMax.Size = new System.Drawing.Size(38, 21);
            this.txtM3u8ListDownMax.TabIndex = 46;
            this.txtM3u8ListDownMax.Text = "1";
            this.txtM3u8ListDownMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(599, 208);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(173, 12);
            this.label8.TabIndex = 45;
            this.label8.Text = "m3u8待下载列表同时下载几个：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(287, 12);
            this.label7.TabIndex = 44;
            this.label7.Text = "临时文件夹用于存放下载的 ts 文件片段和 key 文件";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 119);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(653, 12);
            this.label6.TabIndex = 43;
            this.label6.Text = "当m3u8为http开头的网络文件时，将会下载到设置的“下载目录”文件夹里，m3u8为本地文件时，将会下载到同级文件夹中";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 42;
            this.label5.Text = "下载目录";
            // 
            // btnOpenPath
            // 
            this.btnOpenPath.Location = new System.Drawing.Point(688, 86);
            this.btnOpenPath.Name = "btnOpenPath";
            this.btnOpenPath.Size = new System.Drawing.Size(140, 28);
            this.btnOpenPath.TabIndex = 41;
            this.btnOpenPath.Text = "打开下载目录";
            this.btnOpenPath.UseVisualStyleBackColor = true;
            this.btnOpenPath.Click += new System.EventHandler(this.btnOpenPath_Click);
            // 
            // btnChangePath
            // 
            this.btnChangePath.Location = new System.Drawing.Point(535, 86);
            this.btnChangePath.Name = "btnChangePath";
            this.btnChangePath.Size = new System.Drawing.Size(140, 28);
            this.btnChangePath.TabIndex = 40;
            this.btnChangePath.Text = "更改下载路径";
            this.btnChangePath.UseVisualStyleBackColor = true;
            this.btnChangePath.Click += new System.EventHandler(this.btnChangePath_Click);
            // 
            // txtDownloadPath
            // 
            this.txtDownloadPath.Location = new System.Drawing.Point(72, 91);
            this.txtDownloadPath.Name = "txtDownloadPath";
            this.txtDownloadPath.Size = new System.Drawing.Size(448, 21);
            this.txtDownloadPath.TabIndex = 39;
            // 
            // btnRegFileType
            // 
            this.btnRegFileType.Location = new System.Drawing.Point(701, 37);
            this.btnRegFileType.Name = "btnRegFileType";
            this.btnRegFileType.Size = new System.Drawing.Size(140, 28);
            this.btnRegFileType.TabIndex = 35;
            this.btnRegFileType.Text = "注册关联文件";
            this.btnRegFileType.UseVisualStyleBackColor = true;
            this.btnRegFileType.Click += new System.EventHandler(this.btnRegFileType_Click);
            // 
            // txtFileThreadMax
            // 
            this.txtFileThreadMax.Location = new System.Drawing.Point(415, 204);
            this.txtFileThreadMax.Name = "txtFileThreadMax";
            this.txtFileThreadMax.Size = new System.Drawing.Size(38, 21);
            this.txtFileThreadMax.TabIndex = 34;
            this.txtFileThreadMax.Text = "1";
            this.txtFileThreadMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(284, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 12);
            this.label4.TabIndex = 33;
            this.label4.Text = "单个片段几线程下载：";
            // 
            // txtM3u8FileDownMax
            // 
            this.txtM3u8FileDownMax.Location = new System.Drawing.Point(204, 204);
            this.txtM3u8FileDownMax.Name = "txtM3u8FileDownMax";
            this.txtM3u8FileDownMax.Size = new System.Drawing.Size(38, 21);
            this.txtM3u8FileDownMax.TabIndex = 32;
            this.txtM3u8FileDownMax.Text = "3";
            this.txtM3u8FileDownMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 208);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(185, 12);
            this.label3.TabIndex = 31;
            this.label3.Text = "每个m3u8文件同时下载几个片段：";
            // 
            // chkDeleteTmp
            // 
            this.chkDeleteTmp.AutoSize = true;
            this.chkDeleteTmp.Checked = true;
            this.chkDeleteTmp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDeleteTmp.Location = new System.Drawing.Point(15, 62);
            this.chkDeleteTmp.Name = "chkDeleteTmp";
            this.chkDeleteTmp.Size = new System.Drawing.Size(252, 16);
            this.chkDeleteTmp.TabIndex = 30;
            this.chkDeleteTmp.Text = "视频处理完成后删除临时目录所有相关文件";
            this.chkDeleteTmp.UseVisualStyleBackColor = true;
            // 
            // btnOpenTmpPath
            // 
            this.btnOpenTmpPath.Location = new System.Drawing.Point(748, 143);
            this.btnOpenTmpPath.Name = "btnOpenTmpPath";
            this.btnOpenTmpPath.Size = new System.Drawing.Size(80, 28);
            this.btnOpenTmpPath.TabIndex = 29;
            this.btnOpenTmpPath.Text = "打开目录";
            this.btnOpenTmpPath.UseVisualStyleBackColor = true;
            this.btnOpenTmpPath.Click += new System.EventHandler(this.btnOpenTmpPath_Click);
            // 
            // btnChangeTmpPath
            // 
            this.btnChangeTmpPath.Location = new System.Drawing.Point(689, 143);
            this.btnChangeTmpPath.Name = "btnChangeTmpPath";
            this.btnChangeTmpPath.Size = new System.Drawing.Size(53, 28);
            this.btnChangeTmpPath.TabIndex = 28;
            this.btnChangeTmpPath.Text = "更改";
            this.btnChangeTmpPath.UseVisualStyleBackColor = true;
            this.btnChangeTmpPath.Click += new System.EventHandler(this.btnChangeTmpPath_Click);
            // 
            // txtTempPath
            // 
            this.txtTempPath.Location = new System.Drawing.Point(156, 148);
            this.txtTempPath.Name = "txtTempPath";
            this.txtTempPath.Size = new System.Drawing.Size(519, 21);
            this.txtTempPath.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 12);
            this.label2.TabIndex = 26;
            this.label2.Text = "临时文件夹(英文目录)：";
            // 
            // rbtnVideoTypeMP4
            // 
            this.rbtnVideoTypeMP4.AutoSize = true;
            this.rbtnVideoTypeMP4.Checked = true;
            this.rbtnVideoTypeMP4.Location = new System.Drawing.Point(146, 12);
            this.rbtnVideoTypeMP4.Name = "rbtnVideoTypeMP4";
            this.rbtnVideoTypeMP4.Size = new System.Drawing.Size(41, 16);
            this.rbtnVideoTypeMP4.TabIndex = 22;
            this.rbtnVideoTypeMP4.TabStop = true;
            this.rbtnVideoTypeMP4.Text = "MP4";
            this.rbtnVideoTypeMP4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "合成的视频文件格式：";
            // 
            // linkLabel
            // 
            this.linkLabel.AutoSize = true;
            this.linkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.linkLabel.Location = new System.Drawing.Point(620, 9);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(221, 12);
            this.linkLabel.TabIndex = 20;
            this.linkLabel.TabStop = true;
            this.linkLabel.Text = "wujinhai(940390@qq.com) 点击检查更新";
            this.linkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
            // 
            // timrRefresh
            // 
            this.timrRefresh.Interval = 1000;
            this.timrRefresh.Tick += new System.EventHandler(this.timrRefresh_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 566);
            this.Controls.Add(this.tctlMain);
            this.Controls.Add(this.DownloadProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "M3U8 GETTER 1.0.3 批量下载版";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tctlMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tctlList.ResumeLayout(false);
            this.tpM3u8FileList.ResumeLayout(false);
            this.ctmsM3u8List.ResumeLayout(false);
            this.tpCurrentDownList.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ProgressBar DownloadProgress;
        private System.Windows.Forms.FolderBrowserDialog changeFolderDialog;
        private System.Windows.Forms.TabControl tctlMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnOpenTmpPath;
        private System.Windows.Forms.Button btnChangeTmpPath;
        private System.Windows.Forms.TextBox txtTempPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbtnVideoTypeMP4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.TextBox txtFileThreadMax;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtM3u8FileDownMax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkDeleteTmp;
        private System.Windows.Forms.Button btnRegFileType;
        private System.Windows.Forms.RichTextBox rtxtLogs;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStartDownload;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.TabControl tctlList;
        private System.Windows.Forms.TabPage tpM3u8FileList;
        private System.Windows.Forms.TabPage tpCurrentDownList;
        private M3U8_GETTER.ListViewNF lvwStatusList;
        private System.Windows.Forms.ColumnHeader clnDownId;
        private System.Windows.Forms.ColumnHeader clnDownFilename;
        private System.Windows.Forms.ColumnHeader clnDownDownLength;
        private System.Windows.Forms.ColumnHeader clnDownPercent;
        private System.Windows.Forms.ColumnHeader clnDownSpeed;
        private System.Windows.Forms.ColumnHeader clnDownState;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnOpenPath;
        private System.Windows.Forms.Button btnChangePath;
        private System.Windows.Forms.TextBox txtDownloadPath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnAddToList;
        private System.Windows.Forms.ColumnHeader clnDownFilesize;
        private M3U8_GETTER.ListViewNF lvwM3U8List;
        private System.Windows.Forms.ColumnHeader clnListId;
        private System.Windows.Forms.ColumnHeader clnListShortname;
        private System.Windows.Forms.ColumnHeader clnListFilePath;
        private System.Windows.Forms.ColumnHeader clnListUrl;
        private System.Windows.Forms.ColumnHeader clnListState;
        private System.Windows.Forms.TextBox txtM3u8ListDownMax;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ContextMenuStrip ctmsM3u8List;
        private System.Windows.Forms.ToolStripMenuItem tsmitOpenVideoFolder;
        private System.Windows.Forms.ToolStripMenuItem tsmitCopyM3u8Url;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmitDeleteSelect;
        private System.Windows.Forms.ToolStripMenuItem tsmitClearAll;
        private System.Windows.Forms.ColumnHeader clnVideoState;
        private System.Windows.Forms.ToolStripMenuItem tsmitResetStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmitOpenM3u8Folder;
        private System.Windows.Forms.ToolStripMenuItem tsmitMakeVideo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tsmitClearFinished;
        private System.Windows.Forms.TextBox txtFfmpegCommand;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnResetFfmpegCommand;
        private System.Windows.Forms.Timer timrRefresh;
    }
}

