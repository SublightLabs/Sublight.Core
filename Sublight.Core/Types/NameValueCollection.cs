using System.Collections.Generic;

namespace Sublight.Core.Types
{
    class NameValueCollection
    {
        public void Add(string name, object value)
        {
            _collection.Add(name, value);
        }

        public Dictionary<string, object> Collection
        {
            get { return _collection; }
        }

        private readonly Dictionary<string, object> _collection = new Dictionary<string, object>();
    }
}
