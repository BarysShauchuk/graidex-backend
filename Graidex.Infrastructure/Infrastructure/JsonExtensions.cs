using Graidex.Domain.Models.Questions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Graidex.Infrastructure.Infrastructure
{   
    /// <summary>
    /// JsonExtensions class for the application.
    /// </summary>
    internal static class JsonExtensions
    {   
        /// <summary>
        /// Creates a converter for serializing the object to json and deserializing json back to the object.
        /// </summary>
        /// <typeparam name="T">Type of object, for which the converter is created.</typeparam>
        /// <returns>ValueConverter, converting the object to json and back to the object.</returns>
        public static ValueConverter<T, string> CreateJsonConverter<T>() where T : new()
        {
            var jsonOptions = default(JsonSerializerOptions?);

            return new ValueConverter<T, string>(
                    option => JsonSerializer.Serialize(option, jsonOptions),
                    option => JsonSerializer.Deserialize<T>(option, jsonOptions) ?? new T()
                    );
        }

        /// <summary>
        /// Creates a converter for serializing the object to json and deserializing json back to the object.
        /// </summary>
        /// <typeparam name="T">Type of object, for which the converter is created.</typeparam>
        /// <param name="defaultValue">The default value assigned if the result of deserializing is null.</param>
        /// <returns>ValueConverter, converting the object to json and back to the object.</returns>
        public static ValueConverter<T, string> CreateJsonConverter<T>(T defaultValue)
        {
            var jsonOptions = default(JsonSerializerOptions?);

            return new ValueConverter<T, string>(
                    option => JsonSerializer.Serialize(option, jsonOptions),
                    option => JsonSerializer.Deserialize<T>(option, jsonOptions) ?? defaultValue
                    );
        }
    }
}
