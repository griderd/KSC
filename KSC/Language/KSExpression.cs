using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSC.Language
{
    class KSExpression : BlockChild, ICloneable<KSExpression>
    {
        List<KSExpressionPart> parts;
        KSField field;

        public KSExpressionPart[] Parts { get { return parts.ToArray(); } }
        public KSField Field { get; private set; }

        public KSExpression(KSField field)
        {
            parts = new List<KSExpressionPart>();
            this.field = field;
        }

        public KSExpression Clone()
        {
            throw new NotImplementedException();
        }

        public override string Contents
        {
            get { return null; }
        }

        public override string ToKerboScript(kOSVersions version)
        {
            throw new NotImplementedException();
        }
    }
}
