using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace SmartContractEthereum.Domain.Utilities
{
    public static class DataAnnotationsColumnName
    {
        public static IList<string> GetPropertiesName(Type objectType)
        {
            IList<string> list = new List<string>();

            if (objectType == null)
                return list;

            PropertyInfo[] props = objectType.GetProperties();

            foreach (PropertyInfo prp in props)
            {
                var property = objectType.GetProperty(prp.Name);

                var columnName = (((Attribute[])(property.GetCustomAttributes())));

                if (columnName.Count() > 1)
                {
                    if (columnName[0] is DisplayAttribute)
                        list.Add(((DisplayAttribute)(columnName)[0]).Name);
                    else
                        list.Add(property.Name);
                }
            }
               

            return list;
        }

        public static IList<string> GetPropertiesExportName(Type objectType)
        {
            IList<string> list = new List<string>();

            if (objectType == null)
                return list;

            PropertyInfo[] props = objectType.GetProperties();

            foreach (PropertyInfo prp in props)
            {
                var property = objectType.GetProperty(prp.Name);

                var columnName = (((Attribute[])(property.GetCustomAttributes())));

                if (columnName.Count() > 0)
                {
                    if (columnName[0] is DisplayAttribute)
                        list.Add(((DisplayAttribute)(columnName)[0]).Name);
                    else
                        list.Add(property.Name);
                }
            }


            return list;
        }
    }
}
