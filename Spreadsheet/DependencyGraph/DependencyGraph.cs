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
        // Instance variable for holding the two dictionaries that will make up the dependency graph 
        // dependeeToDependent stores dependencies in the form (s, r)
        Dictionary<String, HashSet<String>> dependeeToDependent;
        // dependentToDependee stores dependencies in the form (r, s)
        Dictionary<String, HashSet<String>> dependentToDependee;
        // Size counter for keeping track of how big the DG is
        int size = 0;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            // Initialize dictionaries taking parameters (string, HashSet<String>)
            // A Dictionary represents the dependency between the string and it's dependents which are stored in the HashSet 
            dependeeToDependent = new Dictionary<String, HashSet<String>>();
            dependentToDependee = new Dictionary<String, HashSet<String>>();
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            // Count the number of entries in dependentToDependee dictionary 
            // get { return dependentToDependee.Count; }
            get { return size; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            // If the key s exists in dependeeToDependent dictionary, 
            // s will have dependents so return true 
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
            // If the key s exists in dependentToDependee dictionary, 
            // s will have dependees so return true 
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
            HashSet<String> dependents;
            // If the TryGetValue fails, there are no dependents, therefore return empty list 
            // If TryGetValue passes, return the output of that value 
            if (!dependeeToDependent.TryGetValue(s, out dependents))
            {
                HashSet<String> emptyDependents = new HashSet<String>();
                return emptyDependents;
            }
            return dependents;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            HashSet<String> dependees;
            // If the TryGetValue fails, there are no dependees, therefore return empty list 
            // If TryGetValue passes, return the output of that value 
            if (!dependentToDependee.TryGetValue(s, out dependees))
            {
                HashSet<String> emptyDependees = new HashSet<String>();
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
            // If dependee s already exists
            if (dependeeToDependent.ContainsKey(s))
            {
                // If dependency is already in the  graph, return without adding  
                HashSet<String> dependents = new HashSet<String>();
                dependeeToDependent.TryGetValue(s, out dependents);
                for (int i = 0; i < dependents.Count; i++)
                {
                    if (dependents.Contains(t))
                    {
                        return;
                    }
                }
                dependeeToDependent[s].Add(t);
                // Increment size 
                size++;
            }
            // If dependee s doesn't exist already
            else
            {
                HashSet<String> temp = new HashSet<String>();
                temp.Add(t);
                dependeeToDependent.Add(s, temp);
                // Increment size 
                size++;
            }

            // Add dependentToDependee 
            // If dependee t already exists 
            if (dependentToDependee.ContainsKey(t))
            {
                dependentToDependee[t].Add(s);
            }
            // If dependee t doesn't already exist 
            else
            {
                HashSet<String> temp = new HashSet<String>();
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
            // Remove dependeeToDependent
            HashSet<String> dependents;
            // If the dependency does not exist, do nothing 
            if (dependeeToDependent.TryGetValue(s, out dependents))
            {
                // If dependee s only has one dependent, remove it from the graph 
                if (dependents.Count - 1 == 0)
                {
                    dependeeToDependent.Remove(s);
                    // Decrement Size 
                    size--;
                }
                // If dependee s has more than one dependent, remove the dependent from the list 
                // and readd the list 
                else
                {
                    dependents.Remove(t);
                    dependeeToDependent.Remove(s);
                    dependeeToDependent.Add(s, dependents);
                    // Decrement Size 
                    size--;
                }
            }

            // Remove dependentToDependee
            HashSet<String> dependees;
            // If the dependency does not exist, do nothing 
            if (dependentToDependee.TryGetValue(t, out dependees))
            {
                // If dependee t only has one dependent, remove it from the graph 
                if (dependees.Count - 1 == 0)
                {
                    dependentToDependee.Remove(t);
                }
                // If dependent t has more than one dependee, remove the dependent from the list 
                // and readd the list 
                else
                {
                    dependees.Remove(s);
                    dependentToDependee.Remove(t);
                    dependentToDependee.Add(t, dependees);
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            // Store newDependents in the newDepends HashSet 
            HashSet<String> newDepends = new HashSet<String>();
            foreach (String temp in newDependents)
            {
                newDepends.Add(temp);
            }

            // Store the dependents of s into toBeDeleted to make deleting from 
            // dependentToDependee easier 
            HashSet<String> toBeDeleted = new HashSet<String>();
            if (dependeeToDependent.TryGetValue(s, out toBeDeleted))
            {
                size -= toBeDeleted.Count;
                // Remove all dependencies from dependeeToDependent with dependee s
                dependeeToDependent.Remove(s);

                // Loop through and remove the dependencies from dependentToDependee 
                foreach (String temp in toBeDeleted)
                 {
                     HashSet<String> tempDependents = new HashSet<String>();
                     if (dependentToDependee.TryGetValue(temp, out tempDependents))
                     {
                         dependentToDependee.Remove(temp);
                         tempDependents.Remove(s);
                         dependentToDependee.Add(temp, tempDependents);
                         size--;
                     }
                }
            }

            // Add new dependencies to dependeeTodependent 
            dependeeToDependent.Add(s, newDepends);
            size += newDepends.Count;

            // Add new dependencies to dependentToDependee 
            HashSet<String> param = new HashSet<String>();
            param.Add(s);
            foreach (String temp in newDepends)
            {
                HashSet<String> exists = new HashSet<String>();
                // If the key is already in the dictionary, add the value to the HashSet 
                if (dependentToDependee.TryGetValue(temp, out exists))
                {
                    dependentToDependee[temp].Add(s);
                    size++;
                }
                else
                {
                    dependentToDependee.Add(temp, param);
                    size++;
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,s).  Then, for each 
        /// t in newDependees, adds the dependency (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            // Store newDependees in newDepees 
            HashSet<String> newDepees = new HashSet<String>();
            foreach (String temp in newDependees)
            {
                newDepees.Add(temp);
            }

            // Store the dependees of s into toBeDeleted to make deleting from 
            // dependeeToDependent easier 
            HashSet<String> toBeDeleted = new HashSet<String>();
            if (dependentToDependee.TryGetValue(s, out toBeDeleted))
            {
                size -= toBeDeleted.Count;
                // Remove s from dependentToDependee 
                dependentToDependee.Remove(s);

                // Loop through and remove dependecies from dependeeToDependent 
                foreach (String temp in toBeDeleted)
                {
                    HashSet<String> tempDependees = new HashSet<String>();
                    if (dependeeToDependent.TryGetValue(temp, out tempDependees))
                    {
                        dependeeToDependent.Remove(temp);
                        tempDependees.Remove(s);
                        dependeeToDependent.Add(temp, tempDependees);
                        size--;
                    }
                }
            }
            
            // Add the new dependencies to dependentToDependee 
            dependentToDependee.Add(s, newDepees);
            size += newDepees.Count;

            // Add the new dependencies to dependeeToDependent 
            HashSet<String> param = new HashSet<String>();
            param.Add(s);
            foreach (String temp in newDepees)
            {
                HashSet<String> exists = new HashSet<String>();
                // If the key already exists, add s to the value 
                if (dependeeToDependent.TryGetValue(temp, out exists))
                {
                    dependeeToDependent[temp].Add(s);
                    size++;
                }
                else
                {
                    dependeeToDependent.Add(temp, param);
                    size++;
                }
            }
        }
    }
} 
   

