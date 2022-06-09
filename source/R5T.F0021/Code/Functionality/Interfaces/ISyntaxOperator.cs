using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using R5T.T0132;
using R5T.T0134;


namespace R5T.F0021
{
	[FunctionalityMarker]
	public interface ISyntaxOperator : IFunctionalityMarker,
        F0003.ISyntaxOperator,
        F0007.ISyntaxOperator
	{
        public TParentNode AddMember<TParentNode, TMember>(
            ISyntaxNodeAnnotation<NamespaceDeclarationSyntax> namespaceAnnotation,
            TParentNode parentNode,
            TMember member,
            out ISyntaxNodeAnnotation<TMember> memberAnnotation)
            where TParentNode : SyntaxNode
            where TMember : MemberDeclarationSyntax
        {
            member = member.Annotate(out memberAnnotation);

            parentNode = namespaceAnnotation.Modify(
                parentNode,
                @namespace => @namespace.AddMembers(member));

            return parentNode;
        }

        /// <inheritdoc cref="MemberDeclarationSyntaxExtensions.AddModifier{T}(T, Func{SyntaxToken}, Func{SyntaxTokenList, int}, out ISyntaxTokenAnnotation)"/>
        public T AddModifier<T>(
            T member,
            Func<SyntaxToken> modifierConstructor,
            Func<SyntaxTokenList, int> modifierInsertionIndexProvider,
            out ISyntaxTokenAnnotation modifierAnnotation)
            where T : MemberDeclarationSyntax
        {
            return member.AddModifier(
                modifierConstructor,
                modifierInsertionIndexProvider,
                out modifierAnnotation);   
        }

        /// <summary>
        /// Adds the public access modifier.
        /// </summary>
        public T AddPublicModifier<T>(
            T member,
            out ISyntaxTokenAnnotation publicModifierAnnotation)
            where T : MemberDeclarationSyntax
        {
            return member.AddPublicModifier(
                out publicModifierAnnotation);
        }

        public T AddStaticModifier<T>(
            T member,
            out ISyntaxTokenAnnotation staticModifierAnnotation)
            where T : MemberDeclarationSyntax
        {
            return member.AddStaticModifier(
                out staticModifierAnnotation);
        }

        /// <summary>
        /// <test>Tests to see if there is any separating trivia between the last modifier and the next token (either the keyword for types, or the return type for methods).</test>
        /// If there is not, prepends the separation to the next token.
        /// </summary>
        public T EnsureSeparatedFromLastModifier<T>(
            T member,
            SyntaxTriviaList separation)
            where T : MemberDeclarationSyntax
        {
            var hasLastModifier = member.HasLastModifier();
            if (hasLastModifier)
            {
                var nextToken = hasLastModifier.Result.GetNextToken();

                var lastModifierHasAnyTrailingSeparatingTrivia = hasLastModifier.Result.HasAnyTrailingSeparatingTrivia(nextToken);
                if (!lastModifierHasAnyTrailingSeparatingTrivia)
                {
                    // Ok to replace, as the next token *will* be within the member for valid syntax.
                    member = member.ReplaceToken_Better(
                        nextToken,
                        nextToken.Prepend(separation));
                }
            }

            return member;
        }

        /// <summary>
        /// <inheritdoc cref="EnsureSeparatedFromLastModifier{T}(T, SyntaxTriviaList)" path="/summary/test"/>
        /// If there is not, prepends a space to the next token.
        /// </summary>
        public T EnsureSeparatedFromLastModifier<T>(
            T member)
            where T : MemberDeclarationSyntax
        {
            var output = member.EnsureSeparatedFromLastModifier(
                Instances.SyntaxGenerator.Space().ToSyntaxTriviaList());

            return output;
        }

        /// <summary>
        /// Add the public access modifier (removing all other access modifiers).
        /// </summary>
        public T MakePublic<T>(
            T member,
            out ISyntaxTokenAnnotation publicModifierAnnotation)
            where T : MemberDeclarationSyntax
        {
            return member.MakePublic(out publicModifierAnnotation);
        }

        /// <summary>
        /// Add the static modifier.
        /// </summary>
        public T MakeStatic<T>(
            T member,
            out ISyntaxTokenAnnotation staticModifierAnnotation)
            where T : MemberDeclarationSyntax
        {
            return member.MakeStatic(out staticModifierAnnotation);
        }
    }
}