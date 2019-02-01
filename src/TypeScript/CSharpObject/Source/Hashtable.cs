using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.DataVisualization.TypeScript
{
    public class Hashtable<TKey, TValue> : Object, IEnumerable<KeyValuePair<TKey, TValue>>
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
            this._value = _dic = new Dictionary<TKey, TValue>();
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
            this._value = value;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public TValue this[TKey key]
        {
            get
            {
                this.CheckUndefined();

                return _dic[key];
            }
            set
            {
                this.CheckUndefined();

                _dic[key] = value;
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
            return new Hashtable<TKey, TValue>(dic);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Dictionary<TKey, TValue>(Hashtable<TKey, TValue> table)
        {
            return table._dic;
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
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            this.CheckUndefined();

            _dic.Add(key, value);
        }
        #endregion

    }
}
