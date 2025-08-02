namespace Bookistry.API.Abstractions.Consts;

public static class DefaultRoles
{
    public partial class Author
    {
        public const string Name = nameof(Author);
        public const string Id = "868826A7-5589-4BF0-82DA-5E04408ADC8F";
        public const string ConcurrencyStamp = "13071EF4-9B9D-4594-804F-1E8650DA4417";
    }
    public partial class Reader
    {
        public const string Name = nameof(Reader);
        public const string Id = "4D447E8A-B35A-4DAE-BCE3-4552BF828693";
        public const string ConcurrencyStamp = "E9FD0D85-6770-4A99-B3A2-69158B9EF3D7";
    }
    public partial class Developer //TODO : Remove this role in the future Developer is not a role in Bookistry
    {
        public const string Name = nameof(Developer);
        public const string Id = "8757DDE1-DA74-4A92-9EEB-46C4A35AC090";
        public const string ConcurrencyStamp = "F167EA47-FC22-4A47-81F9-1E21C11DB217";
    }
}
