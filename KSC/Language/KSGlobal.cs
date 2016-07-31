using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSC.Language
{
    class KSGlobal : IBlockParent<KSExpression>, ICloneable<KSGlobal>
    {
        List<KSExpression> children;

        public KSExpression[] Children
        {
            get { return children.ToArray(); }
        }

        public KSGlobal()
        {
            children = new List<KSExpression>();
        }

        IBlockChild[] IBlockParent.Children
        {
            get { return Children; }
        }

        public IBlockChild Parent
        {
            get { return null; }
        }

        public KSGlobal Clone()
        {
            KSGlobal copy = new KSGlobal();

            for (int i = 0; i < children.Count; i++)
            {
                copy.AddChild(children[i].Clone());
            }

            return copy;
        }

        public void AddChild(KSExpression child)
        {
            children.Add(child);
        }


        public void AddChild(IBlockChild child)
        {
            throw new NotImplementedException();
        }


        public void SetParent(IBlockChild parent)
        {
            throw new NotImplementedException();
        }


        public string ToKerboScript(kOSVersions version)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < children.Count; i++)
            {
                sb.Append(children[i].ToKerboScript(version));
            }

            return sb.ToString();
        }
    }
}
