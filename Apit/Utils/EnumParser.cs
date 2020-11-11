using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Apit.Utils
{
    public static class EnumParser<TEnum> where TEnum : struct, Enum
    {
        public static IList<TEnum> GetValues()
        {
            return typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(fi => (TEnum) Enum.Parse(typeof(TEnum), fi.Name, false)).ToList();
        }

        public static TEnum Parse(string value)
        {
            return (TEnum) Enum.Parse(typeof(TEnum), value, true);
        }

        public static IEnumerable<string> GetNames()
        {
            return typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name);
        }

        public static IEnumerable<string> GetDisplayValues()
        {
            return GetNames().Select(obj => GetDisplayValue(Parse(obj)));
        }

        public static string GetDisplayValue(TEnum value)
        {
            var fieldInfo = typeof(TEnum).GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes?[0].ResourceType != null)
                return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }


        private static string LookupResource(IReflect resourceManagerProvider, string resourceKey)
        {
            foreach (var staticProperty in resourceManagerProvider.GetProperties(
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (staticProperty.PropertyType != typeof(ResourceManager)) continue;
                var resourceManager = (ResourceManager) staticProperty.GetValue(null, null);
                if (resourceManager != null) return resourceManager.GetString(resourceKey);
            }

            return resourceKey;
        }
    }
}