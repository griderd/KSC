using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSC.Language;

namespace KSC.Types
{
    class StructureType
    {
        Dictionary<string, StructureType> fields;
        List<KSProperty> properties;
        List<KSFunction> functions;
        StructureType inherits;

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public string TypeName { get; private set; }

        /// <summary>
        /// Gets the field members inside the type.
        /// </summary>
        public string[] FieldNames { get { return fields.Keys.ToArray<string>(); } }

        /// <summary>
        /// Gets any property members (fields with get/set functions) inside the type.
        /// </summary>
        public KSProperty[] Properties { get { return properties.ToArray(); } }

        /// <summary>
        /// Gets the function members inside the type.
        /// </summary>
        public KSFunction[] Functions { get { return functions.ToArray(); } }

        /// <summary>
        /// Gets the type that this type inherits from.
        /// </summary>
        public StructureType Inherits { get { return inherits; } }

        /// <summary>
        /// Gets the default value, if any.
        /// </summary>
        public string DefaultValue { get; private set; }

        /// <summary>
        /// Gets the constructors of the type, if any.
        /// </summary>
        public KSFunction[] Constructors { get; private set; }

        /// <summary>
        /// Gets a value determining if the type has a public constructor.
        /// </summary>
        public bool HasPublicConstructor { get; private set; }

        /// <summary>
        /// Gets a value determining if the structure is a fundamental data type.
        /// </summary>
        public bool IsFundamentalType { get; private set; }
        
        /// <summary>
        /// Creates an instance of StructureType. Inherits from nothing.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="defaultValue">Default value. If the type has no default value, use null.</param>
        /// <param name="constructors">Any public constructors that the type uses.</param>
        public StructureType(string typeName, string defaultValue, params KSFunction[] constructors)
        {
            if ((constructors == null) | (typeName == null))
                throw new ArgumentNullException();
            if (typeName == "")
                throw new ArgumentException();

            TypeName = typeName;
            fields = new Dictionary<string, StructureType>();
            properties = new List<KSProperty>();
            functions = new List<KSFunction>();
            inherits = null;
            DefaultValue = defaultValue;
            Constructors = constructors;
            HasPublicConstructor = constructors.Length > 0;
            IsFundamentalType = (constructors.Length == 0) & (defaultValue != null);
        }

        /// <summary>
        /// Creates an instance of StructureType with a defined inherited type.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="inherits">The type that this type inherits from.</param>
        /// <param name="defaultValue">Default value. If the type has no default value, use null.</param>
        /// <param name="constructors">Any public constructors that this type uses.</param>
        public StructureType(string typeName, StructureType inherits, string defaultValue, params KSFunction[] constructors)
        {
            if ((constructors == null) | (typeName == null))
                throw new ArgumentNullException();
            if (typeName == "")
                throw new ArgumentException();

            TypeName = typeName;
            fields = new Dictionary<string, StructureType>();
            properties = new List<KSProperty>();
            functions = new List<KSFunction>();
            this.inherits = inherits;
            DefaultValue = defaultValue;
            Constructors = constructors;
            HasPublicConstructor = constructors.Length > 0;
            IsFundamentalType = (constructors.Length == 0) & (defaultValue != null);
        }

        public void AddProperty(KSProperty property, StructureType type)
        {
            if (property == null)
                throw new ArgumentNullException();

            properties.Add(property);
            
            KSFunction getter = new KSFunction(property.Name + "_get", TypeName + "_" + property.Name + "_get@", property.Type);
            getter.AddParameter("instance", type);

            if (property.SetIsPublic)
            {
                KSFunction setter = new KSFunction(property.Name + "_set", TypeName + "_" + property.Name + "_set@", BuiltIns.Scalar);
                setter.AddParameter("instance", type);
                setter.AddParameter("value", property.Type);
            }


        }

        public void AddField(string name, StructureType type)
        {
            // ValueTypes aren't specific enough for fields or
            // property types, because StructureType can be any
            // user type. You don't want user type A to be assigned
            // to user type B, which can happen under the current architecture.
        }

        public void AddFunction(KSFunction function)
        {
            if (function == null)
                throw new ArgumentNullException();

            functions.Add(function);
        }

        public StructureType Clone()
        {
            StructureType clone = new StructureType(TypeName, Inherits, DefaultValue, Constructors);

            string[] fields = FieldNames;
            foreach (string field in fields)
            {
                clone.AddField(field, this.fields[field]);
            }

            foreach (KSFunction function in functions)
            {
                clone.AddFunction(function.Clone());
            }

            foreach (KSProperty property in properties)
            {
                clone.AddProperty(property.Clone(), property.Type);
            }

            return clone;
        }
    }
}
