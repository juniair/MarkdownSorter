using Markdig.Syntax;
using MarkdownSorter.Repository;
using MarkdownSorter.Util;
using System.Linq;

namespace MarkdownSorter.Services
{
    internal class MarkdownService
    {
        private readonly MarkdownRepository _repository;

        public MarkdownService(string filePath)
        {
            _repository = new MarkdownRepository(filePath);
        }

        public void Sort(string outFilePath)
        {
            var sections = _repository.GetSectionByDelimiter("---").SortByHeaderLevel();
            sections = sections.SortByHeaderBlockText();

            var sectionContents = sections.Select(section => section.GetSectionContent()).ToList();

            using var writer = new StreamWriter(outFilePath);
            foreach (var sectionContent in sectionContents)
            {
                writer.WriteLine(sectionContent);
                writer.WriteLine(Environment.NewLine);
                writer.WriteLine("---");
                writer.WriteLine(Environment.NewLine);
            }

            Console.WriteLine($@"{outFilePath} write complate");

        }

    }
}
