using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_6502
{
    public class Assembler6502
    {
        public static readonly Dictionary<string, byte> opcodes = new Dictionary<string, byte>
        {
            // Implied / Relative
            { "BRK", 0x00 },
            { "RTI", 0x40 },
            { "RTS", 0x60 },
            { "BPL_REL", 0x10 },
            { "BMI_REL", 0x30 },
            { "BVC_REL", 0x50 },
            { "BVS_REL", 0x70 },
            { "BCC_REL", 0x90 },
            { "BCS_REL", 0xB0 },
            { "BNE_REL", 0xD0 },
            { "BEQ_REL", 0xF0 },

            // Immediate
            { "LDA_IMM", 0xA9 },
            { "LDX_IMM", 0xA2 },
            { "LDY_IMM", 0xA0 },
            { "CPX_IMM", 0xE0 },
            { "CPY_IMM", 0xC0 },
            { "CMP_IMM", 0xC9 },

            // Zero Page
            { "LDA_ZPG", 0xA5 },
            { "LDA_ZPG_X", 0xB5 },
            { "STA_ZPG", 0x85 },
            { "STA_ZPG_X", 0x95 },
            { "AND_ZPG", 0x25 },
            { "AND_ZPG_X", 0x35 },
            { "ORA_ZPG", 0x05 },
            { "ORA_ZPG_X", 0x15 },
            { "EOR_ZPG", 0x45 },
            { "EOR_ZPG_X", 0x55 },
            { "CMP_ZPG", 0xC5 },
            { "CMP_ZPG_X", 0xD5 },
            { "SBC_ZPG", 0xE5 },
            { "SBC_ZPG_X", 0xF5 },
            { "LDY_ZPG", 0xA4 },
            { "LDY_ZPG_X", 0xB4 },
            { "STY_ZPG", 0x84 },
            { "STY_ZPG_X", 0x94 },
            { "CPX_ZPG", 0xE4 },
            { "CPY_ZPG", 0xC4 },
            { "BIT_ZPG", 0x24 },

            // Absolute
            { "LDA_ABS", 0xAD },
            { "LDA_ABS_X", 0xBD },
            { "LDA_ABS_Y", 0xB9 },
            { "STA_ABS", 0x8D },
            { "STA_ABS_X", 0x9D },
            { "STA_ABS_Y", 0x99 },
            { "JSR_ABS", 0x20 },
            { "ADC_ABS", 0x6D },
            { "ADC_ABS_X", 0x7D },
            { "ADC_ABS_Y", 0x79 },

            // (Indirect,X)
            { "ORA_X_IND", 0x01 },
            { "AND_X_IND", 0x21 },
            { "EOR_X_IND", 0x41 },
            { "ADC_X_IND", 0x61 },
            { "STA_X_IND", 0x81 },
            { "LDA_X_IND", 0xA1 },
            { "CMP_X_IND", 0xC1 },
            { "SBC_X_IND", 0xE1 },

            // (Indirect),Y
            { "ORA_IND_Y", 0x11 },
            { "AND_IND_Y", 0x31 },
            { "EOR_IND_Y", 0x51 },
            { "ADC_IND_Y", 0x71 },
            { "STA_IND_Y", 0x91 },
            { "LDA_IND_Y", 0xB1 },
            { "CMP_IND_Y", 0xD1 },
            { "SBC_IND_Y", 0xF1 },

            // 0x06 a 0xF6 — Instruções de leitura/modificação/escrita Zero Page e Zero Page,X
            { "ASL_ZPG", 0x06 },
            { "ASL_ZPG_X", 0x16 },
            { "ROL_ZPG", 0x26 },
            { "ROL_ZPG_X", 0x36 },
            { "LSR_ZPG", 0x46 },
            { "LSR_ZPG_X", 0x56 },
            { "ROR_ZPG", 0x66 },
            { "ROR_ZPG_X", 0x76 },
            { "STX_ZPG", 0x86 },
            { "STX_ZPG_Y", 0x96 },
            { "LDX_ZPG", 0xA6 },
            { "LDX_ZPG_Y", 0xB6 },
            { "DEC_ZPG", 0xC6 },
            { "DEC_ZPG_X", 0xD6 },
            { "INC_ZPG", 0xE6 },
            { "INC_ZPG_X", 0xF6 },
        };

        private static readonly Dictionary<string, byte> opcode = opcodes;

        public static byte[] Assemble(string code)
        {
            var lines = code.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> labels = new Dictionary<string, int>();
            List<(string line, int address)> instructions = new List<(string, int)>();
            List<byte> output = new List<byte>();

            int currentAddress = 0;

            // 1ª PASSAGEM — identificar labels e calcular endereços
            foreach (var raw in lines)
            {
                string line = raw.Trim();

                int commentIndex = line.IndexOf(';');
                if (commentIndex >= 0)
                    line = line.Substring(0, commentIndex);

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                line = line.ToLower();

                if (line.EndsWith(":"))
                {
                    string label = line.TrimEnd(':');
                    labels[label] = currentAddress;
                }
                else
                {
                    instructions.Add((line, currentAddress));
                    if (line.StartsWith("dcb"))
                        currentAddress += EstimateDcbSize(line);
                    else
                        currentAddress += EstimateInstructionSize(line);
                }
            }

            currentAddress = 0;

            // 2ª PASSAGEM — gerar os bytes
            foreach (var (line, _) in instructions)
            {
                string trimmed = line.Trim().ToLower();

                if (trimmed.StartsWith("dcb"))
                {
                    string[] parts = trimmed.Substring(3).Split(',');
                    int i = 0;

                    while (i < parts.Length)
                    {
                        string part1 = parts[i].Trim();

                        if (i + 1 < parts.Length && byte.TryParse(parts[i + 1].Trim(), out byte val))
                        {
                            if (int.TryParse(part1, out int count))
                            {
                                for (int j = 0; j < count; j++)
                                    output.Add(val);

                                currentAddress += count;
                                i += 2;
                                continue;
                            }
                        }

                        byte value;
                        if (part1.StartsWith("$"))
                            value = Convert.ToByte(part1.Substring(1), 16);
                        else if (part1.StartsWith("0x"))
                            value = Convert.ToByte(part1.Substring(2), 16);
                        else
                            value = Convert.ToByte(part1);

                        output.Add(value);
                        currentAddress++;
                        i++;
                    }
                }
                else
                {
                    string[] parts = line.Split(new[] { ' ', '\t' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    string mnemonic = parts[0];
                    string operand = parts.Length > 1 ? parts[1].Trim() : "";

                    string mode = GetAddressingMode(operand);
                    string key = (mnemonic + (string.IsNullOrEmpty(mode) ? "" : "_" + mode)).ToUpper();

                    if (!opcodes.ContainsKey(key))
                        throw new Exception($"Instrução desconhecida ou não suportada: {key}");

                    output.Add(opcodes[key]);

                    switch (mode)
                    {
                        case "IMM":
                            output.Add(Convert.ToByte(operand.Substring(2), 16));
                            break;
                        case "ABS":
                            ushort addr = Convert.ToUInt16(operand.Substring(1), 16);
                            output.Add((byte)(addr & 0xFF));
                            output.Add((byte)(addr >> 8));
                            break;
                        case "ZPG":
                            output.Add(Convert.ToByte(operand.Substring(1), 16));
                            break;
                        case "ZPG_X":
                        case "ZPG_Y":
                            output.Add(Convert.ToByte(operand.Substring(1, 2), 16));
                            break;
                        case "ABS_X":
                        case "ABS_Y":
                            ushort addrxy = Convert.ToUInt16(operand.Substring(1, 4), 16);
                            output.Add((byte)(addrxy & 0xFF));
                            output.Add((byte)(addrxy >> 8));
                            break;
                        case "REL":
                            int target = labels.ContainsKey(operand) ? labels[operand] : throw new Exception($"Label não encontrado: {operand}");
                            int offset = (target - output.Count - 1);
                            output.Add((byte)(offset & 0xFF));
                            break;
                        case "IND_Y":
                        case "X_IND":
                            byte zp = Convert.ToByte(operand.Substring(2, 2), 16);
                            output.Add(zp);
                            break;
                        case "":
                            break;
                        default:
                            throw new Exception($"Modo de endereçamento não suportado: {mode}");
                    }

                    currentAddress = output.Count;
                }
            }

            return output.ToArray();
        }

        static int EstimateDcbSize(string line)
        {
            string[] parts = line.Substring(3).Split(',');
            int size = 0;
            int i = 0;

            while (i < parts.Length)
            {
                string part1 = parts[i].Trim();

                if (i + 1 < parts.Length && byte.TryParse(parts[i + 1].Trim(), out byte _))
                {
                    if (int.TryParse(part1, out int count))
                    {
                        size += count;
                        i += 2;
                        continue;
                    }
                }

                size++;
                i++;
            }

            return size;
        }

        static int EstimateInstructionSize(string line)
        {
            if (line.Contains("#$")) return 2;         // imediato
            if (line.Contains("$") || line.Contains("(")) return 3; // absoluto, indireto
            if (line.Split().Length == 1) return 1;     // implícito
            return 2;                                   // relativo, zero page...
        }

        static string GetAddressingMode(string operand)
        {
            if (string.IsNullOrEmpty(operand)) return "";

            operand = operand.Trim().ToUpper(); // normaliza

            if (operand.StartsWith("#$")) return "IMM";
            if (operand.StartsWith("$"))
            {
                if (operand.EndsWith(",X"))
                {
                    // Determina se é zero page ou absoluto pelo tamanho do valor
                    if (operand.Length == 5) // Ex: $44,X
                        return "ZPG_X";
                    else
                        return "ABS_X";
                }
                if (operand.EndsWith(",Y"))
                {
                    if (operand.Length == 5) // Ex: $44,Y
                        return "ZPG_Y";
                    else
                        return "ABS_Y";
                }
                // Zero page puro
                if (operand.Length == 3) // Ex: $44
                    return "ZPG";
                return "ABS";
            }
            if (operand.StartsWith("(") && operand.EndsWith(",Y")) return "IND_Y";
            if (operand.StartsWith("(") && operand.Contains(",X")) return "X_IND";
            if (!operand.Contains("$") && !operand.Contains("(")) return "REL";

            return ""; // implícito
        }
    }
}
