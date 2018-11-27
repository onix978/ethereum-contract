using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace SmartContractEthereum.Presentation.Manager.Helpers
{
    public static class ReflectionHelper
    {
        public static bool IsAnyNullOrEmpty(this object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);

                    if (string.IsNullOrEmpty(value))
                        return true;
                }
                else if (pi.PropertyType.IsEnum)
                {
                    var value = pi.GetValue(myObject);

                    if (string.IsNullOrEmpty(value.ToString()))
                        return true;
                }
            }

            return false;
        }

        public static bool IsAllNullOrEmpty(this object myObject)
        {
            bool result = true;

            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);

                    if (!string.IsNullOrEmpty(value) && (value.All(char.IsDigit) && Convert.ToInt64(value) > 0))
                        result = false;
                }
                else if (pi.PropertyType.IsEnum)
                {
                    var value = pi.GetValue(myObject);

                    if (!string.IsNullOrEmpty(value.ToString()))
                        result = false;
                }
            }

            return result;
        }

        public static bool IsAnyNotNullOrEmpty(this object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);

                    if (!string.IsNullOrEmpty(value))
                        return true;
                }
                else if(pi.PropertyType == typeof(int))
                {
                    var value = pi.GetValue(myObject);

                    if (Convert.ToInt32(value) != 0)
                        return true;
                }
                else if (pi.PropertyType.IsEnum)
                {
                    var value = pi.GetValue(myObject);

                    if (!string.IsNullOrEmpty(value.ToString()) && Convert.ToInt32(value) != 0)
                        return true;
                }
            }

            return false;
        }

        public static IDictionary<string, string> GetNameAndDisplayNameProperty(this object myObject)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            PropertyInfo[] properties = typeof(object).GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                object[] attrs = prop.GetCustomAttributes(true);

                foreach (object attr in attrs)
                {
                    DisplayNameAttribute displayName = attr as DisplayNameAttribute;

                    if (displayName != null)
                        result.Add(prop.Name, displayName.DisplayName); 
                }
            }

            return result;
        }

        public static string ReturnNamePropertyByDisplayName(Type type, string displayNameValue)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                object[] attrs = prop.GetCustomAttributes(true);

                foreach (object attr in attrs)
                {
                    DisplayAttribute displayName = attr as DisplayAttribute;

                    if (displayName != null)
                        result.Add(prop.Name, displayName.Name);
                }
            }

            return result.Where(x => x.Value == displayNameValue).FirstOrDefault().Key;
        }
    }
}