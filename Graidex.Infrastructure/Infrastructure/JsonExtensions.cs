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
    internal static class JsonExtensions
    {
        public static ValueConverter<T, string> CreateJsonConverter<T>() where T : new()
        {
            var jsonOptions = default(JsonSerializerOptions?);

            return new ValueConverter<T, string>(
                    option => JsonSerializer.Serialize(option, jsonOptions),
                    option => JsonSerializer.Deserialize<T>(option, jsonOptions) ?? new T()
                    );
        }

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
