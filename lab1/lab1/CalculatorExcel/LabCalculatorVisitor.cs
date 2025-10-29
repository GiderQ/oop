using LabCalculator;

class LabCalculatorVisitor : LabCalculatorBaseVisitor<double>
{
    Dictionary<string, double> tableIdentifier = new Dictionary<string, double>();

    public override double VisitCompileUnit(LabCalculatorParser.CompileUnitContext context)
    {
        return Visit(context.logicExpression());
    }

    private bool AreEqual(double a, double b, double epsilon = 1e-9)
    {
        return Math.Abs(a - b) < epsilon;
    }

    public override double VisitComparison(LabCalculatorParser.ComparisonContext context)
    {
        double leftVal = Visit(context.left);
        double rightVal = Visit(context.right);
        var op = context.op.Type;

        if (op == LabCalculatorLexer.OP_EQUAL)
            return AreEqual(leftVal, rightVal) ? 1.0 : 0.0;
        if (op == LabCalculatorLexer.OP_LESS)
            return leftVal < rightVal ? 1.0 : 0.0;
        if (op == LabCalculatorLexer.OP_GREATER)
            return leftVal > rightVal ? 1.0 : 0.0;

        return 0.0;
    }

    public override double VisitNotExpr(LabCalculatorParser.NotExprContext context)
    {
        double val = Visit(context.expr);
        return val == 0.0 ? 1.0 : 0.0;
    }

    public override double VisitAndExpr(LabCalculatorParser.AndExprContext context)
    {
        double left = Visit(context.left);
        double right = Visit(context.right);
        return (left != 0.0 && right != 0.0) ? 1.0 : 0.0;
    }

    public override double VisitOrExpr(LabCalculatorParser.OrExprContext context)
    {
        double left = Visit(context.left);
        double right = Visit(context.right);
        return (left != 0.0 || right != 0.0) ? 1.0 : 0.0;
    }
    public override double VisitBoolExpr(LabCalculatorParser.BoolExprContext context)
    {
        return context.GetText() == "true" ? 1.0 : 0.0;
    }
    public override double VisitAddOperand(LabCalculatorParser.AddOperandContext context)
    {
        double left = Visit(context.expression(0));
        double right = Visit(context.expression(1));
        return left + right;
    }

    public override double VisitSubOperand(LabCalculatorParser.SubOperandContext context)
    {
        double left = Visit(context.expression(0));
        double right = Visit(context.expression(1));
        return left - right;
    }

    public override double VisitMulOperand(LabCalculatorParser.MulOperandContext context)
    {
        double left = Visit(context.expression(0));
        double right = Visit(context.expression(1));
        return left * right;
    }

    public override double VisitDivOperand(LabCalculatorParser.DivOperandContext context)
    {
        double left = Visit(context.expression(0));
        double right = Visit(context.expression(1));
        return right != 0 ? Math.Floor(left / right) : 0.0;
    }

    public override double VisitModOperand(LabCalculatorParser.ModOperandContext context)
    {
        double left = Visit(context.expression(0));
        double right = Visit(context.expression(1));
        return right != 0 ? left % right : 0.0;
    }
    public override double VisitAtomNumber(LabCalculatorParser.AtomNumberContext context)
    {
        return double.Parse(context.GetText());
    }

    public override double VisitAtomBool(LabCalculatorParser.AtomBoolContext context)
    {
        return context.GetText() == "true" ? 1.0 : 0.0;
    }

    public override double VisitAtomParenthesized(LabCalculatorParser.AtomParenthesizedContext context)
    {
        return Visit(context.expression());
    }

    public override double VisitUnaryPlus(LabCalculatorParser.UnaryPlusContext context)
    {
        return Visit(context.expression());
    }

    public override double VisitUnaryMinus(LabCalculatorParser.UnaryMinusContext context)
    {
        return -Visit(context.expression());
    }


}