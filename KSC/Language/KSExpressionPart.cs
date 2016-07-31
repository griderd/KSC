using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSC.Language
{
    class KSExpressionPart : IBlock
    {
        public KSOperators Operator { get; private set; }
        public KSField Operand { get; private set; }

        public KSExpressionPart(KSField operand, KSOperators _operator = KSOperators.UnaryPlus)
        {
            Operator = _operator;
            Operand = operand;
        }

        public string ToKerboScript(kOSVersions version)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(OperatorToString(Operator));
            sb.Append(Operand.Name);

            return sb.ToString();
        }

        public static string OperatorToString(KSOperators op)
        {
            switch (op)
            {
                case KSOperators.Addition:
                    return "+";
                case KSOperators.And:
                    return "AND";
                case KSOperators.Division:
                    return "/";
                case KSOperators.Exponent:
                    return "^";
                case KSOperators.Multiplication:
                    return "*";
                case KSOperators.Not:
                    return "NOT";
                case KSOperators.Or:
                    return "OR";
                case KSOperators.UnaryMinus:
                case KSOperators.Subtraction:
                    return "-";
                case KSOperators.UnaryPlus:
                    return "";
                default:
                    return "";
            }
        }
    }
}
