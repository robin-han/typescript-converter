using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GrapeCity.DataVisualization.TypeScript
{
    public class Array<T> : Object, IEnumerable<T>
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private List<T> _list;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public Array()
        {
            this._value = _list = new List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        public Array(IEnumerable<T> objs) : this()
        {
            if (objs == null)
            {
                throw new ArgumentNullException();
            }

            _list.AddRange(objs);
        }

        /// <summary>
        /// 
        /// </summary>
        private Array(Undefined value) : this()
        {
            this._value = value;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public T this[Number index]
        {
            get
            {
                this.CheckUndefined();

                return _list[(int)index];
            }
            set
            {
                this.CheckUndefined();

                _list[(int)index] = value;
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
        public static implicit operator Array<T>(List<T> ts)
        {
            return new Array<T>(ts);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator List<T>(Array<T> array)
        {
            return array._list;
        }

        /// 
        /// </summary>
        public static implicit operator T[] (Array<T> array)
        {
            return array._list.ToArray();
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

            _list.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRange(IEnumerable<T> items)
        {
            this.CheckUndefined();

            _list.AddRange(items);
        }

        /// <summary>
        /// 
        /// </summary>
        public void clear()
        {
            this.CheckUndefined();

            this._list.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> concat(params T[] items)
        {
            this.CheckUndefined();

            return this.concat(items.ToList());
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
            this._list.RemoveAt(index);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number push(params T[] items)
        {
            this.CheckUndefined();

            this._list.AddRange(items);
            return this._list.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        public T reduce(Func<T, T, T> func, T initialValue = default(T))
        {
            this.CheckUndefined();

            List<T> items = this._list;
            if (initialValue == null && items.Count == 0)
            {
                throw new InvalidOperationException("Reduce of empty array with no initial value");
            }
            if (initialValue == null && items.Count > 0)
            {
                initialValue = items[0];
            }

            T result = initialValue;
            for (int i = 0; i < items.Count; i++)
            {
                result = func(result, items[i]);
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
            if (initialValue == null && items.Count > 0)
            {
                initialValue = items[0];
            }

            T result = initialValue;
            for (int i = 0; i < items.Count; i++)
            {
                result = func(result, items[i], i);
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
            if (initialValue == null && items.Count > 0)
            {
                initialValue = items[items.Count - 1];
            }

            T result = initialValue;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                result = func(result, items[i]);
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
            if (initialValue == null && items.Count > 0)
            {
                initialValue = items[items.Count - 1];
            }

            T result = initialValue;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                result = func(result, items[i], i);
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

            this._list.Reverse();
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
            this._list.RemoveAt(0);
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
                end = System.Math.Max(0, items.Count + begin);
            }

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
                this._list.Sort();
            }
            else
            {
                this._list.Sort(comparer);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public Array<T> sort(Func<T, T, Number> fn)
        {
            ArrayCompare<T> compare = new ArrayCompare<T>(fn);
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
            items.AddRange(addItems);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number unshift(params T[] items)
        {
            this._list.InsertRange(0, items);
            return this._list.Count;
        }
        #endregion
    }

    class ArrayCompare<T> : IComparer<T>
    {
        private Func<T, T, Number> _fn;

        public ArrayCompare(Func<T, T, Number> fn)
        {
            this._fn = fn;
        }

        public int Compare(T x, T y)
        {
            return (int)_fn(x, y);
        }
    }

    public class Array
    {
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
