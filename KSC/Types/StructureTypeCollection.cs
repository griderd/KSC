using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSC.Types
{
    class StructureTypeCollection : ICollection<StructureType>
    {
        List<StructureType> types;

        public StructureTypeCollection()
        {
            types = new List<StructureType>();
        }

        public bool Contains(string typeName)
        {
            return GetType(typeName) != null;
        }

        public StructureType GetType(string typeName)
        {
            for (int i = 0; i < Count; i++)
            {
                if (types[i].TypeName == typeName)
                    return types[i];
            }
            return null;
        }

        public bool TryGetType(string typeName, out StructureType type)
        {
            for (int i = 0; i < Count; i++)
            {
                if (types[i].TypeName == typeName)
                {
                    type = types[i];
                    return true;
                }
            }
            type = null;
            return false;
        }

        public void Add(StructureType item)
        {
            if (types.Contains(item))
                throw new Exception("Duplicate object found.");
            types.Add(item);
        }

        public void AddRange(StructureType[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (Contains(items[i]))
                    throw new Exception("Duplicate object found.");
                types.Add(items[i]);
            }
        }

        public void Clear()
        {
            types.Clear();
        }

        public bool Contains(StructureType item)
        {
            return types.Contains(item);
        }

        public void CopyTo(StructureType[] array, int arrayIndex)
        {
            types.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return types.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(StructureType item)
        {
            if (Contains(item))
            {
                types.Remove(item);
                return true;
            }
            return false;
        }

        public IEnumerator<StructureType> GetEnumerator()
        {
            return types.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
