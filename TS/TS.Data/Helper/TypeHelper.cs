using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Interfaces;

namespace TS.Data.Helper
{
    public static class TypeHelper
    {
        public static Dictionary<string, Type> GetAssignableFromTypes<T, TAtr>(string searchPattern = "*")
            where T : class
            where TAtr : class, TypeAttribute
        {
            Dictionary<string, Type> typeDic = new Dictionary<string, Type>();

            var assemblys = Directory
               .GetFiles(AppDomain.CurrentDomain.BaseDirectory, searchPattern)
               .Select(file => Assembly.LoadFrom(file));

            // 获取所有继承自父类的类型
            Type baseType = typeof(T);
            foreach (var assembly in assemblys)
            {
                foreach (var type in assembly.GetExportedTypes().Where(t => baseType.IsAssignableFrom(t) && !baseType.Equals(t)))
                {
                    var attribute = type.GetCustomAttribute(typeof(TAtr)) as TAtr;
                    typeDic[attribute.GetType()] = type;
                }
            }

            return typeDic;
        }
    }
}
