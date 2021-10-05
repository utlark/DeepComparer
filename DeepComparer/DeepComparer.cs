using System.Collections.Generic;
using System.Reflection;

namespace DeepComparer
{
    interface IComparer
    {
        IEnumerable<Difference> Compare<T>(T First, T Second);
    }

    public class Difference
    {
        public string Path { get; set; }
        public object First { get; set; }
        public object Second { get; set; }

        public Difference(string path, object first, object second)
        {
            Path = path;
            First = first;
            Second = second;
        }

        /// <summary>Converts the value of this instance to its equivalent string representation.</summary>
        /// <returns><see cref="Path"/>: <see cref="First"/> - <see cref="Second"/></returns>
        public override string ToString()
        {
            return $"{Path}: {First} - {Second}";
        }
    }

    public class DeepComparer : IComparer
    {
        /// <summary>Compares all public, readable properties of the specified object instances.</summary>
        /// <param name="First">The first object to compare.</param>
        /// <param name="Second">The second object to compare.</param>
        /// <returns><see cref="IEnumerable{Difference}"/> containing the path to the differing properties and values of the <paramref name="First"/> and <paramref name="Second"/> objects.</returns>
        public IEnumerable<Difference> Compare<T>(T First, T Second)
        {
            if (!Equals(First, Second) || First != null || Second != null)
            {
                PropertyInfo[] properties = First.GetType().GetProperties();
                List<Difference> differences = new List<Difference>(properties.Length);

                foreach (PropertyInfo propertie in properties)
                    if (propertie.CanRead && !Equals(propertie.GetValue(First), propertie.GetValue(Second)))
                        if (propertie.PropertyType.IsPrimitive || propertie.PropertyType.IsValueType || (propertie.PropertyType == typeof(string)))
                        {
                            differences.Add(new Difference(propertie.Name, propertie.GetValue(First), propertie.GetValue(Second)));
                        }
                        else
                        {
                            var temp = Compare(propertie.GetValue(First), propertie.GetValue(Second));
                            foreach (Difference item in temp)
                                item.Path = $"{propertie.Name}.{item.Path}";

                            differences.AddRange(temp);
                        }
                return differences;
            }
            return new List<Difference>();
        }
    }
}
