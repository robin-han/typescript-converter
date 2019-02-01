using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace GrapeCity.DataVisualization.TypeScript
{
    public class DynamicObject : System.Dynamic.DynamicObject, IEnumerable
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, object> _dic;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public DynamicObject()
        {
            this._dic = new Dictionary<string, object>();
        }
        #endregion

        #region Operator Implicit
        /// <summary>
        /// 
        /// </summary>
        public static implicit operator bool(DynamicObject obj)
        {
            return !Object.IsNull(obj) && !Object.IsUndefined(obj);
        }
        #endregion

        #region Override Methods and Interface
        /// <summary>
        /// 
        /// </summary>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;
            if (!this._dic.TryGetValue(name, out result))
            {
                result = Object.undefined;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            string name = binder.Name;
            this._dic[name] = value;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            string name = indexes[0].ToString();
            if (!this._dic.TryGetValue(name, out result))
            {
                result = Object.undefined;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            string name = indexes[0].ToString();
            this._dic[name] = value;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return this._dic.GetEnumerator();
        }
        #endregion

        #region Methods
        public void Add(string key, object value)
        {
            this._dic.Add(key, value);
        }
        #endregion
    }
}
