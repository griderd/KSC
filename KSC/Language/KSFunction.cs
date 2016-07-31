using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSC.Types;

namespace KSC.Language
{
    class KSFunction : BlockChild<KSGlobal>
    {
        public string Name { get; private set; }
        public string Delegate { get; private set; }
        public bool IsUserFunction { get; private set; }
        public StructureType ReturnType { get; private set; }
        Dictionary<string, KSField> parameters;

        public string[] ParameterNames { get { return parameters.Keys.ToArray<string>(); } }

        public KSFunction(string name, string deleg, StructureType returnType)
        {
            Name = name;
            Delegate = deleg;
            ReturnType = returnType;
            parameters = new Dictionary<string, KSField>();
            IsUserFunction = true;
        }

        public KSFunction(string name, StructureType returnType)
        {
            Name = name;
            Delegate = "";
            ReturnType = returnType;
            parameters = new Dictionary<string, KSField>();
            IsUserFunction = false;
        }

        public void AddParameter(KSField field)
        {
            parameters.Add(field.Name, field);
        }

        public void AddParameter(string name, StructureType type)
        {
            parameters.Add(name, new KSField(name, true, type));
        }

        public KSField GetParameterType(string parameter)
        {
            if (parameters.ContainsKey(parameter))
            {
                return parameters[parameter];
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public KSFunction GetAlias(string alias)
        {
            KSFunction clone = Clone();
            clone.Name = alias;

            return clone;
        }

        public KSFunction Clone()
        {
            KSFunction clone = new KSFunction(Name, ReturnType.Clone());

            string[] parameters = ParameterNames;
            foreach (string parameter in parameters)
            {
                clone.AddParameter(this.parameters[parameter]);
            }

            return clone;
        }

        public override string ToKerboScript(kOSVersions version)
        {
            StringBuilder result = new StringBuilder();

            result.Append("function ");
            result.AppendLine(Name);
            result.AppendLine("{");

            string[] paramNames = ParameterNames;
            foreach (string paramName in paramNames)
            {
                result.Append("local parameter ");
                result.Append(paramName);
                result.Append('.');
                result.AppendLine();
            }

            return result.ToString();
        }
    }
}
