using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FullSailAFI.GamePlaying.CoreAI;

namespace FullSailAFI.GamePlaying
{
	/// <summary>
	/// Summary description for OptionsDialog.
	/// </summary>
	public class OptionsDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl optionsTabControl;
		private System.Windows.Forms.TabPage displayTabPage;
		private System.Windows.Forms.CheckBox showValidMovesCheckBox;
		private System.Windows.Forms.CheckBox previewMovesCheckBox;
		private System.Windows.Forms.CheckBox animateMovesCheckBox;
		private System.Windows.Forms.Label boardColorLabel;
		private System.Windows.Forms.Panel boardColorPanel;
		private System.Windows.Forms.Button boardColorButton;
		private System.Windows.Forms.Label validColorLabel;
		private System.Windows.Forms.Panel validColorPanel;
		private System.Windows.Forms.Button validColorButton;
		private System.Windows.Forms.Label activeColorLabel;
		private System.Windows.Forms.Panel activeColorPanel;
		private System.Windows.Forms.Button activeColorButton;
		private System.Windows.Forms.Label moveIndicatorColorLabel;
		private System.Windows.Forms.Panel moveIndicatorColorPanel;
		private System.Windows.Forms.Button moveIndicatorColorButton;
		private System.Windows.Forms.TabPage playersTabPage;
		private System.Windows.Forms.Panel blackPlayerPanel;
		private System.Windows.Forms.RadioButton blackPlayerComputerRadioButton;
		private System.Windows.Forms.RadioButton blackPlayerUserRadioButton;
		private System.Windows.Forms.Panel whitePlayerPanel;
		private System.Windows.Forms.RadioButton whitePlayerComputerRadioButton;
		private System.Windows.Forms.RadioButton whitePlayerUserRadioButton;
		private System.Windows.Forms.Label blackPlayerLabel;
		private System.Windows.Forms.Label whitePlayerLabel;
		private System.Windows.Forms.TabPage difficultyTabPage;
		private System.Windows.Forms.Button restoreDefaultsButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		/// 
		private System.ComponentModel.Container components = null;

		// The game options.
        public Options Options;
		private GroupBox groupBox2;
		private GroupBox groupBox1;
		private TableLayoutPanel tableLayoutPanel1;
		private TrackBar blackDifficultyTB;
		private TrackBar whiteDifficultyTB;
		private Label blackDifficultyLBL;
		private Label whiteDifficultyLBL;
		private RadioButton whiteFullSailAIRadioButton;
        private RadioButton blackFullSailAIRadioButton;
        private TabPage fullSailAiTabPage;
        private TableLayoutPanel tableLayoutPanel2;
        private GroupBox groupBox3;
        private ComboBox blackFullSailAiComboBox;
        private CheckBox blackTranspositionTableCheckBox;
        private TextBox blackMctsSimulationsTextBox;
        private Label blackSimulationsPerMoveLabel;
        private TextBox blackCachingMemoryTextBox;
        private Label blackCachingMemoryLabel;
        private GroupBox groupBox4;
        private ComboBox whiteFullSailAiComboBox;
        private CheckBox whiteTranspositionTableCheckBox;
        private TextBox whiteMctsSimulationsTextBox;
        private Label whiteSimulationsPerMoveLabel;
        private TextBox whiteCachingMemoryTextBox;
        private Label whiteCachingMemoryLabel;

		// An array to store custom colors added by the user.
		private static int[] customColors = new int[] {};

		public OptionsDialog(Options options)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			// Create a copy of the given game options.
			this.Options = new Options(options);

			// Set the form controls based on those options.
			this.MapOptionsToControls();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.optionsTabControl = new System.Windows.Forms.TabControl();
            this.displayTabPage = new System.Windows.Forms.TabPage();
            this.previewMovesCheckBox = new System.Windows.Forms.CheckBox();
            this.moveIndicatorColorButton = new System.Windows.Forms.Button();
            this.moveIndicatorColorPanel = new System.Windows.Forms.Panel();
            this.moveIndicatorColorLabel = new System.Windows.Forms.Label();
            this.animateMovesCheckBox = new System.Windows.Forms.CheckBox();
            this.validColorButton = new System.Windows.Forms.Button();
            this.validColorPanel = new System.Windows.Forms.Panel();
            this.validColorLabel = new System.Windows.Forms.Label();
            this.activeColorButton = new System.Windows.Forms.Button();
            this.activeColorPanel = new System.Windows.Forms.Panel();
            this.activeColorLabel = new System.Windows.Forms.Label();
            this.boardColorButton = new System.Windows.Forms.Button();
            this.boardColorPanel = new System.Windows.Forms.Panel();
            this.boardColorLabel = new System.Windows.Forms.Label();
            this.showValidMovesCheckBox = new System.Windows.Forms.CheckBox();
            this.playersTabPage = new System.Windows.Forms.TabPage();
            this.whitePlayerPanel = new System.Windows.Forms.Panel();
            this.whitePlayerUserRadioButton = new System.Windows.Forms.RadioButton();
            this.whiteFullSailAIRadioButton = new System.Windows.Forms.RadioButton();
            this.whitePlayerComputerRadioButton = new System.Windows.Forms.RadioButton();
            this.whitePlayerLabel = new System.Windows.Forms.Label();
            this.blackPlayerPanel = new System.Windows.Forms.Panel();
            this.blackPlayerUserRadioButton = new System.Windows.Forms.RadioButton();
            this.blackFullSailAIRadioButton = new System.Windows.Forms.RadioButton();
            this.blackPlayerComputerRadioButton = new System.Windows.Forms.RadioButton();
            this.blackPlayerLabel = new System.Windows.Forms.Label();
            this.difficultyTabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.blackDifficultyLBL = new System.Windows.Forms.Label();
            this.blackDifficultyTB = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.whiteDifficultyLBL = new System.Windows.Forms.Label();
            this.whiteDifficultyTB = new System.Windows.Forms.TrackBar();
            this.fullSailAiTabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.whiteTranspositionTableCheckBox = new System.Windows.Forms.CheckBox();
            this.whiteFullSailAiComboBox = new System.Windows.Forms.ComboBox();
            this.whiteCachingMemoryLabel = new System.Windows.Forms.Label();
            this.whiteCachingMemoryTextBox = new System.Windows.Forms.TextBox();
            this.whiteMctsSimulationsTextBox = new System.Windows.Forms.TextBox();
            this.whiteSimulationsPerMoveLabel = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.blackTranspositionTableCheckBox = new System.Windows.Forms.CheckBox();
            this.blackFullSailAiComboBox = new System.Windows.Forms.ComboBox();
            this.blackCachingMemoryLabel = new System.Windows.Forms.Label();
            this.blackCachingMemoryTextBox = new System.Windows.Forms.TextBox();
            this.blackMctsSimulationsTextBox = new System.Windows.Forms.TextBox();
            this.blackSimulationsPerMoveLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.restoreDefaultsButton = new System.Windows.Forms.Button();
            this.optionsTabControl.SuspendLayout();
            this.displayTabPage.SuspendLayout();
            this.playersTabPage.SuspendLayout();
            this.whitePlayerPanel.SuspendLayout();
            this.blackPlayerPanel.SuspendLayout();
            this.difficultyTabPage.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blackDifficultyTB)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.whiteDifficultyTB)).BeginInit();
            this.fullSailAiTabPage.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // optionsTabControl
            // 
            this.optionsTabControl.Controls.Add(this.displayTabPage);
            this.optionsTabControl.Controls.Add(this.playersTabPage);
            this.optionsTabControl.Controls.Add(this.difficultyTabPage);
            this.optionsTabControl.Controls.Add(this.fullSailAiTabPage);
            this.optionsTabControl.Location = new System.Drawing.Point(13, 23);
            this.optionsTabControl.Name = "optionsTabControl";
            this.optionsTabControl.SelectedIndex = 0;
            this.optionsTabControl.Size = new System.Drawing.Size(461, 322);
            this.optionsTabControl.TabIndex = 0;
            // 
            // displayTabPage
            // 
            this.displayTabPage.Controls.Add(this.previewMovesCheckBox);
            this.displayTabPage.Controls.Add(this.moveIndicatorColorButton);
            this.displayTabPage.Controls.Add(this.moveIndicatorColorPanel);
            this.displayTabPage.Controls.Add(this.moveIndicatorColorLabel);
            this.displayTabPage.Controls.Add(this.animateMovesCheckBox);
            this.displayTabPage.Controls.Add(this.validColorButton);
            this.displayTabPage.Controls.Add(this.validColorPanel);
            this.displayTabPage.Controls.Add(this.validColorLabel);
            this.displayTabPage.Controls.Add(this.activeColorButton);
            this.displayTabPage.Controls.Add(this.activeColorPanel);
            this.displayTabPage.Controls.Add(this.activeColorLabel);
            this.displayTabPage.Controls.Add(this.boardColorButton);
            this.displayTabPage.Controls.Add(this.boardColorPanel);
            this.displayTabPage.Controls.Add(this.boardColorLabel);
            this.displayTabPage.Controls.Add(this.showValidMovesCheckBox);
            this.displayTabPage.Location = new System.Drawing.Point(4, 29);
            this.displayTabPage.Name = "displayTabPage";
            this.displayTabPage.Size = new System.Drawing.Size(453, 289);
            this.displayTabPage.TabIndex = 0;
            this.displayTabPage.Text = "Display";
            // 
            // previewMovesCheckBox
            // 
            this.previewMovesCheckBox.Location = new System.Drawing.Point(230, 15);
            this.previewMovesCheckBox.Name = "previewMovesCheckBox";
            this.previewMovesCheckBox.Size = new System.Drawing.Size(167, 35);
            this.previewMovesCheckBox.TabIndex = 1;
            this.previewMovesCheckBox.Text = "Preview moves";
            // 
            // moveIndicatorColorButton
            // 
            this.moveIndicatorColorButton.Location = new System.Drawing.Point(280, 229);
            this.moveIndicatorColorButton.Name = "moveIndicatorColorButton";
            this.moveIndicatorColorButton.Size = new System.Drawing.Size(120, 34);
            this.moveIndicatorColorButton.TabIndex = 15;
            this.moveIndicatorColorButton.Text = "Select...";
            this.moveIndicatorColorButton.Click += new System.EventHandler(this.moveIndicatorColorButton_Click);
            // 
            // moveIndicatorColorPanel
            // 
            this.moveIndicatorColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.moveIndicatorColorPanel.Location = new System.Drawing.Point(229, 234);
            this.moveIndicatorColorPanel.Name = "moveIndicatorColorPanel";
            this.moveIndicatorColorPanel.Size = new System.Drawing.Size(25, 23);
            this.moveIndicatorColorPanel.TabIndex = 14;
            // 
            // moveIndicatorColorLabel
            // 
            this.moveIndicatorColorLabel.AutoSize = true;
            this.moveIndicatorColorLabel.Location = new System.Drawing.Point(43, 237);
            this.moveIndicatorColorLabel.Name = "moveIndicatorColorLabel";
            this.moveIndicatorColorLabel.Size = new System.Drawing.Size(153, 20);
            this.moveIndicatorColorLabel.TabIndex = 13;
            this.moveIndicatorColorLabel.Text = "Move indicator color:";
            // 
            // animateMovesCheckBox
            // 
            this.animateMovesCheckBox.Location = new System.Drawing.Point(26, 50);
            this.animateMovesCheckBox.Name = "animateMovesCheckBox";
            this.animateMovesCheckBox.Size = new System.Drawing.Size(166, 35);
            this.animateMovesCheckBox.TabIndex = 2;
            this.animateMovesCheckBox.Text = "Animate moves";
            // 
            // validColorButton
            // 
            this.validColorButton.Location = new System.Drawing.Point(280, 136);
            this.validColorButton.Name = "validColorButton";
            this.validColorButton.Size = new System.Drawing.Size(120, 34);
            this.validColorButton.TabIndex = 9;
            this.validColorButton.Text = "Select...";
            this.validColorButton.Click += new System.EventHandler(this.validColorButton_Click);
            // 
            // validColorPanel
            // 
            this.validColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.validColorPanel.Location = new System.Drawing.Point(229, 140);
            this.validColorPanel.Name = "validColorPanel";
            this.validColorPanel.Size = new System.Drawing.Size(25, 24);
            this.validColorPanel.TabIndex = 8;
            // 
            // validColorLabel
            // 
            this.validColorLabel.AutoSize = true;
            this.validColorLabel.Location = new System.Drawing.Point(72, 143);
            this.validColorLabel.Name = "validColorLabel";
            this.validColorLabel.Size = new System.Drawing.Size(128, 20);
            this.validColorLabel.TabIndex = 7;
            this.validColorLabel.Text = "Valid move color:";
            // 
            // activeColorButton
            // 
            this.activeColorButton.Location = new System.Drawing.Point(280, 183);
            this.activeColorButton.Name = "activeColorButton";
            this.activeColorButton.Size = new System.Drawing.Size(120, 33);
            this.activeColorButton.TabIndex = 12;
            this.activeColorButton.Text = "Select...";
            this.activeColorButton.Click += new System.EventHandler(this.activeColorButton_Click);
            // 
            // activeColorPanel
            // 
            this.activeColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.activeColorPanel.Location = new System.Drawing.Point(229, 187);
            this.activeColorPanel.Name = "activeColorPanel";
            this.activeColorPanel.Size = new System.Drawing.Size(25, 23);
            this.activeColorPanel.TabIndex = 11;
            // 
            // activeColorLabel
            // 
            this.activeColorLabel.AutoSize = true;
            this.activeColorLabel.Location = new System.Drawing.Point(51, 190);
            this.activeColorLabel.Name = "activeColorLabel";
            this.activeColorLabel.Size = new System.Drawing.Size(147, 20);
            this.activeColorLabel.TabIndex = 10;
            this.activeColorLabel.Text = "Active square color:";
            // 
            // boardColorButton
            // 
            this.boardColorButton.Location = new System.Drawing.Point(280, 89);
            this.boardColorButton.Name = "boardColorButton";
            this.boardColorButton.Size = new System.Drawing.Size(120, 34);
            this.boardColorButton.TabIndex = 6;
            this.boardColorButton.Text = "Select...";
            this.boardColorButton.Click += new System.EventHandler(this.boardColorButton_Click);
            // 
            // boardColorPanel
            // 
            this.boardColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.boardColorPanel.Location = new System.Drawing.Point(229, 94);
            this.boardColorPanel.Name = "boardColorPanel";
            this.boardColorPanel.Size = new System.Drawing.Size(25, 23);
            this.boardColorPanel.TabIndex = 5;
            // 
            // boardColorLabel
            // 
            this.boardColorLabel.AutoSize = true;
            this.boardColorLabel.Location = new System.Drawing.Point(114, 96);
            this.boardColorLabel.Name = "boardColorLabel";
            this.boardColorLabel.Size = new System.Drawing.Size(94, 20);
            this.boardColorLabel.TabIndex = 4;
            this.boardColorLabel.Text = "Board color:";
            // 
            // showValidMovesCheckBox
            // 
            this.showValidMovesCheckBox.Location = new System.Drawing.Point(26, 15);
            this.showValidMovesCheckBox.Name = "showValidMovesCheckBox";
            this.showValidMovesCheckBox.Size = new System.Drawing.Size(192, 35);
            this.showValidMovesCheckBox.TabIndex = 0;
            this.showValidMovesCheckBox.Text = "Show valid moves";
            // 
            // playersTabPage
            // 
            this.playersTabPage.Controls.Add(this.whitePlayerPanel);
            this.playersTabPage.Controls.Add(this.blackPlayerPanel);
            this.playersTabPage.Location = new System.Drawing.Point(4, 29);
            this.playersTabPage.Name = "playersTabPage";
            this.playersTabPage.Size = new System.Drawing.Size(453, 289);
            this.playersTabPage.TabIndex = 1;
            this.playersTabPage.Text = "Players";
            // 
            // whitePlayerPanel
            // 
            this.whitePlayerPanel.Controls.Add(this.whitePlayerUserRadioButton);
            this.whitePlayerPanel.Controls.Add(this.whiteFullSailAIRadioButton);
            this.whitePlayerPanel.Controls.Add(this.whitePlayerComputerRadioButton);
            this.whitePlayerPanel.Controls.Add(this.whitePlayerLabel);
            this.whitePlayerPanel.Location = new System.Drawing.Point(64, 145);
            this.whitePlayerPanel.Name = "whitePlayerPanel";
            this.whitePlayerPanel.Size = new System.Drawing.Size(320, 120);
            this.whitePlayerPanel.TabIndex = 5;
            // 
            // whitePlayerUserRadioButton
            // 
            this.whitePlayerUserRadioButton.Location = new System.Drawing.Point(141, 79);
            this.whitePlayerUserRadioButton.Name = "whitePlayerUserRadioButton";
            this.whitePlayerUserRadioButton.Size = new System.Drawing.Size(166, 35);
            this.whitePlayerUserRadioButton.TabIndex = 1;
            this.whitePlayerUserRadioButton.Text = "User";
            // 
            // whiteFullSailAIRadioButton
            // 
            this.whiteFullSailAIRadioButton.Location = new System.Drawing.Point(141, 9);
            this.whiteFullSailAIRadioButton.Name = "whiteFullSailAIRadioButton";
            this.whiteFullSailAIRadioButton.Size = new System.Drawing.Size(166, 35);
            this.whiteFullSailAIRadioButton.TabIndex = 0;
            this.whiteFullSailAIRadioButton.Text = "Full Sail AI";
            // 
            // whitePlayerComputerRadioButton
            // 
            this.whitePlayerComputerRadioButton.Location = new System.Drawing.Point(141, 44);
            this.whitePlayerComputerRadioButton.Name = "whitePlayerComputerRadioButton";
            this.whitePlayerComputerRadioButton.Size = new System.Drawing.Size(166, 35);
            this.whitePlayerComputerRadioButton.TabIndex = 0;
            this.whitePlayerComputerRadioButton.Text = "Student AI";
            // 
            // whitePlayerLabel
            // 
            this.whitePlayerLabel.AutoSize = true;
            this.whitePlayerLabel.Location = new System.Drawing.Point(13, 18);
            this.whitePlayerLabel.Name = "whitePlayerLabel";
            this.whitePlayerLabel.Size = new System.Drawing.Size(100, 20);
            this.whitePlayerLabel.TabIndex = 4;
            this.whitePlayerLabel.Text = "White player:";
            // 
            // blackPlayerPanel
            // 
            this.blackPlayerPanel.Controls.Add(this.blackPlayerUserRadioButton);
            this.blackPlayerPanel.Controls.Add(this.blackFullSailAIRadioButton);
            this.blackPlayerPanel.Controls.Add(this.blackPlayerComputerRadioButton);
            this.blackPlayerPanel.Controls.Add(this.blackPlayerLabel);
            this.blackPlayerPanel.Location = new System.Drawing.Point(64, 13);
            this.blackPlayerPanel.Name = "blackPlayerPanel";
            this.blackPlayerPanel.Size = new System.Drawing.Size(320, 116);
            this.blackPlayerPanel.TabIndex = 3;
            // 
            // blackPlayerUserRadioButton
            // 
            this.blackPlayerUserRadioButton.Location = new System.Drawing.Point(141, 79);
            this.blackPlayerUserRadioButton.Name = "blackPlayerUserRadioButton";
            this.blackPlayerUserRadioButton.Size = new System.Drawing.Size(166, 35);
            this.blackPlayerUserRadioButton.TabIndex = 2;
            this.blackPlayerUserRadioButton.Text = "User";
            // 
            // blackFullSailAIRadioButton
            // 
            this.blackFullSailAIRadioButton.Location = new System.Drawing.Point(141, 9);
            this.blackFullSailAIRadioButton.Name = "blackFullSailAIRadioButton";
            this.blackFullSailAIRadioButton.Size = new System.Drawing.Size(166, 35);
            this.blackFullSailAIRadioButton.TabIndex = 0;
            this.blackFullSailAIRadioButton.Text = "Full Sail AI";
            // 
            // blackPlayerComputerRadioButton
            // 
            this.blackPlayerComputerRadioButton.Location = new System.Drawing.Point(141, 44);
            this.blackPlayerComputerRadioButton.Name = "blackPlayerComputerRadioButton";
            this.blackPlayerComputerRadioButton.Size = new System.Drawing.Size(166, 35);
            this.blackPlayerComputerRadioButton.TabIndex = 1;
            this.blackPlayerComputerRadioButton.Text = "Student AI";
            // 
            // blackPlayerLabel
            // 
            this.blackPlayerLabel.AutoSize = true;
            this.blackPlayerLabel.Location = new System.Drawing.Point(14, 18);
            this.blackPlayerLabel.Name = "blackPlayerLabel";
            this.blackPlayerLabel.Size = new System.Drawing.Size(98, 20);
            this.blackPlayerLabel.TabIndex = 2;
            this.blackPlayerLabel.Text = "Black player:";
            // 
            // difficultyTabPage
            // 
            this.difficultyTabPage.Controls.Add(this.tableLayoutPanel1);
            this.difficultyTabPage.Location = new System.Drawing.Point(4, 29);
            this.difficultyTabPage.Name = "difficultyTabPage";
            this.difficultyTabPage.Size = new System.Drawing.Size(453, 289);
            this.difficultyTabPage.TabIndex = 2;
            this.difficultyTabPage.Text = "Difficulty";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(453, 289);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.blackDifficultyLBL);
            this.groupBox1.Controls.Add(this.blackDifficultyTB);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(447, 132);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Black Player";
            // 
            // blackDifficultyLBL
            // 
            this.blackDifficultyLBL.AutoSize = true;
            this.blackDifficultyLBL.Location = new System.Drawing.Point(13, 90);
            this.blackDifficultyLBL.Name = "blackDifficultyLBL";
            this.blackDifficultyLBL.Size = new System.Drawing.Size(237, 20);
            this.blackDifficultyLBL.TabIndex = 1;
            this.blackDifficultyLBL.Text = "Look Ahead Depth: 0 - Beginner";
            // 
            // blackDifficultyTB
            // 
            this.blackDifficultyTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blackDifficultyTB.AutoSize = false;
            this.blackDifficultyTB.LargeChange = 1;
            this.blackDifficultyTB.Location = new System.Drawing.Point(6, 35);
            this.blackDifficultyTB.Name = "blackDifficultyTB";
            this.blackDifficultyTB.Size = new System.Drawing.Size(428, 41);
            this.blackDifficultyTB.TabIndex = 0;
            this.blackDifficultyTB.Scroll += new System.EventHandler(this.blackDifficultyTB_Scroll);
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.whiteDifficultyLBL);
            this.groupBox2.Controls.Add(this.whiteDifficultyTB);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(447, 132);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "White Player";
            // 
            // whiteDifficultyLBL
            // 
            this.whiteDifficultyLBL.AutoSize = true;
            this.whiteDifficultyLBL.Location = new System.Drawing.Point(13, 90);
            this.whiteDifficultyLBL.Name = "whiteDifficultyLBL";
            this.whiteDifficultyLBL.Size = new System.Drawing.Size(237, 20);
            this.whiteDifficultyLBL.TabIndex = 1;
            this.whiteDifficultyLBL.Text = "Look Ahead Depth: 0 - Beginner";
            // 
            // whiteDifficultyTB
            // 
            this.whiteDifficultyTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.whiteDifficultyTB.AutoSize = false;
            this.whiteDifficultyTB.LargeChange = 1;
            this.whiteDifficultyTB.Location = new System.Drawing.Point(6, 35);
            this.whiteDifficultyTB.Name = "whiteDifficultyTB";
            this.whiteDifficultyTB.Size = new System.Drawing.Size(428, 41);
            this.whiteDifficultyTB.TabIndex = 0;
            this.whiteDifficultyTB.Scroll += new System.EventHandler(this.whiteDifficultyTB_Scroll);
            // 
            // fullSailAiTabPage
            // 
            this.fullSailAiTabPage.Controls.Add(this.tableLayoutPanel2);
            this.fullSailAiTabPage.Location = new System.Drawing.Point(4, 29);
            this.fullSailAiTabPage.Name = "fullSailAiTabPage";
            this.fullSailAiTabPage.Size = new System.Drawing.Size(453, 289);
            this.fullSailAiTabPage.TabIndex = 3;
            this.fullSailAiTabPage.Text = "Full Sail AI";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox4, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(453, 296);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // groupBox4
            // 
            this.groupBox4.AutoSize = true;
            this.groupBox4.Controls.Add(this.whiteTranspositionTableCheckBox);
            this.groupBox4.Controls.Add(this.whiteFullSailAiComboBox);
            this.groupBox4.Controls.Add(this.whiteCachingMemoryLabel);
            this.groupBox4.Controls.Add(this.whiteCachingMemoryTextBox);
            this.groupBox4.Controls.Add(this.whiteMctsSimulationsTextBox);
            this.groupBox4.Controls.Add(this.whiteSimulationsPerMoveLabel);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 141);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(447, 132);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "White Full Sail AI";
            // 
            // whiteTranspositionTableCheckBox
            // 
            this.whiteTranspositionTableCheckBox.Location = new System.Drawing.Point(272, 27);
            this.whiteTranspositionTableCheckBox.Name = "whiteTranspositionTableCheckBox";
            this.whiteTranspositionTableCheckBox.Size = new System.Drawing.Size(169, 24);
            this.whiteTranspositionTableCheckBox.TabIndex = 1;
            this.whiteTranspositionTableCheckBox.Text = "Transposition table";
            this.whiteTranspositionTableCheckBox.CheckedChanged += new System.EventHandler(this.whiteTranspositionTableCheckBox_CheckedChanged);
            // 
            // whiteFullSailAiComboBox
            // 
            this.whiteFullSailAiComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.whiteFullSailAiComboBox.FormattingEnabled = true;
            this.whiteFullSailAiComboBox.Items.AddRange(new object[] {
            "Regular minimax",
            "Alpha-beta pruning",
            "Alpha-beta pruning with ID",
            "PVS with ID",
            "MCTS (ignores difficulty)"});
            this.whiteFullSailAiComboBox.Location = new System.Drawing.Point(11, 25);
            this.whiteFullSailAiComboBox.Name = "whiteFullSailAiComboBox";
            this.whiteFullSailAiComboBox.Size = new System.Drawing.Size(243, 28);
            this.whiteFullSailAiComboBox.TabIndex = 0;
            this.whiteFullSailAiComboBox.SelectedIndexChanged += new System.EventHandler(this.whiteFullSailAiComboBox_SelectedIndexChanged);
            // 
            // whiteCachingMemoryLabel
            // 
            this.whiteCachingMemoryLabel.Enabled = false;
            this.whiteCachingMemoryLabel.Location = new System.Drawing.Point(152, 66);
            this.whiteCachingMemoryLabel.Name = "whiteCachingMemoryLabel";
            this.whiteCachingMemoryLabel.Size = new System.Drawing.Size(286, 22);
            this.whiteCachingMemoryLabel.TabIndex = 14;
            this.whiteCachingMemoryLabel.Text = "MB cache (suggested, needs TT/ID)";
            // 
            // whiteCachingMemoryTextBox
            // 
            this.whiteCachingMemoryTextBox.Enabled = false;
            this.whiteCachingMemoryTextBox.Location = new System.Drawing.Point(46, 62);
            this.whiteCachingMemoryTextBox.Name = "whiteCachingMemoryTextBox";
            this.whiteCachingMemoryTextBox.Size = new System.Drawing.Size(100, 26);
            this.whiteCachingMemoryTextBox.TabIndex = 13;
            this.whiteCachingMemoryTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // whiteMctsSimulationsTextBox
            // 
            this.whiteMctsSimulationsTextBox.Enabled = false;
            this.whiteMctsSimulationsTextBox.Location = new System.Drawing.Point(46, 97);
            this.whiteMctsSimulationsTextBox.Name = "whiteMctsSimulationsTextBox";
            this.whiteMctsSimulationsTextBox.Size = new System.Drawing.Size(100, 26);
            this.whiteMctsSimulationsTextBox.TabIndex = 2;
            this.whiteMctsSimulationsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // whiteSimulationsPerMoveLabel
            // 
            this.whiteSimulationsPerMoveLabel.Enabled = false;
            this.whiteSimulationsPerMoveLabel.Location = new System.Drawing.Point(152, 100);
            this.whiteSimulationsPerMoveLabel.Name = "whiteSimulationsPerMoveLabel";
            this.whiteSimulationsPerMoveLabel.Size = new System.Drawing.Size(222, 20);
            this.whiteSimulationsPerMoveLabel.TabIndex = 13;
            this.whiteSimulationsPerMoveLabel.Text = "MCTS simulations per move";
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.Controls.Add(this.blackTranspositionTableCheckBox);
            this.groupBox3.Controls.Add(this.blackFullSailAiComboBox);
            this.groupBox3.Controls.Add(this.blackCachingMemoryLabel);
            this.groupBox3.Controls.Add(this.blackCachingMemoryTextBox);
            this.groupBox3.Controls.Add(this.blackMctsSimulationsTextBox);
            this.groupBox3.Controls.Add(this.blackSimulationsPerMoveLabel);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(447, 132);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Black Full Sail AI";
            // 
            // blackTranspositionTableCheckBox
            // 
            this.blackTranspositionTableCheckBox.Location = new System.Drawing.Point(272, 27);
            this.blackTranspositionTableCheckBox.Name = "blackTranspositionTableCheckBox";
            this.blackTranspositionTableCheckBox.Size = new System.Drawing.Size(169, 24);
            this.blackTranspositionTableCheckBox.TabIndex = 1;
            this.blackTranspositionTableCheckBox.Text = "Transposition table";
            this.blackTranspositionTableCheckBox.CheckedChanged += new System.EventHandler(this.blackTranspositionTableCheckBox_CheckedChanged);
            // 
            // blackFullSailAiComboBox
            // 
            this.blackFullSailAiComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.blackFullSailAiComboBox.FormattingEnabled = true;
            this.blackFullSailAiComboBox.Items.AddRange(new object[] {
            "Regular minimax",
            "Alpha-beta pruning",
            "Alpha-beta pruning with ID",
            "PVS with ID",
            "MCTS (ignores difficulty)"});
            this.blackFullSailAiComboBox.Location = new System.Drawing.Point(11, 25);
            this.blackFullSailAiComboBox.Name = "blackFullSailAiComboBox";
            this.blackFullSailAiComboBox.Size = new System.Drawing.Size(243, 28);
            this.blackFullSailAiComboBox.TabIndex = 0;
            this.blackFullSailAiComboBox.SelectedIndexChanged += new System.EventHandler(this.blackFullSailAiComboBox_SelectedIndexChanged);
            // 
            // blackCachingMemoryLabel
            // 
            this.blackCachingMemoryLabel.Enabled = false;
            this.blackCachingMemoryLabel.Location = new System.Drawing.Point(152, 66);
            this.blackCachingMemoryLabel.Name = "blackCachingMemoryLabel";
            this.blackCachingMemoryLabel.Size = new System.Drawing.Size(286, 22);
            this.blackCachingMemoryLabel.TabIndex = 8;
            this.blackCachingMemoryLabel.Text = "MB cache (suggested, needs TT/ID)";
            // 
            // blackCachingMemoryTextBox
            // 
            this.blackCachingMemoryTextBox.Enabled = false;
            this.blackCachingMemoryTextBox.Location = new System.Drawing.Point(46, 62);
            this.blackCachingMemoryTextBox.Name = "blackCachingMemoryTextBox";
            this.blackCachingMemoryTextBox.Size = new System.Drawing.Size(100, 26);
            this.blackCachingMemoryTextBox.TabIndex = 3;
            this.blackCachingMemoryTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.blackCachingMemoryTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // blackMctsSimulationsTextBox
            // 
            this.blackMctsSimulationsTextBox.Enabled = false;
            this.blackMctsSimulationsTextBox.Location = new System.Drawing.Point(46, 97);
            this.blackMctsSimulationsTextBox.Name = "blackMctsSimulationsTextBox";
            this.blackMctsSimulationsTextBox.Size = new System.Drawing.Size(100, 26);
            this.blackMctsSimulationsTextBox.TabIndex = 2;
            this.blackMctsSimulationsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.blackMctsSimulationsTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // blackSimulationsPerMoveLabel
            // 
            this.blackSimulationsPerMoveLabel.Enabled = false;
            this.blackSimulationsPerMoveLabel.Location = new System.Drawing.Point(152, 100);
            this.blackSimulationsPerMoveLabel.Name = "blackSimulationsPerMoveLabel";
            this.blackSimulationsPerMoveLabel.Size = new System.Drawing.Size(222, 20);
            this.blackSimulationsPerMoveLabel.TabIndex = 7;
            this.blackSimulationsPerMoveLabel.Text = "MCTS simulations per move";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(218, 351);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(120, 33);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(354, 351);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(120, 33);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            // 
            // restoreDefaultsButton
            // 
            this.restoreDefaultsButton.Location = new System.Drawing.Point(13, 351);
            this.restoreDefaultsButton.Name = "restoreDefaultsButton";
            this.restoreDefaultsButton.Size = new System.Drawing.Size(153, 33);
            this.restoreDefaultsButton.TabIndex = 3;
            this.restoreDefaultsButton.Text = "Restore Defaults";
            this.restoreDefaultsButton.Click += new System.EventHandler(this.restoreDefaultsButton_Click);
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(487, 397);
            this.ControlBox = false;
            this.Controls.Add(this.restoreDefaultsButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.optionsTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.optionsTabControl.ResumeLayout(false);
            this.displayTabPage.ResumeLayout(false);
            this.displayTabPage.PerformLayout();
            this.playersTabPage.ResumeLayout(false);
            this.whitePlayerPanel.ResumeLayout(false);
            this.whitePlayerPanel.PerformLayout();
            this.blackPlayerPanel.ResumeLayout(false);
            this.blackPlayerPanel.PerformLayout();
            this.difficultyTabPage.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blackDifficultyTB)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.whiteDifficultyTB)).EndInit();
            this.fullSailAiTabPage.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		//
		// Sets the form controls based on the current game options.
		//
		private void MapOptionsToControls()
		{
			this.showValidMovesCheckBox.Checked             = this.Options.ShowValidMoves;
			this.previewMovesCheckBox.Checked               = this.Options.PreviewMoves;
			this.animateMovesCheckBox.Checked               = this.Options.AnimateMoves;
			this.boardColorPanel.BackColor                  = this.Options.BoardColor;
			this.validColorPanel.BackColor                  = this.Options.ValidMoveColor;
			this.activeColorPanel.BackColor                 = this.Options.ActiveSquareColor;
			this.moveIndicatorColorPanel.BackColor          = this.Options.MoveIndicatorColor;

			this.blackPlayerComputerRadioButton.Checked     = this.Options.ComputerPlaysBlack & (!this.Options.BlackUsesFullSailAI);
			this.blackPlayerUserRadioButton.Checked         = !this.Options.ComputerPlaysBlack;
			this.blackFullSailAIRadioButton.Checked         = this.Options.ComputerPlaysBlack & this.Options.BlackUsesFullSailAI;
			this.whitePlayerComputerRadioButton.Checked     = this.Options.ComputerPlaysWhite & (!this.Options.WhiteUsesFullSailAI);
			this.whitePlayerUserRadioButton.Checked         = !this.Options.ComputerPlaysWhite;
			this.whiteFullSailAIRadioButton.Checked         = this.Options.ComputerPlaysWhite & this.Options.WhiteUsesFullSailAI;

            this.blackDifficultyTB.Value = this.Options.BlackDifficulty;
            this.blackDifficultyLBL.Text = "Look Ahead Depth: " + blackDifficultyTB.Value;
			switch(this.Options.BlackDifficulty)
			{
				case 0:
                    this.blackDifficultyLBL.Text += " - Beginner";
					break;
                case 1:
					this.blackDifficultyLBL.Text += " - Intermediate";
					break;
				case 3:
					this.blackDifficultyLBL.Text += " - Advanced";
					break;
			}
            this.whiteDifficultyTB.Value = this.Options.WhiteDifficulty;
            this.whiteDifficultyLBL.Text = "Look Ahead Depth: " + whiteDifficultyTB.Value;
			switch (this.Options.WhiteDifficulty)
			{
                case 0:
                    this.whiteDifficultyLBL.Text += " - Beginner";
                    break;
                case 1:
                    this.whiteDifficultyLBL.Text += " - Intermediate";
                    break;
                case 3:
                    this.whiteDifficultyLBL.Text += " - Advanced";
                    break;
			}

            this.blackFullSailAiComboBox.SelectedIndex      = this.Options.BlackFullSailAiMode;
            this.blackTranspositionTableCheckBox.Checked    = this.Options.BlackTranspositionTable;
            this.blackMctsSimulationsTextBox.Text           = this.Options.BlackMctsSimulationNumber.ToString();
            this.blackCachingMemoryTextBox.Text             = this.Options.BlackCachingMemoryNumber.ToString();
            this.whiteFullSailAiComboBox.SelectedIndex      = this.Options.WhiteFullSailAiMode;
            this.whiteTranspositionTableCheckBox.Checked    = this.Options.WhiteTranspositionTable;
            this.whiteMctsSimulationsTextBox.Text           = this.Options.WhiteMctsSimulationNumber.ToString();
            this.whiteCachingMemoryTextBox.Text             = this.Options.WhiteCachingMemoryNumber.ToString();

			this.Refresh();
		}

		//
		// Sets the game options based on the current state of the form
		// controls.
		//
		private void MapControlsToOptions()
		{
			this.Options.ShowValidMoves         = this.showValidMovesCheckBox.Checked;
			this.Options.PreviewMoves           = this.previewMovesCheckBox.Checked;
			this.Options.AnimateMoves           = this.animateMovesCheckBox.Checked;
			this.Options.BoardColor             = this.boardColorPanel.BackColor;
			this.Options.ValidMoveColor         = this.validColorPanel.BackColor;
			this.Options.ActiveSquareColor      = this.activeColorPanel.BackColor;
			this.Options.MoveIndicatorColor     = this.moveIndicatorColorPanel.BackColor;

			if (this.blackPlayerComputerRadioButton.Checked)
			{
				this.Options.ComputerPlaysBlack = true;
				this.Options.BlackUsesFullSailAI = false;
			}
			else if (this.blackFullSailAIRadioButton.Checked)
			{
				this.Options.ComputerPlaysBlack = true;
				this.Options.BlackUsesFullSailAI = true;
			}
			else
				this.Options.ComputerPlaysBlack = false;
			if (this.whitePlayerComputerRadioButton.Checked)
			{
				this.Options.ComputerPlaysWhite = true;
				this.Options.WhiteUsesFullSailAI = false;
			}
			else if (this.whiteFullSailAIRadioButton.Checked)
			{
				this.Options.ComputerPlaysWhite = true;
				this.Options.WhiteUsesFullSailAI = true;
			}
			else
				this.Options.ComputerPlaysWhite = false;

			this.Options.BlackDifficulty = blackDifficultyTB.Value;
			this.Options.WhiteDifficulty = whiteDifficultyTB.Value;

            this.Options.BlackFullSailAiMode        = this.blackFullSailAiComboBox.SelectedIndex;
            this.Options.BlackTranspositionTable    = this.blackTranspositionTableCheckBox.Checked;
            this.Options.BlackMctsSimulationNumber  = Int32.Parse(this.blackMctsSimulationsTextBox.Text);
            this.Options.BlackCachingMemoryNumber   = Int32.Parse(this.blackCachingMemoryTextBox.Text);
            this.Options.WhiteFullSailAiMode        = this.whiteFullSailAiComboBox.SelectedIndex;
            this.Options.WhiteTranspositionTable    = this.whiteTranspositionTableCheckBox.Checked;
            this.Options.WhiteMctsSimulationNumber  = Int32.Parse(this.whiteMctsSimulationsTextBox.Text);
            this.Options.WhiteCachingMemoryNumber   = Int32.Parse(this.whiteCachingMemoryTextBox.Text);
		}

		// ===================================================================
		// Event handlers for the color select buttons.
		// ===================================================================

		private void boardColorButton_Click(object sender, System.EventArgs e)
		{
			// Open a color dialog.
			ColorDialog dlg = new ColorDialog();
			dlg.Color = this.boardColorPanel.BackColor;
			dlg.CustomColors = OptionsDialog.customColors;

			// Set the board color based on that selection.
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				this.boardColorPanel.BackColor = dlg.Color;
				this.boardColorPanel.Refresh();
				OptionsDialog.customColors = dlg.CustomColors;
			}
		}

		private void validColorButton_Click(object sender, System.EventArgs e)
		{
			// Open a color dialog.
			ColorDialog dlg = new ColorDialog();
			dlg.Color = this.validColorPanel.BackColor;
			dlg.CustomColors = OptionsDialog.customColors;

			// Set the valid move color based on that selection.
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				this.validColorPanel.BackColor = dlg.Color;
				this.validColorPanel.Refresh();
				OptionsDialog.customColors = dlg.CustomColors;
			}
		}

		private void activeColorButton_Click(object sender, System.EventArgs e)
		{
			// Open a color dialog.
			ColorDialog dlg = new ColorDialog();
			dlg.Color = this.activeColorPanel.BackColor;
			dlg.CustomColors = OptionsDialog.customColors;

			// Set the active square color based on that selection.
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				this.activeColorPanel.BackColor = dlg.Color;
				this.activeColorPanel.Refresh();
				OptionsDialog.customColors = dlg.CustomColors;
			}
		}

		private void moveIndicatorColorButton_Click(object sender, System.EventArgs e)
		{
			// Open a color dialog.
			ColorDialog dlg = new ColorDialog();
			dlg.Color = this.moveIndicatorColorPanel.BackColor;
			dlg.CustomColors = OptionsDialog.customColors;

			// Set the move indicator color based on that selection.
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				this.moveIndicatorColorPanel.BackColor = dlg.Color;
				this.moveIndicatorColorPanel.Refresh();
				OptionsDialog.customColors = dlg.CustomColors;
			}
		}

		// ===================================================================
		// Event handlers for the form buttons.
		// ===================================================================

		private void restoreDefaultsButton_Click(object sender, System.EventArgs e)
		{
			// Reset the game options to their defaults.
			this.Options.RestoreDefaults();

			// Update the form controls.
			this.MapOptionsToControls();
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			// Set game options based on the form control values.
			this.MapControlsToOptions();
		}

		private void blackDifficultyTB_Scroll(object sender, EventArgs e)
		{
            this.blackDifficultyLBL.Text = "Look Ahead Depth: " + this.blackDifficultyTB.Value;
            switch(blackDifficultyTB.Value)
			{
				case 0:
					this.blackDifficultyLBL.Text += " - Beginner";
					break;
				case 1:
					this.blackDifficultyLBL.Text += " - Intermediate";
					break;
				case 3:
					this.blackDifficultyLBL.Text += " - Advanced";
					break;
			}
		}

		private void whiteDifficultyTB_Scroll(object sender, EventArgs e)
		{
            this.whiteDifficultyLBL.Text = "Look Ahead Depth: " + this.whiteDifficultyTB.Value;
            switch (whiteDifficultyTB.Value)
			{
				case 0:
					this.whiteDifficultyLBL.Text += " - Beginner";
					break;
				case 1:
					this.whiteDifficultyLBL.Text += " - Intermediate";
					break;
				case 3:
					this.whiteDifficultyLBL.Text += " - Advanced";
					break;
			}
		}

        // ===================================================================
        // Event handlers for the Full Sail AI form elements.
        // ===================================================================

        private void blackFullSailAiComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool cachingBool = shouldCachingMemoryBeEnabledBlack();
            this.blackCachingMemoryTextBox.Enabled = cachingBool;
            this.blackCachingMemoryLabel.Enabled = cachingBool;

            if (blackFullSailAiComboBox.SelectedIndex == Agent.AI_MCTS_MODE)
            {
                this.blackTranspositionTableCheckBox.Enabled = false;
                this.blackMctsSimulationsTextBox.Enabled = true;
                this.blackSimulationsPerMoveLabel.Enabled = true;
            }
            else  // not MCTS
            {
                this.blackTranspositionTableCheckBox.Enabled = true;
                this.blackMctsSimulationsTextBox.Enabled = false;
                this.blackSimulationsPerMoveLabel.Enabled = false;
            }
        }

        private void whiteFullSailAiComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool cachingBool = shouldCachingMemoryBeEnabledWhite();
            this.whiteCachingMemoryTextBox.Enabled = cachingBool;
            this.whiteCachingMemoryLabel.Enabled = cachingBool;

            if (whiteFullSailAiComboBox.SelectedIndex == Agent.AI_MCTS_MODE)
            {
                this.whiteTranspositionTableCheckBox.Enabled = false;
                this.whiteMctsSimulationsTextBox.Enabled = true;
                this.whiteSimulationsPerMoveLabel.Enabled = true;
            }
            else  // not MCTS
            {
                this.whiteTranspositionTableCheckBox.Enabled = true;
                this.whiteMctsSimulationsTextBox.Enabled = false;
                this.whiteSimulationsPerMoveLabel.Enabled = false;
            }
        }

        private void blackTranspositionTableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool cachingBool = shouldCachingMemoryBeEnabledBlack();
            
            this.blackCachingMemoryTextBox.Enabled = cachingBool;
            this.blackCachingMemoryLabel.Enabled = cachingBool;
        }

        private void whiteTranspositionTableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool cachingBool = shouldCachingMemoryBeEnabledWhite();
            
            this.whiteCachingMemoryTextBox.Enabled = cachingBool;
            this.whiteCachingMemoryLabel.Enabled = cachingBool;
        }

        private bool shouldCachingMemoryBeEnabledBlack()
        {
            if ((!blackTranspositionTableCheckBox.Checked  // no transposition table
                && !(blackFullSailAiComboBox.SelectedIndex == Agent.AI_ALPHA_BETA_ID_MODE)
                && !(blackFullSailAiComboBox.SelectedIndex == Agent.AI_PVS_ID_MODE))  // and neither of the ID modes
                || (blackFullSailAiComboBox.SelectedIndex == Agent.AI_MCTS_MODE))  // or MCTS mode is checked
            {
                return false;  // so disable caching memory input
            }
            else  // one is selected, so enable cache memory MB input (might be redundant)
            {
                return true;
            }
        }

        private bool shouldCachingMemoryBeEnabledWhite()
        {
            if ((!whiteTranspositionTableCheckBox.Checked  // no transposition table
                && !(whiteFullSailAiComboBox.SelectedIndex == Agent.AI_ALPHA_BETA_ID_MODE)
                && !(whiteFullSailAiComboBox.SelectedIndex == Agent.AI_PVS_ID_MODE))  // and neither of the ID modes
                || (whiteFullSailAiComboBox.SelectedIndex == Agent.AI_MCTS_MODE))  // or MCTS mode is checked
            {
                return false;  // so disable caching memory input
            }
            else  // one is selected, so enable cache memory MB input (might be redundant)
            {
                return true;
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))  // only allows numeric integer input
            {
                e.Handled = true;
            }
        }
	}
}
