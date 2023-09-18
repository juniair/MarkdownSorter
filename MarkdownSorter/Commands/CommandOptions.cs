using CommandLine;

namespace MarkdownSorter.Commands
{
    internal class CommandOptions
    {
        [Option("source", HelpText = "정렬할 대상의 원본 파일 경로를 설정합니다.", Required = true)]
        public string Source { get; set; }

        [Option("output", HelpText = "정렬 후 결과 파일 경로를 설정합니다.", Required = true)]
        public string Output { get; set; }
    }
}
