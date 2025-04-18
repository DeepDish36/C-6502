﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C_6502
{
    public partial class frmMain : Form
    {
        private CPU6502 cpu;
        private Bitmap screenBitmap = new Bitmap(224, 224); // 32x32 pixels, cada um com 7x7

        private int programStart = 0x0600;
        private int programLength = 0;

        private string ficheiroAtual = null;
        private bool modificacoesPorSalvar = false;

        private byte[] assembledBytes;

        public frmMain()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += FrmMain_DragEnter;
            this.DragDrop += FrmMain_DragDrop;

            txtCode.TextChanged += (s, e) =>
            {
                btnAssemble.Enabled = true;

                // Desativa execução até novo assemble
                btnRun.Enabled = false;
                btnStep.Enabled = false;
                btnReset.Enabled = false;
                btnJumpTo.Enabled = false;
                btnDisassemble.Enabled = false;
                btnHexDump.Enabled = false;
            };
            cpu = new CPU6502();
            cpu.OnVideoWrite = AtualizarPixel;
        }

        private void FrmMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void FrmMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] ficheiros = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (ficheiros.Length > 0)
            {
                string caminho = ficheiros[0];
                if (caminho.EndsWith(".asm") || caminho.EndsWith(".txt"))
                {
                    try
                    {
                        string conteudo = System.IO.File.ReadAllText(caminho);
                        txtCode.Text = conteudo;
                        ficheiroAtual = caminho;
                        modificacoesPorSalvar = false;
                        txtLog.Clear();
                        LimparEcrã();
                        AtualizarInterface();

                        btnAssemble.Enabled = true;
                        btnStep.Enabled = false;
                        btnRun.Enabled = false;
                        btnReset.Enabled = false;
                        btnDisassemble.Enabled = false;
                        btnHexDump.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao abrir: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                GuardarFicheiro();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.O))
            {
                AbrirFicheiro();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            Debugger.PrintHeader();

            // Apenas o Assemble começa ativo
            btnRun.Enabled = false;
            btnStep.Enabled = false;
            btnJumpTo.Enabled = false;
            btnReset.Enabled = false;
            btnDisassemble.Enabled = false;
            btnHexDump.Enabled = false;
            chkDebug.Checked = false;

            cpu.Reset();
            AtualizarInterface();
        }

        private void AtualizarInterface()
        {
            lblAccumulator.Text = $"A: ${cpu.A:X2}";
            lblX.Text = $"X: ${cpu.X:X2}";
            lblY.Text = $"Y: ${cpu.Y:X2}";
            lblStackPointer.Text = $"SP: ${cpu.SP:X2}";
            lblProgramCounter.Text = $"PC: ${cpu.PC:X4}";

            string flags = Convert.ToString(cpu.Status, 2).PadLeft(8, '0');
            lblFlags.Text = $"{flags}";
        }

        private void btnAssemble_Click(object sender, EventArgs e)
        {
            txtLog.Clear();

            AppendLog("Preprocessing ...");
            Thread.Sleep(200);

            string code = txtCode.Text.Trim();

            AppendLog("Indexing labels ...");
            Thread.Sleep(200);

            if (string.IsNullOrWhiteSpace(code))
            {
                AppendLog("Found 0 labels.");
                AppendLog("Assembling code ...");
                AppendLog("No code to run.");
                return;
            }

            AppendLog("Found 0 labels.");
            AppendLog("Assembling code ...");

            try
            {
                // Usa o assembler real
                byte[] assembledProgram = Assembler6502.Assemble(code);

                assembledBytes = assembledProgram;

                cpu.Reset();
                cpu.LoadProgram(assembledProgram, 0x0600);

                programLength = assembledProgram.Length; // Guarda quantos bytes tem o programa

                AppendLog($"Code assembled successfully, {assembledProgram.Length} bytes.");
                AppendLog("Ready to run.");

                // Ativa os botões se correu bem
                btnAssemble.Enabled = false;
                btnRun.Enabled = true;
                btnStep.Enabled = chkDebug.Checked; // só se debug estiver ativo
                btnJumpTo.Enabled = chkDebug.Checked;
                btnReset.Enabled = true;
                btnDisassemble.Enabled = true;
                btnHexDump.Enabled = true;
            }
            catch (Exception ex)
            {
                AppendLog($"**Syntax Error: {ex.Message}**");

                // Se deu erro, desativa tudo menos o Assemble
                btnRun.Enabled = false;
                btnStep.Enabled = false;
                btnJumpTo.Enabled = false;
                btnReset.Enabled = false;
                btnDisassemble.Enabled = false;
                btnHexDump.Enabled = false;
            }
        }

        private void AppendLog(string text)
        {
            txtLog.AppendText(text + Environment.NewLine);
        }

        public void AtualizarPixel(int endereco, byte cor)
        {
            int pixelIndex = endereco - 0x0200;
            if (pixelIndex < 0 || pixelIndex >= 1024) return;

            int x = (pixelIndex % 32) * 7;
            int y = (pixelIndex / 32) * 7;

            using (Graphics g = Graphics.FromImage(screenBitmap))
            {
                using (Brush b = new SolidBrush(Video.Cores[cor % 16]))
                {
                    g.FillRectangle(b, x, y, 7, 7);
                }
            }

            pctScreen.Image = screenBitmap;
            pctScreen.Refresh();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            btnRun.Enabled = false;
            btnStep.Enabled = false;
            btnJumpTo.Enabled = false;
            btnReset.Enabled = false;
            btnAssemble.Enabled = false;

            try
            {
                int steps = 0;
                while (true)
                {
                    byte opcode = cpu.ReadByte(cpu.PC);
                    if (opcode == 0x00) break;

                    cpu.Step();
                    steps++;

                    if (steps > 10000)
                    {
                        AppendLog("Execution stopped: too many instructions.");
                        break;
                    }
                }

                AppendLog($"Program executed in {steps} steps.");
            }
            catch (Exception ex)
            {
                AppendLog($"Execution error: {ex.Message}");
            }

            AtualizarInterface();
            btnReset.Enabled = true;
            btnDisassemble.Enabled = true;
            btnHexDump.Enabled = true;
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            try
            {
                byte opcode = cpu.ReadByte(cpu.PC);
                if (opcode == 0x00)
                {
                    AppendLog("End of program (BRK).");
                    return;
                }

                cpu.Step();
                AppendLog($"Executed one instruction at ${cpu.PC:X4}");
            }
            catch (Exception ex)
            {
                AppendLog($"Step error: {ex.Message}");
            }

            AtualizarInterface();
        }

        private void btnHexDump_Click(object sender, EventArgs e)
        {
            if (programLength == 0)
            {
                MessageBox.Show("No program to display.");
                return;
            }

            StringBuilder dump = new StringBuilder();
            int start = programStart;
            int end = programStart + programLength;

            for (int addr = start; addr < end; addr += 16)
            {
                dump.Append($"{addr:X4}: ");
                for (int i = 0; i < 16; i++)
                {
                    int current = addr + i;
                    if (current < end)
                        dump.Append($"{cpu.ReadByte((ushort)current):X2} ");
                    else
                        dump.Append("   ");
                }
                dump.AppendLine();
            }

            MessageBox.Show(dump.ToString(), "Hex Dump");
        }

        private void btnDisassemble_Click(object sender, EventArgs e)
        {
            if (programLength == 0)
            {
                MessageBox.Show("No program to disassemble.");
                return;
            }

            StringBuilder disasm = new StringBuilder();
            ushort pc = (ushort)programStart;
            ushort end = (ushort)(programStart + programLength);

            int maxInstructions = 1000; // Limite de segurança
            int count = 0;

            while (pc < end && count < maxInstructions)
            {
                count++;

                try
                {
                    ushort currentPC = pc; // Guarda o PC antes de avançar

                    byte opcode = cpu.ReadByte(pc);
                    string line = $"${pc:X4}    {opcode:X2}";

                    string dis = cpu.DisassembleSingle(ref pc);

                    // Se o PC não mudou, forçamos avanço para evitar loop infinito
                    if (pc == currentPC)
                        pc++;

                    line = line.PadRight(18) + dis;
                    disasm.AppendLine(line);

                    if (opcode == 0x00) break; // BRK
                }
                catch
                {
                    disasm.AppendLine($"${pc:X4}    ??   [Invalid opcode]");
                    pc++;
                }
            }

            if (count >= maxInstructions)
            {
                disasm.AppendLine("\n[Interrompido após 1000 instruções — possível loop infinito]");
            }

            MessageBox.Show(disasm.ToString(), "Disassembly");
        }

        private void chkDebug_CheckedChanged(object sender, EventArgs e)
        {
            btnStep.Enabled = chkDebug.Checked;
            btnJumpTo.Enabled = chkDebug.Checked;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cpu.Reset();

            // Limpa a memória de vídeo (endereços 0x0200–0x05FF)
            for (int addr = 0x0200; addr <= 0x05FF; addr++)
                cpu.WriteByte((ushort)addr, 0x00);

            // Limpa o bitmap
            using (Graphics g = Graphics.FromImage(screenBitmap))
            {
                g.Clear(Color.Black);
            }
            pctScreen.Image = screenBitmap;
            pctScreen.Refresh();

            txtLog.Clear();
            AppendLog("System reset.");

            AtualizarInterface();

            // Desativa botões
            btnRun.Enabled = false;
            btnStep.Enabled = false;
            btnJumpTo.Enabled = false;
            btnReset.Enabled = false;
            btnDisassemble.Enabled = false;
            btnHexDump.Enabled = false;

            // Permite novo assemble
            btnAssemble.Enabled = true;
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            modificacoesPorSalvar = true;
            btnAssemble.Enabled = true;

            // Desativa os outros
            btnRun.Enabled = false;
            btnStep.Enabled = false;
            btnReset.Enabled = false;
            btnDisassemble.Enabled = false;
            btnHexDump.Enabled = false;
            btnJumpTo.Enabled = false;

            // Limpa o log e ecrã, reseta CPU
            txtLog.Clear();
            cpu.Reset();
            AtualizarInterface();

            // Limpa o ecrã
            using (Graphics g = Graphics.FromImage(screenBitmap))
            {
                g.Clear(Color.Black);
            }

            pctScreen.Image = screenBitmap;
            pctScreen.Refresh();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFicheiro();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuardarFicheiro();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NovoPrograma();
        }

        private void aseembleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAssemble_Click(sender, e);
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnRun_Click(sender, e);
        }

        private void stepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnStep_Click(sender, e);
        }

        private void disassembleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnDisassemble_Click(sender, e);
        }

        private void resetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            btnReset_Click(sender, e);
        }

        private void hexdumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnHexDump_Click(sender, e);
        }

        private void NovoPrograma()
        {
            txtCode.Clear();
            txtLog.Clear();
            cpu.Reset();
            LimparEcrã();
            AtualizarInterface();

            btnAssemble.Enabled = true;
            btnStep.Enabled = false;
            btnRun.Enabled = false;
            btnReset.Enabled = false;
            btnDisassemble.Enabled = false;
            btnHexDump.Enabled = false;
        }

        private void LimparEcrã()
        {
            using (Graphics g = Graphics.FromImage(screenBitmap))
            {
                g.Clear(Color.Black);
            }

            pctScreen.Image = screenBitmap;
            pctScreen.Refresh();
        }

        private void AbrirFicheiro()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Ficheiros ASM (*.asm)|*.asm|Ficheiros de Texto (*.txt)|*.txt";
            openDialog.Title = "Abrir Código Assembly";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string conteudo = System.IO.File.ReadAllText(openDialog.FileName);
                    txtCode.Text = conteudo;
                    txtLog.Clear();
                    LimparEcrã();
                    AtualizarInterface();

                    btnAssemble.Enabled = true;
                    btnStep.Enabled = false;
                    btnRun.Enabled = false;
                    btnReset.Enabled = false;
                    btnDisassemble.Enabled = false;
                    btnHexDump.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao abrir o ficheiro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GuardarFicheiro()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Ficheiros ASM (*.asm)|*.asm|Ficheiros de Texto (*.txt)|*.txt";
            saveDialog.Title = "Guardar Código Assembly";
            saveDialog.DefaultExt = "asm";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.File.WriteAllText(saveDialog.FileName, txtCode.Text);
                    MessageBox.Show("Ficheiro guardado com sucesso!", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao guardar o ficheiro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modificacoesPorSalvar)
            {
                var resposta = MessageBox.Show("Tens alterações por guardar. Queres guardar antes de sair?", "Sair", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                if (resposta == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (resposta == DialogResult.Yes)
                {
                    GuardarFicheiro();
                }
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.S)
            {
                e.SuppressKeyPress = true;
                GuardarComo();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                e.SuppressKeyPress = true;
                GuardarFicheiro();
            }
        }
        private void GuardarComo()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Código Assembly (*.asm;*.txt)|*.asm;*.txt|Todos os ficheiros (*.*)|*.*";
                sfd.Title = "Guardar código como...";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ficheiroAtual = sfd.FileName;
                    GuardarFicheiro();
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuardarComo();
        }

        private void exportBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (assembledBytes == null || assembledBytes.Length == 0)
            {
                MessageBox.Show("Nenhum código compilado para exportar.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Ficheiro binário (*.bin)|*.bin";
                saveDialog.Title = "Exportar código compilado";
                saveDialog.FileName = "programa.bin";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveDialog.FileName, assembledBytes);
                    MessageBox.Show("Ficheiro exportado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
