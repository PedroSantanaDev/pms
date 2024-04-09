namespace pms.app.Enums
{
    public static class Status
    {
        public enum Statuses
        {
            Active,
            Innactive,
        }

        public static IEnumerable<string> GetStatusOptions()
        {
            return Enum.GetNames(typeof(Statuses));
        }
    }
}
