using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (c) 1999, 2018, Oracle and/or its affiliates. All rights reserved.
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

namespace com.sun.tools.javac.code
{
    //using Kind =com.sun.tools.javac.code.Kinds.Kind;

    //using CompletionFailure =com.sun.tools.javac.code.Symbol.CompletionFailure;
    //using TypeSymbol =com.sun.tools.javac.code.Symbol.TypeSymbol;
    //using JCImport =com.sun.tools.javac.tree.JCTree.JCImport;
    //usingcom.sun.tools.javac.util;

    //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //    import staticcom.sun.tools.javac.code.Scope.LookupKind.NON_RECURSIVE;
    //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //    import staticcom.sun.tools.javac.code.Scope.LookupKind.RECURSIVE;
    //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //    import staticcom.sun.tools.javac.util.Iterators.createCompoundIterator;
    //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    //    import staticcom.sun.tools.javac.util.Iterators.createFilterIterator;

    /// <summary>
    /// A scope represents an area of visibility in a Java program. The
    ///  Scope class is a container for symbols which provides
    ///  efficient access to symbols given their names. Scopes are implemented
    ///  as hash tables with "open addressing" and "double hashing".
    ///  Scopes can be nested. Nested scopes can share their hash tables.
    /// 
    ///  <para><b>This is NOT part of any supported API.
    ///  If you write code that depends on this, you do so at your own risk.
    ///  This code and its internal interfaces are subject to change or
    ///  deletion without notice.</b>
    /// </para>
    /// </summary>
    public abstract class Scope
    {

        //        /// <summary>
        //        /// The scope's owner.
        //        /// </summary>
        //        public readonly Symbol owner;

        //        protected internal Scope(Symbol owner)
        //        {
        //            this.owner = owner;
        //        }

        //        /// <summary>
        //        ///Returns all Symbols in this Scope. Symbols from outward Scopes are included.
        //        /// </summary>
        //        public IEnumerable<Symbol> getSymbols()
        //        {
        //            return getSymbols(noFilter);
        //        }

        //        /// <summary>
        //        ///Returns Symbols that match the given filter. Symbols from outward Scopes are included.
        //        /// </summary>
        //        public IEnumerable<Symbol> getSymbols(Filter<Symbol> sf)
        //        {
        //            return getSymbols(sf, RECURSIVE);
        //        }

        //        /// <summary>
        //        ///Returns all Symbols in this Scope. Symbols from outward Scopes are included
        //        /// iff lookupKind == RECURSIVE.
        //        /// </summary>
        //        public IEnumerable<Symbol> getSymbols(LookupKind lookupKind)
        //        {
        //            return getSymbols(noFilter, lookupKind);
        //        }

        //        /// <summary>
        //        ///Returns Symbols that match the given filter. Symbols from outward Scopes are included
        //        /// iff lookupKind == RECURSIVE.
        //        /// </summary>
        //        public abstract IEnumerable<Symbol> getSymbols(Filter<Symbol> sf, LookupKind lookupKind);

        //        /// <summary>
        //        ///Returns Symbols with the given name. Symbols from outward Scopes are included.
        //        /// </summary>
        //        public IEnumerable<Symbol> getSymbolsByName(Name name)
        //        {
        //            return getSymbolsByName(name, RECURSIVE);
        //        }

        //        /// <summary>
        //        ///Returns Symbols with the given name that match the given filter.
        //        /// Symbols from outward Scopes are included.
        //        /// </summary>
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public final Iterable<Symbol> getSymbolsByName(final Name name, final Filter<Symbol> sf)
        //        public IEnumerable<Symbol> getSymbolsByName(Name name, Filter<Symbol> sf)
        //        {
        //            return getSymbolsByName(name, sf, RECURSIVE);
        //        }

        //        /// <summary>
        //        ///Returns Symbols with the given name. Symbols from outward Scopes are included
        //        /// iff lookupKind == RECURSIVE.
        //        /// </summary>
        //        public IEnumerable<Symbol> getSymbolsByName(Name name, LookupKind lookupKind)
        //        {
        //            return getSymbolsByName(name, noFilter, lookupKind);
        //        }

        //        /// <summary>
        //        ///Returns Symbols with the given name that match the given filter.
        //        /// Symbols from outward Scopes are included iff lookupKind == RECURSIVE.
        //        /// </summary>
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public abstract Iterable<Symbol> getSymbolsByName(final Name name, final Filter<Symbol> sf, final LookupKind lookupKind);
        //        public abstract IEnumerable<Symbol> getSymbolsByName(Name name, Filter<Symbol> sf, LookupKind lookupKind);

        //        /// <summary>
        //        /// Return the first Symbol from this or outward scopes with the given name.
        //        /// Returns null if none.
        //        /// </summary>
        //        public Symbol findFirst(Name name)
        //        {
        //            return findFirst(name, noFilter);
        //        }

        //        /// <summary>
        //        /// Return the first Symbol from this or outward scopes with the given name that matches the
        //        ///  given filter. Returns null if none.
        //        /// </summary>
        //        public virtual Symbol findFirst(Name name, Filter<Symbol> sf)
        //        {
        //            IEnumerator<Symbol> it = getSymbolsByName(name, sf).GetEnumerator();
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //            return it.hasNext() ? it.next() : null;
        //        }

        //        /// <summary>
        //        /// Returns true iff there are is at least one Symbol in this scope matching the given filter.
        //        ///  Does not inspect outward scopes.
        //        /// </summary>
        //        public virtual bool anyMatch(Filter<Symbol> filter)
        //        {
        //            return getSymbols(filter, NON_RECURSIVE).GetEnumerator().hasNext();
        //        }

        //        /// <summary>
        //        /// Returns true iff the given Symbol is in this scope or any outward scope.
        //        /// </summary>
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public boolean includes(final Symbol sym)
        //        public virtual bool includes(Symbol sym)
        //        {
        //            return includes(sym, RECURSIVE);
        //        }

        //        /// <summary>
        //        /// Returns true iff the given Symbol is in this scope, optionally checking outward scopes.
        //        /// </summary>
        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public boolean includes(final Symbol sym, LookupKind lookupKind)
        //        public virtual bool includes(Symbol sym, LookupKind lookupKind)
        //        {
        //            return getSymbolsByName(sym.name, t => t == sym, lookupKind).GetEnumerator().hasNext();
        //        }

        //        /// <summary>
        //        /// Returns true iff this scope does not contain any Symbol. Does not inspect outward scopes.
        //        /// </summary>
        //        public virtual bool isEmpty()
        //        {
        //            return !getSymbols(NON_RECURSIVE).GetEnumerator().hasNext();
        //        }

        //        /// <summary>
        //        /// Returns the Scope from which the givins Symbol originates in this scope.
        //        /// </summary>
        //        public abstract Scope getOrigin(Symbol byName);

        //        /// <summary>
        //        /// Returns true iff the given Symbol is part of this scope due to a static import.
        //        /// </summary>
        //        public abstract bool isStaticallyImported(Symbol byName);

        //        private const Filter<Symbol> noFilter = null;

        //        /// <summary>
        //        /// A list of scopes to be notified if items are to be removed from this scope.
        //        /// </summary>
        //        internal ScopeListenerList listeners = new ScopeListenerList();

        //        public interface ScopeListener
        //        {
        //            void symbolAdded(Symbol sym, Scope s);
        //            void symbolRemoved(Symbol sym, Scope s);
        //        }

        //        /// <summary>
        //        /// A list of scope listeners; listeners are stored in weak references, to avoid memory leaks.
        //        /// When the listener list is scanned (upon notification), elements corresponding to GC-ed
        //        /// listeners are removed so that the listener list size is kept in check.
        //        /// </summary>
        //        public class ScopeListenerList
        //        {

        //            internal List<WeakReference<ScopeListener>> listeners = List.nil();

        //            internal virtual void add(ScopeListener sl)
        //            {
        //                listeners = listeners.prepend(new WeakReference<>(sl));
        //            }

        //            internal virtual void symbolAdded(Symbol sym, Scope scope)
        //            {
        //                walkReferences(sym, scope, false);
        //            }

        //            internal virtual void symbolRemoved(Symbol sym, Scope scope)
        //            {
        //                walkReferences(sym, scope, true);
        //            }

        //            internal virtual void walkReferences(Symbol sym, Scope scope, bool isRemove)
        //            {
        //                ListBuffer<WeakReference<ScopeListener>> newListeners = new ListBuffer<WeakReference<ScopeListener>>();
        //                foreach (WeakReference<ScopeListener> wsl in listeners)
        //                {
        //                    ScopeListener sl = wsl.get();
        //                    if (sl != null)
        //                    {
        //                        if (isRemove)
        //                        {
        //                            sl.symbolRemoved(sym, scope);
        //                        }
        //                        else
        //                        {
        //                            sl.symbolAdded(sym, scope);
        //                        }
        //                        newListeners.add(wsl);
        //                    }
        //                }
        //                listeners = newListeners.toList();
        //            }
        //        }

        //        public enum LookupKind
        //        {
        //            RECURSIVE,
        //            NON_RECURSIVE
        //        }

        //        /// <summary>
        //        ///A scope into which Symbols can be added. </summary>
        //        public abstract class WriteableScope : Scope
        //        {

        //            public WriteableScope(Symbol owner) : base(owner)
        //            {
        //            }

        //            /// <summary>
        //            /// Enter the given Symbol into this scope.
        //            /// </summary>
        //            public abstract void enter(Symbol c);
        //            /// <summary>
        //            /// Enter symbol sym in this scope if not already there.
        //            /// </summary>
        //            public abstract void enterIfAbsent(Symbol c);

        //            public abstract void remove(Symbol c);

        //            /// <summary>
        //            /// Construct a fresh scope within this scope, with same owner. The new scope may
        //            ///  shares internal structures with the this scope. Used in connection with
        //            ///  method leave if scope access is stack-like in order to avoid allocation
        //            ///  of fresh tables.
        //            /// </summary>
        //            public WriteableScope dup()
        //            {
        //                return dup(this.owner);
        //            }

        //            /// <summary>
        //            /// Construct a fresh scope within this scope, with new owner. The new scope may
        //            ///  shares internal structures with the this scope. Used in connection with
        //            ///  method leave if scope access is stack-like in order to avoid allocation
        //            ///  of fresh tables.
        //            /// </summary>
        //            public abstract WriteableScope dup(Symbol newOwner);

        //            /// <summary>
        //            /// Must be called on dup-ed scopes to be able to work with the outward scope again.
        //            /// </summary>
        //            public abstract WriteableScope leave();

        //            /// <summary>
        //            /// Construct a fresh scope within this scope, with same owner. The new scope
        //            ///  will not share internal structures with this scope.
        //            /// </summary>
        //            public WriteableScope dupUnshared()
        //            {
        //                return dupUnshared(owner);
        //            }

        //            /// <summary>
        //            /// Construct a fresh scope within this scope, with new owner. The new scope
        //            ///  will not share internal structures with this scope.
        //            /// </summary>
        //            public abstract WriteableScope dupUnshared(Symbol newOwner);

        //            /// <summary>
        //            /// Create a new WriteableScope.
        //            /// </summary>
        //            public static WriteableScope create(Symbol owner)
        //            {
        //                return new ScopeImpl(owner);
        //            }

        //        }

        //        private class ScopeImpl : WriteableScope
        //        {
        //            /// <summary>
        //            /// The number of scopes that share this scope's hash table.
        //            /// </summary>
        //            internal int shared;

        //            /// <summary>
        //            /// Next enclosing scope (with whom this scope may share a hashtable)
        //            /// </summary>
        //            public ScopeImpl next;

        //            /// <summary>
        //            /// A hash table for the scope's entries.
        //            /// </summary>
        //            internal Entry[] table;

        //            /// <summary>
        //            /// Mask for hash codes, always equal to (table.length - 1).
        //            /// </summary>
        //            internal int hashMask;

        //            /// <summary>
        //            /// A linear list that also contains all entries in
        //            ///  reverse order of appearance (i.e later entries are pushed on top).
        //            /// </summary>
        //            public Entry elems;

        //            /// <summary>
        //            /// The number of elements in this scope.
        //            /// This includes deleted elements, whose value is the sentinel.
        //            /// </summary>
        //            internal int nelems = 0;

        //            internal int removeCount = 0;

        //            /// <summary>
        //            /// Use as a "not-found" result for lookup.
        //            /// Also used to mark deleted entries in the table.
        //            /// </summary>
        //            internal static readonly Entry sentinel = new Entry(null, null, null, null);

        //            /// <summary>
        //            /// The hash table's initial size.
        //            /// </summary>
        //            internal const int INITIAL_SIZE = 0x10;

        //            /// <summary>
        //            /// Construct a new scope, within scope next, with given owner, using
        //            ///  given table. The table's length must be an exponent of 2.
        //            /// </summary>
        //            internal ScopeImpl(ScopeImpl next, Symbol owner, Entry[] table) : base(owner)
        //            {
        //                this.next = next;
        //                Assert.check(owner != null);
        //                this.table = table;
        //                this.hashMask = table.Length - 1;
        //            }

        //            /// <summary>
        //            /// Convenience constructor used for dup and dupUnshared. </summary>
        //            internal ScopeImpl(ScopeImpl next, Symbol owner, Entry[] table, int nelems) : this(next, owner, table)
        //            {
        //                this.nelems = nelems;
        //            }

        //            /// <summary>
        //            /// Construct a new scope, within scope next, with given owner,
        //            ///  using a fresh table of length INITIAL_SIZE.
        //            /// </summary>
        //            public ScopeImpl(Symbol owner) : this(null, owner, new Entry[INITIAL_SIZE])
        //            {
        //            }

        //            /// <summary>
        //            /// Construct a fresh scope within this scope, with new owner,
        //            ///  which shares its table with the outer scope. Used in connection with
        //            ///  method leave if scope access is stack-like in order to avoid allocation
        //            ///  of fresh tables.
        //            /// </summary>
        //            public override WriteableScope dup(Symbol newOwner)
        //            {
        //                ScopeImpl result = new ScopeImpl(this, newOwner, this.table, this.nelems);
        //                shared++;
        //                // System.out.println("====> duping scope " + this.hashCode() + " owned by " + newOwner + " to " + result.hashCode());
        //                // new Error().printStackTrace(System.out);
        //                return result;
        //            }

        //            /// <summary>
        //            /// Construct a fresh scope within this scope, with new owner,
        //            ///  with a new hash table, whose contents initially are those of
        //            ///  the table of its outer scope.
        //            /// </summary>
        //            public override WriteableScope dupUnshared(Symbol newOwner)
        //            {
        //                if (shared > 0)
        //                {
        //                    //The nested Scopes might have already added something to the table, so all items
        //                    //that don't originate in this Scope or any of its outer Scopes need to be cleared:
        //                    ISet<Scope> acceptScopes = Collections.newSetFromMap(new IdentityHashMap<Scope>());
        //                    ScopeImpl c = this;
        //                    while (c != null)
        //                    {
        //                        acceptScopes.Add(c);
        //                        c = c.next;
        //                    }
        //                    int n = 0;
        //                    Entry[] oldTable = this.table;
        //                    Entry[] newTable = new Entry[this.table.Length];
        //                    for (int i = 0; i < oldTable.Length; i++)
        //                    {
        //                        Entry e = oldTable[i];
        //                        while (e != null && e != sentinel && !acceptScopes.Contains(e.scope))
        //                        {
        //                            e = e.shadowed;
        //                        }
        //                        if (e != null)
        //                        {
        //                            n++;
        //                            newTable[i] = e;
        //                        }
        //                    }
        //                    return new ScopeImpl(this, newOwner, newTable, n);
        //                }
        //                else
        //                {
        //                    return new ScopeImpl(this, newOwner, this.table.Clone(), this.nelems);
        //                }
        //            }

        //            /// <summary>
        //            /// Remove all entries of this scope from its table, if shared
        //            ///  with next.
        //            /// </summary>
        //            public override WriteableScope leave()
        //            {
        //                Assert.check(shared == 0);
        //                if (table != next.table)
        //                {
        //                    return next;
        //                }
        //                while (elems != null)
        //                {
        //                    int hash = getIndex(elems.sym.name);
        //                    Entry e = table[hash];
        //                    Assert.check(e == elems, elems.sym);
        //                    table[hash] = elems.shadowed;
        //                    elems = elems.nextSibling;
        //                }
        //                Assert.check(next.shared > 0);
        //                next.shared--;
        //                next.nelems = nelems;
        //                // System.out.println("====> leaving scope " + this.hashCode() + " owned by " + this.owner + " to " + next.hashCode());
        //                // new Error().printStackTrace(System.out);
        //                return next;
        //            }

        //            /// <summary>
        //            /// Double size of hash table.
        //            /// </summary>
        //            internal virtual void dble()
        //            {
        //                Assert.check(shared == 0);
        //                Entry[] oldtable = table;
        //                Entry[] newtable = new Entry[oldtable.Length * 2];
        //                for (ScopeImpl s = this; s != null; s = s.next)
        //                {
        //                    if (s.table == oldtable)
        //                    {
        //                        Assert.check(s == this || s.shared != 0);
        //                        s.table = newtable;
        //                        s.hashMask = newtable.Length - 1;
        //                    }
        //                }
        //                int n = 0;
        //                for (int i = oldtable.Length; --i >= 0;)
        //                {
        //                    Entry e = oldtable[i];
        //                    if (e != null && e != sentinel)
        //                    {
        //                        table[getIndex(e.sym.name)] = e;
        //                        n++;
        //                    }
        //                }
        //                // We don't need to update nelems for shared inherited scopes,
        //                // since that gets handled by leave().
        //                nelems = n;
        //            }

        //            /// <summary>
        //            /// Enter symbol sym in this scope.
        //            /// </summary>
        //            public override void enter(Symbol sym)
        //            {
        //                Assert.check(shared == 0);
        //                if (nelems * 3 >= hashMask * 2)
        //                {
        //                    dble();
        //                }
        //                int hash = getIndex(sym.name);
        //                Entry old = table[hash];
        //                if (old == null)
        //                {
        //                    old = sentinel;
        //                    nelems++;
        //                }
        //                Entry e = new Entry(sym, old, elems, this);
        //                table[hash] = e;
        //                elems = e;

        //                //notify listeners
        //                listeners.symbolAdded(sym, this);
        //            }

        //            /// <summary>
        //            /// Remove symbol from this scope.
        //            /// </summary>
        //            public override void remove(Symbol sym)
        //            {
        //                Assert.check(shared == 0);
        //                Entry e = lookup(sym.name, candidate => candidate == sym);
        //                if (e.scope == null)
        //                {
        //                    return;
        //                }

        //                // remove e from table and shadowed list;
        //                int i = getIndex(sym.name);
        //                Entry te = table[i];
        //                if (te == e)
        //                {
        //                    table[i] = e.shadowed;
        //                }
        //                else
        //                {
        //                    while (true)
        //                    {
        //                    if (te.shadowed == e)
        //                    {
        //                        te.shadowed = e.shadowed;
        //                        break;
        //                    }
        //                    te = te.shadowed;
        //                    }
        //                }

        //                // remove e from elems and sibling list
        //                if (elems == e)
        //                {
        //                    elems = e.nextSibling;
        //                    if (elems != null)
        //                    {
        //                        elems.prevSibling = null;
        //                    }
        //                }
        //                else
        //                {
        //                    Assert.check(e.prevSibling != null, e.sym);
        //                    e.prevSibling.nextSibling = e.nextSibling;
        //                    if (e.nextSibling != null)
        //                    {
        //                        e.nextSibling.prevSibling = e.prevSibling;
        //                    }
        //                }

        //                removeCount++;

        //                //notify listeners
        //                listeners.symbolRemoved(sym, this);
        //            }

        //            /// <summary>
        //            /// Enter symbol sym in this scope if not already there.
        //            /// </summary>
        //            public override void enterIfAbsent(Symbol sym)
        //            {
        //                Assert.check(shared == 0);
        //                Entry e = lookup(sym.name);
        //                while (e.scope == this && e.sym.kind != sym.kind)
        //                {
        //                    e = e.next();
        //                }
        //                if (e.scope != this)
        //                {
        //                    enter(sym);
        //                }
        //            }

        //            /// <summary>
        //            /// Given a class, is there already a class with same fully
        //            ///  qualified name in this (import) scope?
        //            /// </summary>
        //            public override bool includes(Symbol c)
        //            {
        //                for (Scope.Entry e = lookup(c.name); e.scope == this; e = e.next())
        //                {
        //                    if (e.sym == c)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                return false;
        //            }

        //            /// <summary>
        //            /// Return the entry associated with given name, starting in
        //            ///  this scope and proceeding outwards. If no entry was found,
        //            ///  return the sentinel, which is characterized by having a null in
        //            ///  both its scope and sym fields, whereas both fields are non-null
        //            ///  for regular entries.
        //            /// </summary>
        //            protected internal virtual Entry lookup(Name name)
        //            {
        //                return lookup(name, noFilter);
        //            }

        //            protected internal virtual Entry lookup(Name name, Filter<Symbol> sf)
        //            {
        //                Entry e = table[getIndex(name)];
        //                if (e == null || e == sentinel)
        //                {
        //                    return sentinel;
        //                }
        //                while (e.scope != null && (e.sym.name != name || (sf != null && !sf.accepts(e.sym))))
        //                {
        //                    e = e.shadowed;
        //                }
        //                return e;
        //            }

        //            public override Symbol findFirst(Name name, Filter<Symbol> sf)
        //            {
        //                return lookup(name, sf).sym;
        //            }

        //            /*void dump (java.io.PrintStream out) {
        //                out.println(this);
        //                for (int l=0; l < table.length; l++) {
        //                    Entry le = table[l];
        //                    out.print("#"+l+": ");
        //                    if (le==sentinel) out.println("sentinel");
        //                    else if(le == null) out.println("null");
        //                    else out.println(""+le+" s:"+le.sym);
        //                }
        //            }*/

        //            /// <summary>
        //            /// Look for slot in the table.
        //            ///  We use open addressing with double hashing.
        //            /// </summary>
        //            internal virtual int getIndex(Name name)
        //            {
        //                int h = name.GetHashCode();
        //                int i = h & hashMask;
        //                // The expression below is always odd, so it is guaranteed
        //                // to be mutually prime with table.length, a power of 2.
        //                int x = hashMask - ((h + (h >> 16)) << 1);
        //                int d = -1; // Index of a deleted item.
        //                for (;;)
        //                {
        //                    Entry e = table[i];
        //                    if (e == null)
        //                    {
        //                        return d >= 0 ? d : i;
        //                    }
        //                    if (e == sentinel)
        //                    {
        //                        // We have to keep searching even if we see a deleted item.
        //                        // However, remember the index in case we fail to find the name.
        //                        if (d < 0)
        //                        {
        //                            d = i;
        //                        }
        //                    }
        //                    else if (e.sym.name == name)
        //                    {
        //                        return i;
        //                    }
        //                    i = (i + x) & hashMask;
        //                }
        //            }

        //            public override bool anyMatch(Filter<Symbol> sf)
        //            {
        //                return getSymbols(sf, NON_RECURSIVE).GetEnumerator().hasNext();
        //            }

        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public Iterable<Symbol> getSymbols(final Filter<Symbol> sf, final LookupKind lookupKind)
        //            public override IEnumerable<Symbol> getSymbols(Filter<Symbol> sf, LookupKind lookupKind)
        //            {
        //                return () => new IteratorAnonymousInnerClass(this, sf, lookupKind);
        //            }

        //            private class IteratorAnonymousInnerClass : IEnumerator<Symbol>
        //            {
        //                private readonly ScopeImpl outerInstance;

        //                private Filter<Symbol> sf;
        //                privatecom.sun.tools.javac.code.Scope.LookupKind lookupKind;

        //                public IteratorAnonymousInnerClass(ScopeImpl outerInstance, Filter<Symbol> sf,com.sun.tools.javac.code.Scope.LookupKind lookupKind)
        //                {
        //                    this.outerInstance = outerInstance;
        //                    this.sf = sf;
        //                    this.lookupKind = lookupKind;
        //                    currScope = outerInstance;
        //                    currEntry = outerInstance.elems;
        //                    seenRemoveCount = currScope.removeCount;

        //                    update();
        //                }

        //                private ScopeImpl currScope;
        //                private Entry currEntry;
        //                private int seenRemoveCount;

        //                public virtual bool hasNext()
        //                {
        //                    if (seenRemoveCount != currScope.removeCount && currEntry != null && !currEntry.scope.includes(currEntry.sym))
        //                    {
        //                        doNext(); //skip entry that is no longer in the Scope
        //                        seenRemoveCount = currScope.removeCount;
        //                    }
        //                    return currEntry != null;
        //                }

        //                public virtual Symbol next()
        //                {
        //                    if (!hasNext())
        //                    {
        //                        throw new NoSuchElementException();
        //                    }

        //                    return doNext();
        //                }
        //                private Symbol doNext()
        //                {
        //                    Symbol sym = (currEntry == null ? null : currEntry.sym);
        //                    if (currEntry != null)
        //                    {
        //                        currEntry = currEntry.nextSibling;
        //                    }
        //                    update();
        //                    return sym;
        //                }

        //                private void update()
        //                {
        //                    skipToNextMatchingEntry();
        //                    if (lookupKind == RECURSIVE)
        //                    {
        //                        while (currEntry == null && currScope.next != null)
        //                        {
        //                            currScope = currScope.next;
        //                            currEntry = currScope.elems;
        //                            seenRemoveCount = currScope.removeCount;
        //                            skipToNextMatchingEntry();
        //                        }
        //                    }
        //                }

        //                internal virtual void skipToNextMatchingEntry()
        //                {
        //                    while (currEntry != null && sf != null && !sf.accepts(currEntry.sym))
        //                    {
        //                        currEntry = currEntry.nextSibling;
        //                    }
        //                }
        //            }

        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public Iterable<Symbol> getSymbolsByName(final Name name, final Filter<Symbol> sf, final LookupKind lookupKind)
        //            public override IEnumerable<Symbol> getSymbolsByName(Name name, Filter<Symbol> sf, LookupKind lookupKind)
        //            {
        //                return () => new IteratorAnonymousInnerClass2(this, name, sf, lookupKind);
        //            }

        //            private class IteratorAnonymousInnerClass2 : IEnumerator<Symbol>
        //            {
        //                private readonly ScopeImpl outerInstance;

        //                private Name name;
        //                private Filter<Symbol> sf;
        //                privatecom.sun.tools.javac.code.Scope.LookupKind lookupKind;

        //                public IteratorAnonymousInnerClass2(ScopeImpl outerInstance, Name name, Filter<Symbol> sf,com.sun.tools.javac.code.Scope.LookupKind lookupKind)
        //                {
        //                    this.outerInstance = outerInstance;
        //                    this.name = name;
        //                    this.sf = sf;
        //                    this.lookupKind = lookupKind;
        //                    currentEntry = outerInstance.lookup(name, sf);
        //                    seenRemoveCount = currentEntry.scope != null ? currentEntry.scope.removeCount : -1;
        //                }

        //                internal Entry currentEntry;
        //                internal int seenRemoveCount;

        //                public virtual bool hasNext()
        //                {
        //                    if (currentEntry.scope != null && seenRemoveCount != currentEntry.scope.removeCount && !currentEntry.scope.includes(currentEntry.sym))
        //                    {
        //                        doNext(); //skip entry that is no longer in the Scope
        //                    }
        //                    return currentEntry.scope != null && (lookupKind == RECURSIVE || currentEntry.scope == outerInstance);
        //                }
        //                public virtual Symbol next()
        //                {
        //                    if (!hasNext())
        //                    {
        //                        throw new NoSuchElementException();
        //                    }
        //                    return doNext();
        //                }
        //                private Symbol doNext()
        //                {
        //                    Entry prevEntry = currentEntry;
        //                    currentEntry = currentEntry.next(sf);
        //                    return prevEntry.sym;
        //                }
        //                public virtual void remove()
        //                {
        //                    throw new System.NotSupportedException();
        //                }
        //            }

        //            public override Scope getOrigin(Symbol s)
        //            {
        //                for (Scope.Entry e = lookup(s.name); e.scope != null ; e = e.next())
        //                {
        //                    if (e.sym == s)
        //                    {
        //                        return this;
        //                    }
        //                }
        //                return null;
        //            }

        //            public override bool isStaticallyImported(Symbol s)
        //            {
        //                return false;
        //            }

        //            public override string ToString()
        //            {
        //                StringBuilder result = new StringBuilder();
        //                result.Append("Scope[");
        //                for (ScopeImpl s = this; s != null ; s = s.next)
        //                {
        //                    if (s != this)
        //                    {
        //                        result.Append(" | ");
        //                    }
        //                    for (Entry e = s.elems; e != null; e = e.nextSibling)
        //                    {
        //                        if (e != s.elems)
        //                        {
        //                            result.Append(", ");
        //                        }
        //                        result.Append(e.sym);
        //                    }
        //                }
        //                result.Append("]");
        //                return result.ToString();
        //            }
        //        }

        //        /// <summary>
        //        /// A class for scope entries.
        //        /// </summary>
        //        private class Entry
        //        {

        //            /// <summary>
        //            /// The referenced symbol.
        //            ///  sym == null   iff   this == sentinel
        //            /// </summary>
        //            public Symbol sym;

        //            /// <summary>
        //            /// An entry with the same hash code, or sentinel.
        //            /// </summary>
        //            internal Entry shadowed;

        //            /// <summary>
        //            /// Next entry in same scope.
        //            /// </summary>
        //            public Entry nextSibling;

        //            /// <summary>
        //            /// Prev entry in same scope.
        //            /// </summary>
        //            public Entry prevSibling;

        //            /// <summary>
        //            /// The entry's scope.
        //            ///  scope == null   iff   this == sentinel
        //            /// </summary>
        //            public ScopeImpl scope;

        //            public Entry(Symbol sym, Entry shadowed, Entry nextSibling, ScopeImpl scope)
        //            {
        //                this.sym = sym;
        //                this.shadowed = shadowed;
        //                this.nextSibling = nextSibling;
        //                this.scope = scope;
        //                if (nextSibling != null)
        //                {
        //                    nextSibling.prevSibling = this;
        //                }
        //            }

        //            /// <summary>
        //            /// Return next entry with the same name as this entry, proceeding
        //            ///  outwards if not found in this scope.
        //            /// </summary>
        //            public virtual Entry next()
        //            {
        //                return shadowed;
        //            }

        //            public virtual Entry next(Filter<Symbol> sf)
        //            {
        //                if (shadowed.sym == null || sf == null || sf.accepts(shadowed.sym))
        //                {
        //                    return shadowed;
        //                }
        //                else
        //                {
        //                    return shadowed.next(sf);
        //                }
        //            }

        //        }

        //        public class ImportScope : CompoundScope
        //        {

        //            public ImportScope(Symbol owner) : base(owner)
        //            {
        //            }

        //            /// <summary>
        //            ///Finalize the content of the ImportScope to speed-up future lookups.
        //            /// No further changes to class hierarchy or class content will be reflected.
        //            /// </summary>
        //            public virtual void finalizeScope()
        //            {
        //                for (List<Scope> scopes = this.subScopes.toList(); scopes.nonEmpty(); scopes = scopes.tail)
        //                {
        //                    scopes.head = finalizeSingleScope(scopes.head);
        //                }
        //            }

        //            protected internal virtual Scope finalizeSingleScope(Scope impScope)
        //            {
        //                if (impScope is FilterImportScope && impScope.owner.kind == Kind.TYP && ((FilterImportScope) impScope).isStaticallyImported())
        //                {
        //                    WriteableScope finalized = WriteableScope.create(impScope.owner);

        //                    foreach (Symbol sym in impScope.getSymbols())
        //                    {
        //                        finalized.enter(sym);
        //                    }

        //                    finalized.listeners.add(new ScopeListenerAnonymousInnerClass(this));

        //                    return finalized;
        //                }

        //                return impScope;
        //            }

        //            private class ScopeListenerAnonymousInnerClass : ScopeListener
        //            {
        //                private readonly ImportScope outerInstance;

        //                public ScopeListenerAnonymousInnerClass(ImportScope outerInstance)
        //                {
        //                    this.outerInstance = outerInstance;
        //                }

        //                public virtual void symbolAdded(Symbol sym, Scope s)
        //                {
        //                    Assert.error("The scope is sealed.");
        //                }

        //                public virtual void symbolRemoved(Symbol sym, Scope s)
        //                {
        //                    Assert.error("The scope is sealed.");
        //                }
        //            }

        //        }

        //        public class NamedImportScope : ImportScope
        //        {

        //            /*A cache for quick lookup of Scopes that may contain the given name.
        //              ScopeImpl and Entry is not used, as it is maps names to Symbols,
        //              but it is necessary to map names to Scopes at this place (so that any
        //              changes to the content of the Scopes is reflected when looking up the
        //              Symbols.
        //             */
        //            internal readonly IDictionary<Name, Scope[]> name2Scopes = new Dictionary<Name, Scope[]>();

        //            public NamedImportScope(Symbol owner) : base(owner)
        //            {
        //            }

        //            public virtual Scope importByName(Types types, Scope origin, Name name, ImportFilter filter, JCImport imp, System.Action<JCImport, CompletionFailure> cfHandler)
        //            {
        //                return appendScope(new FilterImportScope(types, origin, name, filter, imp, cfHandler), name);
        //            }

        //            public virtual Scope importType(Scope @delegate, Scope origin, Symbol sym)
        //            {
        //                return appendScope(new SingleEntryScope(@delegate.owner, sym, origin), sym.name);
        //            }

        //            internal virtual Scope appendScope(Scope newScope, Name name)
        //            {
        //                appendSubScope(newScope);
        //                Scope[] existing = name2Scopes[name];
        //                if (existing != null)
        //                {
        //                    existing = Arrays.CopyOf(existing, existing.Length + 1);
        //                }
        //                else
        //                {
        //                    existing = new Scope[1];
        //                }
        //                existing[existing.Length - 1] = newScope;
        //                name2Scopes[name] = existing;
        //                return newScope;
        //            }

        //            public override IEnumerable<Symbol> getSymbolsByName(Name name, Filter<Symbol> sf, LookupKind lookupKind)
        //            {
        //                Scope[] scopes = name2Scopes[name];
        //                if (scopes == null)
        //                {
        //                    return java.util.Collections.emptyList();
        //                }
        //                return () => Iterators.createCompoundIterator(scopes, scope => scope.getSymbolsByName(name, sf, lookupKind).GetEnumerator());
        //            }
        //            public override void finalizeScope()
        //            {
        //                base.finalizeScope();
        //                foreach (Scope[] scopes in name2Scopes.Values)
        //                {
        //                    for (int i = 0; i < scopes.Length; i++)
        //                    {
        //                        scopes[i] = finalizeSingleScope(scopes[i]);
        //                    }
        //                }
        //            }

        //            private class SingleEntryScope : Scope
        //            {

        //                internal readonly Symbol sym;
        //                internal readonly List<Symbol> content;
        //                internal readonly Scope origin;

        //                public SingleEntryScope(Symbol owner, Symbol sym, Scope origin) : base(owner)
        //                {
        //                    this.sym = sym;
        //                    this.content = List.of(sym);
        //                    this.origin = origin;
        //                }

        //                public override IEnumerable<Symbol> getSymbols(Filter<Symbol> sf, LookupKind lookupKind)
        //                {
        //                    return sf == null || sf.accepts(sym) ? content : java.util.Collections.emptyList();
        //                }

        //                public override IEnumerable<Symbol> getSymbolsByName(Name name, Filter<Symbol> sf, LookupKind lookupKind)
        //                {
        //                    return sym.name == name && (sf == null || sf.accepts(sym)) ? content : java.util.Collections.emptyList();
        //                }

        //                public override Scope getOrigin(Symbol byName)
        //                {
        //                    return sym == byName ? origin : null;
        //                }

        //                public override bool isStaticallyImported(Symbol byName)
        //                {
        //                    return false;
        //                }

        //            }
        //        }

        //        public class StarImportScope : ImportScope
        //        {

        //            public StarImportScope(Symbol owner) : base(owner)
        //            {
        //            }

        //            public virtual void importAll(Types types, Scope origin, ImportFilter filter, JCImport imp, System.Action<JCImport, CompletionFailure> cfHandler)
        //            {
        //                foreach (Scope existing in subScopes)
        //                {
        //                    Assert.check(existing is FilterImportScope);
        //                    FilterImportScope fis = (FilterImportScope) existing;
        //                    if (fis.origin == origin && fis.filter == filter && fis.imp.staticImport == imp.staticImport)
        //                    {
        //                        return; //avoid entering the same scope twice
        //                    }
        //                }
        //                prependSubScope(new FilterImportScope(types, origin, null, filter, imp, cfHandler));
        //            }

        //            public virtual bool isFilled()
        //            {
        //                return subScopes.nonEmpty();
        //            }

        //        }

        //        public interface ImportFilter
        //        {
        //            bool accepts(Scope origin, Symbol sym);
        //        }

        //        private class FilterImportScope : Scope
        //        {

        //            internal readonly Types types;
        //            internal readonly Scope origin;
        //            internal readonly Name filterName;
        //            internal readonly ImportFilter filter;
        //            internal readonly JCImport imp;
        //            internal readonly System.Action<JCImport, CompletionFailure> cfHandler;

        //            public FilterImportScope(Types types, Scope origin, Name filterName, ImportFilter filter, JCImport imp, System.Action<JCImport, CompletionFailure> cfHandler) : base(origin.owner)
        //            {
        //                this.types = types;
        //                this.origin = origin;
        //                this.filterName = filterName;
        //                this.filter = filter;
        //                this.imp = imp;
        //                this.cfHandler = cfHandler;
        //            }

        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: @Override public Iterable<Symbol> getSymbols(final Filter<Symbol> sf, final LookupKind lookupKind)
        //            public override IEnumerable<Symbol> getSymbols(Filter<Symbol> sf, LookupKind lookupKind)
        //            {
        //                if (filterName != null)
        //                {
        //                    return getSymbolsByName(filterName, sf, lookupKind);
        //                }
        //                try
        //                {
        //                    SymbolImporter si = new SymbolImporterAnonymousInnerClass(this, imp.staticImport, sf, lookupKind);
        //                    List<IEnumerable<Symbol>> results = si.importFrom((TypeSymbol) origin.owner, List.nil());
        //                    return () => createFilterIterator(createCompoundIterator(results, IEnumerable.iterator), s => filter.accepts(origin, s));
        //                }
        //                catch (CompletionFailure cf)
        //                {
        //                    cfHandler.accept(imp, cf);
        //                    return java.util.Collections.emptyList();
        //                }
        //            }

        //            private class SymbolImporterAnonymousInnerClass : SymbolImporter
        //            {
        //                private readonly FilterImportScope outerInstance;

        //                private Filter<Symbol> sf;
        //                privatecom.sun.tools.javac.code.Scope.LookupKind lookupKind;

        //                public SymbolImporterAnonymousInnerClass(FilterImportScope outerInstance, UnknownType staticImport, Filter<Symbol> sf,com.sun.tools.javac.code.Scope.LookupKind lookupKind) : base(outerInstance, staticImport)
        //                {
        //                    this.outerInstance = outerInstance;
        //                    this.sf = sf;
        //                    this.lookupKind = lookupKind;
        //                }

        //                internal override IEnumerable<Symbol> doLookup(TypeSymbol tsym)
        //                {
        //                    return tsym.members().getSymbols(sf, lookupKind);
        //                }
        //            }

        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: @Override public Iterable<Symbol> getSymbolsByName(final Name name, final Filter<Symbol> sf, final LookupKind lookupKind)
        //            public override IEnumerable<Symbol> getSymbolsByName(Name name, Filter<Symbol> sf, LookupKind lookupKind)
        //            {
        //                if (filterName != null && filterName != name)
        //                {
        //                    return java.util.Collections.emptyList();
        //                }
        //                try
        //                {
        //                    SymbolImporter si = new SymbolImporterAnonymousInnerClass2(this, imp.staticImport, name, sf, lookupKind);
        //                    List<IEnumerable<Symbol>> results = si.importFrom((TypeSymbol) origin.owner, List.nil());
        //                    return () => createFilterIterator(createCompoundIterator(results, IEnumerable.iterator), s => filter.accepts(origin, s));
        //                }
        //                catch (CompletionFailure cf)
        //                {
        //                    cfHandler.accept(imp, cf);
        //                    return java.util.Collections.emptyList();
        //                }
        //            }

        //            private class SymbolImporterAnonymousInnerClass2 : SymbolImporter
        //            {
        //                private readonly FilterImportScope outerInstance;

        //                private Name name;
        //                private Filter<Symbol> sf;
        //                privatecom.sun.tools.javac.code.Scope.LookupKind lookupKind;

        //                public SymbolImporterAnonymousInnerClass2(FilterImportScope outerInstance, UnknownType staticImport, Name name, Filter<Symbol> sf,com.sun.tools.javac.code.Scope.LookupKind lookupKind) : base(outerInstance, staticImport)
        //                {
        //                    this.outerInstance = outerInstance;
        //                    this.name = name;
        //                    this.sf = sf;
        //                    this.lookupKind = lookupKind;
        //                }

        //                internal override IEnumerable<Symbol> doLookup(TypeSymbol tsym)
        //                {
        //                    return tsym.members().getSymbolsByName(name, sf, lookupKind);
        //                }
        //            }

        //            public override Scope getOrigin(Symbol byName)
        //            {
        //                return origin;
        //            }

        //            public override bool isStaticallyImported(Symbol byName)
        //            {
        //                return isStaticallyImported();
        //            }

        //            public virtual bool isStaticallyImported()
        //            {
        //                return imp.staticImport;
        //            }

        //            internal abstract class SymbolImporter
        //            {
        //                private readonly Scope.FilterImportScope outerInstance;

        //                internal ISet<Symbol> processed = new HashSet<Symbol>();
        //                internal List<IEnumerable<Symbol>> delegates = List.nil();
        //                internal readonly bool inspectSuperTypes;
        //                public SymbolImporter(Scope.FilterImportScope outerInstance, bool inspectSuperTypes)
        //                {
        //                    this.outerInstance = outerInstance;
        //                    this.inspectSuperTypes = inspectSuperTypes;
        //                }
        //                internal virtual List<IEnumerable<Symbol>> importFrom(TypeSymbol tsym, List<IEnumerable<Symbol>> results)
        //                {
        //                    if (tsym == null || !processed.Add(tsym))
        //                    {
        //                        return results;
        //                    }


        //                    if (inspectSuperTypes)
        //                    {
        //                        // also import inherited names
        //                        results = importFrom(outerInstance.types.supertype(tsym.type).tsym, results);
        //                        foreach (Type t in outerInstance.types.interfaces(tsym.type))
        //                        {
        //                            results = importFrom(t.tsym, results);
        //                        }
        //                    }

        //                    return results.prepend(doLookup(tsym));
        //                }
        //                internal abstract IEnumerable<Symbol> doLookup(TypeSymbol tsym);
        //            }

        //        }

        //        /// <summary>
        //        /// A class scope adds capabilities to keep track of changes in related
        //        ///  class scopes - this allows client to realize whether a class scope
        //        ///  has changed, either directly (because a new member has been added/removed
        //        ///  to this scope) or indirectly (i.e. because a new member has been
        //        ///  added/removed into a supertype scope)
        //        /// </summary>
        //        public class CompoundScope : Scope, ScopeListener
        //        {

        //            internal ListBuffer<Scope> subScopes = new ListBuffer<Scope>();
        //            internal int mark = 0;

        //            public CompoundScope(Symbol owner) : base(owner)
        //            {
        //            }

        //            public virtual void prependSubScope(Scope that)
        //            {
        //               if (that != null)
        //               {
        //                    subScopes.prepend(that);
        //                    that.listeners.add(this);
        //                    mark++;
        //                    listeners.symbolAdded(null, this);
        //               }
        //            }

        //            public virtual void appendSubScope(Scope that)
        //            {
        //               if (that != null)
        //               {
        //                    subScopes.append(that);
        //                    that.listeners.add(this);
        //                    mark++;
        //                    listeners.symbolAdded(null, this);
        //               }
        //            }

        //            public virtual void symbolAdded(Symbol sym, Scope s)
        //            {
        //                mark++;
        //                listeners.symbolAdded(sym, s);
        //            }

        //            public virtual void symbolRemoved(Symbol sym, Scope s)
        //            {
        //                mark++;
        //                listeners.symbolRemoved(sym, s);
        //            }

        //            public virtual int getMark()
        //            {
        //                return mark;
        //            }

        //            public override string ToString()
        //            {
        //                StringBuilder buf = new StringBuilder();
        //                buf.Append("CompoundScope{");
        //                string sep = "";
        //                foreach (Scope s in subScopes)
        //                {
        //                    buf.Append(sep);
        //                    buf.Append(s);
        //                    sep = ",";
        //                }
        //                buf.Append("}");
        //                return buf.ToString();
        //            }

        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: @Override public Iterable<Symbol> getSymbols(final Filter<Symbol> sf, final LookupKind lookupKind)
        //            public override IEnumerable<Symbol> getSymbols(Filter<Symbol> sf, LookupKind lookupKind)
        //            {
        //                return () => Iterators.createCompoundIterator(subScopes, scope => scope.getSymbols(sf, lookupKind).GetEnumerator());
        //            }

        ////JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: @Override public Iterable<Symbol> getSymbolsByName(final Name name, final Filter<Symbol> sf, final LookupKind lookupKind)
        //            public override IEnumerable<Symbol> getSymbolsByName(Name name, Filter<Symbol> sf, LookupKind lookupKind)
        //            {
        //                return () => Iterators.createCompoundIterator(subScopes, scope => scope.getSymbolsByName(name, sf, lookupKind).GetEnumerator());
        //            }

        //            public override Scope getOrigin(Symbol sym)
        //            {
        //                foreach (Scope @delegate in subScopes)
        //                {
        //                    if (@delegate.includes(sym))
        //                    {
        //                        return @delegate.getOrigin(sym);
        //                    }
        //                }

        //                return null;
        //            }

        //            public override bool isStaticallyImported(Symbol sym)
        //            {
        //                foreach (Scope @delegate in subScopes)
        //                {
        //                    if (@delegate.includes(sym))
        //                    {
        //                        return @delegate.isStaticallyImported(sym);
        //                    }
        //                }

        //                return false;
        //            }

        //        }

        //        /// <summary>
        //        /// An error scope, for which the owner should be an error symbol. </summary>
        //        public class ErrorScope : ScopeImpl
        //        {
        //            internal ErrorScope(ScopeImpl next, Symbol errSymbol, Entry[] table) : base(next, errSymbol, table)
        //            {
        //            }
        //            public ErrorScope(Symbol errSymbol) : base(errSymbol)
        //            {
        //            }
        //            public override WriteableScope dup(Symbol newOwner)
        //            {
        //                return new ErrorScope(this, newOwner, table);
        //            }
        //            public override WriteableScope dupUnshared(Symbol newOwner)
        //            {
        //                return new ErrorScope(this, newOwner, table.Clone());
        //            }
        //            public override Entry lookup(Name name)
        //            {
        //                Entry e = base.lookup(name);
        //                if (e.scope == null)
        //                {
        //                    return new Entry(owner, null, null, null);
        //                }
        //                else
        //                {
        //                    return e;
        //                }
        //            }
        //        }
    }

}