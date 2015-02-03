// Skeleton implementation written by Joe Zachary for CS 3500, January 2015.
// Version 1.1 (1/28/15 7:00 p.m.): Changed name of namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.  (Recall that sets never contain duplicates.  If an attempt
    /// is made to add an element to a set, and the element is already in the set, the set remains unchanged.)
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If s is a string, the set of all strings t such that the dependency (t,s) is in DG 
    ///    is called the dependees of s, which we will denote as dependees(s).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of the class.
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
    /// IMPLEMENTATION NOTE:  The simplest way to describe a DependencyGraph is as a set of dependencies.
    /// This is neither the simplest nor the most efficient way to implement a DependencyGraph, though.  Choose
    /// a representation that is both easy to work with and acceptably efficient.  Some of the test cases
    /// with which you will be graded will create massive DependencyGraphs.
    /// </summary>
    public class DependencyGraph
    {
        Dictionary<String, List<String>> dependeeToDependent;
        Dictionary<String, List<String>> dependentToDependee;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependeeToDependent = new Dictionary<String, List<String>>();
            dependentToDependee = new Dictionary<String, List<String>>();
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return dependentToDependee.Count; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (dependeeToDependent.ContainsKey(s))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (dependentToDependee.ContainsKey(s))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            List<String> dependents;
            if (!dependeeToDependent.TryGetValue(s, out dependents))
            {
                List<String> emptyDependents = new List<String>();
                return emptyDependents;
            }
            return dependents;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            List<String> dependees;
            if (!dependentToDependee.TryGetValue(s, out dependees))
            {
                List<String> emptyDependees = new List<String>();
                return emptyDependees;
            }
            return dependees;
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph
        /// This has no effect if (s,t) already belongs to this DependencyGraph
        /// </summary>
        public void AddDependency(string s, string t)
        {
            // Add dependentToDependee
            if (dependeeToDependent.ContainsKey(s))
            {
                dependeeToDependent[s].Add(t);
            }
            else
            {
                List<String> temp = new List<String>();
                temp.Add(t);
                dependeeToDependent.Add(s, temp);
            }

            // Add dependeeToDependent
            if (dependentToDependee.ContainsKey(t))
            {
                dependentToDependee[s].Add(s);
            }
            else
            {
                List<String> temp = new List<String>();
                temp.Add(s);
                dependentToDependee.Add(t, temp);
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            // Remove dependentToDependee
            List<String> dependents;
            if (dependeeToDependent.TryGetValue(s, out dependents))
            {
                if (dependents.Count - 1 == 0) 
                {
                    dependeeToDependent.Remove(s);
                }

                dependents.Remove(t);
                dependeeToDependent.Remove(s);
                dependeeToDependent.Add(s, dependents);
            }

            // Remove dependeeToDependent
            List<String> dependees; 
            if (dependentToDependee.TryGetValue(t, out dependees))
            {
                if (dependees.Count - 1 == 0)
                {
                    dependentToDependee.Remove(t);
                }

                dependees.Remove(s);
                dependentToDependee.Remove(t);
                dependentToDependee.Add(t, dependees);
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            List<String> values = new List<String>();
            foreach (String temp in newDependents)
            {
                values.Add(temp);
            }

            // Get values from s 
            List<String> dependents = new List<String>();
            dependeeToDependent.TryGetValue(s, out dependents);

            // Replace dependeeToDependent 
            dependeeToDependent.Remove(s);
            dependeeToDependent.Add(s, values);

            // Replace dependentToDependee 
            for (int i = 0; i < dependents.Count; i++)
            {
                dependentToDependee.Remove(dependents[i]);
            }
            List<String> dependee = new List<String>();
            dependee.Add(s);
            foreach (String t in newDependents)
            {
                dependentToDependee.Add(t, dependee);
            }

            
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,s).  Then, for each 
        /// t in newDependees, adds the dependency (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            // Replacing in dependeeToDependent 
            List<String> dependees = new List<String>();
            dependentToDependee.TryGetValue(s, out dependees);

            for (int i = 0; i < dependees.Count; i++ )
            {
                dependeeToDependent.Remove(dependees[i]);
            }


            List<String> dependent = new List<String>();
            dependent.Add(s);
            foreach (String t in newDependees)
            {
                dependeeToDependent.Add(t, dependent);
            }

            // Replacing in dependentToDependee 
            List<String> values = new List<String>();
            foreach (String temp in newDependees)
            {
                values.Add(temp);
            }

            // Get values from t 
          //  List<String> dependee = new List<String>();
           // dependeeToDependent.TryGetValue(s, out dependee);

            // Replace dependeeToDependent 
            dependentToDependee.Remove(s);
            dependentToDependee.Add(s, values);
            
        }
    }
}
