﻿// Skeleton implementation written by Joe Zachary for CS 3500, January 2015.
// Revised for CS 3500 by Joe Zachary, January 29, 2016

// Alex Kofford
// u0358110

using System;
using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Contains the dependents of each key.
        /// </summary>
        private Dictionary<string, HashSet<string>> depents;

        /// <summary>
        /// Contains the dependees of each key.
        /// </summary>
        private Dictionary<string, HashSet<string>> depees;

        /// <summary>
        /// Contains the number of ordered pairs in a dependency graph. Cannot be negative or null, will be 0 if there are no pairs.
        /// </summary>
        private int size;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            depents = new Dictionary<string, HashSet<string>>();
            depees = new Dictionary<string, HashSet<string>>();

            size = 0;
        }

        /// <summary>
        /// Creates a DependencyGraph that is a copy of the passed dependency graph. Requires d != null else throws ArgumentNullException.
        /// </summary>
        public DependencyGraph(DependencyGraph d)
        {
            if(d != null)
            {
                depees = new Dictionary<string, HashSet<string>>();
                depents = new Dictionary<string, HashSet<string>>();

                foreach(KeyValuePair<string, HashSet<string>> kvp in d.depees)
                {
                    depees.Add(kvp.Key, new HashSet<string>(kvp.Value));
                }
                foreach (KeyValuePair<string, HashSet<string>> kvp in d.depents)
                {
                    depents.Add(kvp.Key, new HashSet<string>(kvp.Value));
                }

                // Commented out code didn't work
                // I think it was because it was just linking the hashsets
                //depents = new Dictionary<string, HashSet<string>>(d.depents);
                //depees = new Dictionary<string, HashSet<string>>(d.depees);

                size = d.Size;
            }
            else
            {
                throw new ArgumentNullException("Cannot be passed null parameter");
            }
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get{ return size; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null else throws ArgumentNullException.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (s != null)
            {
                if (depents.ContainsKey(s))
                {
                    return true;
                }
            }
            else
            {
                throw new ArgumentNullException("Cannot be passed null parameter");
            }
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null else throws ArgumentNullException.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s != null)
            {
                if (depees.ContainsKey(s))
                {
                    return true;
                }
            }
            else
            {
                throw new ArgumentNullException("Cannot be passed null parameter");
            }
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null else throws ArgumentNullException.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            HashSet<string> d;
            if (s != null)
            {
                if (depents.TryGetValue(s, out d))
                {
                    foreach (string z in d)
                    {
                        yield return z;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Cannot be passed null parameter");
            }
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null else throws ArgumentNullException.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            HashSet<string> d;
            if (s != null)
            {
                if (depees.TryGetValue(s, out d))
                {
                    foreach (string z in d)
                    {
                        yield return z;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Cannot be passed null parameter");
            }
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null else throws ArgumentNullException.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            HashSet<string> d = new HashSet<string>();
            if (s != null && t != null)
            {
                if (!depents.ContainsKey(s) || depents.Equals(null))
                {
                    d.Add(t);
                    depents.Add(s, d);
                    size++;
                    d = new HashSet<string>();
                    if(!depees.ContainsKey(t))
                    {
                        d.Add(s);
                        depees.Add(t, d);
                    }
                    else
                    {
                        if(depees.TryGetValue(t, out d))
                        {
                            d.Add(s);
                        }
                    }
                }
                else
                {
                    if (depents.TryGetValue(s, out d))
                    {
                        if (!d.Contains(t))
                        {
                            d.Add(t);
                            size++;
                            if (!depees.ContainsKey(t))
                            {
                                d = new HashSet<string>();
                                d.Add(s);
                                depees.Add(t, d);
                            }
                            else
                            {
                                if (depees.TryGetValue(t, out d))
                                {
                                    d.Add(s);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Cannot be passed null parameter");
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null else throws ArgumentNullException.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            HashSet<string> d;
            if (s != null && t != null)
            {
                if(depents.ContainsKey(s))
                {
                    size--;
                    if (depents.TryGetValue(s, out d))
                    {
                        d.Remove(t);
                        if (d.Count == 0)
                        {
                            depents.Remove(s);
                        }
                    }
                    if (depees.TryGetValue(t, out d))
                    {
                        d.Remove(s);
                        if (d.Count == 0)
                        {
                            depees.Remove(t);
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Cannot be passed null parameter");
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null else throws ArgumentNullException.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            HashSet<string> d;
            if (s != null)
            {
                if (depents.TryGetValue(s, out d))
                {
                    List<string> list = new List<string>(d);
                    foreach (string t in list)
                    {
                        RemoveDependency(s, t);
                    }
                }
                if (newDependents != null)
                {
                    foreach (string t in newDependents)
                    {
                        if (t != null)
                        {
                            AddDependency(s, t);
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Cannot be passed null parameter");
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null else throws ArgumentNullException.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            HashSet<string> d;
            if (t != null)
            {
                if (depees.TryGetValue(t, out d))
                {
                    List<string> list = new List<string>(d);
                    foreach (string s in list)
                    {
                        RemoveDependency(s, t);
                    }
                }
                if (newDependees != null)
                {
                    foreach (string s in newDependees)
                    {
                        if (s != null)
                        {
                            AddDependency(s, t);
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Cannot be passed null parameter");
            }
        }
    }
}
