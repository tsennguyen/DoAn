using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System;

namespace Shopping_Laptop.Repository
{
    public static class SessionExtensions
    {
        // Lưu đối tượng vào session dưới dạng JSON
        public static void SetJson(this ISession session, string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                session.Remove(key);
                return;
            }

            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Lấy đối tượng từ session và deserialize thành kiểu dữ liệu mong muốn
        public static T? GetJson<T>(this ISession session, string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                session.Remove(key);
                return;
            }

            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
