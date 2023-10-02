namespace Controller.Components.ComboController
{
    public static class IDGenerator
    {
        private static int _id;

        public static int Get()
        {
            return _id++;
        }
    }
}