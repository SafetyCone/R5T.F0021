using System;


namespace R5T.F0021
{
	public class SyntaxOperator : ISyntaxOperator
	{
		#region Infrastructure

	    public static SyntaxOperator Instance { get; } = new();

	    private SyntaxOperator()
	    {
	    }

	    #endregion
	}
}