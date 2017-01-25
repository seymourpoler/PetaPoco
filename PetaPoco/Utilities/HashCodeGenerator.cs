namespace PetaPoco.Utilities
{
    internal class HashCodeGenerator
    {
        public static int Generate<T>(T[] keys)
        {
            var hashCodeResult = 17;
            foreach (var k in keys)
            {
                hashCodeResult = hashCodeResult * 23 + (k == null ? 0 : k.GetHashCode());
            }
            return hashCodeResult;
        }
    }
}
