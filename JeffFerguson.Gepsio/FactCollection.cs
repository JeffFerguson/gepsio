using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A collection of <see cref="Fact"/> objects.
    /// </summary>
    public class FactCollection : List<Fact>
    {
        private Dictionary<string, List<Fact>> thisFactNameDictionary;
        private Dictionary<string, List<Fact>> thisFactIdDictionary;

        /// <summary>
        /// Create a new, empty collection.
        /// </summary>
        public FactCollection()
        {
            thisFactNameDictionary = new Dictionary<string, List<Fact>>();
            thisFactIdDictionary = new Dictionary<string, List<Fact>>();
        }

        /// <summary>
        /// Adds a <see cref="Fact"/> object to the collection.
        /// </summary>
        /// <param name="factToAdd">
        /// A reference to the fact to be added.
        /// </param>
        public new void Add(Fact factToAdd)
        {
            AddFactToDictionary(factToAdd, factToAdd.Name, thisFactNameDictionary);
            AddFactToDictionary(factToAdd, factToAdd.Id, thisFactIdDictionary);
            base.Add(factToAdd);
        }

        /// <summary>
        /// Gets a reference to a fact given the fact's name.
        /// </summary>
        /// <remarks>
        /// If the loaded XBRL document contains more than one fact with the given
        /// name, then only one of the facts will be returned. To get a list of
        /// all facts with the given name, then use the <see cref="GetFactsByName"/> method.
        /// </remarks>
        /// <param name="factName">
        /// The name of the fact whose fact reference is to be returned.
        /// </param>
        /// <returns>
        /// A reference to the fact with the given name, or null if no fact with
        /// the given name can be found.
        /// </returns>
        public Fact GetFactByName(string factName)
        {
            List<Fact> existingList;

            thisFactNameDictionary.TryGetValue(factName, out existingList);
            if (existingList == null)
                return null;
            return existingList[0];
        }

        /// <summary>
        /// Gets a list of all facts with the given name.
        /// </summary>
        /// <remarks>
        /// The returned list will be read only, and no modifications can be
        /// made to the list itself.
        /// </remarks>
        /// <param name="factName">
        /// The name of the fact whose fact references are to be returned.
        /// </param>
        /// <returns>
        /// A list of all facts with the given name, or null if no fact with
        /// the given name can be found.
        /// </returns>
        public ReadOnlyCollection<Fact> GetFactsByName(string factName)
        {
            List<Fact> existingList;

            thisFactNameDictionary.TryGetValue(factName, out existingList);
            if (existingList == null)
                return null;
            return existingList.AsReadOnly();
        }

        /// <summary>
        /// Gets a reference to a fact given the fact's ID.
        /// </summary>
        /// <remarks>
        /// If the loaded XBRL document contains more than one fact with the given
        /// ID, then only one of the facts will be returned. To get a list of
        /// all facts with the given IF, then use the <see cref="GetFactsById"/> method.
        /// </remarks>
        /// <param name="factId">
        /// The ID of the fact whose fact reference is to be returned.
        /// </param>
        /// <returns>
        /// A reference to the fact with the given ID, or null if no fact with
        /// the given ID can be found.
        /// </returns>
        public Fact GetFactById(string factId)
        {
            List<Fact> existingList;

            thisFactIdDictionary.TryGetValue(factId, out existingList);
            if (existingList == null)
                return null;
            return existingList[0];
        }

        /// <summary>
        /// Gets a list of all facts with the given ID.
        /// </summary>
        /// <remarks>
        /// The returned list will be read only, and no modifications can be
        /// made to the list itself.
        /// </remarks>
        /// <param name="factId">
        /// The ID of the fact whose fact references are to be returned.
        /// </param>
        /// <returns>
        /// A list of all facts with the given ID, or null if no fact with
        /// the given ID can be found.
        /// </returns>
        public ReadOnlyCollection<Fact> GetFactsById(string factId)
        {
            List<Fact> existingList;

            thisFactNameDictionary.TryGetValue(factId, out existingList);
            if (existingList == null)
                return null;
            return existingList.AsReadOnly();
        }


        private void AddFactToDictionary(Fact fact, string key, Dictionary<string, List<Fact>> dictionary)
        {
            if(dictionary.ContainsKey(key) == false)
            {
                var newList = new List<Fact>();
                newList.Add(fact);
                dictionary.Add(key, newList);
            }
            else
            {
                List<Fact> existingList;
                
                dictionary.TryGetValue(key, out existingList);
                existingList.Add(fact);
            }
        }
    }
}
