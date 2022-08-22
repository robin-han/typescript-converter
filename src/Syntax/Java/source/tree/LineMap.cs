/*
 * Copyright (c) 2006, 2014, Oracle and/or its affiliates. All rights reserved.
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

namespace com.sun.source.tree
{
    /// <summary>
    /// Provides methods to convert between character positions and line numbers
    /// for a compilation unit.
    /// 
    /// @since 1.6
    /// </summary>
    public interface LineMap
    {
        /// <summary>
        /// Finds the start position of a line.
        /// </summary>
        /// <param name="line"> line number (beginning at 1) </param>
        /// <returns>     position of first character in line </returns>
        /// <exception cref="IndexOutOfBoundsException">
        ///           if {@code lineNumber < 1}
        ///           if {@code lineNumber > no. of lines} </exception>
        long getStartPosition(long line);

        /// <summary>
        /// Finds the position corresponding to a (line,column).
        /// </summary>
        /// <param name="line">    line number (beginning at 1) </param>
        /// <param name="column">  tab-expanded column number (beginning 1)
        /// </param>
        /// <returns>  position of character </returns>
        /// <exception cref="IndexOutOfBoundsException">
        ///           if {@code line < 1}
        ///           if {@code line > no. of lines} </exception>
        long getPosition(long line, long column);

        /// <summary>
        /// Finds the line containing a position; a line termination
        /// character is on the line it terminates.
        /// </summary>
        /// <param name="pos">  character offset of the position </param>
        /// <returns> the line number of pos (first line is 1) </returns>
        long getLineNumber(long pos);

        /// <summary>
        /// Finds the column for a character position.
        /// Tab characters preceding the position on the same line
        /// will be expanded when calculating the column number.
        /// </summary>
        /// <param name="pos">   character offset of the position </param>
        /// <returns>       the tab-expanded column number of pos (first column is 1) </returns>
        long getColumnNumber(long pos);

    }

}