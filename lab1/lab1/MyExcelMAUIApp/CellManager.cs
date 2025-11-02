using LabCalculator;
using System;
using System.Collections.Generic;

namespace MyExcelMAUIApp
{
    public class CellManager
    {
        private Dictionary<string, string> expressions = new();
        private Dictionary<string, double> values = new();
        public string GetDisplayText(string cellName)
        {
            if (!values.ContainsKey(cellName))
                return "";

            double val = values[cellName];
            if (double.IsNaN(val))
                return "Error";

            string expression = GetExpression(cellName);

            if ((val == 1.0 || val == 0.0) && IsLogical(expression))
                return val == 1.0 ? "true" : "false";

            return val.ToString();
        }

        public void SetExpression(string cellName, string expression)
        {
            expressions[cellName] = expression;

            RecalculateAll();
            RecalculateAll();
        }

        public string GetExpression(string cellName)
        {
            return expressions.TryGetValue(cellName, out var expr) ? expr : "";
        }

        public double GetValue(string cellName)
        {
            return values.TryGetValue(cellName, out var val) ? val : 0.0;
        }
        private bool IsLogical(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return false;

            string normalized = expression.ToLowerInvariant().Replace(" ", "");

            return normalized.Contains("=")
                || normalized.Contains("<")
                || normalized.Contains(">")
                || normalized.Contains("and")
                || normalized.Contains("or")
                || normalized.Contains("not")
                || normalized.Contains("true")
                || normalized.Contains("false");
        }


        public Dictionary<string, double> GetAllValues() => new(values);

        private void Recalculate(string cellName)
        {
            string expr = GetExpression(cellName);
            try
            {
                double result = Calculator.Evaluate(expr, values);
                values[cellName] = result;
            }
            catch
            {
                values[cellName] = double.NaN;
            }
        }
        public void RecalculateAll()
        {
            foreach (var cell in expressions.Keys)
            {
                Recalculate(cell);
            }
        }

    }
}
