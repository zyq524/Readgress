using System;
using System.IO.IsolatedStorage;

namespace Readgress.WP8.Utils
{
    public class StorageSettings
    {
        IsolatedStorageSettings isolatedStore;

        // The isolated storage key names of our settings
        const string FbAccessTokenKeyName = "FBAccessToken";
        const string FbAccessTokenExpiresKeyName = "FBAccessTokenExpires";

        // The default value of our settings
        const string FbAccessTokenDefault = "";
        DateTime FbAccessTokenExpiresDefault = DateTime.Now.AddDays(-30); //Datetime value that already expired. This is default.

        ///
        /// Constructor that gets the application settings.
        public StorageSettings()
        {
            // Get the settings for this application.
            isolatedStore = IsolatedStorageSettings.ApplicationSettings;
        }

        ///
        /// Update a setting value for our application. If the setting does not exist, then add the setting.
        ///
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (isolatedStore.Contains(Key))
            {
                // If the value has changed
                if (isolatedStore[Key] != value)
                {
                    // Store the new value
                    isolatedStore[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                isolatedStore.Add(Key, value);
                valueChanged = true;
            }

            return valueChanged;
        }

        ///
        /// Get the current value of the setting, or if it is not found, set the setting to the default setting. /// 
        ///
        public object GetValueOrDefault(string Key, object defaultValue)
        {
            object value;

            // If the key exists, retrieve the value.
            if (isolatedStore.Contains(Key))
            {
                value = isolatedStore[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }

            return value;
        }

        ///
        /// Save the settings.
        public void Save()
        {
            isolatedStore.Save();
        }

        ///
        /// Property to get and set Facebook AccessToken 
        public string FacebookAccessToken
        {
            get
            {
                var token = GetValueOrDefault(FbAccessTokenKeyName, FbAccessTokenDefault);
                return token == null ? string.Empty : token.ToString();
            }
            set
            {
                AddOrUpdateValue(FbAccessTokenKeyName, value);
                Save();
            }
        }

        ///
 /// Property to get and set Facebook Access Token Expires value /// 


        public DateTime FacebookAccessTokenExpires
        {
            get
            {
                return (DateTime)GetValueOrDefault(FbAccessTokenExpiresKeyName, FbAccessTokenExpiresDefault);
            }
            set
            {
                AddOrUpdateValue(FbAccessTokenExpiresKeyName, value);
                Save();
            }
        }

    }
}
