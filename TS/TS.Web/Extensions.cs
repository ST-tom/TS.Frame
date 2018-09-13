using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TS.Web
{
    public static partial class Extensions
    {
        public static List<SelectListItem> EnumToSelectList<T>()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Type enumType = typeof(T);
            var names = Enum.GetNames(enumType);
            foreach (var name in names)
            {
                var field = enumType.GetField(name);
                string description = string.Empty;
                DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attr != null)
                {
                    description = attr.Description;   //属性描述
                }
                else
                {
                    description = name;  //描述不存在取字段名称
                }
                list.Add(new SelectListItem() { Value = name, Text = description });
            }
            return list;
        }

        public static void SetSelectedItem<T>(this List<SelectListItem> list, T value)
        {
            if (list == null)
                return;

            list.FirstOrDefault(e => e.Value == value.ToString()).Selected = true;
        }

        public static void AddAllItem<T>(this List<SelectListItem> list,bool disabled = false,bool selected = false)
        {
            if (list == null)
                return;

            list.Add(new SelectListItem()
            {
                Value = "All",
                Text = "所有",
                Disabled = disabled,
                Selected = selected,
            });
        }
    }
}