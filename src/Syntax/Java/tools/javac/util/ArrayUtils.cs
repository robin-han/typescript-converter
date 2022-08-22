using System;

/*
 * Copyright (c) 1999, 2013, Oracle and/or its affiliates. All rights reserved.
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
    /// <summary>
    /// <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class ArrayUtils
    {
        private static int calculateNewLength(int currentLength, int maxIndex)
        {
            while (currentLength < maxIndex + 1)
            {
                currentLength = currentLength * 2;
            }
            return currentLength;
        }

        public static T[] ensureCapacity<T>(T[] array, int maxIndex)
        {
            if (maxIndex < array.Length)
            {
                return array;
            }
            else
            {
                int newLength = calculateNewLength(array.Length, maxIndex);
                //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
                //ORIGINAL LINE: @SuppressWarnings("unchecked") T[] result = (T[]) Array.newInstance(array.getClass().getComponentType(), newLength);
                T[] result = (T[])Array.CreateInstance(array.GetType().GetElementType(), newLength);
                Array.Copy(array, 0, result, 0, array.Length);
                return result;
            }
        }

        public static sbyte[] ensureCapacity(sbyte[] array, int maxIndex)
        {
            if (maxIndex < array.Length)
            {
                return array;
            }
            else
            {
                int newLength = calculateNewLength(array.Length, maxIndex);
                sbyte[] result = new sbyte[newLength];
                Array.Copy(array, 0, result, 0, array.Length);
                return result;
            }
        }

        public static char[] ensureCapacity(char[] array, int maxIndex)
        {
            if (maxIndex < array.Length)
            {
                return array;
            }
            else
            {
                int newLength = calculateNewLength(array.Length, maxIndex);
                char[] result = new char[newLength];
                Array.Copy(array, 0, result, 0, array.Length);
                return result;
            }
        }

        public static int[] ensureCapacity(int[] array, int maxIndex)
        {
            if (maxIndex < array.Length)
            {
                return array;
            }
            else
            {
                int newLength = calculateNewLength(array.Length, maxIndex);
                int[] result = new int[newLength];
                Array.Copy(array, 0, result, 0, array.Length);
                return result;
            }
        }

    }

}