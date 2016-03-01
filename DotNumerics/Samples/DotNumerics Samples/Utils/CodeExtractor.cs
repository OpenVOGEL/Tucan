using System;
using System.Text;

namespace DotNumerics_Samples.Utils
{
    internal static class CodeExtractor
    {
        public static string GetCodeBlock(string allCode, string blockName)
        {
            int blockStart = allCode.IndexOf(blockName, StringComparison.OrdinalIgnoreCase);

            if (blockStart == -1)
                return "// " + blockName + " code not found";
            blockStart = allCode.LastIndexOf(Environment.NewLine, blockStart, StringComparison.OrdinalIgnoreCase);
            if (blockStart == -1)
                blockStart = 0;
            else
                blockStart += Environment.NewLine.Length;

            int pos = blockStart;
            int braceCount = 0;
            char c;
            do
            {
                pos++;

                c = allCode[pos];
                switch (c)
                {
                    case '{':
                        braceCount++;
                        break;

                    case '}':
                        braceCount--;
                        break;
                }
            } while (pos < allCode.Length && !(c == '}' && braceCount == 0));

            int blockEnd = pos;

            string blockCode = allCode.Substring(blockStart, blockEnd - blockStart + 1);

            return removeIndent(blockCode);
        }

        private static string removeIndent(string code)
        {
            int indentSpaces = 0;
            while (code[indentSpaces] == ' ')
            {
                indentSpaces++;
            }

            StringBuilder builder = new StringBuilder();
            string[] codeLines = code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in codeLines)
            {
                if (indentSpaces < line.Length)
                    builder.AppendLine(line.Substring(indentSpaces));
                else
                    builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
