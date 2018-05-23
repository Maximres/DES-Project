namespace QueueSimulation
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Продукт1", 0);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Продукт2", 1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Продукт3", 2);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Станок1", 3);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("Станок2", 4);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("Станок3", 5);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("Станок4", 6);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("Конвейер", 7);
            this.menuPanel = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.модельToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обАвтореToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsPanel = new System.Windows.Forms.ToolStrip();
            this.btnStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripItemsCount = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnInterval = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listModelObjects = new System.Windows.Forms.ListView();
            this.MainImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.viewerPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.nodesWithCountList = new System.Windows.Forms.ListBox();
            this.statusPanel = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolPanelTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolPanelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.TheTimer = new System.Windows.Forms.Timer(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextItemDeleteElement = new System.Windows.Forms.ToolStripMenuItem();
            this.contextItemDeleteSelectedElements = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextItemConnectElements = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPanel.SuspendLayout();
            this.toolsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusPanel.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuPanel
            // 
            this.menuPanel.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.модельToolStripMenuItem,
            this.справкаToolStripMenuItem,
            this.обАвтореToolStripMenuItem});
            this.menuPanel.Location = new System.Drawing.Point(0, 0);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.Size = new System.Drawing.Size(1282, 28);
            this.menuPanel.TabIndex = 0;
            this.menuPanel.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // модельToolStripMenuItem
            // 
            this.модельToolStripMenuItem.Name = "модельToolStripMenuItem";
            this.модельToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.модельToolStripMenuItem.Text = "Модель";
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(79, 24);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // обАвтореToolStripMenuItem
            // 
            this.обАвтореToolStripMenuItem.Name = "обАвтореToolStripMenuItem";
            this.обАвтореToolStripMenuItem.Size = new System.Drawing.Size(93, 24);
            this.обАвтореToolStripMenuItem.Text = "Об авторе";
            // 
            // toolsPanel
            // 
            this.toolsPanel.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolsPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStart,
            this.toolStripItemsCount,
            this.toolStripSeparator2,
            this.btnPause,
            this.toolStripSeparator3,
            this.btnStop,
            this.toolStripSeparator4,
            this.btnInterval});
            this.toolsPanel.Location = new System.Drawing.Point(0, 28);
            this.toolsPanel.Name = "toolsPanel";
            this.toolsPanel.Size = new System.Drawing.Size(1282, 27);
            this.toolsPanel.TabIndex = 1;
            this.toolsPanel.Text = "toolStrip1";
            // 
            // btnStart
            // 
            this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(71, 24);
            this.btnStart.Text = "Старт";
            this.btnStart.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripItemsCount
            // 
            this.toolStripItemsCount.Name = "toolStripItemsCount";
            this.toolStripItemsCount.Size = new System.Drawing.Size(50, 27);
            this.toolStripItemsCount.Text = "10";
            this.toolStripItemsCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripItemsCount_KeyPress);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // btnPause
            // 
            this.btnPause.Image = ((System.Drawing.Image)(resources.GetObject("btnPause.Image")));
            this.btnPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(70, 24);
            this.btnPause.Text = "Pause";
            this.btnPause.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // btnStop
            // 
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(66, 24);
            this.btnStop.Text = "Стоп";
            this.btnStop.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // btnInterval
            // 
            this.btnInterval.Image = ((System.Drawing.Image)(resources.GetObject("btnInterval.Image")));
            this.btnInterval.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInterval.Name = "btnInterval";
            this.btnInterval.Size = new System.Drawing.Size(100, 24);
            this.btnInterval.Text = "Интервал";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 55);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listModelObjects);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.statusPanel);
            this.splitContainer1.Size = new System.Drawing.Size(1282, 598);
            this.splitContainer1.SplitterDistance = 163;
            this.splitContainer1.TabIndex = 2;
            // 
            // listModelObjects
            // 
            this.listModelObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewItem6.Checked = true;
            listViewItem6.StateImageIndex = 2;
            this.listModelObjects.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8});
            this.listModelObjects.LargeImageList = this.MainImageList;
            this.listModelObjects.Location = new System.Drawing.Point(0, 0);
            this.listModelObjects.Name = "listModelObjects";
            this.listModelObjects.Size = new System.Drawing.Size(163, 598);
            this.listModelObjects.SmallImageList = this.MainImageList;
            this.listModelObjects.TabIndex = 0;
            this.listModelObjects.UseCompatibleStateImageBehavior = false;
            this.listModelObjects.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listModelObjects_ItemDrag);
            this.listModelObjects.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListView_SelectedIndexChanged);
            // 
            // MainImageList
            // 
            this.MainImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("MainImageList.ImageStream")));
            this.MainImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.MainImageList.Images.SetKeyName(0, "icons8-коробка-80(1).png");
            this.MainImageList.Images.SetKeyName(1, "icons8-подержанный-80(1).png");
            this.MainImageList.Images.SetKeyName(2, "icons8-пакет-64(1).png");
            this.MainImageList.Images.SetKeyName(3, "drilling.png");
            this.MainImageList.Images.SetKeyName(4, "machine(1).png");
            this.MainImageList.Images.SetKeyName(5, "machine(3).png");
            this.MainImageList.Images.SetKeyName(6, "machine.png");
            this.MainImageList.Images.SetKeyName(7, "factory.png");
            this.MainImageList.Images.SetKeyName(8, "warehouse.png");
            this.MainImageList.Images.SetKeyName(9, "dashed-circle.png");
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.viewerPanel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer2.Size = new System.Drawing.Size(1115, 573);
            this.splitContainer2.SplitterDistance = 801;
            this.splitContainer2.TabIndex = 1;
            // 
            // viewerPanel
            // 
            this.viewerPanel.BackColor = System.Drawing.SystemColors.Window;
            this.viewerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewerPanel.Location = new System.Drawing.Point(0, 0);
            this.viewerPanel.Name = "viewerPanel";
            this.viewerPanel.Size = new System.Drawing.Size(801, 573);
            this.viewerPanel.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.propertyGrid1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.nodesWithCountList, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(310, 573);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(304, 251);
            this.propertyGrid1.TabIndex = 1;
            // 
            // nodesWithCountList
            // 
            this.nodesWithCountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodesWithCountList.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nodesWithCountList.FormattingEnabled = true;
            this.nodesWithCountList.ItemHeight = 28;
            this.nodesWithCountList.Location = new System.Drawing.Point(3, 260);
            this.nodesWithCountList.Name = "nodesWithCountList";
            this.nodesWithCountList.Size = new System.Drawing.Size(304, 310);
            this.nodesWithCountList.TabIndex = 2;
            // 
            // statusPanel
            // 
            this.statusPanel.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolPanelTime,
            this.toolStripStatusLabel3,
            this.toolPanelStatus});
            this.statusPanel.Location = new System.Drawing.Point(0, 573);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new System.Drawing.Size(1115, 25);
            this.statusPanel.TabIndex = 0;
            this.statusPanel.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(45, 20);
            this.toolStripStatusLabel1.Text = "Time:";
            // 
            // toolPanelTime
            // 
            this.toolPanelTime.Name = "toolPanelTime";
            this.toolPanelTime.Size = new System.Drawing.Size(17, 20);
            this.toolPanelTime.Text = "0";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(52, 20);
            this.toolStripStatusLabel3.Text = "Status:";
            // 
            // toolPanelStatus
            // 
            this.toolPanelStatus.Name = "toolPanelStatus";
            this.toolPanelStatus.Size = new System.Drawing.Size(28, 20);
            this.toolPanelStatus.Text = "off";
            // 
            // TheTimer
            // 
            this.TheTimer.Interval = 1000;
            this.TheTimer.Tick += new System.EventHandler(this.TheTimer_Tick);
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextItemDeleteElement,
            this.contextItemDeleteSelectedElements,
            this.toolStripSeparator1,
            this.contextItemConnectElements});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(299, 82);
            // 
            // contextItemDeleteElement
            // 
            this.contextItemDeleteElement.Name = "contextItemDeleteElement";
            this.contextItemDeleteElement.Size = new System.Drawing.Size(298, 24);
            this.contextItemDeleteElement.Text = "Удалить элемент";
            this.contextItemDeleteElement.Click += new System.EventHandler(this.contextItemDeleteElement_Click);
            // 
            // contextItemDeleteSelectedElements
            // 
            this.contextItemDeleteSelectedElements.Name = "contextItemDeleteSelectedElements";
            this.contextItemDeleteSelectedElements.Size = new System.Drawing.Size(298, 24);
            this.contextItemDeleteSelectedElements.Text = "Удалить выделенные элементы";
            this.contextItemDeleteSelectedElements.Click += new System.EventHandler(this.contextItemDeleteSelectedElements_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(295, 6);
            // 
            // contextItemConnectElements
            // 
            this.contextItemConnectElements.Name = "contextItemConnectElements";
            this.contextItemConnectElements.Size = new System.Drawing.Size(298, 24);
            this.contextItemConnectElements.Text = "Присоеднить";
            this.contextItemConnectElements.Click += new System.EventHandler(this.contextItemConnectElements_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1282, 653);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolsPanel);
            this.Controls.Add(this.menuPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuPanel;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "MainForm";
            this.Text = "DES";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuPanel.ResumeLayout(false);
            this.menuPanel.PerformLayout();
            this.toolsPanel.ResumeLayout(false);
            this.toolsPanel.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.statusPanel.ResumeLayout(false);
            this.statusPanel.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuPanel;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem модельToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обАвтореToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolsPanel;
        private System.Windows.Forms.ToolStripButton btnStart;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripButton btnInterval;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.StatusStrip statusPanel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolPanelTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolPanelStatus;
        private System.Windows.Forms.Panel viewerPanel;
        private System.Windows.Forms.ListView listModelObjects;
        private System.Windows.Forms.ImageList MainImageList;
        private System.Windows.Forms.Timer TheTimer;
        private System.Windows.Forms.ToolStripButton btnPause;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem contextItemDeleteSelectedElements;
        private System.Windows.Forms.ToolStripMenuItem contextItemDeleteElement;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem contextItemConnectElements;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox nodesWithCountList;
        private System.Windows.Forms.ToolStripTextBox toolStripItemsCount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}

