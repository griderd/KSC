using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSC.Language
{
    abstract class BlockChild : IBlockChild
    {
        IBlockChild parent;

        public IBlockChild Parent
        {
            get { return parent; }
        }

        public void SetParent(IBlockChild parent)
        {
            this.parent = parent;
        }

        public abstract string ToKerboScript(kOSVersions version);
    }

    abstract class BlockChild<TParent> : IBlockChild<TParent>
        where TParent : class, IBlockParent
    {
        protected TParent parent;

        public TParent Parent
        {
            get { return parent; }
        }

        public void SetParent(TParent parent)
        {
            this.parent = parent;
        }

        IBlockChild IBlockChild.Parent
        {
            get { return parent; }
        }

        void IBlockChild.SetParent(IBlockChild parent)
        {
            this.parent = (TParent)parent;
        }

        public abstract string ToKerboScript(kOSVersions version);
    }

    abstract class Block<TParent, TChild> : BlockChild<TParent>, IBlockParent
        where TParent : class, IBlockParent
        where TChild : IBlockChild
    {
        protected List<TChild> children;

        public Block()
        {
            children = new List<TChild>();
        }

        public TChild[] Children { get { return children.ToArray(); } }

        public void AddChild(TChild child)
        {
            children.Add(child);
        }

        IBlockChild[] IBlockParent.Children
        {
            get { throw new NotImplementedException(); }
        }

        void IBlockParent.AddChild(IBlockChild child)
        {
            throw new NotImplementedException();
        }
    }
}
