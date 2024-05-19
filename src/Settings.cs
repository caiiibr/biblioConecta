using Biblioconecta.Data.Models;
using System.Text.Json;

namespace Biblioconecta
{
    public class Settings
    {
        public static Usuario? Usuario { get => GetSettings<Usuario>("current_user"); set => SetSettings("current_user", value); }

        public static void Clear()
        {
            Preferences.Clear();
        }

        private static T? GetSettings<T>(string key) where T : class
        {
            string setting = Preferences.Get(key, "");
            if (!string.IsNullOrEmpty(setting))
            {
                return JsonSerializer.Deserialize<T>(setting);
            }
            return null;
        }

        private static void SetSettings<T>(string key, T? value) where T : class
        {
            if (value == null)
            {
                Preferences.Remove(key);
            }
            else
            {
                Preferences.Set(key, JsonSerializer.Serialize(value));
            }
        }
    }
}
