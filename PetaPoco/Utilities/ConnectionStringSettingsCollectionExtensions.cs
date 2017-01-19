using System.Configuration;

namespace PetaPoco.Utilities
{
    public static class ConnectionStringSettingsCollectionExtensions
    {
        /// <summary>
        /// Returns if is empty or not.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>bool</returns>
        public static bool IsEmpty(this ConnectionStringSettingsCollection collection)
        {
            return collection.Count == 0;
        }
        /// <summary>
        /// Returns the first elemento of the collection
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>ConnectionStringSettings</returns>
        public static ConnectionStringSettings First(this ConnectionStringSettingsCollection collection)
        {
            return collection[0];
        }
    }
}
