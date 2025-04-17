using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_6502
{
    internal static class OpcodeLoader
    {
        public static List<OpcodeEntry> LoadOpcodesFromExcel(string path)
        {
            List<OpcodeEntry> opcodes = new List<OpcodeEntry>();

            // EPPlus exige esta linha para aceitar o Excel sem licença comercial

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Primeira folha

                int row = 2; // Começa na linha 2 (assumimos que linha 1 tem cabeçalhos)

                while (true)
                {
                    var mnemonic = worksheet.Cells[row, 1].Text.Trim(); // Ex: "LDA"
                    var mode = worksheet.Cells[row, 2].Text.Trim();     // Ex: "IMM"
                    var opcodeStr = worksheet.Cells[row, 3].Text.Trim(); // Ex: "A9"
                    var sizeStr = worksheet.Cells[row, 4].Text.Trim();   // Ex: "2"

                    // Se a linha estiver vazia, parar
                    if (string.IsNullOrWhiteSpace(mnemonic))
                        break;

                    try
                    {
                        OpcodeEntry entry = new OpcodeEntry
                        {
                            Mnemonic = mnemonic,
                            Mode = mode,
                            Opcode = Convert.ToByte(opcodeStr, 16),
                            Size = int.Parse(sizeStr)
                        };

                        opcodes.Add(entry);
                    }
                    catch
                    {
                        throw new Exception($"Erro ao ler linha {row}: {mnemonic} {mode} {opcodeStr} {sizeStr}");
                    }

                    row++;
                }
            }

            return opcodes;
        }
    }
}
