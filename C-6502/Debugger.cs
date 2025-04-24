using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_6502
{
    internal class Debugger
    {
        public static void PrintHeader()
        {
            Console.Clear();
            Console.Title = "C-6502 Emulator v1.0";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("================================");
            Console.WriteLine("      C-6502 Emulator v1.0     ");
            Console.WriteLine("================================");
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void PrintState(CPU6502 cpu)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"PC:  {cpu.PC:X4}  |  A: {cpu.A:X2}  X: {cpu.X:X2}  Y: {cpu.Y:X2}");
            Console.WriteLine($"SP:  {cpu.SP:X2}   |  Flags: {Convert.ToString(cpu.Status, 2).PadLeft(8, '0')}");
            Console.ResetColor();
            Console.WriteLine("--------------------------------");
        }

        public static void PrintHexDump(CPU6502 cpu, ushort startAddress, int length = 16)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Memory Dump:");
            for (ushort i = 0; i < length; i += 8)
            {
                Console.Write($"{(startAddress + i):X4}: ");
                for (ushort j = 0; j < 8; j++)
                {
                    Console.Write($"{cpu.ReadByte((ushort)(startAddress + i + j)):X2} ");
                }
                Console.WriteLine();
            }
            Console.ResetColor();
            Console.WriteLine("--------------------------------");
        }

        public static void PrintMemoryMonitor(byte[] memory, int start, int length)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Monitor de Memória ===");

            for (int i = start; i < start + length; i += 16)
            {
                StringBuilder line = new StringBuilder();
                line.Append(i.ToString("X4") + ": ");

                for (int j = 0; j < 16; j++)
                {
                    if (i + j < memory.Length)
                        line.Append(memory[i + j].ToString("X2") + " ");
                    else
                        line.Append("   ");
                }

                Console.WriteLine(line.ToString().TrimEnd());
            }

            Console.WriteLine("===========================");
        }

    }
}
