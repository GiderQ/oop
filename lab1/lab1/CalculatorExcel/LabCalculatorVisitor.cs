using LabCalculator;

class LabCalculatorVisitor : LabCalculatorBaseVisitor<double>
{
    Dictionary<string, double> tableIdentifier = new Dictionary<string, double>();

    public override double VisitCompileUnit(LabCalculatorParser.CompileUnitContext context)
    {
        return Visit(context.expression());
    }

    public override double VisitNumberOperand(LabCalculatorParser.NumberOperandContext context)
    {
        return double.Parse(context.GetText());
    }

    public override double VisitIdentifierOperand(LabCalculatorParser.IdentifierOperandContext context)
    {
        double value;
        if (tableIdentifier.TryGetValue(context.GetText(), out value))
            return value;
        return 0.0;
    }

    public override double VisitParenthesizedOperand(LabCalculatorParser.ParenthesizedOperandContext context)
    {
        return Visit(context.expression());
    }

    public override double VisitUnaryOperand(LabCalculatorParser.UnaryOperandContext context)
    {
        double val = Visit(context.atom());
        if (context.OP_ADD() != null) return val;     // +x
        if (context.OP_SUB() != null) return -val;   // -x
        return val;
    }

    public override double VisitModDivOperand(LabCalculatorParser.ModDivOperandContext context)
    {
        double left = Visit(context.operand(0));
        double right = Visit(context.operand(1));
        if (context.OP_MOD() != null) return left % right;
        if (context.OP_DIV() != null) return Math.Floor(left / right); // integer division
        return 0.0;
    }

    public override double VisitComparison(LabCalculatorParser.ComparisonContext context)
    {
        double leftVal = Visit(context.left);
        double rightVal = Visit(context.right);

        var op = context.op.Type;

        if (op == LabCalculatorLexer.OP_EQUAL) return leftVal == rightVal ? 1.0 : 0.0;
        if (op == LabCalculatorLexer.OP_LESS) return leftVal < rightVal ? 1.0 : 0.0;
        if (op == LabCalculatorLexer.OP_GREATER) return leftVal > rightVal ? 1.0 : 0.0;

        return 0.0;
    }


    public override double VisitNotExpr(LabCalculatorParser.NotExprContext context)
    {
        double val = Visit(context.logicExpression());
        return val == 0.0 ? 1.0 : 0.0;
    }

    public override double VisitAndExpr(LabCalculatorParser.AndExprContext context)
    {
        double left = Visit(context.logicExpression(0));
        double right = Visit(context.logicExpression(1));
        return (left != 0.0 && right != 0.0) ? 1.0 : 0.0;
    }

    public override double VisitOrExpr(LabCalculatorParser.OrExprContext context)
    {
        double left = Visit(context.logicExpression(0));
        double right = Visit(context.logicExpression(1));
        return (left != 0.0 || right != 0.0) ? 1.0 : 0.0;
    }
}
