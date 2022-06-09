using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using R5T.T0134;

using Instances = R5T.F0021.Instances;


namespace System
{
    public static class NamespaceDeclarationSyntaxExtensions
    {
        public static TParentNode AddMember<TParentNode, TMember>(this ISyntaxNodeAnnotation<NamespaceDeclarationSyntax> namespaceAnnotation,
            TParentNode parentNode,
            TMember member,
            out ISyntaxNodeAnnotation<TMember> memberAnnotation)
            where TParentNode : SyntaxNode
            where TMember : MemberDeclarationSyntax
        {
            return Instances.SyntaxOperator.AddMember(
                namespaceAnnotation,
                parentNode,
                member,
                out memberAnnotation);
        }

        public static TParentNode AddMember<TParentNode, TMember>(this ISyntaxNodeAnnotation<NamespaceDeclarationSyntax> namespaceAnnotation,
            TParentNode parentNode,
            TMember member)
            where TParentNode : SyntaxNode
            where TMember : MemberDeclarationSyntax
        {
            return namespaceAnnotation.AddMember(
                parentNode,
                member,
                out _);
        }
    }
}
