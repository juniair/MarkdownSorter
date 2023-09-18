using Markdig;
using Markdig.Syntax;
using MarkdownSorter.Models;
using MarkdownSorter.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownSorter.Repository
{
    internal class MarkdownRepository
    {

        private readonly MarkdownDocument _document;

        public MarkdownRepository(string source)
        {
            var text = File.ReadAllText(source);

            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            _document = Markdown.Parse(text, pipeline);
        }

        public List<Section> GetSectionByDelimiter(string delimiter = "---")
        {
            var sections = new List<Section>();
            var currentSections = new Section();
            
            foreach(var block in _document)
            {
                if(block is ThematicBreakBlock)
                {
                    sections.Add(currentSections);
                    currentSections = new Section();
                }
                else
                {
                    var content = block.GetContentString();
                    currentSections.Content[block] = content;
                }
            }

            return sections;
        }

    }
}
