using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCalculator
{
    public static class Calculator
    {
        public static bool Evaluate(string expression)
        {
            var inputStream = new AntlrInputStream(expression);
            var lexer = new LabCalculatorLexer(inputStream);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new ThrowExceptionErrorListener());

            var tokens = new CommonTokenStream(lexer);
            var parser = new LabCalculatorParser(tokens);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ThrowExceptionErrorListener());

            var tree = parser.compileUnit();

            if (parser.NumberOfSyntaxErrors > 0)
                throw new ArgumentException("Синтаксична помилка у виразі");

            var visitor = new LabCalculatorVisitor();
            double result = visitor.Visit(tree);
            return result != 0.0;
        }



    }
}