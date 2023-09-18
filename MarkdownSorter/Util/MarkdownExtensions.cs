using Markdig.Syntax;
using MarkdownSorter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownSorter.Util
{
    internal static class MarkdownExtensions
    {
        public static string? GetContentString(this Block block) => block switch
        {
            HtmlBlock htmlBlock => GetHtmlBlockString(htmlBlock),
            HeadingBlock headingBlock => GetHeadingBlockString(headingBlock),
            FencedCodeBlock codeBlock => GetCodeBlockString(codeBlock),
            _ => string.Empty
        };

        private static string? GetCodeBlockString(FencedCodeBlock codeBlock)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var line in codeBlock.Lines)
            {
                builder.AppendLine(line.ToString());
            }

            return builder.ToString();
        }

        private static string? GetHtmlBlockString(HtmlBlock htmlBlock)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var line in htmlBlock.Lines)
            {
                var str = line.ToString();
                if (string.IsNullOrEmpty(str))
                {
                    builder.AppendLine(Environment.NewLine);
                }
                else
                {
                    builder.AppendLine(line.ToString());
                }

            }

            return builder.ToString();
        }

        private static string? GetHeadingBlockString(HeadingBlock headingBlock)
        {

            StringBuilder builder = new StringBuilder();

            foreach (var line in headingBlock.Inline)
            {
                var str = line.ToString();
                if(string.IsNullOrEmpty(str))
                {
                    builder.Append(Environment.NewLine);
                }
                else
                {
                    builder.Append(line.ToString());
                }
                
            }

            return builder.ToString();
        }

        public static List<Section> SortByHeaderBlockText(this List<Section> sections, bool isReverse = false)
        {
            var target = sections.ToList();
            target.Sort((sectionX, sectionY) =>
            {
                if (sectionX.HasContent && !sectionY.HasContent)
                {
                    return isReverse ? -1 : 1;
                }
                else if (!sectionX.HasContent && sectionY.HasContent)
                {
                    return isReverse ? 1 :- 1;
                }
                else
                {
                    var sectionXHeaderText = sectionX.Content.Select(item => item.Value)
                                                                .FirstOrDefault(string.Empty);
                    var sectionYHeaderText = sectionY.Content.Select(item => item.Value)
                                                                .FirstOrDefault(string.Empty);

                    var result = (string.IsNullOrEmpty(sectionXHeaderText), string.IsNullOrEmpty(sectionYHeaderText)) switch
                    {
                        (false, true) => 1,
                        (true, false) => -1,
                        _ => 0
                    };

                    if(result == 0)
                    {
                        result = sectionXHeaderText.CompareTo(sectionYHeaderText);
                    }

                    return isReverse ? -result : result;
                }
            });

            return target;
        }

        public static List<Section> SortByHeaderLevel(this List<Section> sections, bool isReverse = false)
        {
            var target = sections.ToList();
            target.Sort((sectionX, sectionY) =>
            {
                if (sectionX.HasContent && !sectionY.HasContent)
                {
                    return isReverse ? -1 : 1;
                }
                else if (!sectionX.HasContent && sectionY.HasContent)
                {
                    return isReverse ? 1 : -1;
                }
                else
                {
                    var sectionXHeaderBlock = sectionX.Content.Select(item => item.Key)
                                                                .FirstOrDefault(block => block is HeadingBlock, null) as HeadingBlock;
                    var sectionYHeaderBlock = sectionY.Content.Select(item => item.Key)
                                                                .FirstOrDefault(block => block is HeadingBlock, null) as HeadingBlock;
                    if (sectionXHeaderBlock is not null && sectionYHeaderBlock is null)
                    {
                        return isReverse ? -1 : 1;
                    }
                    else if (sectionXHeaderBlock is null && sectionYHeaderBlock is not null)
                    {
                        return isReverse ? 1 : -1;
                    }
                    else
                    {
                        var sectionXHeaderLevel = sectionXHeaderBlock?.Level ?? 0;
                        var sectionYHeaderLeve = sectionYHeaderBlock?.Level ?? 0;
                        return isReverse ? sectionYHeaderLeve.CompareTo(sectionXHeaderLevel) :
                                                sectionXHeaderLevel.CompareTo(sectionYHeaderLeve);
                    }
                }
            });

            return target;
        }
    }
}
