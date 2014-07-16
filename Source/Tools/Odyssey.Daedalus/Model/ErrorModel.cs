using System;
using System.Text.RegularExpressions;

namespace Odyssey.Daedalus.Model
{
    public enum Category
    {
        Warning,
        Error,
        NodeException
    }

    public struct ErrorModel
    {
        const string regexLineColumnPattern = @"((?<line>\d+),(?<column>\d+))\W+(?<category>\w+)\s(?<message>.*)";
        const string regexGenericPattern = @"(?<category>\w+)\s(?<message>.*)";

        public int Line { get; private set; }
        public int Column { get; private set; }
        public string Shader { get; private set; }
        public string Description { get; private set; }
        public Category Category { get; private set; }

        public ErrorModel(string shader, string errorMessage, bool fromFXCError = true)
            : this()
        {
            Shader = shader;
            if (fromFXCError)
                InitFromFXCError(errorMessage);
            else
                InitFromException(errorMessage);
        }

        void InitFromException(string errorMessage)
        {
            Description = errorMessage;
            Category = Category.NodeException;
        }

        void InitFromFXCError(string errorMessage)
        {
            Regex regex = new Regex(regexLineColumnPattern);
            Match match = regex.Match(errorMessage);

            Line = match.Groups["line"].Success ? Int32.Parse(match.Groups["line"].Value) : 0;
            Column = match.Groups["column"].Success ? Int32.Parse(match.Groups["column"].Value) : 0;

            if (Line == 0 && Column == 0)
            {
                regex = new Regex(regexGenericPattern);
                match = regex.Match(errorMessage);
            }

            Description = match.Groups["message"].Value;
            Category = match.Groups["category"].Value == "error" ? Category.Error : Category.Warning;
        }
    }
}
