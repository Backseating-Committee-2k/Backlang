﻿using Backlang.Codeanalysis.Parsing.AST;
using Loyc.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TestProject1
{
    public class ParserTestBase
    {
        protected static LNodeList ParseAndGetNodes(string source)
        {
            var ast = CompilationUnit.FromText(source);

            var node = ast.Body;

            Assert.IsNotNull(node);
            Assert.AreEqual(ast.Messages.Count, 0);

            return node;
        }

        protected static LNodeList ParseAndGetNodesInFunction(string source)
        {
            var tree = ParseAndGetNodes("fn main() {" + source + "}");

            return tree.First().Args[3].Args;
        }
    }
}