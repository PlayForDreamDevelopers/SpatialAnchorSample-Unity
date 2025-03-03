using System.Collections;
using System.Collections.Generic;

namespace YVR.Platform
{
    public class DeserializableList<T> : IList<T>
    {
        protected List<T> data { set; get; }

        public T this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }

        public int Count => data.Count;

        public bool IsReadOnly => ((IList<T>) data).IsReadOnly;

        public void Add(T item) { data.Add(item); }

        public void Clear() { data.Clear(); }

        public bool Contains(T item) { return data.Contains(item); }

        public void CopyTo(T[] array, int arrayIndex) { data.CopyTo(array, arrayIndex); }

        public IEnumerator<T> GetEnumerator() { return data.GetEnumerator(); }

        public int IndexOf(T item) { return data.IndexOf(item); }

        public void Insert(int index, T item) { data.Insert(index, item); }

        public bool Remove(T item) { return data.Remove(item); }

        public void RemoveAt(int index) { data.RemoveAt(index); }

        IEnumerator IEnumerable.GetEnumerator() { throw new System.NotImplementedException(); }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            
            str.Append($"Count:[{Count}],\n\r");
            str.Append($"IsReadOnly:[{IsReadOnly}],\n\r");
            str.Append($"data:[{data}],\n\r");
            
            return str.ToString();
        }
    }
}