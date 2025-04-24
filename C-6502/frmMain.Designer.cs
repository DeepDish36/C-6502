namespace C_6502
{
    partial class frmMain
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pctScreen = new System.Windows.Forms.PictureBox();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblFlags = new System.Windows.Forms.Label();
            this.btnStep = new System.Windows.Forms.Button();
            this.btnJumpTo = new System.Windows.Forms.Button();
            this.grpStuff = new System.Windows.Forms.GroupBox();
            this.lblProgramCounter = new System.Windows.Forms.Label();
            this.lblStackPointer = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.lblX = new System.Windows.Forms.Label();
            this.lblAccumulator = new System.Windows.Forms.Label();
            this.btnAssemble = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnHexDump = new System.Windows.Forms.Button();
            this.btnDisassemble = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.tooltipDica = new System.Windows.Forms.ToolTip(this.components);
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportBinaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportRAWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aseembleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.hexdumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disassembleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineNumbers = new System.Windows.Forms.RichTextBox();
            this.txtCode = new System.Windows.Forms.RichTextBox();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeThemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkMonitor = new System.Windows.Forms.CheckBox();
            this.txtMonitorStart = new System.Windows.Forms.TextBox();
            this.txtMonitorLength = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pctScreen)).BeginInit();
            this.grpStuff.SuspendLayout();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pctScreen
            // 
            this.pctScreen.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pctScreen.Location = new System.Drawing.Point(544, 56);
            this.pctScreen.Name = "pctScreen";
            this.pctScreen.Size = new System.Drawing.Size(224, 224);
            this.pctScreen.TabIndex = 0;
            this.pctScreen.TabStop = false;
            // 
            // chkDebug
            // 
            this.chkDebug.AutoSize = true;
            this.chkDebug.Enabled = false;
            this.chkDebug.Font = new System.Drawing.Font("MS Gothic", 9.75F);
            this.chkDebug.Location = new System.Drawing.Point(73, 11);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(82, 17);
            this.chkDebug.TabIndex = 1;
            this.chkDebug.Text = "Debugger";
            this.chkDebug.UseVisualStyleBackColor = true;
            this.chkDebug.CheckedChanged += new System.EventHandler(this.chkDebug_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS Gothic", 9.75F);
            this.label6.Location = new System.Drawing.Point(70, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "NV-BDIZC";
            this.tooltipDica.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            // 
            // lblFlags
            // 
            this.lblFlags.AutoSize = true;
            this.lblFlags.Font = new System.Drawing.Font("MS Gothic", 9.75F);
            this.lblFlags.Location = new System.Drawing.Point(70, 101);
            this.lblFlags.Name = "lblFlags";
            this.lblFlags.Size = new System.Drawing.Size(63, 13);
            this.lblFlags.TabIndex = 8;
            this.lblFlags.Text = "00000000";
            // 
            // btnStep
            // 
            this.btnStep.Enabled = false;
            this.btnStep.Font = new System.Drawing.Font("MS Gothic", 9.75F);
            this.btnStep.Location = new System.Drawing.Point(47, 122);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(48, 23);
            this.btnStep.TabIndex = 9;
            this.btnStep.Text = "Step";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnJumpTo
            // 
            this.btnJumpTo.Enabled = false;
            this.btnJumpTo.Font = new System.Drawing.Font("MS Gothic", 9.75F);
            this.btnJumpTo.Location = new System.Drawing.Point(101, 122);
            this.btnJumpTo.Name = "btnJumpTo";
            this.btnJumpTo.Size = new System.Drawing.Size(85, 23);
            this.btnJumpTo.TabIndex = 10;
            this.btnJumpTo.Text = "Jump to...";
            this.btnJumpTo.UseVisualStyleBackColor = true;
            // 
            // grpStuff
            // 
            this.grpStuff.Controls.Add(this.lblProgramCounter);
            this.grpStuff.Controls.Add(this.lblStackPointer);
            this.grpStuff.Controls.Add(this.lblY);
            this.grpStuff.Controls.Add(this.lblX);
            this.grpStuff.Controls.Add(this.lblAccumulator);
            this.grpStuff.Controls.Add(this.chkDebug);
            this.grpStuff.Controls.Add(this.lblFlags);
            this.grpStuff.Controls.Add(this.btnStep);
            this.grpStuff.Controls.Add(this.label6);
            this.grpStuff.Controls.Add(this.btnJumpTo);
            this.grpStuff.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grpStuff.Location = new System.Drawing.Point(544, 286);
            this.grpStuff.Name = "grpStuff";
            this.grpStuff.Size = new System.Drawing.Size(224, 153);
            this.grpStuff.TabIndex = 11;
            this.grpStuff.TabStop = false;
            // 
            // lblProgramCounter
            // 
            this.lblProgramCounter.AutoSize = true;
            this.lblProgramCounter.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgramCounter.Location = new System.Drawing.Point(136, 60);
            this.lblProgramCounter.Name = "lblProgramCounter";
            this.lblProgramCounter.Size = new System.Drawing.Size(17, 12);
            this.lblProgramCounter.TabIndex = 11;
            this.lblProgramCounter.Text = "PC";
            // 
            // lblStackPointer
            // 
            this.lblStackPointer.AutoSize = true;
            this.lblStackPointer.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStackPointer.Location = new System.Drawing.Point(50, 60);
            this.lblStackPointer.Name = "lblStackPointer";
            this.lblStackPointer.Size = new System.Drawing.Size(17, 12);
            this.lblStackPointer.TabIndex = 10;
            this.lblStackPointer.Text = "SP";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblY.Location = new System.Drawing.Point(168, 32);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(11, 12);
            this.lblY.TabIndex = 9;
            this.lblY.Text = "Y";
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblX.Location = new System.Drawing.Point(95, 31);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(11, 12);
            this.lblX.TabIndex = 8;
            this.lblX.Text = "X";
            this.tooltipDica.SetToolTip(this.lblX, "Registo\tNome\tDescrição\r\nX\tRegisto X\tUsado para indexação de memória e algumas ope" +
        "rações");
            // 
            // lblAccumulator
            // 
            this.lblAccumulator.AutoSize = true;
            this.lblAccumulator.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccumulator.Location = new System.Drawing.Point(19, 32);
            this.lblAccumulator.Name = "lblAccumulator";
            this.lblAccumulator.Size = new System.Drawing.Size(11, 12);
            this.lblAccumulator.TabIndex = 7;
            this.lblAccumulator.Text = "A";
            this.tooltipDica.SetToolTip(this.lblAccumulator, "Registo\tNome\tDescrição\r\nA\tAcumulador\tUsado para operações aritméticas/lógicas e t" +
        "ransferência de dados");
            // 
            // btnAssemble
            // 
            this.btnAssemble.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAssemble.Location = new System.Drawing.Point(12, 27);
            this.btnAssemble.Name = "btnAssemble";
            this.btnAssemble.Size = new System.Drawing.Size(75, 23);
            this.btnAssemble.TabIndex = 13;
            this.btnAssemble.Text = "Assemble";
            this.btnAssemble.UseVisualStyleBackColor = true;
            this.btnAssemble.Click += new System.EventHandler(this.btnAssemble_Click);
            // 
            // btnRun
            // 
            this.btnRun.Font = new System.Drawing.Font("MS Gothic", 9.75F);
            this.btnRun.Location = new System.Drawing.Point(93, 27);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(37, 23);
            this.btnRun.TabIndex = 14;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("MS Gothic", 9.75F);
            this.btnReset.Location = new System.Drawing.Point(136, 27);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(54, 23);
            this.btnReset.TabIndex = 15;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnHexDump
            // 
            this.btnHexDump.Font = new System.Drawing.Font("MS Gothic", 9.75F);
            this.btnHexDump.Location = new System.Drawing.Point(196, 27);
            this.btnHexDump.Name = "btnHexDump";
            this.btnHexDump.Size = new System.Drawing.Size(64, 23);
            this.btnHexDump.TabIndex = 16;
            this.btnHexDump.Text = "Hexdump";
            this.btnHexDump.UseVisualStyleBackColor = true;
            this.btnHexDump.Click += new System.EventHandler(this.btnHexDump_Click);
            // 
            // btnDisassemble
            // 
            this.btnDisassemble.Font = new System.Drawing.Font("MS Gothic", 9.75F);
            this.btnDisassemble.Location = new System.Drawing.Point(266, 27);
            this.btnDisassemble.Name = "btnDisassemble";
            this.btnDisassemble.Size = new System.Drawing.Size(92, 23);
            this.btnDisassemble.TabIndex = 17;
            this.btnDisassemble.Text = "Disassemble";
            this.btnDisassemble.UseVisualStyleBackColor = true;
            this.btnDisassemble.Click += new System.EventHandler(this.btnDisassemble_Click);
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtLog.Enabled = false;
            this.txtLog.Font = new System.Drawing.Font("MS Gothic", 9.75F);
            this.txtLog.Location = new System.Drawing.Point(12, 486);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(756, 91);
            this.txtLog.TabIndex = 18;
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.runToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(780, 24);
            this.MenuStrip.TabIndex = 19;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exportBinaryToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exportBinaryToolStripMenuItem
            // 
            this.exportBinaryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.exportRAWToolStripMenuItem});
            this.exportBinaryToolStripMenuItem.Name = "exportBinaryToolStripMenuItem";
            this.exportBinaryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportBinaryToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.exportBinaryToolStripMenuItem.Text = "Export Binary";
            this.exportBinaryToolStripMenuItem.Click += new System.EventHandler(this.exportBinaryToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.E)));
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.exportToolStripMenuItem.Text = "Export with Address";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // exportRAWToolStripMenuItem
            // 
            this.exportRAWToolStripMenuItem.Name = "exportRAWToolStripMenuItem";
            this.exportRAWToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.exportRAWToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.exportRAWToolStripMenuItem.Text = "Export RAW";
            this.exportRAWToolStripMenuItem.Click += new System.EventHandler(this.exportRAWToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aseembleToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.stepToolStripMenuItem,
            this.resetToolStripMenuItem1,
            this.hexdumpToolStripMenuItem,
            this.disassembleToolStripMenuItem});
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.runToolStripMenuItem.Text = "Run";
            // 
            // aseembleToolStripMenuItem
            // 
            this.aseembleToolStripMenuItem.Name = "aseembleToolStripMenuItem";
            this.aseembleToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.aseembleToolStripMenuItem.Text = "Assemble";
            this.aseembleToolStripMenuItem.Click += new System.EventHandler(this.aseembleToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.resetToolStripMenuItem.Text = "Run";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // stepToolStripMenuItem
            // 
            this.stepToolStripMenuItem.Name = "stepToolStripMenuItem";
            this.stepToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.stepToolStripMenuItem.Text = "Step";
            this.stepToolStripMenuItem.Click += new System.EventHandler(this.stepToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem1
            // 
            this.resetToolStripMenuItem1.Name = "resetToolStripMenuItem1";
            this.resetToolStripMenuItem1.Size = new System.Drawing.Size(139, 22);
            this.resetToolStripMenuItem1.Text = "Reset";
            this.resetToolStripMenuItem1.Click += new System.EventHandler(this.resetToolStripMenuItem1_Click);
            // 
            // hexdumpToolStripMenuItem
            // 
            this.hexdumpToolStripMenuItem.Name = "hexdumpToolStripMenuItem";
            this.hexdumpToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.hexdumpToolStripMenuItem.Text = "Hexdump";
            this.hexdumpToolStripMenuItem.Click += new System.EventHandler(this.hexdumpToolStripMenuItem_Click);
            // 
            // disassembleToolStripMenuItem
            // 
            this.disassembleToolStripMenuItem.Name = "disassembleToolStripMenuItem";
            this.disassembleToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.disassembleToolStripMenuItem.Text = "Disassemble";
            this.disassembleToolStripMenuItem.Click += new System.EventHandler(this.disassembleToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // lineNumbers
            // 
            this.lineNumbers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lineNumbers.Enabled = false;
            this.lineNumbers.Font = new System.Drawing.Font("MS Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lineNumbers.Location = new System.Drawing.Point(12, 56);
            this.lineNumbers.Name = "lineNumbers";
            this.lineNumbers.ReadOnly = true;
            this.lineNumbers.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.lineNumbers.Size = new System.Drawing.Size(28, 383);
            this.lineNumbers.TabIndex = 20;
            this.lineNumbers.Text = "";
            // 
            // txtCode
            // 
            this.txtCode.AcceptsTab = true;
            this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCode.Font = new System.Drawing.Font("MS Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Location = new System.Drawing.Point(36, 56);
            this.txtCode.Name = "txtCode";
            this.txtCode.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtCode.Size = new System.Drawing.Size(502, 383);
            this.txtCode.TabIndex = 21;
            this.txtCode.Text = "";
            this.txtCode.VScroll += new System.EventHandler(this.txtCode_VScroll);
            this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            this.txtCode.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtCode_MouseMove);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeThemeToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // changeThemeToolStripMenuItem
            // 
            this.changeThemeToolStripMenuItem.Name = "changeThemeToolStripMenuItem";
            this.changeThemeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.changeThemeToolStripMenuItem.Text = "Change Theme";
            this.changeThemeToolStripMenuItem.Click += new System.EventHandler(this.changeThemeToolStripMenuItem_Click);
            // 
            // chkMonitor
            // 
            this.chkMonitor.AutoSize = true;
            this.chkMonitor.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMonitor.Location = new System.Drawing.Point(36, 456);
            this.chkMonitor.Name = "chkMonitor";
            this.chkMonitor.Size = new System.Drawing.Size(66, 16);
            this.chkMonitor.TabIndex = 22;
            this.chkMonitor.Text = "Monitor";
            this.chkMonitor.UseVisualStyleBackColor = true;
            // 
            // txtMonitorStart
            // 
            this.txtMonitorStart.Location = new System.Drawing.Point(178, 454);
            this.txtMonitorStart.Name = "txtMonitorStart";
            this.txtMonitorStart.Size = new System.Drawing.Size(55, 20);
            this.txtMonitorStart.TabIndex = 23;
            // 
            // txtMonitorLength
            // 
            this.txtMonitorLength.Location = new System.Drawing.Point(314, 453);
            this.txtMonitorLength.Name = "txtMonitorLength";
            this.txtMonitorLength.Size = new System.Drawing.Size(55, 20);
            this.txtMonitorLength.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(119, 458);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 25;
            this.label1.Text = "Start: $";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(249, 458);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 26;
            this.label2.Text = "Length: $";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 589);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMonitorLength);
            this.Controls.Add(this.txtMonitorStart);
            this.Controls.Add(this.chkMonitor);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lineNumbers);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnDisassemble);
            this.Controls.Add(this.btnHexDump);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnAssemble);
            this.Controls.Add(this.pctScreen);
            this.Controls.Add(this.grpStuff);
            this.Controls.Add(this.MenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "C-6502";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pctScreen)).EndInit();
            this.grpStuff.ResumeLayout(false);
            this.grpStuff.PerformLayout();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctScreen;
        private System.Windows.Forms.CheckBox chkDebug;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblFlags;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Button btnJumpTo;
        private System.Windows.Forms.GroupBox grpStuff;
        private System.Windows.Forms.Button btnAssemble;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnHexDump;
        private System.Windows.Forms.Button btnDisassemble;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblAccumulator;
        private System.Windows.Forms.Label lblProgramCounter;
        private System.Windows.Forms.Label lblStackPointer;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.ToolTip tooltipDica;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aseembleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hexdumpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disassembleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportBinaryToolStripMenuItem;
        private System.Windows.Forms.RichTextBox lineNumbers;
        private System.Windows.Forms.RichTextBox txtCode;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportRAWToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeThemeToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkMonitor;
        private System.Windows.Forms.TextBox txtMonitorStart;
        private System.Windows.Forms.TextBox txtMonitorLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

