using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;

namespace Cobilas.Unity.Management.Resources {
    internal readonly struct CRC : IEnumerable<ResourceManager> {

        private readonly ResourceManager[] enumerator;

        public CRC(ResourceManager[] enumerator) => this.enumerator = enumerator;

        public IEnumerator<ResourceManager> GetEnumerator() {
            for (int I = 0; I < ArrayManipulation.ArrayLength(enumerator); I++)
                yield return enumerator[I];
        }

        IEnumerator IEnumerable.GetEnumerator()
            => (this as IEnumerable<ResourceManager>).GetEnumerator();
    }
}
