using System;
using System.IO;
using UnityEngine;

namespace Cobilas.Unity.Management.Resources {
    /// <summary>Cobilas Resources Type</summary>
    public struct CRItem : IEquatable<string>, IEquatable<Type>, IDisposable {
        [SerializeField] private string objectType;
        [SerializeField] private string relativePath;

        public string RelativePath => relativePath;
        public Type ObjectType => Type.GetType(objectType);
        public string FileName => Path.GetFileName(relativePath);
        public string RelativeDirectoryName => Path.GetDirectoryName(relativePath).Replace('\\', '/');

        internal CRItem(string relativePath, string objectType) {
            this.relativePath = relativePath;
            this.objectType = objectType;
        }

        public CRItem(string relativePath, Type objectType) :
            this(relativePath, objectType.AssemblyQualifiedName) { }

        public bool Equals(Type other)
            => other == ObjectType && other.IsSubclassOf(typeof(UnityEngine.Object));

        public bool Equals(string other)
            => other == relativePath || other == FileName || other == RelativeDirectoryName;

        public override int GetHashCode()
            => base.GetHashCode() >> GetHashCodeSafe(relativePath) ^ GetHashCodeSafe(objectType);

        public override bool Equals(object obj)
            => (obj is string str && Equals(str)) || (obj is Type typ && Equals(typ));

        public override string ToString()
            => "{\n" +
            $"\tRelativePath:{relativePath}\n" +
            $"\tObjectType:{objectType}\n" +
            "}\n";

        public void Dispose()
            => objectType = relativePath = (string)null;

        private int GetHashCodeSafe(object obj)
            => obj == null ? 0 : obj.GetHashCode();

        public static bool operator ==(CRItem A, Type B) => A.Equals(B);
        public static bool operator !=(CRItem A, Type B) => !(A == B);
        public static bool operator ==(Type A, CRItem B) => A.Equals(B);
        public static bool operator !=(Type A, CRItem B) => !(A == B);

        public static bool operator ==(CRItem A, string B) => A.Equals(B);
        public static bool operator !=(CRItem A, string B) => !(A == B);
        public static bool operator ==(string A, CRItem B) => A.Equals(B);
        public static bool operator !=(string A, CRItem B) => !(A == B);
    }
}
