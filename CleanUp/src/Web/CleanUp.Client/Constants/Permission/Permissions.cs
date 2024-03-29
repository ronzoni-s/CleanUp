﻿//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;

//namespace CleanUp.Client.Constants.Permission
//{
//    public static class Permissions
//    {
//        public static class Products
//        {
//            public const string View = "Permissions.Products.View";
//            public const string Create = "Permissions.Products.Create";
//            public const string Edit = "Permissions.Products.Edit";
//            public const string Delete = "Permissions.Products.Delete";
//            public const string Export = "Permissions.Products.Export";
//            public const string Search = "Permissions.Products.Search";
//        }
//        public static class Orders
//        {
//            public const string View = "Permissions.Orders.View";
//            public const string Create = "Permissions.Orders.Create";
//            public const string Edit = "Permissions.Orders.Edit";
//            public const string Delete = "Permissions.Orders.Delete";
//            public const string Export = "Permissions.Orders.Export";
//            public const string Search = "Permissions.Orders.Search";
//        }

//        public static class ReceiptEod
//        {
//            public const string View = "Permissions.ReceiptEod.View";
//            public const string Create = "Permissions.ReceiptEod.Create";
//        }

//        public static class Users
//        {
//            public const string View = "Permissions.Users.View";
//            public const string Create = "Permissions.Users.Create";
//            public const string Edit = "Permissions.Users.Edit";
//            public const string Delete = "Permissions.Users.Delete";
//            public const string Export = "Permissions.Users.Export";
//            public const string Search = "Permissions.Users.Search";
//        }

//       /// <summary>
//       /// Returns a list of Permissions.
//       /// </summary>
//       /// <returns></returns>
//        public static List<string> GetRegisteredPermissions()
//        {
//            var permssions = new List<string>();
//            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
//            {
//                var propertyValue = prop.GetValue(null);
//                if (propertyValue is not null)
//                    permssions.Add(propertyValue.ToString());
//            }
//            return permssions;
//        }
//    }
//}