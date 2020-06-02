using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.CSharp
{
    public class Hashtable<TKey, TValue> : Object, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, IDictionary
    {
        #region Fields
        private Dictionary<TKey, TValue> _dic;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public Hashtable()
        {
            this.__value__ = _dic = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// 
        /// </summary>
        public Hashtable(IDictionary<TKey, TValue> dic) : this()
        {
            if (dic == null)
            {
                throw new ArgumentNullException();
            }
            if (dic is Hashtable<TKey, TValue>)
            {
                dic = (dic as Hashtable<TKey, TValue>)._dic;
            }

            foreach (var item in dic)
            {
                _dic.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Hashtable(Undefined value) : this()
        {
            this.__value__ = value;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public Type KeyType
        {
            get { return typeof(TKey); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type ValueType
        {
            get { return typeof(TValue); }
        }

        /// <summary>
        /// 
        /// </summary>
        public TValue this[TKey key]
        {
            get
            {
                this.CheckUndefined();

                if (key != null && this._dic.ContainsKey(key))
                {
                    return _dic[key];
                }
                return default(TValue);
            }
            set
            {
                this.CheckUndefined();

                _dic[key] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                this.CheckUndefined();

                return this._dic.Keys;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                this.CheckUndefined();

                return this._dic.Values;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                this.CheckUndefined();

                return this._dic.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                return this.Keys;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                return this.Values;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                this.CheckUndefined();

                return (this._dic as ICollection<KeyValuePair<TKey, TValue>>).IsReadOnly;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                this.CheckUndefined();
                return false;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                this.CheckUndefined();
                return false;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return this.Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return this.Values;
            }
        }

        int ICollection.Count
        {
            get
            {
                return this.Count;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                this.CheckUndefined();
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                this.CheckUndefined();

                return (this._dic as ICollection).SyncRoot;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return this[ConvertKey(key)];
            }
            set
            {
                this[ConvertKey(key)] = (TValue)value;
            }
        }

        #endregion

        #region Operator Implicit
        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Hashtable<TKey, TValue>(Undefined value)
        {
            return new Hashtable<TKey, TValue>(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Hashtable<TKey, TValue>(Dictionary<TKey, TValue> dic)
        {
            return dic == null ? null : new Hashtable<TKey, TValue>(dic);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Dictionary<TKey, TValue>(Hashtable<TKey, TValue> table)
        {
            return (IsNull(table) || IsUndefined(table)) ? null : table._dic;
        }
        #endregion

        #region Implements Interfaces
        /// <summary>
        /// 
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            this.CheckUndefined();

            return _dic.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            this.CheckUndefined();

            return _dic.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            this.CheckUndefined();

            _dic.Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Remove(TKey key)
        {
            this.CheckUndefined();

            return _dic.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            this.CheckUndefined();

            return _dic.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            this.CheckUndefined();

            return this._dic.TryGetValue(key, out value);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            this.CheckUndefined();

            this._dic.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.CheckUndefined();

            (this._dic as ICollection<KeyValuePair<TKey, TValue>>).Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            this.CheckUndefined();

            return (this._dic as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.CheckUndefined();

            (this._dic as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return (this._dic as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Hashtable<TKey, TValue> valueOf()
        {
            return this;
        }

        void IDictionary.Add(object key, object value)
        {
            this.Add(ConvertKey(key), (TValue)value);
        }

        void IDictionary.Clear()
        {
            this.Clear();
        }

        bool IDictionary.Contains(object key)
        {
            return this.ContainsKey(ConvertKey(key));
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            this.CheckUndefined();

            return this._dic.GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            this.Remove(ConvertKey(key));
        }

        void ICollection.CopyTo(System.Array array, int index)
        {
            this.CheckUndefined();

            (this._dic as IDictionary).CopyTo(array, index);
        }


        private TKey ConvertKey(object key)
        {
            Type toKeyType = typeof(TKey);
            Type keyType = key.GetType();

            if (toKeyType == keyType)
            {
                return (TKey)key;
            }

            //
            if (toKeyType == typeof(String) && keyType == typeof(string))
            {
                key = (String)(string)key;
            }
            else if (toKeyType == typeof(string) && keyType == typeof(String))
            {
                key = (string)(String)key;
            }
            else if (toKeyType == typeof(Number) && ObjectUtil.IsNumber(keyType))
            {
                key = (Number)ObjectUtil.ToDouble(key);
            }
            else if (ObjectUtil.IsNumber(toKeyType) && toKeyType != typeof(Number) && keyType == typeof(Number))
            {
                key = (double)(Number)key;
            }
            else if (toKeyType == typeof(Boolean) && keyType == typeof(bool))
            {
                key = (Boolean)(bool)key;
            }
            else if (toKeyType == typeof(bool) && keyType == typeof(Boolean))
            {
                key = (bool)(Boolean)key;
            }

            return (TKey)key;
        }
        #endregion

    }
}
