using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TypeScript.CSharp
{
    public class Array<T> : Object, IEnumerable<T>
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private List<T> _list;

        private object _from = null; //Used for As, 
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public Array()
        {
            this.__value__ = this._list = new List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        public Array(Number length)
        {
            this.__value__ = this._list = new List<T>();
            this.length = length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public Array(T v)
        {
            this.__value__ = this._list = new List<T>();
            this._list.Add(v);
        }

        /// <summary>
        /// Initialize a new instance with the items.
        /// </summary>
        public Array(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException();
            }

            this.__value__ = this._list = new List<T>(items);
        }

        /// <summary>
        /// 
        /// </summary>
        private Array(Undefined value)
        {
            this._list = null;
            this.__value__ = value;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public Type ValueType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// 
        /// </summary>
        public T this[Number index]
        {
            get
            {
                return this[(int)index];
            }
            set
            {
                this[(int)index] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public T this[int index]
        {
            get
            {
                this.CheckUndefined();

                if (0 <= index && index < _list.Count)
                {
                    return _list[(int)index];
                }
                return default(T);
            }
            set
            {
                this.CheckUndefined();

                int count = _list.Count;
                if (index >= count)
                {
                    this.length = index + 1;
                }
                _list[index] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Number length
        {
            get
            {
                this.CheckUndefined();

                return _list.Count;
            }
            set
            {
                this.CheckUndefined();

                int newCount = Convert.ToInt32((double)value);
                if (value < 0 || newCount != (double)value)
                {
                    throw new ArgumentException("length must be 0 or positive integer.");
                }

                int count = this._list.Count;
                if (newCount < count)
                {
                    this.InternalRemoveRange(newCount, count - newCount);
                }
                else if (newCount > count)
                {
                    for (int i = count; i < newCount; i++)
                    {
                        this.InternalAdd(default(T));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Number Length
        {
            get { return this.length; }
            set { this.length = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return (int)this.length;
            }
            set
            {
                this.length = value;
            }
        }
        #endregion

        #region Operator Implicit
        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Array<T>(Undefined value)
        {
            return new Array<T>(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Array<T>(List<T> list)
        {
            if (list == null)
            {
                return null;
            }

            Array<T> arr = new Array<T>();
            arr.__value__ = arr._list = list;
            return arr;
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator List<T>(Array<T> array)
        {
            return array == null ? null : array._list;
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator T[](Array<T> array)
        {
            return array == null ? null : array._list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Array<T>(T[] ts)
        {
            return ts == null ? null : new Array<T>(ts);
        }
        #endregion

        #region  Operator Implicit(list<string/bool/double/datetime)
        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Array<T>(List<string> list)
        {
            if (list == null)
            {
                return null;
            }
            if (typeof(T).Name != "String")
            {
                throw new InvalidOperationException("can not convert string to " + typeof(T).Name);
            }

            Array<T> ret = new Array<T>();
            foreach (string item in list)
            {
                ret.Add((T)Convert.ChangeType((String)item, typeof(T)));
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Array<T>(List<double> list)
        {
            if (list == null)
            {
                return null;
            }
            if (typeof(T).Name != "Number")
            {
                throw new InvalidOperationException("can not convert double to " + typeof(T).Name);
            }

            Array<T> ret = new Array<T>();
            foreach (double item in list)
            {
                ret.Add((T)Convert.ChangeType((Number)item, typeof(T)));
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Array<T>(List<bool> list)
        {
            if (list == null)
            {
                return null;
            }
            if (typeof(T).Name != "Boolean")
            {
                throw new InvalidOperationException("can not convert bool to " + typeof(T).Name);
            }

            Array<T> ret = new Array<T>();
            foreach (bool item in list)
            {
                ret.Add((T)Convert.ChangeType((Boolean)item, typeof(T)));
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Array<T>(List<DateTime> list)
        {
            if (list == null)
            {
                return null;
            }
            if (typeof(T).Name != "Date")
            {
                throw new InvalidOperationException("can not convert DateTime to " + typeof(T).Name);
            }

            Array<T> ret = new Array<T>();
            foreach (DateTime item in list)
            {
                ret.Add((T)Convert.ChangeType((Date)item, typeof(T)));
            }
            return ret;
        }
        #endregion

        #region Implements Interfaces
        /// <summary>
        /// 
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            this.CheckUndefined();

            return _list.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            this.CheckUndefined();

            return _list.GetEnumerator();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public void Add(T item)
        {
            this.CheckUndefined();

            this.InternalAdd(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRange(IEnumerable<T> items)
        {
            this.CheckUndefined();

            this.InternalAddRange(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        public override Array<U> AsArray<U>()
        {
            if (typeof(U) == typeof(T))
            {
                return (this as Array<U>);
            }

            Array<U> ret = new Array<U>();
            bool canAs = (this._list.Count > 0 ? this._list[0] is U : false);
            foreach (object item in this._list)
            {
                if (item == null || canAs)
                {
                    ret.push((U)item);
                }
                else
                {
                    string type = item.GetType().Name;
                    switch (type)
                    {
                        case "Double":
                        case "Int32":
                        case "Int64":
                            double dItem = (double)item;
                            ret.push((U)Convert.ChangeType((Number)dItem, typeof(U)));
                            break;

                        case "String":
                            string sItem = (string)item;
                            ret.push((U)Convert.ChangeType((String)sItem, typeof(U)));
                            break;

                        case "Boolean":
                            bool bItem = (bool)item;
                            ret.push((U)Convert.ChangeType((Boolean)bItem, typeof(U)));
                            break;

                        default:
                            throw new InvalidCastException(string.Format("Cannot convert from {0} to {1}", item.GetType().Name, typeof(U).Name)); ;
                    }
                }
            }
            ret._from = this;
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public void clear()
        {
            this.CheckUndefined();

            this.InternalClear();
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> concat()
        {
            return this.concat(new Array<T>());
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> concat(params T[] items)
        {
            return this.concat(items.ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> concat(params T[][] items)
        {
            Array<T> all = new Array<T>();
            foreach (var item in items)
            {
                all.AddRange(item);
            }
            return this.concat(all);
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> concat(Array<T> items)
        {
            this.CheckUndefined();

            Array<T> ret = new Array<T>(this);
            ret.AddRange(items);
            return ret;
        }

        //public abstract Array<T> copyWithin(Number index);
        //public abstract Array<T> copyWithin(Number index, Number from);
        //public abstract Array<T> copyWithin(Number index, Number from, Number to);
        //public abstract IEnumerable<TypeScript.KeyValuePair> entries();
        //public abstract Number[] keys();

        /// <summary>
        /// 
        /// </summary>
        public bool every(Func<T, bool> predicate)
        {
            this.CheckUndefined();

            return this._list.All(predicate);
        }

        public bool every(Func<T, Number, bool> predicate)
        {
            this.CheckUndefined();

            List<T> items = this._list;
            for (int i = 0, count = items.Count; i < count; i++)
            {
                if (!predicate(items[i], i))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> filter(Func<T, bool> predicate)
        {
            this.CheckUndefined();

            var items = this._list.Where(predicate);
            return new Array<T>(items);
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> filter(Func<T, int, bool> predicate)
        {
            this.CheckUndefined();

            var items = this._list.Where(predicate);
            return new Array<T>(items);
        }

        /// <summary>
        /// 
        /// </summary>
        public T find(Predicate<T> match)
        {
            this.CheckUndefined();

            return this._list.Find(match);
        }

        /// <summary>
        /// 
        /// </summary>
        public Number findIndex(Predicate<T> match)
        {
            this.CheckUndefined();

            return this._list.FindIndex(match);
        }

        /// <summary>
        /// 
        /// </summary>
        public void forEach(Action<T> action)
        {
            this.CheckUndefined();

            this._list.ForEach(action);
        }

        /// <summary>
        /// 
        /// </summary>
        public void forEach(Action<T, Number> action)
        {
            this.CheckUndefined();

            for (int i = 0; i < this._list.Count; i++)
            {
                action(this._list[i], i);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool includes(T item)
        {
            return this.includes(item, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool includes(T item, Number fromIndex)
        {
            this.CheckUndefined();

            return this._list.IndexOf(item, (int)fromIndex) >= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number indexOf(T item)
        {
            return this.indexOf(item, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public Number indexOf(T item, Number fromIndex)
        {
            this.CheckUndefined();

            return this._list.IndexOf(item, 0);

        }

        /// <summary>
        /// 
        /// </summary>
        public String join()
        {
            return this.join(",");
        }

        /// <summary>
        /// 
        /// </summary>
        public String join(String separator)
        {
            return string.Join(separator, this._list);
        }

        /// <summary>
        /// 
        /// </summary>
        public Number lastIndexOf(T item)
        {
            return this.lastIndexOf(item, this._list.Count - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        public Number lastIndexOf(T item, Number fromIndex)
        {
            this.CheckUndefined();

            return this._list.LastIndexOf(item, (int)fromIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<U> map<U>(Func<T, U> func)
        {
            this.CheckUndefined();

            Array<U> ret = new Array<U>();
            foreach (var item in this._list)
            {
                ret.Add(func(item));
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<U> map<U>(Func<T, Number, U> func)
        {
            this.CheckUndefined();

            Array<U> ret = new Array<U>();
            for (int i = 0; i < this._list.Count; i++)
            {
                ret.Add(func(this._list[i], i));
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public T pop()
        {
            this.CheckUndefined();

            int index = this._list.Count - 1;
            if (index < 0)
            {
                return default(T);
            }

            T ret = this._list[index];
            this.InternalRemoveAt(index);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number push(params T[] items)
        {
            this.CheckUndefined();

            this.InternalAddRange(items);

            return this._list.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number push(T item)
        {
            this.CheckUndefined();

            this.InternalAdd(item);

            return this._list.Count;
        }

        public T reduce(Func<T, T, T> func, T initialValue = default(T))
        {
            return this.reduce<T>(func, initialValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public T1 reduce<T1>(Func<T1, T, T1> func, T1 initialValue = default(T1))
        {
            this.CheckUndefined();

            List<T> items = this._list;
            if (initialValue == null && items.Count == 0)
            {
                throw new InvalidOperationException("Reduce of empty array with no initial value");
            }

            int index = 0;
            if (initialValue == null && items.Count > 0)
            {
                initialValue = (T1)Convert.ChangeType(items[0], typeof(T1));
                index++;
            }

            T1 result = initialValue;
            for (; index < items.Count; index++)
            {
                result = func(result, items[index]);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public T reduce(Func<T, T, Number, T> func, T initialValue = default(T))
        {
            this.CheckUndefined();

            List<T> items = this._list;
            if (initialValue == null && items.Count == 0)
            {
                throw new InvalidOperationException("Reduce of empty array with no initial value");
            }

            int index = 0;
            if (initialValue == null && items.Count > 0)
            {
                initialValue = items[0];
                index++;
            }

            T result = initialValue;
            for (; index < items.Count; index++)
            {
                result = func(result, items[index], index);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public U reduce<U>(Func<U, T, Number, U> func, U initialValue = default(U))
        {
            this.CheckUndefined();

            List<T> items = this._list;
            if (initialValue == null && items.Count == 0)
            {
                throw new InvalidOperationException("Reduce of empty array with no initial value");
            }

            U result = initialValue;
            for (int i = 0; i < items.Count; i++)
            {
                result = func(result, items[i], i);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public T reduceRight(Func<T, T, T> func, T initialValue = default(T))
        {
            this.CheckUndefined();

            List<T> items = this._list;
            if (initialValue == null && items.Count == 0)
            {
                throw new InvalidOperationException("Reduce of empty array with no initial value");
            }

            int index = items.Count - 1;
            if (initialValue == null && items.Count > 0)
            {
                initialValue = items[items.Count - 1];
                index--;
            }

            T result = initialValue;
            for (; index >= 0; index--)
            {
                result = func(result, items[index]);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public T reduceRight(Func<T, T, Number, T> func, T initialValue = default(T))
        {
            this.CheckUndefined();

            List<T> items = this._list;
            if (initialValue == null && items.Count == 0)
            {
                throw new InvalidOperationException("Reduce of empty array with no initial value");
            }

            int index = items.Count - 1;
            if (initialValue == null && items.Count > 0)
            {
                initialValue = items[items.Count - 1];
                index--;
            }

            T result = initialValue;
            for (; index >= 0; index--)
            {
                result = func(result, items[index], index);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public U reduceRight<U>(Func<U, T, Number, U> func, U initialValue = default(U))
        {
            this.CheckUndefined();

            List<T> items = this._list;
            if (initialValue == null && items.Count == 0)
            {
                throw new InvalidOperationException("Reduce of empty array with no initial value");
            }

            U result = initialValue;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                result = func(result, items[i], i);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> reverse()
        {
            this.CheckUndefined();

            this.InternalReverse();
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public T shift()
        {
            this.CheckUndefined();

            if (this._list.Count == 0)
            {
                return default(T);
            }

            T ret = this._list[0];
            this.InternalRemoveAt(0);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> slice()
        {
            return this.slice(0, this._list.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> slice(Number start)
        {
            return this.slice(start, this._list.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> slice(Number begin, Number end)
        {
            this.CheckUndefined();

            List<T> items = this._list;
            if (IsNull(begin) || IsUndefined(begin))
            {
                begin = 0;
            }
            if (begin < 0)
            {
                begin = System.Math.Max(0, items.Count + begin);
            }

            if (IsNull(end) || IsUndefined(end))
            {
                end = items.Count;
            }
            if (end < 0)
            {
                end = System.Math.Max(0, items.Count + end);
            }
            end = System.Math.Min(end, items.Count);

            Array<T> ret = new Array<T>();
            int beginIndex = (int)begin;
            int endIndex = (int)end;
            for (int i = beginIndex; i < endIndex; i++)
            {
                ret.Add(items[i]);
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool some(Func<T, bool> predicate)
        {
            this.CheckUndefined();

            return this._list.Any(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool some(Func<T, Number, bool> func)
        {
            this.CheckUndefined();

            List<T> items = this._list;
            for (int i = 0; i < items.Count; i++)
            {
                if (func(items[i], i))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> sort(IComparer<T> comparer = null)
        {
            this.CheckUndefined();

            if (comparer == null)
            {
                this.InternalSort();
            }
            else
            {
                this.InternalCompareSort(comparer);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> sort(Func<T, T, Number> fn)
        {
            ArrayComparer<T> compare = new ArrayComparer<T>(fn);
            return this.sort(compare);
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> sort(Comparison<T> comparison)
        {
            ArrayComparer<T> compare = new ArrayComparer<T>(comparison);
            return this.sort(compare);
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> splice(Number start, Number deleteCount = null, params T[] addItems)
        {
            this.CheckUndefined();

            List<T> items = this._list;
            if (IsNull(start) || IsUndefined(start))
            {
                start = items.Count;
            }
            if (start < 0)
            {
                start = System.Math.Max(0, items.Count + start);
            }
            if (IsNull(deleteCount) || IsUndefined(deleteCount))
            {
                deleteCount = items.Count;
            }
            deleteCount = System.Math.Max(0, deleteCount);

            //
            int index = (int)start;
            int count = (int)deleteCount;
            Array<T> ret = items.GetRange(index, count);
            items.RemoveRange(index, count);
            items.InsertRange(index, addItems);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number unshift(params T[] items)
        {
            this.InternalInsertRange(0, items);

            return this._list.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Array<T> valueOf()
        {
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override String toString()
        {
            List<string> stringItems = new List<string>();
            foreach (T item in this._list)
            {
                if (IsNull(item))
                {
                    stringItems.Add("");
                }
                else if (item is Object)
                {
                    if (IsUndefined(item as Object))
                    {
                        stringItems.Add("");
                    }
                    else
                    {
                        stringItems.Add((item as Object).toString());
                    }
                }
                else
                {
                    stringItems.Add(item.ToString());
                }
            }
            return string.Join(",", stringItems);
        }
        #region Private Methods
        private void InternalAdd(T item)
        {
            this._list.Add(item);

            if (this._from != null)
            {
                System.Reflection.MethodInfo addMethod = this._from.GetType().GetMethod("InternalAdd", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Type toType = this._from.GetType().GetProperty("ValueType").GetValue(this._from) as Type;
                Type fromType = this.ValueType;

                addMethod.Invoke(this._from, new object[] { this.TypeCast(fromType, toType, item) });
            }
        }
        private void InternalAddRange(IEnumerable<T> items)
        {
            this._list.AddRange(items);

            if (this._from != null)
            {
                System.Reflection.MethodInfo addMethod = this._from.GetType().GetMethod("InternalAdd", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Type toType = this._from.GetType().GetProperty("ValueType").GetValue(this._from) as Type;
                Type fromType = this.ValueType;

                foreach (var item in items)
                {
                    addMethod.Invoke(this._from, new object[] { this.TypeCast(fromType, toType, item) });
                }
            }
        }
        private void InternalInsert(int index, T item)
        {
            this._list.Insert(index, item);

            if (this._from != null)
            {
                System.Reflection.MethodInfo insertMethod = this._from.GetType().GetMethod("InternalInsert", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Type toType = this._from.GetType().GetProperty("ValueType").GetValue(this._from) as Type;
                Type fromType = this.ValueType;

                insertMethod.Invoke(this._from, new object[] { index, this.TypeCast(fromType, toType, item) });
            }
        }
        private void InternalInsertRange(int index, IEnumerable<T> items)
        {
            this._list.InsertRange(index, items);

            if (this._from != null)
            {
                System.Reflection.MethodInfo insertMethod = this._from.GetType().GetMethod("InternalInsert", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Type toType = this._from.GetType().GetProperty("ValueType").GetValue(this._from) as Type;
                Type fromType = this.ValueType;

                for (int i = items.Count() - 1; i >= 0; i--)
                {
                    insertMethod.Invoke(this._from, new object[] { index, this.TypeCast(fromType, toType, items.ElementAt(i)) });
                }
            }
        }
        private void InternalRemoveAt(int index)
        {
            this._list.RemoveAt(index);

            if (this._from != null)
            {
                System.Reflection.MethodInfo removeAtMethod = this._from.GetType().GetMethod("InternalRemoveAt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                removeAtMethod.Invoke(this._from, new object[] { index });
            }
        }
        private void InternalRemoveRange(int index, int count)
        {
            this._list.RemoveRange(index, count);

            if (this._from != null)
            {
                System.Reflection.MethodInfo removeAtMethod = this._from.GetType().GetMethod("InternalRemoveAt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                for (int i = 0; i < count; i++)
                {
                    removeAtMethod.Invoke(this._from, new object[] { index });
                }
            }
        }
        private void InternalClear()
        {
            this._list.Clear();

            if (this._from != null)
            {
                System.Reflection.MethodInfo clearMethod = this._from.GetType().GetMethod("InternalClear", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                clearMethod.Invoke(this._from, new object[] { });
            }
        }

        private void InternalSort()
        {
            this._list = this._list.OrderBy(item => item).ToList();

            if (this._from != null)
            {
                System.Reflection.MethodInfo sortMethod = this._from.GetType().GetMethod("InternalSort", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                sortMethod.Invoke(this._from, new object[] { });
            }
        }
        private void InternalCompareSort(IComparer<T> comparer)
        {
            this._list = this._list.OrderBy(item => item, comparer).ToList();

            if (this._from != null)
            {
                if (comparer is IComparer)
                {
                    System.Reflection.MethodInfo compareSortMethod = this._from.GetType().GetMethod("InternalCompareSort2", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    compareSortMethod.Invoke(this._from, new object[] { comparer });
                }
                else
                {
                    Type toType = this._from.GetType().GetProperty("ValueType").GetValue(this._from) as Type;
                    throw new InvalidCastException(string.Format("Cannot cast from {0} to System.Collections.Generic.IComparer<{1}>", comparer.GetType(), toType));
                }
            }
        }
        private void InternalCompareSort2(IComparer comparer)
        {
            IComparer<T> orderByComparer = null;
            if (comparer is IComparer<T>)
            {
                orderByComparer = (IComparer<T>)comparer;
            }
            else
            {
                orderByComparer = new ArrayComparer<T>(comparer);
            }
            this._list = this._list.OrderBy(item => item, orderByComparer).ToList();

            if (this._from != null)
            {
                System.Reflection.MethodInfo compareSortMethod = this._from.GetType().GetMethod("InternalCompareSort2", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                compareSortMethod.Invoke(this._from, new object[] { comparer });
            }
        }

        private void InternalReverse()
        {
            this._list.Reverse();

            if (this._from != null)
            {
                System.Reflection.MethodInfo reverseMethod = this._from.GetType().GetMethod("InternalReverse", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                reverseMethod.Invoke(this._from, new object[] { });
            }
        }

        private object TypeCast(Type fromType, Type toType, object item)
        {
            if (fromType.Name == "Number" && toType.Name == "Double")
            {
                return item == null ? 0 : (double)(Number)item;
            }
            else
            {
                return item;
            }
        }
        #endregion
        #endregion
    }



    class ArrayComparer<T> : IComparer<T>, IComparer
    {
        private readonly Func<T, T, Number> _fn;
        private readonly Comparison<T> _comparison;
        private readonly IComparer _comparer;

        public ArrayComparer(Func<T, T, Number> fn)
        {
            this._fn = fn ?? throw new ArgumentNullException("fn");
        }

        public ArrayComparer(Comparison<T> comparison)
        {
            this._comparison = comparison ?? throw new ArgumentNullException("comparison");
        }

        public ArrayComparer(IComparer comparer)
        {
            this._comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            double result = 0;
            if (this._fn != null)
            {
                result = _fn(x, y);
            }
            else if (this._comparison != null)
            {
                result = this._comparison(x, y);
            }
            else if (this._comparer != null)
            {
                result = this._comparer.Compare(x, y);
            }

            if (result > 0)
            {
                return 1;
            }
            else if (result < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public int Compare(object x, object y)
        {
            return this.Compare((T)x, (T)y);
        }

    }

    public class Array : Array<object>
    {
        public Array(object arg)
        {
            if (ObjectUtil.IsNumber(arg))
            {
                double value = ObjectUtil.ToDouble(arg);
                int length = Convert.ToInt32(value);
                if (length < 0 || length != value)
                {
                    throw new ArgumentException("length must be 0 or position integer.");
                }
                this.Length = length;
            }
            else
            {
                this.Add(arg);
            }
        }

        public static bool isArray(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj.GetType().Name == "Array`1";
        }
    }
}
