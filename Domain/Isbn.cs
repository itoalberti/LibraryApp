// namespace LibraryApp.Domain
// {
//     public struct Isbn
//     {
//         public string Value { get; }

//         public Isbn(string value)
//         {
//             if (string.IsNullOrWhiteSpace(value) || !value.Trim().All(char.IsDigit))
//                 throw new ArgumentException("🚫 Invalid ISBN 🚫");
//             Value = value;
//         }

//         public static Isbn Parse(string value) => new Isbn(value);

//         public override string ToString() => Value;
//     }
// }
