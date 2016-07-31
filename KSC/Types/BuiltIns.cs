using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSC.Language;

namespace KSC.Types
{
    static class BuiltIns
    {
        public static bool Initialized { get; private set; }

        #region Type List
        static StructureTypeCollection BuiltInTypes { get; private set; }

        public static StructureType Structure { get; private set; }
        public static StructureType Enumerable { get; private set; }
        public static StructureType Iterator { get; private set; }
        public static StructureType Lexicon { get; private set; }
        public static StructureType List { get; private set; }
        public static StructureType Queue { get; private set; }
        public static StructureType Range { get; private set; }
        public static StructureType Stack { get; private set; }
        public static StructureType Scalar { get; private set; }
        public static StructureType Vector { get; private set; }
        public static StructureType Direction { get; private set; }
        public static StructureType GeoCoordinates { get; private set; }
        public static StructureType TimeSpan { get; private set; }
        public static StructureType Volume { get; private set; }
        public static StructureType FileContent { get; private set; }
        public static StructureType AggregateResource { get; private set; }
        public static StructureType CrewMember { get; private set; }
        public static StructureType DockingPort { get; private set; }
        public static StructureType Element { get; private set; }
        public static StructureType Engine { get; private set; }
        public static StructureType Gimbal { get; private set; }
        public static StructureType kOSProcessor { get; private set; }
        public static StructureType Core { get; private set; }
        public static StructureType Orbit { get; private set; }
        public static StructureType Orbital { get; private set; }
        public static StructureType Vessel { get; private set; }
        public static StructureType Body { get; private set; }
        public static StructureType OrbitalVelocity { get; private set; }
        public static StructureType Atmosphere { get; private set; }
        public static StructureType BooleanType { get; private set; }
        public static StructureType RGBA { get; private set; }
        public static StructureType Config { get; private set; }
        public static StructureType Highlight { get; private set; }
        public static StructureType KOSDelegate { get; private set; }
        public static StructureType PIDLoop { get; private set; }
        public static StructureType ResourceTransfer { get; private set; }
        public static StructureType SteeringManager { get; private set; }
        public static StructureType StringType { get; private set; }
        public static StructureType Terminal { get; private set; }
        public static StructureType VecDraw { get; private set; }
        public static StructureType Waypoint { get; private set; }
        #endregion

        public static bool IsBuiltInType(string typeName)
        {
            return BuiltInTypes.Contains(typeName);
        }

        public static StructureType GetBuiltInType(string typeName)
        {
            return BuiltInTypes.GetType(typeName);
        }

        public static bool TryGetBuiltInType(string typeName, out StructureType type)
        {
            return BuiltInTypes.TryGetType(typeName, out type);
        }

        public static void Initialize()
        {
            BuiltInTypes = new StructureTypeCollection();

            // Base Type
            Structure = new StructureType("Structure", null);
            BuiltInTypes.Add(Structure);
            
            // Primitive Data Types
            Scalar = new StructureType("Scalar", Structure, "0"); BuiltInTypes.Add(Scalar);
            BooleanType = new StructureType("Boolean", Structure, "false", new KSFunction[]{}); BuiltInTypes.Add(BooleanType);

            // Strings
            StringType = new StructureType("String", Structure, "");
            KSFunction f = new KSFunction("contains", BooleanType);
            f.AddParameter("string", StringType);
            InitString();
            BuiltInTypes.Add(StringType);

            // Virtual Types
            Enumerable = new StructureType("Enumerable", Structure, null);
            BuiltInTypes.Add(Enumerable);
        }

        static void InitString()
        {
            KSFunction contains = new KSFunction("contains", BooleanType);
            contains.AddParameter("string", StringType);
            StringType.AddFunction(contains);

            KSFunction endswith = new KSFunction("endswith", BooleanType);
            endswith.AddParameter("string", StringType);
            StringType.AddFunction(endswith);

            KSFunction find = new KSFunction("find", Scalar);
            endswith.AddParameter("string", StringType);
            StringType.AddFunction(find);

            KSFunction findat = new KSFunction("findat", Scalar);
            findat.AddParameter("string", StringType);
            findat.AddParameter("startAt", Scalar);
            StringType.AddFunction(findat);

            KSFunction findlast = new KSFunction("findlast", Scalar);
            findlast.AddParameter("string", StringType);
            StringType.AddFunction(findlast);

            KSFunction findlastat = new KSFunction("findlastat", Scalar);
            findlastat.AddParameter("string", StringType);
            findlastat.AddParameter("startAt", Scalar);
            StringType.AddFunction(findlastat);

            StringType.AddFunction(find.GetAlias("indexof"));

            KSFunction insert = new KSFunction("insert", StringType);
            insert.AddParameter("index", Scalar);
            insert.AddParameter("string", StringType);
            StringType.AddFunction(insert);

            StringType.AddFunction(findlast.GetAlias("lastindexof"));

            
        }
    }
}
