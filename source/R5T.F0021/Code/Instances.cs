using System;

using R5T.F0004;
using R5T.F0007;

using ISyntaxGenerator = R5T.F0004.ISyntaxGenerator;


namespace R5T.F0021
{
    public static class Instances
    {
        public static ISpacingGenerator SpacingGenerator { get; } = F0007.SpacingGenerator.Instance;
        public static ISyntaxGenerator SyntaxGenerator { get; } = F0004.SyntaxGenerator.Instance;
        public static ISyntaxOperator SyntaxOperator { get; } = F0021.SyntaxOperator.Instance;
    }
}
