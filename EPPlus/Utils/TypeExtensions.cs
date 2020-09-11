using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EPPlus.Utils
{
    internal static class TypeExtensions
    {
        /// <summary>
        ///     获取显示名
        /// </summary>
        /// <param name="customAttributeProvider"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static string GetDisplayName(this ICustomAttributeProvider customAttributeProvider, bool inherit = false)
        {
            var displayAttribute = customAttributeProvider.GetAttribute<DisplayAttribute>();
            string displayName;
            if (displayAttribute != null)
            {
                displayName = displayAttribute.Name;
            }
            else
            {
                displayName = customAttributeProvider.GetAttribute<DisplayNameAttribute>()?.DisplayName;
            }
            return displayName;
        }

        /// <summary>
        ///     获取程序集属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this ICustomAttributeProvider assembly, bool inherit = false)
            where T : Attribute
        {
            return assembly
                .GetCustomAttributes(typeof(T), inherit)
                .OfType<T>()
                .FirstOrDefault();
        }

        /// <summary>
        ///     获取类型描述
        /// </summary>
        /// <param name="customAttributeProvider"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static string GetDescription(this ICustomAttributeProvider customAttributeProvider, bool inherit = false)
        {
            var des = string.Empty;
            var desAttribute = customAttributeProvider.GetAttribute<DescriptionAttribute>();
            if (desAttribute != null) des = desAttribute.Description;
            return des;
        }

        /// <summary>
        ///     获取枚举定义列表
        /// </summary>
        /// <returns>返回枚举列表元组（名称、值、显示名、描述）</returns>
        public static IEnumerable<Tuple<string, string, string, string>> GetEnumDefinitionList(this Type type)
        {
            var list = new List<Tuple<string, string, string, string>>();
            var attrType = type;
            if (!attrType.IsEnum) return null;
            var names = Enum.GetNames(attrType);
            var values = Enum.GetValues(attrType);
            var index = 0;
            foreach (var value in values)
            {
                var name = names[index];
                var field = value.GetType().GetField(value.ToString());
                var displayName = field.GetDisplayName();
                var des = field.GetDescription();
                var item = new Tuple<string, string, string, string>(
                    name,
                    value.ToString(),
                    string.IsNullOrWhiteSpace(displayName) ? null : displayName,
                    string.IsNullOrWhiteSpace(des) ? null : des
                );
                list.Add(item);
                index++;
            }

            return list;
        }

        /// <summary>
        ///     获取枚举列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        ///     key :返回显示名称或者描述
        ///     value：值
        /// </returns>
        public static IDictionary<string, string> GetEnumTextAndValues(this Type type)
        {
            if (!type.IsEnum) throw new InvalidOperationException();
            var items = type.GetEnumDefinitionList();
            var dic = new Dictionary<string, string>();
            //枚举名 值 显示名称 描述
            foreach (var tuple in items)
            {
                //如果描述、显示名不存在，则返回枚举名称
                dic.Add(tuple.Item4 ?? tuple.Item3 ?? tuple.Item1, tuple.Item2);
            }
            return dic;
        }

    }
}
