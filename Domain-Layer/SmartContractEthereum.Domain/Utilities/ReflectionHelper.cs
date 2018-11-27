using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace SmartContractEthereum.Domain.Utilities
{
    public static class ReflectionHelper
    {
        public static void ValidateFields(object entity)
        {
            foreach (PropertyInfo pi in entity.GetType().GetProperties())
            {
                if (pi.PropertyType.Name == "IList`1")
                {
                    foreach (PropertyInfo property in pi.GetType().GetProperties())
                        ValidateFields(property);
                }
                else if (!pi.PropertyType.IsPrimitive)
                {
                    foreach (PropertyInfo entityProperty in entity.GetType().GetProperties())
                        if (!pi.PropertyType.IsPrimitive)
                            ValidateFields(entityProperty);
                }
            }
        }

        public static bool IsAllNullOrEmpty(object entity)
        {
            bool result = false;

            if (entity == null)
                return false;

            foreach (PropertyInfo property in entity.GetType().GetProperties())
            {
                if (result == true)
                    break;

                if (property.PropertyType.Name == "IList`1")
                {
                    Type type = property.PropertyType;

                    foreach (Type interfaceType in type.GetInterfaces())
                    {
                        if (interfaceType.IsGenericType && (interfaceType.GetGenericTypeDefinition() == typeof(IList<>) || interfaceType.GetGenericTypeDefinition() == typeof(ICollection<>)))
                        {
                            var list = (IEnumerable)property.GetValue(entity, null);

                            if (list != null)
                                foreach (var item in list)
                                    result = IsAllNullOrEmpty(item);
                        }
                    }
                }
                else if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string) || property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(decimal) || property.PropertyType.IsValueType)
                    result = IsNotNullOrEmpty(property, entity);

                else
                    result = IsAllNullOrEmpty(property.GetValue(entity, null));
            }

            return result;
        }

        private static bool IsNotNullOrEmpty(PropertyInfo property, object entity)
        {
            object value = property.GetValue(entity, null);

            return value != null;
        }
    }
}
