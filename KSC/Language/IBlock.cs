using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSC.Language
{
    interface IBlock
    {
        string ToKerboScript(kOSVersions version);
    }

    interface IBlockChild : IBlock
    {
        IBlockChild Parent { get; }

        void SetParent(IBlockChild parent);
    }

    interface IBlockParent : IBlockChild
    {
        IBlockChild[] Children { get; }

        void AddChild(IBlockChild child);
    }

    interface IBlockParent<TChild> : IBlockParent
        where TChild : IBlockChild
    {
        new TChild[] Children { get; }

        void AddChild(TChild child);
    }

    interface IBlockChild<TParent> : IBlockChild
        where TParent : IBlockParent
    {
        new TParent Parent { get; }

        void SetParent(TParent parent);
    }
}
