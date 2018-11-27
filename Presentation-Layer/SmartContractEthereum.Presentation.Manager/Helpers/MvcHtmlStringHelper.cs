
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SmartContractEthereum.Presentation.Manager.Helpers
{
    public static class MvcHtmlStringHelper
    {
        public static MvcHtmlString ActionLinkWithColumnOrder(this HtmlHelper helper, string columnName, string action, string currentColumn, bool? currentOrder, int? page, int? status)
        {
            object routeValues;
            object htmlAttributes = null;

            if (columnName == currentColumn)
            {
                routeValues = new { sortOrder = columnName, asc = !currentOrder, status = status, page = page };
                htmlAttributes = new { @class = (currentOrder.HasValue ? (currentOrder.Value ? "sort_asc" : "sort_desc") : "sort_asc") };
            }
            else
                routeValues = new { sortOrder = columnName };

            return helper.ActionLink(columnName, action, routeValues, htmlAttributes);
        }

        public static MvcHtmlString CustomDropDownList(this HtmlHelper htmlHelper, string optionLabel, IList<SelectListItem> list, object htmlAttributes) 
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();

            foreach (var item in list)
            {
                selectList.Add(new SelectListItem()
                {
                    Value = item.Value,
                    //Text = ((StatusEnum)Enum.Parse(typeof(StatusEnum), item.Value)).ToDescriptionString()
                });
            }

            if (!string.IsNullOrEmpty(optionLabel))
                selectList.Insert(0, new SelectListItem { Text = optionLabel, });

            return htmlHelper.DropDownList(optionLabel, selectList, htmlAttributes);
        }

        public static string GetDisplayName(this Enum enumeration)
        {
            Type enumType = enumeration.GetType();

            string enumName = Enum.GetName(enumType, enumeration);
            string displayName = enumName;

            try
            {
                MemberInfo member = enumType.GetMember(enumName)[0];

                object[] attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
                DisplayAttribute attribute = (DisplayAttribute)attributes[0];
                displayName = attribute.Name;

                if (attribute.ResourceType != null)
                    displayName = attribute.GetName();
            }
            catch { }

            return displayName;
        }
    }
}