using System;

namespace CodeDOMExample
{
    [Serializable]
    internal class CompilationException : Exception
    {
        private static readonly string nl = Environment.NewLine;

        public CompilationException(string sourceFileName, string pathToAssembly, string[] errors)
        {
            string stockErrorText = $"encountered while building \"{sourceFileName}\" into \"{pathToAssembly}\"";
            if (errors?.Length > 0)
            {
                string errorText = string.Join(nl, errors);
                Message = $"Errors {stockErrorText}: {nl}{errorText}";
            }
            else
            {
                Message = $"Error of unknown reason {stockErrorText}.";
            }
        }

        public override string Message { get; }
    }
}