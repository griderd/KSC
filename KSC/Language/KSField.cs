using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSC.Types;

namespace KSC.Language
{
    class KSField : BlockChild
    {
        public string Name { get; private set; }
        public bool IsLocal { get; private set; }
        public StructureType Type { get; private set; }
        public string Declaration { get; private set; }
        public int Length { get; private set; }
        public bool IsArray { get; private set; }

        /// <summary>
        /// Creates a KSField.
        /// </summary>
        /// <param name="name">Name of the field.</param>
        /// <param name="isLocal">Determines if the variable is local.</param>
        /// <param name="type">Type of the field.</param>
        /// <param name="length">Determines if the field is an array (LIST with compiler-defined length). A value of 0 or greater defines an array.</param>
        public KSField(string name, bool isLocal, StructureType type, int length = -1)
        {
            Name = name;
            IsLocal = isLocal;
            Type = type;
            Length = length;
            IsArray = length >= 0;

            StringBuilder sb = new StringBuilder();
            sb.Append("declare ");
            if (IsLocal)
                sb.Append("local ");
            sb.Append(Name);

            Declaration = sb.ToString();
        }

        /// <summary>
        /// Gets the declaration with the default value.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public override string ToKerboScript(kOSVersions version)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Declaration);
            sb.Append(" is ");

            if (Type.DefaultValue == null)
                throw new Exception("Type has no default value.");

            sb.Append(Type.DefaultValue);
            sb.Append(".");
            return sb.ToString();
        }
    }
}
