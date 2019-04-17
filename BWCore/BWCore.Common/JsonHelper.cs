using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BWCore.Common
{
    public static class JsonHelper
    {
        static JsonHelper()
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            jsonPropsIgnoreSetting = new JsonSerializerSettings
            {
                ContractResolver = new JsonPropsIgnoreContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            jsonPropsIgnoreSetting.Converters.Add(timeFormat);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, jsonPropsIgnoreSetting);
        }
        private static JsonSerializerSettings jsonPropsIgnoreSetting;
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj)
        {
            if (obj == null)
            {
                return "{}";
            }
            return JsonConvert.SerializeObject(obj, jsonPropsIgnoreSetting);
        }
        /// <summary>
        /// 复制对象属性，可用于复制对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CloneObject<T>(object obj)
        {
            return DeserializeObject<T>(SerializeObject(obj));
        }
        /// <summary>
        /// 复制对象属性，可用于复制对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T2> CloneList<T, T2>(List<T> list)
        {
            List<T2> result = new List<T2>();
            foreach (var item in list)
            {
                result.Add(DeserializeObject<T2>(SerializeObject(item)));
            }
            return result;
        }
        /// <summary>
        /// 复制对象属性，可用于复制对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ObservableCollection<T2> CloneListToObservableCollection<T, T2>(List<T> list)
        {
            ObservableCollection<T2> result = new ObservableCollection<T2>();
            foreach (var item in list)
            {
                result.Add(DeserializeObject<T2>(SerializeObject(item)));
            }
            return result;
        }

    }
}
