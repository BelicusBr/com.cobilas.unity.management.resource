using System;
using UnityEngine;
using UEObject = UnityEngine.Object;

namespace Cobilas.Unity.Management.Resources {
    [Serializable]
    public struct ResourceItem : IDisposable, IEquatable<ResourceItem>, IEquatable<string>, IEquatable<Type> {
        [SerializeField] private string relativePath;
        [SerializeField] private UEObject item;

        public UEObject Item => item;
        public string Name => GetItemName();
        public string RelativePath => relativePath;

        public ResourceItem(UEObject item, string relativePath) {
            this.item = item;
            this.relativePath = relativePath;
        }

        public void Dispose() {
            item = null;
            relativePath = string.Empty;
        }

        public bool Equals(string other)
            => other == relativePath || 
            other ==  relativePath.Replace("Resources/", string.Empty).Replace("Resources", string.Empty) || 
            other == GetItemName();

        public bool Equals(Type other) {
            if (item == null) return false;
            return other.IsSubclassOf(typeof(UEObject)) && (item.CompareType(other) || item.GetType().IsSubclassOf(other));
        }

        public bool Equals(ResourceItem other)
            => other.relativePath == relativePath && other.item == item;

        public override int GetHashCode() {
            int h = relativePath == null ? 0 : relativePath.GetHashCode();
            int h2 = item == null ? 0 : item.GetHashCode();
            return h >> h2 ^ base.GetHashCode();
        }

        public override bool Equals(object obj)
            => (obj is ResourceItem rci && Equals(rci)) || (obj is string stg && Equals(stg)) || (obj is Type tp && Equals(tp));

        private string GetItemName() => item == null ? string.Empty : item.name;

        public static bool operator ==(ResourceItem A, ResourceItem B) => A.Equals(B);
        public static bool operator !=(ResourceItem A, ResourceItem B) => !(A == B);

        public static bool operator ==(ResourceItem A, Type B) => A.Equals(B);
        public static bool operator !=(ResourceItem A, Type B) => !(A == B);
        public static bool operator ==(Type A, ResourceItem B) => B.Equals(A);
        public static bool operator !=(Type A, ResourceItem B) => !(A == B);

        public static bool operator ==(ResourceItem A, string B) => A.Equals(B);
        public static bool operator !=(ResourceItem A, string B) => !(A == B);
        public static bool operator ==(string A, ResourceItem B) => B.Equals(A);
        public static bool operator !=(string A, ResourceItem B) => !(A == B);
    }
}
