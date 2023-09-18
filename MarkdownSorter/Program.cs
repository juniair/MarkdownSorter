// See https://aka.ms/new-console-template for more information
using CommandLine;
using CommandLine.Text;
using Markdig;
using MarkdownSorter.Commands;
using MarkdownSorter.Repository;
using MarkdownSorter.Services;

using (var parser = new Parser())
{
    var parsed = parser.ParseArguments<CommandOptions>(args);
    parsed
        .WithParsed(options =>
        {
            if (!File.Exists(options.Source))
            {
                Console.Error.WriteLine(HelpText.AutoBuild(parsed, null, null));
            }


            var service = new MarkdownService(options.Source);
            service.Sort(options.Output);
        })
        .WithNotParsed(errors => Console.WriteLine(HelpText.AutoBuild(parsed)));
}
