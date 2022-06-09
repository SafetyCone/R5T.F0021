using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using R5T.T0134;

using Instances = R5T.F0021.Instances;


namespace System
{
    public static class MemberDeclarationSyntaxExtensions
    {
        /// <summary>
        /// Adds a modifier, ensuring that any leading trivia is transferred to the first modifier.
        /// </summary>
        /// <seealso cref="R5T.F0021.ISyntaxOperator.AddModifier{T}(T, Func{SyntaxToken}, Func{SyntaxTokenList, int}, out ISyntaxTokenAnnotation)"/>
        public static T AddModifier<T>(this T member,
            Func<SyntaxToken> modifierConstructor,
            Func<SyntaxTokenList, int> modifierInsertionIndexProvider,
            out ISyntaxTokenAnnotation modifierAnnotation)
            where T : MemberDeclarationSyntax
        {
            // First, create and annotate the modifier token.
            var modifierToken = modifierConstructor()
                .Annotate(out modifierAnnotation)
                ;

            // Insert the modifier token at the proper place, moving leading trivia from member to first modifier if the added modifier is the first modifier.
            T ModifiersModifier(T member, SyntaxTokenList modifiers, out SyntaxTokenList modifiedModifiers)
            {
                // First find the position at which to add the modifier token.
                var indexForModifierToken = modifierInsertionIndexProvider(modifiers);

                if (IndexHelper.IsFirstIndex(indexForModifierToken))
                {
                    // If the modifier is the first modifier, move whatever leading trivia the member had onto the modifier.
                    // Get the leading trivia before modifer is added, thus making leading trivia in separating trivia between the modifier and the first token.
                    var leadingTrivia = member.GetLeadingTrivia();

                    modifierToken = modifierToken.AddLeadingLeadingTrivia(leadingTrivia);

                    member = member.WithoutLeadingTrivia();
                }
                else
                {
                    // If the modifier is not the first modifier, prepend a separating space.
                    modifierToken = modifierToken.PrependSpace();
                }

                modifiedModifiers = modifiers.Insert(indexForModifierToken, modifierToken);

                return member;
            }

            member = member.ModifyModifiers(ModifiersModifier);

            // Check that the member is spaced from it's last modifier.
            member = member.EnsureSeparatedFromLastModifier();

            return member;
        }

        /// <inheritdoc cref="R5T.F0021.ISyntaxOperator.AddPublicModifier{T}(T, out ISyntaxTokenAnnotation)"/>
        public static T AddPublicModifier<T>(this T member,
           out ISyntaxTokenAnnotation publicModifierAnnotation)
           where T : MemberDeclarationSyntax
        {
            var output = member.AddModifier(
                Instances.SyntaxGenerator.Public,
                Instances.SyntaxOperator.GetIndexForPublicAccessModifer,
                out publicModifierAnnotation);

            return output;
        }

        public static T AddStaticModifier<T>(this T member,
           out ISyntaxTokenAnnotation staticModifierAnnotation)
           where T : MemberDeclarationSyntax
        {
            var output = member.AddModifier(
                Instances.SyntaxGenerator.Static,
                Instances.SyntaxOperator.GetIndexForStaticAccessModifer,
                out staticModifierAnnotation);

            return output;
        }

        public static T EnsureSeparatedFromLastModifier<T>(this T member,
            SyntaxTriviaList separation)
            where T : MemberDeclarationSyntax
        {
            return Instances.SyntaxOperator.EnsureSeparatedFromLastModifier(
                member,
                separation);
        }

        public static T EnsureSeparatedFromLastModifier<T>(this T member)
            where T : MemberDeclarationSyntax
        {
            return Instances.SyntaxOperator.EnsureSeparatedFromLastModifier(member);
        }

        /// <inheritdoc cref="R5T.F0021.ISyntaxOperator.MakePublic{T}(T, out ISyntaxTokenAnnotation)"/>
        public static T MakePublic<T>(this T member,
            out ISyntaxTokenAnnotation publicModifierAnnotation)
            where T : MemberDeclarationSyntax
        {
            // If the member is already public, short-circuit.
            var hasPublicModifier = member.HasPublicModifier();
            if (hasPublicModifier)
            {
                var modified = member.AnnotateToken(
                    hasPublicModifier.Result,
                    out publicModifierAnnotation);

                return modified;
            }
            else
            {
                // If not already public, add the partial modifier.
                // First remove all access modifiers.
                var modified = member.RemoveAccessModifiers();

                // Then add the public modifier.
                var output = modified.AddPublicModifier(
                    out publicModifierAnnotation);

                return output;
            }
        }

        /// <inheritdoc cref="MakePublic{T}(T, out ISyntaxTokenAnnotation)"/>
        public static T MakePublic<T>(this T member)
            where T : MemberDeclarationSyntax
        {
            return member.MakePublic(out _);
        }

        /// <inheritdoc cref="R5T.F0021.ISyntaxOperator.MakeStatic{T}(T, out ISyntaxTokenAnnotation)"/>
        public static T MakeStatic<T>(this T member,
            out ISyntaxTokenAnnotation staticModifierAnnotation)
            where T : MemberDeclarationSyntax
        {
            // If the member is already public, short-circuit.
            var hasStaticModifier = member.HasStaticModifier();
            if (hasStaticModifier)
            {
                member = member.AnnotateToken(
                    hasStaticModifier.Result,
                    out staticModifierAnnotation);

                return member;
            }
            else
            {
                // If not already public, add the partial modifier.
                return member.AddStaticModifier(
                    out staticModifierAnnotation);
            }
        }

        /// <inheritdoc cref="MakeStatic{T}(T, out ISyntaxTokenAnnotation)"/>
        public static T MakeStatic<T>(this T member)
            where T : MemberDeclarationSyntax
        {
            return member.MakeStatic(out _);
        }
    }
}
