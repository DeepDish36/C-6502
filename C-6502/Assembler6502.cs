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

            // Absolute
            { "JSR_ABS", 0x20 },
            { "STA_ABS", 0x8D },
            { "LDA_ABS", 0xAD },

            // Immediate
            { "LDY_IMM", 0xA0 },
            { "CPY_IMM", 0xC0 },
            { "CPX_IMM", 0xE0 },
            { "LDX_IMM", 0xA2 },
            { "LDA_IMM", 0xA9 },

            // (Indirect, X)
            { "ORA_X_IND", 0x01 },
            { "AND_X_IND", 0x21 },
            { "EOR_X_IND", 0x41 },
            { "ADC_X_IND", 0x61 },
            { "STA_X_IND", 0x81 },
            { "LDA_X_IND", 0xA1 },
            { "CMP_X_IND", 0xC1 },
            { "SBC_X_IND", 0xE1 },

            // (Indirect), Y
            { "ORA_IND_Y", 0x11 },
            { "AND_IND_Y", 0x31 },
            { "EOR_IND_Y", 0x51 },
            { "ADC_IND_Y", 0x71 },
            { "STA_IND_Y", 0x91 },
            { "LDA_IND_Y", 0xB1 },
            { "CMP_IND_Y", 0xD1 },
            { "SBC_IND_Y", 0xF1 }
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

                // Ignorar comentários
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
                    currentAddress += EstimateInstructionSize(line);
                }
            }

            // 2ª PASSAGEM — gerar código
            foreach (var (line, _) in instructions)
            {
                string[] parts = line.Split(new[] { ' ', '\t' }, 2, StringSplitOptions.RemoveEmptyEntries);
                string mnemonic = parts[0];
                string operand = parts.Length > 1 ? parts[1].Trim() : "";

                string mode = GetAddressingMode(operand);
                string key = mnemonic + (string.IsNullOrEmpty(mode) ? "" : "_" + mode);

                if (!opcodes.ContainsKey(key))
                    throw new Exception($"Instrução desconhecida ou não suportada: {key}");

                output.Add(opcodes[key]);

                // Argumentos
                switch (mode)
                {
                    case "IMM": // ex: #$01
                        output.Add(Convert.ToByte(operand.Substring(2), 16));
                        break;

                    case "ABS": // ex: $0200
                        ushort addr = Convert.ToUInt16(operand.Substring(1), 16);
                        output.Add((byte)(addr & 0xFF));
                        output.Add((byte)(addr >> 8));
                        break;

                    case "REL": // branch ex: BEQ label
                        int target = labels.ContainsKey(operand) ? labels[operand] : throw new Exception($"Label não encontrado: {operand}");
                        int offset = (target - output.Count - 1);
                        output.Add((byte)(offset & 0xFF));
                        break;

                    case "IND_Y": // ex: ($10),Y
                    case "X_IND": // ex: ($10,X)
                        byte zp = Convert.ToByte(operand.Substring(2, 2), 16);
                        output.Add(zp);
                        break;

                    case "":
                        // implícita, como BRK, INX, etc — nada a adicionar
                        break;

                    default:
                        throw new Exception($"Modo de endereçamento não suportado: {mode}");
                }
            }

            return output.ToArray();
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

            if (operand.StartsWith("#$")) return "IMM";
            if (operand.StartsWith("$")) return "ABS";
            if (operand.StartsWith("(") && operand.EndsWith(",Y")) return "IND_Y";
            if (operand.StartsWith("(") && operand.Contains(",X")) return "X_IND";
            if (!operand.Contains("$") && !operand.Contains("(")) return "REL";

            return ""; // fallback para implícito
        }
    }
}
