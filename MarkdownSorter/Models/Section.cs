using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownSorter.Models
{
    internal class Section
    {
        public Dictionary<Block, string?> Content { get;} = new Dictionary<Block, string>();
        public bool HasContent => Content.Count > 0;

        public override string ToString()
        {
            return Content.Select(item => item.Value).FirstOrDefault(string.Empty);
        }

        public string GetSectionContent()
        {
            var builder = new StringBuilder();
            foreach (var item in Content)
            {
                var block = item.Key;
                var text = item.Value;

                switch(block)
                {
                    case HtmlBlock htmlBlock:
                        builder.AppendLine(text);
                        break;
                    case HeadingBlock headingBlock:
                        var headingMark = string.Join(string.Empty, Enumerable.Repeat("#", headingBlock.Level));
                        builder.AppendLine($@"{headingMark} {text}");
                        break;
                    case FencedCodeBlock fencedCodeBlock:
                        var codeType = fencedCodeBlock.Info ?? "text";
                        builder.Append($@"```{codeType}{Environment.NewLine}{text}```");
                        break;
                    default:
                        break;
                }
                builder.AppendLine();
            }

            return builder.ToString().TrimEnd();
        }
    }
}
