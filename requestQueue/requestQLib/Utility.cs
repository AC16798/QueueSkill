using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace RequestQ.requestQLib
{
    public enum QueueType
    {
        [Description("Request Queue")]
        RequestQueue = 0,

        [Description("File Process Queue")]
        FileProcessQueue = 1,

        [Description("Print Batch Queue")]
        PrintBatchQueue = 2,

        [Description("File Validation")]
        ValidationQueue = 3,
    
         [Description("Queue Output")]
        QueueOut = 4
    }

    public static class Utility
    {
        public static string GetDescriptionFromEnumValue(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }
    }
}
