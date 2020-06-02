using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.CSharp
{
    public class Date : Object
    {
        #region Fields
        //Base on UTC
        private static readonly DateTime BASE_UTC_DATETIME = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private DateTime _dt;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> class.
        /// </summary>
        public Date() : this(DateTime.Now)
        {
        }

        public Date(String dateString)
        {
            if (!DateTime.TryParse(dateString, out DateTime dateTime))
            {
                throw new ArgumentException("Invalide Date");
            }
            this.__value__ = this._dt = dateTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> class.
        /// </summary>
        public Date(DateTime dateTime)
        {
            this.__value__ = this._dt = dateTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> class.
        /// </summary>
        /// <param name="d">The time number values.</param>
        public Date(Number n)
        {
            this.__value__ = this._dt = BASE_UTC_DATETIME.AddMilliseconds(n).ToLocalTime();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        public Date(Number year, Number month, Number day) : this(year, month, day, 0, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="day">The day.</param>
        public Date(Number year, Number month, Number day, Number hour) : this(year, month, day, hour, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="day">The day.</param>
        /// <param name="min">The min.</param>
        public Date(Number year, Number month, Number day, Number hour, Number min) : this(year, month, day, hour, min, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month (0 ~ 11).</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="min">The min.</param>
        /// <param name="second">The secend.</param>
        public Date(Number year, Number month, Number day, Number hour, Number min, Number second)
        {
            if (year < 1)
            {
                throw new ArgumentException("year value muse be greater than 0");
            }

            int y = (int)year;
            int m = (int)month;
            int d = (int)day;
            int h = (int)hour;
            int mm = (int)min;
            int s = (int)second;

            m = m < 0 ? 0 : m % 12;
            d = d < 1 ? 1 : System.Math.Min(DateTime.DaysInMonth(y, m + 1), d);
            h = h < 0 ? 0 : h % 24;
            mm = mm < 0 ? 0 : mm % 60;
            s = s < 0 ? 0 : s % 60;

            int m_add = (int)(month - m);
            int d_add = (int)(day - d);
            int h_add = (int)(hour - h);
            int mm_add = (int)(min - mm);
            int s_add = (int)(second - s);

            DateTime date = new DateTime(y, m + 1, d, h, mm, s);
            date = date.AddMonths(m_add);
            date = date.AddDays(d_add);
            date = date.AddHours(h_add);
            date = date.AddMinutes(mm_add);
            date = date.AddSeconds(s_add);

            this.__value__ = this._dt = date;
        }

        /// <summary>
        /// 
        /// </summary>
        private Date(Undefined undefined)
        {
            this.__value__ = undefined;
            this._dt = DateTime.Now;
        }
        #endregion

        #region Operator Implicit
        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Date(Undefined undefined)
        {
            return new Date(undefined);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Date(DateTime dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }

            return new Date(dateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator DateTime(Date date)
        {
            return date._dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator <(Date date1, Date date2)
        {
            return date1._dt < date2._dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator >(Date date1, Date date2)
        {
            return date1._dt > date2._dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator <=(Date date1, Date date2)
        {
            return date1._dt <= date2._dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator >=(Date date1, Date date2)
        {
            return date1._dt >= date2._dt;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public static Number now()
        {
            return (DateTime.Now.ToUniversalTime() - BASE_UTC_DATETIME).TotalMilliseconds;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number parse(String dateString)
        {
            DateTime date = DateTime.Parse(dateString);
            return (date.ToUniversalTime() - BASE_UTC_DATETIME).TotalMilliseconds;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Number UTC(Number year, Number month, Number day)
        {
            return UTC(year, month, day, 0, 0, 0);
        }
        public static Number UTC(Number year, Number month, Number day, Number hour, Number minute, Number second)
        {
            if (month > 11 || month < 0)
            {
                throw new ArgumentException("month between 0-11");
            }

            DateTime date = new DateTime((int)year, (int)month + 1, (int)day, (int)hour, (int)minute, (int)second, DateTimeKind.Utc);
            return (date - BASE_UTC_DATETIME).TotalMilliseconds;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getDate()
        {
            return this._dt.Day;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getDay()
        {
            return (int)this._dt.DayOfWeek;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getFullYear()
        {
            return this._dt.Year;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getHours()
        {
            return this._dt.Hour;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getMilliseconds()
        {
            return this._dt.Millisecond;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getMinutes()
        {
            return this._dt.Minute;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getMonth()
        {
            return this._dt.Month - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getSeconds()
        {
            return this._dt.Second;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getTime()
        {
            return (this._dt.ToUniversalTime() - BASE_UTC_DATETIME).TotalMilliseconds;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getTimezoneOffset()
        {
            return TimeZoneInfo.Local.GetUtcOffset(this._dt).TotalMinutes;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getUTCDate()
        {
            return this._dt.ToUniversalTime().Day;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getUTCDay()
        {
            return (int)this._dt.ToUniversalTime().DayOfWeek;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getUTCFullYear()
        {
            return this._dt.ToUniversalTime().Year;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getUTCHours()
        {
            return this._dt.ToUniversalTime().Hour;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getUTCMilliseconds()
        {
            return this._dt.ToUniversalTime().Millisecond;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getUTCMinutes()
        {
            return this._dt.ToUniversalTime().Minute;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getUTCMonth()
        {
            return this._dt.ToUniversalTime().Month - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public Number getUTCSeconds()
        {
            return this._dt.ToUniversalTime().Second;
        }

        /// <summary>
        /// 
        /// </summary>
        public void setDate(Number date)
        {
            this.__value__ = this._dt = this._dt.AddDays(date - this.getDate());
        }

        /// <summary>
        /// 
        /// </summary>
        public void setFullYear(Number year)
        {
            this.__value__ = this._dt = this._dt.AddYears((int)(year - this.getFullYear()));
        }

        /// <summary>
        /// 
        /// </summary>
        public void setHours(Number hour)
        {
            this.__value__ = this._dt = this._dt.AddHours(hour - this.getHours());
        }

        /// <summary>
        /// 
        /// </summary>
        public void setMilliseconds(Number ms)
        {
            this.__value__ = this._dt = this._dt.AddMilliseconds(ms - this.getMilliseconds());
        }

        /// <summary>
        /// 
        /// </summary>
        public void setMinutes(Number minute)
        {
            this.__value__ = this._dt = this._dt.AddMinutes(minute - this.getMinutes());
        }

        /// <summary>
        /// 
        /// </summary>
        public void setMonth(Number month)
        {
            this.__value__ = this._dt = this._dt.AddMonths((int)(month - this.getMonth()));
        }

        /// <summary>
        /// 
        /// </summary>
        public void setSeconds(Number second)
        {
            this.__value__ = this._dt = this._dt.AddSeconds(second - this.getSeconds());
        }

        /// <summary>
        /// 
        /// </summary>
        public void setTime(Number tick)
        {
            this.__value__ = this._dt = BASE_UTC_DATETIME.AddMilliseconds(tick).ToLocalTime();
        }

        /// <summary>
        /// 
        /// </summary>
        public void setUTCDate(Number date)
        {
            this.__value__ = this._dt = this._dt.ToUniversalTime().AddDays(date - this.getUTCDate()).ToLocalTime();
        }

        /// <summary>
        /// 
        /// </summary>
        public void setUTCFullYear(Number year)
        {
            this.__value__ = this._dt = this._dt.ToUniversalTime().AddYears((int)(year - this.getUTCFullYear())).ToLocalTime();
        }

        /// <summary>
        /// 
        /// </summary>
        public void setUTCHours(Number hour)
        {
            this.__value__ = this._dt = this._dt.ToUniversalTime().AddHours(hour - this.getUTCHours()).ToLocalTime();
        }

        /// <summary>
        /// 
        /// </summary>
        public void setUTCMilliseconds(Number ms)
        {
            this.__value__ = this._dt = this._dt.ToUniversalTime().AddMilliseconds(ms - this.getUTCMilliseconds()).ToLocalTime();
        }

        /// <summary>
        /// 
        /// </summary>
        public void setUTCMinutes(Number minute)
        {
            this.__value__ = this._dt = this._dt.ToUniversalTime().AddMinutes(minute - this.getUTCMinutes()).ToLocalTime();
        }

        /// <summary>
        /// 
        /// </summary>
        public void setUTCMonth(Number month)
        {
            this.__value__ = this._dt = this._dt.ToUniversalTime().AddMonths((int)(month - this.getUTCMonth())).ToLocalTime();
        }

        /// <summary>
        /// 
        /// </summary>
        public void setUTCSeconds(Number second)
        {
            this.__value__ = this._dt = this._dt.ToUniversalTime().AddSeconds(second - this.getUTCSeconds()).ToLocalTime();
        }

        /// <summary>
        /// 
        /// </summary>
        public String toDateString()
        {
            return this._dt.ToLongDateString();
        }

        /// <summary>
        /// 
        /// </summary>
        public String toLocaleDateString()
        {
            string sysFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            return this._dt.ToString(sysFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        public String toLocaleTimeString()
        {
            string sysFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;
            return this._dt.ToString(sysFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        public String toLocaleString()
        {
            string sysFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + ", " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;
            return this._dt.ToString(sysFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        public override String toString()
        {
            var date = this._dt.ToLongDateString();
            var time = this._dt.ToLongTimeString();
            return date + " " + time;
        }

        /// <summary>
        /// 
        /// </summary>
        public String toUTCString()
        {
            return this._dt.ToUniversalTime().ToString("r");
        }

        /// <summary>
        /// 
        /// </summary>
        public Number valueOf()
        {
            return this.getTime();
        }
        #endregion

    }
}
