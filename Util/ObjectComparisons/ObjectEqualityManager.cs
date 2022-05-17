using System.Reflection;

namespace Mercury.Unification.Util.ObjectComparisons
{
    public static class ObjectEqualityManager
    {
        public static bool PropertiesAreEqual(object A, object B)
        {
            if (A != null && B != null)
            {
                foreach (PropertyInfo AInfo in A.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    foreach (PropertyInfo BInfo in B.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (AInfo.Name == BInfo.Name && AInfo.PropertyType == BInfo.PropertyType && AInfo.Name != "Id")
                        {
                            object? AValue = AInfo.GetValue(A, null);
                            object? BValue = BInfo.GetValue(B, null);
                            if (AValue != null && BValue != null && !AValue.Equals(BValue))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
