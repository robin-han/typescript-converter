using System;
using System.Collections;

/*
 * Copyright (c) 1999, 2020, Oracle and/or its affiliates. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

namespace com.sun.tools.javac.util
{
    // using Api = com.sun.tools.javac.util.DefinedBy.Api;

    //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //    import static com.sun.tools.javac.util.LayoutCharacters.*;

    /// <summary>
    /// A class that defines source code positions as simple character
    ///  offsets from the beginning of the file. The first character
    ///  is at position 0.
    /// 
    ///  Support is also provided for (line,column) coordinates, but tab
    ///  expansion is optional and no Unicode escape translation is considered.
    ///  The first character is at location (1,1).
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class Position
    {
        public const int NOPOS = -1;

        public const int FIRSTPOS = 0;
        public const int FIRSTLINE = 1;
        public const int FIRSTCOLUMN = 1;

        public const int LINESHIFT = 10;
        public static readonly int MAXCOLUMN = (1 << LINESHIFT) - 1;
        public static readonly int MAXLINE = (1 << ((sizeof(int) * 8) - LINESHIFT)) - 1;

        public static readonly int MAXPOS = int.MaxValue;

        /// <summary>
        /// This is class is not supposed to be instantiated.
        /// </summary>
        private Position()
        {
        }

        /// <summary>
        /// A two-way map between line/column numbers and positions,
        ///  derived from a scan done at creation time.  Tab expansion is
        ///  optionally supported via a character map.  Text content
        ///  is not retained.
        /// <para>
        ///  Notes:  The first character position FIRSTPOS is at
        ///  (FIRSTLINE,FIRSTCOLUMN).  No account is taken of Unicode escapes.
        /// 
        /// </para>
        /// </summary>
        /// <param name="src">         Source characters </param>
        /// <param name="max">         Number of characters to read </param>
        /// <param name="expandTabs">  If true, expand tabs when calculating columns </param>
        public static LineMap makeLineMap(char[] src, int max, bool expandTabs)
        {
            LineMapImpl lineMap = expandTabs ? new LineTabMapImpl(max) : new LineMapImpl();
            lineMap.build(src, max);
            return lineMap;
        }

        /// <summary>
        /// Encode line and column numbers in an integer as:
        ///  {@code line-number << LINESHIFT + column-number }.
        ///  <seealso cref="Position#NOPOS"/> represents an undefined position.
        /// </summary>
        /// <param name="line">  number of line (first is 1) </param>
        /// <param name="col">   number of character on line (first is 1) </param>
        /// <returns>       an encoded position or <seealso cref="Position#NOPOS"/>
        ///               if the line or column number is too big to
        ///               represent in the encoded format </returns>
        /// <exception cref="IllegalArgumentException"> if line or col is less than 1 </exception>
        public static int encodePosition(int line, int col)
        {
            if (line < 1)
            {
                throw new System.ArgumentException("line must be greater than 0");
            }
            if (col < 1)
            {
                throw new System.ArgumentException("column must be greater than 0");
            }

            if (line > MAXLINE || col > MAXCOLUMN)
            {
                return NOPOS;
            }
            return (line << LINESHIFT) + col;
        }

        static int TabInc = 8;
        public static int tabulate(int column)
        {
            return (column / TabInc * TabInc) + TabInc;
        }

        public interface LineMap : com.sun.source.tree.LineMap
        {
            /// <summary>
            /// Find the start position of a line.
            /// </summary>
            /// <param name="line"> number of line (first is 1) </param>
            /// <returns>     position of first character in line </returns>
            /// <exception cref="ArrayIndexOutOfBoundsException">
            ///           if {@code lineNumber < 1}
            ///           if {@code lineNumber > no. of lines} </exception>
            int getStartPosition(int line);

            /// <summary>
            /// Find the position corresponding to a (line,column).
            /// </summary>
            /// <param name="line">    number of line (first is 1) </param>
            /// <param name="column">  number of character on line (first is 1)
            /// </param>
            /// <returns>  position of character </returns>
            /// <exception cref="ArrayIndexOutOfBoundsException">
            ///           if {@code line < 1}
            ///           if {@code line > no. of lines} </exception>
            int getPosition(int line, int column);

            /// <summary>
            /// Find the line containing a position; a line termination
            /// character is on the line it terminates.
            /// </summary>
            /// <param name="pos">  character offset of the position </param>
            /// <returns> the line number on which pos occurs (first line is 1) </returns>
            int getLineNumber(int pos);

            /// <summary>
            /// Find the column for a character position.
            ///  Note:  this method does not handle tab expansion.
            ///  If tab expansion is needed, use a LineTabMap instead.
            /// </summary>
            /// <param name="pos">   character offset of the position </param>
            /// <returns>       the column number at which pos occurs </returns>
            int getColumnNumber(int pos);
        }

        public class LineMapImpl : LineMap
        {
            protected internal int[] startPosition; // start position of each line

            protected internal LineMapImpl()
            {
            }

            protected internal virtual void build(char[] src, int max)
            {
                int c = 0;
                int i = 0;
                int[] linebuf = new int[max];
                while (i < max)
                {
                    linebuf[c++] = i;
                    do
                    {
                        char ch = src[i];
                        if (ch == '\r' || ch == '\n')
                        {
                            if (ch == '\r' && (i + 1) < max && src[i + 1] == '\n')
                            {
                                i += 2;
                            }
                            else
                            {
                                ++i;
                            }
                            break;
                        }
                        else if (ch == '\t')
                        {
                            setTabPosition(i);
                        }
                    } while (++i < max);
                }
                this.startPosition = new int[c];
                Array.Copy(linebuf, 0, startPosition, 0, c);
            }

            public virtual int getStartPosition(int line)
            {
                return startPosition[line - FIRSTLINE];
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public long getStartPosition(long line)
            public virtual long getStartPosition(long line)
            {
                return getStartPosition(longToInt(line));
            }

            public virtual int getPosition(int line, int column)
            {
                return startPosition[line - FIRSTLINE] + column - FIRSTCOLUMN;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public long getPosition(long line, long column)
            public virtual long getPosition(long line, long column)
            {
                return getPosition(longToInt(line), longToInt(column));
            }

            // Cache of last line number lookup
            internal int lastPosition = Position.FIRSTPOS;
            internal int lastLine = Position.FIRSTLINE;

            public virtual int getLineNumber(int pos)
            {
                if (pos == lastPosition)
                {
                    return lastLine;
                }
                lastPosition = pos;

                int low = 0;
                int high = startPosition.Length - 1;
                while (low <= high)
                {
                    int mid = (low + high) >> 1;
                    int midVal = startPosition[mid];

                    if (midVal < pos)
                    {
                        low = mid + 1;
                    }
                    else if (midVal > pos)
                    {
                        high = mid - 1;
                    }
                    else
                    {
                        lastLine = mid + 1; // pos is at beginning of this line
                        return lastLine;
                    }
                }
                lastLine = low;
                return lastLine; // pos is on this line
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public long getLineNumber(long pos)
            public virtual long getLineNumber(long pos)
            {
                return getLineNumber(longToInt(pos));
            }

            public virtual int getColumnNumber(int pos)
            {
                return pos - startPosition[getLineNumber(pos) - FIRSTLINE] + FIRSTCOLUMN;
            }

            //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
            //ORIGINAL LINE: @DefinedBy(com.sun.tools.javac.util.DefinedBy.Api.COMPILER_TREE) public long getColumnNumber(long pos)
            public virtual long getColumnNumber(long pos)
            {
                return getColumnNumber(longToInt(pos));
            }

            internal static int longToInt(long longValue)
            {
                int intValue = (int)longValue;
                if (intValue != longValue)
                {
                    throw new System.IndexOutOfRangeException();
                }
                return intValue;
            }

            protected internal virtual void setTabPosition(int offset)
            {
            }
        }

        /// <summary>
        /// A LineMap that handles tab expansion correctly.  The cost is
        /// an additional bit per character in the source array.
        /// </summary>
        public class LineTabMapImpl : LineMapImpl
        {
            internal BitArray tabMap; // bits set for tab positions.

            public LineTabMapImpl(int max) : base()
            {
                tabMap = new BitArray(max);
            }

            protected internal override void setTabPosition(int offset)
            {
                tabMap.Set(offset, true);
            }

            public override int getColumnNumber(int pos)
            {
                int lineStart = startPosition[getLineNumber(pos) - FIRSTLINE];
                int column = 0;
                for (int bp = lineStart; bp < pos; bp++)
                {
                    if (tabMap.Get(bp))
                    {
                        column = tabulate(column);
                    }
                    else
                    {
                        column++;
                    }
                }
                return column + FIRSTCOLUMN;
            }

            public override int getPosition(int line, int column)
            {
                int pos = startPosition[line - FIRSTLINE];
                column -= FIRSTCOLUMN;
                int col = 0;
                while (col < column)
                {
                    pos++;
                    if (tabMap.Get(pos))
                    {
                        col = tabulate(col);
                    }
                    else
                    {
                        col++;
                    }
                }
                return pos;
            }
        }
    }

}