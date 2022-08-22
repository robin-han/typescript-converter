using System;

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
    /// <summary>
    /// Access to the compiler's name table.  Standard names are defined,
    /// as well as methods to create new names.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public class Names
    {

        public static readonly Context.Key<Names> namesKey = new Context.Key<Names>();

        public static Names instance(Context context)
        {
            Names instance = context.get(namesKey);
            if (instance == null)
            {
                instance = new Names(context);
                context.put(namesKey, instance);
            }
            return instance;
        }

        // operators and punctuation
        public readonly Name asterisk;
        public readonly Name comma;
        public readonly Name empty;
        public readonly Name hyphen;
        public readonly Name one;
        public readonly Name slash;

        // keywords
        public readonly Name _class;
        public readonly Name _super;
        public readonly Name _this;
        public readonly Name @var;
        public readonly Name exports;
        public readonly Name opens;
        public readonly Name module;
        public readonly Name provides;
        public readonly Name requires;
        public readonly Name to;
        public readonly Name transitive;
        public readonly Name uses;
        public readonly Name open;
        public readonly Name with;
        public readonly Name yield;

        // field and method names
        public readonly Name _name;
        public readonly Name addSuppressed;
        public readonly Name any;
        public readonly Name append;
        public readonly Name clinit;
        public readonly Name clone;
        public readonly Name close;
        public readonly Name deserializeLambda;
        public readonly Name desiredAssertionStatus;
        public readonly Name equals;
        public readonly Name error;
        public readonly Name finalize;
        public readonly Name forRemoval;
        public readonly Name essentialAPI;
        public readonly Name getClass;
        public readonly Name hasNext;
        public readonly Name hashCode;
        public readonly Name init;
        public readonly Name iterator;
        public readonly Name length;
        public readonly Name next;
        public readonly Name ordinal;
        public readonly Name provider;
        public readonly Name serialVersionUID;
        public readonly Name toString;
        public readonly Name value;
        public readonly Name valueOf;
        public readonly Name values;
        public readonly Name readResolve;
        public readonly Name readObject;

        // class names
        public readonly Name java_io_Serializable;
        public readonly Name java_lang_Class;
        public readonly Name java_lang_Cloneable;
        public readonly Name java_lang_Enum;
        public readonly Name java_lang_Object;

        // names of builtin classes
        public readonly Name Array;
        public readonly Name Bound;
        public readonly Name Method;

        // package names
        public readonly Name java_lang;

        // module names
        public readonly Name java_base;

        // attribute names
        public readonly Name Annotation;
        public readonly Name AnnotationDefault;
        public readonly Name BootstrapMethods;
        public readonly Name Bridge;
        public readonly Name CharacterRangeTable;
        public readonly Name Code;
        public readonly Name CompilationID;
        public readonly Name ConstantValue;
        public readonly Name Deprecated;
        public readonly Name EnclosingMethod;
        public readonly Name Enum;
        public readonly Name Exceptions;
        public readonly Name InnerClasses;
        public readonly Name LineNumberTable;
        public readonly Name LocalVariableTable;
        public readonly Name LocalVariableTypeTable;
        public readonly Name MethodParameters;
        public readonly Name Module;
        public readonly Name ModuleResolution;
        public readonly Name NestHost;
        public readonly Name NestMembers;
        public readonly Name Record;
        public readonly Name RuntimeInvisibleAnnotations;
        public readonly Name RuntimeInvisibleParameterAnnotations;
        public readonly Name RuntimeInvisibleTypeAnnotations;
        public readonly Name RuntimeVisibleAnnotations;
        public readonly Name RuntimeVisibleParameterAnnotations;
        public readonly Name RuntimeVisibleTypeAnnotations;
        public readonly Name Signature;
        public readonly Name SourceFile;
        public readonly Name SourceID;
        public readonly Name StackMap;
        public readonly Name StackMapTable;
        public readonly Name Synthetic;
        public readonly Name Value;
        public readonly Name Varargs;
        public readonly Name PermittedSubclasses;

        // members of java.lang.annotation.ElementType
        public readonly Name ANNOTATION_TYPE;
        public readonly Name CONSTRUCTOR;
        public readonly Name FIELD;
        public readonly Name LOCAL_VARIABLE;
        public readonly Name METHOD;
        public readonly Name MODULE;
        public readonly Name PACKAGE;
        public readonly Name PARAMETER;
        public readonly Name TYPE;
        public readonly Name TYPE_PARAMETER;
        public readonly Name TYPE_USE;
        public readonly Name RECORD_COMPONENT;

        // members of java.lang.annotation.RetentionPolicy
        public readonly Name CLASS;
        public readonly Name RUNTIME;
        public readonly Name SOURCE;

        // other identifiers
        public readonly Name T;
        public readonly Name ex;
        public readonly Name module_info;
        public readonly Name package_info;
        public readonly Name requireNonNull;

        // lambda-related
        public readonly Name lambda;
        public readonly Name metafactory;
        public readonly Name altMetafactory;
        public readonly Name dollarThis;

        // string concat
        public readonly Name makeConcat;
        public readonly Name makeConcatWithConstants;

        // record related
        // members of java.lang.runtime.ObjectMethods
        public readonly Name bootstrap;

        public readonly Name record;
        public readonly Name non;

        // serialization members, used by records too
        public readonly Name serialPersistentFields;
        public readonly Name writeObject;
        public readonly Name writeReplace;
        public readonly Name readObjectNoData;

        // sealed types
        public readonly Name permits;
        public readonly Name @sealed;

        public readonly Name.Table table;

        public Names(Context context)
        {
            Options options = Options.instance(context);
            table = createTable(options);

            // operators and punctuation
            asterisk = fromString("*");
            comma = fromString(",");
            empty = fromString("");
            hyphen = fromString("-");
            one = fromString("1");
            slash = fromString("/");

            // keywords
            _class = fromString("class");
            _super = fromString("super");
            _this = fromString("this");
            @var = fromString("var");
            exports = fromString("exports");
            opens = fromString("opens");
            module = fromString("module");
            provides = fromString("provides");
            requires = fromString("requires");
            to = fromString("to");
            transitive = fromString("transitive");
            uses = fromString("uses");
            open = fromString("open");
            with = fromString("with");
            yield = fromString("yield");

            // field and method names
            _name = fromString("name");
            addSuppressed = fromString("addSuppressed");
            any = fromString("<any>");
            append = fromString("append");
            clinit = fromString("<clinit>");
            clone = fromString("clone");
            close = fromString("close");
            deserializeLambda = fromString("$deserializeLambda$");
            desiredAssertionStatus = fromString("desiredAssertionStatus");
            equals = fromString("equals");
            error = fromString("<error>");
            finalize = fromString("finalize");
            forRemoval = fromString("forRemoval");
            essentialAPI = fromString("essentialAPI");
            getClass = fromString("getClass");
            hasNext = fromString("hasNext");
            hashCode = fromString("hashCode");
            init = fromString("<init>");
            iterator = fromString("iterator");
            length = fromString("length");
            next = fromString("next");
            ordinal = fromString("ordinal");
            provider = fromString("provider");
            serialVersionUID = fromString("serialVersionUID");
            toString = fromString("toString");
            value = fromString("value");
            valueOf = fromString("valueOf");
            values = fromString("values");
            readResolve = fromString("readResolve");
            readObject = fromString("readObject");
            dollarThis = fromString("$this");

            // class names
            java_io_Serializable = fromString("java.io.Serializable");
            java_lang_Class = fromString("java.lang.Class");
            java_lang_Cloneable = fromString("java.lang.Cloneable");
            java_lang_Enum = fromString("java.lang.Enum");
            java_lang_Object = fromString("java.lang.Object");

            // names of builtin classes
            Array = fromString("Array");
            Bound = fromString("Bound");
            Method = fromString("Method");

            // package names
            java_lang = fromString("java.lang");

            // module names
            java_base = fromString("java.base");

            // attribute names
            Annotation = fromString("Annotation");
            AnnotationDefault = fromString("AnnotationDefault");
            BootstrapMethods = fromString("BootstrapMethods");
            Bridge = fromString("Bridge");
            CharacterRangeTable = fromString("CharacterRangeTable");
            Code = fromString("Code");
            CompilationID = fromString("CompilationID");
            ConstantValue = fromString("ConstantValue");
            Deprecated = fromString("Deprecated");
            EnclosingMethod = fromString("EnclosingMethod");
            Enum = fromString("Enum");
            Exceptions = fromString("Exceptions");
            InnerClasses = fromString("InnerClasses");
            LineNumberTable = fromString("LineNumberTable");
            LocalVariableTable = fromString("LocalVariableTable");
            LocalVariableTypeTable = fromString("LocalVariableTypeTable");
            MethodParameters = fromString("MethodParameters");
            Module = fromString("Module");
            ModuleResolution = fromString("ModuleResolution");
            NestHost = fromString("NestHost");
            NestMembers = fromString("NestMembers");
            Record = fromString("Record");
            RuntimeInvisibleAnnotations = fromString("RuntimeInvisibleAnnotations");
            RuntimeInvisibleParameterAnnotations = fromString("RuntimeInvisibleParameterAnnotations");
            RuntimeInvisibleTypeAnnotations = fromString("RuntimeInvisibleTypeAnnotations");
            RuntimeVisibleAnnotations = fromString("RuntimeVisibleAnnotations");
            RuntimeVisibleParameterAnnotations = fromString("RuntimeVisibleParameterAnnotations");
            RuntimeVisibleTypeAnnotations = fromString("RuntimeVisibleTypeAnnotations");
            Signature = fromString("Signature");
            SourceFile = fromString("SourceFile");
            SourceID = fromString("SourceID");
            StackMap = fromString("StackMap");
            StackMapTable = fromString("StackMapTable");
            Synthetic = fromString("Synthetic");
            Value = fromString("Value");
            Varargs = fromString("Varargs");
            PermittedSubclasses = fromString("PermittedSubclasses");

            // members of java.lang.annotation.ElementType
            ANNOTATION_TYPE = fromString("ANNOTATION_TYPE");
            CONSTRUCTOR = fromString("CONSTRUCTOR");
            FIELD = fromString("FIELD");
            LOCAL_VARIABLE = fromString("LOCAL_VARIABLE");
            METHOD = fromString("METHOD");
            MODULE = fromString("MODULE");
            PACKAGE = fromString("PACKAGE");
            PARAMETER = fromString("PARAMETER");
            TYPE = fromString("TYPE");
            TYPE_PARAMETER = fromString("TYPE_PARAMETER");
            TYPE_USE = fromString("TYPE_USE");
            RECORD_COMPONENT = fromString("RECORD_COMPONENT");

            // members of java.lang.annotation.RetentionPolicy
            CLASS = fromString("CLASS");
            RUNTIME = fromString("RUNTIME");
            SOURCE = fromString("SOURCE");

            // other identifiers
            T = fromString("T");
            ex = fromString("ex");
            module_info = fromString("module-info");
            package_info = fromString("package-info");
            requireNonNull = fromString("requireNonNull");

            //lambda-related
            lambda = fromString("lambda$");
            metafactory = fromString("metafactory");
            altMetafactory = fromString("altMetafactory");

            // string concat
            makeConcat = fromString("makeConcat");
            makeConcatWithConstants = fromString("makeConcatWithConstants");

            bootstrap = fromString("bootstrap");
            record = fromString("record");
            non = fromString("non");

            serialPersistentFields = fromString("serialPersistentFields");
            writeObject = fromString("writeObject");
            writeReplace = fromString("writeReplace");
            readObjectNoData = fromString("readObjectNoData");

            // sealed types
            permits = fromString("permits");
            @sealed = fromString("sealed");
        }

        protected internal virtual Name.Table createTable(Options options)
        {
            bool useUnsharedTable = options.isSet("useUnsharedTable");
            if (useUnsharedTable)
            {
                return UnsharedNameTable.create(this);
            }
            else
            {
                return SharedNameTable.create(this);
            }
        }

        public virtual void dispose()
        {
            table.dispose();
        }

        public virtual Name fromChars(char[] cs, int start, int len)
        {
            return table.fromChars(cs, start, len);
        }

        public virtual Name fromString(string s)
        {
            return table.fromString(s);
        }

        public virtual Name fromUtf(sbyte[] cs)
        {
            return table.fromUtf(cs);
        }

        public virtual Name fromUtf(sbyte[] cs, int start, int len)
        {
            return table.fromUtf(cs, start, len);
        }
    }

}