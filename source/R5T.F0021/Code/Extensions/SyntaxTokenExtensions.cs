using System;

using Microsoft.CodeAnalysis;

using Instances = R5T.F0021.Instances;


namespace System
{
    public static class SyntaxTokenExtensions
    {
        public static SyntaxToken PrependNewLine(this SyntaxToken token)
        {
            var output = token.Prepend(Instances.SyntaxGenerator.NewLine());
            return output;
        }

        public static SyntaxToken PrependSpace(this SyntaxToken token)
        {
            var output = token.Prepend(Instances.SyntaxGenerator.Space());
            return output;
        }
    }
}
