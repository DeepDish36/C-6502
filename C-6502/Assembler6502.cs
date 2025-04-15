using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_6502
{
    public class Assembler6502
    {
        private static readonly Dictionary<string, byte> dictionary = new Dictionary<string, byte>
        {
            { "JMP_ABS", 0x4C },
            //{ "JMP_IND", 0x6C },
            //{ "LDA_ABS", 0xAD },
            //{ "STA_ABS", 0x8D },
            //{ "LDA_ZP", 0xA5 },
            //{ "STA_ZP", 0x85 },
            //{ "LDA_IMM", 0xA9 },
            //{ "STA_IMM", 0x8D },
            //{ "JMP_REL", 0x90 },
            //{ "JMP_ZP", 0xB5 },
            //{ "LDA_ZPX", 0xB5 },
            //{ "STA_ZPX", 0x95 },
            //{ "LDA_ABY", 0xB9 },
            //{ "STA_ABY", 0x99 },
            //{ "LDA_ABX", 0xBD },
            //{ "STA_ABX", 0x9D },
            //{ "LDA_IMM", 0xA9 },
            //{ "STA_IMM", 0x8D },
            { "LDA_IMM", 0xA9 },
            { "TAX", 0xAA },
            { "INX", 0xE8 },
            { "BRK", 0x00 },
            { "STA_ABS", 0x8D }
        };
        private static readonly Dictionary<string, byte> opcodes = dictionary;

        public static byte[] Assemble(string code)
        {
            var lines = code.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> labels = new Dictionary<string, int>();
            List<(string line, int address)> instructions = new List<(string, int)>();
            List<byte> output = new List<byte>();

            int currentAddress = 0;

            // 1ª PASSAGEM: identificar labels e contar bytes
            foreach (var raw in lines)
            {
                string line = raw.Trim();

                // Ignora comentários
                int commentIndex = line.IndexOf(';');
                if (commentIndex >= 0)
                    line = line.Substring(0, commentIndex);

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                line = line.ToUpper();

                if (line.EndsWith(":"))
                {
                    string label = line.TrimEnd(':');
                    labels[label] = currentAddress;
                }
                else
                {
                    instructions.Add((line, currentAddress));

                    if (line.StartsWith("LDA #$"))
                        currentAddress += 2;
                    else if (line.StartsWith("STA $"))
                        currentAddress += 3;
                    else if (line.StartsWith("JMP "))
                        currentAddress += 3;
                    else if (line == "TAX" || line == "INX" || line == "BRK")
                        currentAddress += 1;
                    else
                        throw new Exception($"Instrução desconhecida: {line}");
                }
            }

            // 2ª PASSAGEM: montar instruções
            foreach (var (line, _) in instructions)
            {
                if (line.StartsWith("LDA #$"))
                {
                    string val = line.Substring(6);
                    output.Add(opcodes["LDA_IMM"]);
                    output.Add(Convert.ToByte(val, 16));
                }
                else if (line.StartsWith("STA $"))
                {
                    string addr = line.Substring(5);
                    ushort address = Convert.ToUInt16(addr, 16);
                    output.Add(opcodes["STA_ABS"]);
                    output.Add((byte)(address & 0xFF));
                    output.Add((byte)(address >> 8));
                }
                else if (line.StartsWith("JMP "))
                {
                    string label = line.Substring(4).Trim();
                    if (!labels.ContainsKey(label))
                        throw new Exception($"Label não encontrado: {label}");

                    int addr = labels[label] + 0x0600; // memória base
                    output.Add(opcodes["JMP_ABS"]);
                    output.Add((byte)(addr & 0xFF));
                    output.Add((byte)(addr >> 8));
                }
                else if (line == "TAX")
                {
                    output.Add(opcodes["TAX"]);
                }
                else if (line == "INX")
                {
                    output.Add(opcodes["INX"]);
                }
                else if (line == "BRK")
                {
                    output.Add(opcodes["BRK"]);
                }
            }

            return output.ToArray();
        }
    }
}
