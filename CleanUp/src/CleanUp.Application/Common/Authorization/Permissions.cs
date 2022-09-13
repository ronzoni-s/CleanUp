using System.Reflection;

namespace CleanUp.Application.Common.Authorization
{
    public static class Permissions
    { 
        public static class User
        {
            public const string View = "permissions.user.view";
            public const string Manage = "permissions.user.manage";
        }

        public static class Event
        {
            public const string View = "permissions.event.view";
            public const string Manage = "permissions.event.manage";
        }

        /// <summary>
        /// Returns a list of Permissions.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRegisteredPermissions()
        {
            var permssions = new List<string>();
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    permssions.Add(propertyValue.ToString());
            }
            return permssions;
        }
    }
}
