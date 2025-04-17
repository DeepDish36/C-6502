using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_6502
{
    class OpcodeEntry
    {
        public string Mnemonic { get; set; }
        public string Mode { get; set; }
        public byte Opcode { get; set; }
        public int Size { get; set; }
    }
}
