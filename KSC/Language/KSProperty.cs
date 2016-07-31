using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSC.Types;

namespace KSC.Language
{
    class KSProperty
    {
        public string Name { get; private set; }
        public StructureType Type { get; private set; }
        public bool IsLocal { get; private set; }
        public bool SetIsPublic { get; private set; }

        public StructureType InstanceType { get; private set; }

        public KSFunction Getter { get; private set; }
        public KSFunction Setter { get; private set; }

        public KSProperty(string name, StructureType type, StructureType instanceType, bool isLocal = true, bool setIsPublic = true)
        {
            Name = name;
            Type = type;
            IsLocal = isLocal;
            SetIsPublic = setIsPublic;
            InstanceType = instanceType;

            Getter = new KSFunction(name + "_get", type);
            Getter.AddParameter(new KSField("instance", true, instanceType));

            if (setIsPublic)
            {
                Setter = new KSFunction(name + "_set", type);
                Setter.AddParameter(new KSField("instance", true, instanceType));
                Setter.AddParameter(new KSField("value", true, type));
            }
            else
            {
                Setter = null;
            }
        }

        public KSProperty Clone()
        {
            return new KSProperty(Name, Type, InstanceType, IsLocal, SetIsPublic);
        }
    }
}
