using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_6502
{
    internal class Video
    {
        public static readonly Color[] Cores = new Color[]
        {
            ColorTranslator.FromHtml("#000000"),
            ColorTranslator.FromHtml("#FFFFFF"),
            ColorTranslator.FromHtml("#8C0404"),
            ColorTranslator.FromHtml("#B0FCEC"),
            ColorTranslator.FromHtml("#D044CC"),
            ColorTranslator.FromHtml("#08CC54"),
            ColorTranslator.FromHtml("#0548CE"),
            ColorTranslator.FromHtml("#EEEE77"),
            ColorTranslator.FromHtml("#DD8855"),
            ColorTranslator.FromHtml("#664400"),
            ColorTranslator.FromHtml("#FF7777"),
            ColorTranslator.FromHtml("#333333"),
            ColorTranslator.FromHtml("#777777"),
            ColorTranslator.FromHtml("#AAFF66"),
            ColorTranslator.FromHtml("#0088FF"),
            ColorTranslator.FromHtml("#BBBBBB")
        };
    }
}
