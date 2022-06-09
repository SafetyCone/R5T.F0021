using System;

using Microsoft.CodeAnalysis;

using Instances = R5T.F0021.Instances;


namespace System
{
    public static class SyntaxNodeExtensions
    {
        public static TNode PrependNewLine<TNode>(this TNode node)
            where TNode : SyntaxNode
        {
            var output = node.AddLeadingLeadingTrivia(
                Instances.SpacingGenerator.NewLine());

            return output;
        }
    }
}
