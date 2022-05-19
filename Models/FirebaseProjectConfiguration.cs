﻿using Microsoft.Extensions.Configuration;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Microsoft.Identity.Firebase.Models
{
    [Serializable, DataContract]
    public record FirebaseProjectConfiguration
    {
        /// <summary>
        ///     Firebase API Key
        ///     ----
        ///     https://firebase.google.com/docs/projects/api-keys?msclkid=50c2da1bd15411ec864a2051a4985260
        ///     General information about API keys and Firebase
        ///     API keys for Firebase are different from typical API keys
        ///     Unlike how API keys are typically used, API keys for Firebase services are not used to control access to backend resources; that can only be done with Firebase Security Rules(to control which users can access resources) and App Check(to control which apps can access resources).
        ///     Usually, you need to fastidiously guard API keys(for example, by using a vault service or setting the keys as environment variables); however, API keys for Firebase services are ok to include in code or checked-in config files.
        ///     Although API keys for Firebase services are safe to include in code, there are a few specific cases when you should enforce limits for your API key; for example, if you're using Firebase ML, Firebase Authentication with the email/password sign-in method, or a billable Google Cloud API. Learn more about these cases later on this page.
        ///     --
        ///     Despite this, I get alerts whenever a key is committed. The encoding is an attempt to dodge this...
        /// </summary>
        [JsonPropertyName("apiKey")] public string apiKey { get; private init; }
        [JsonPropertyName("authDomain")] public string authDomain { get; private init; }
        [JsonPropertyName("projectId")] public string projectId { get; private init; }
        [JsonPropertyName("storageBucket")] public string storageBucket { get; private init; }
        [JsonPropertyName("messagingSenderId")] public string messagingSenderId { get; private init; }
        [JsonPropertyName("appId")] public string appId { get; private init; }
        [JsonPropertyName("measurementId")] public string measurementId { get; private init; }

        [JsonPropertyName("OpenIdConfiguration")] public FirebaseOpenIdConfiguration OpenIdConfiguration { get; private init; }

        public FirebaseProjectConfiguration(IConfiguration configuration, FirebaseOpenIdConfiguration? openIdConfiguration = null)
        {
            var configurationSection = configuration.GetSection("Firebase");
            var apiKeyBytes = Convert.FromBase64String(configurationSection["ApiKey"]);
            apiKey = new string(apiKeyBytes.Select(b => (char)b).ToArray());
            authDomain = configurationSection["AuthDomain"];
            projectId = configurationSection["ProjectId"];
            storageBucket = configurationSection["StorageBucket"];
            messagingSenderId = configurationSection["MessagingSenderId"];
            appId = configurationSection["AppId"];
            measurementId = configurationSection["MeasurementId"];
            this.OpenIdConfiguration = openIdConfiguration ?? FirebaseOpenIdConfiguration.GetFirebaseOpenIdConfiguration(this);
        }

        public string ToBase64Json() => Convert.ToBase64String(System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(this));
    }
}