using System.ComponentModel;

namespace Company.Default.Domain.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            var type = enumerationValue.GetType();

            if (!type.IsEnum)
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));

            var memberInfo = type.GetMember(enumerationValue.ToString());

            if(memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                    return ((DescriptionAttribute)attributes[0]).Description;                    
            }

            return enumerationValue.ToString();
        }

        public static string GetLocalizedDescription<T>(this T enumerationValue) where T : struct
        {
            var fieldInfo = enumerationValue.GetType().GetField(enumerationValue.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length == null || attributes.Length == 0)
                return enumerationValue.ToString();

            return attributes[0].Description;
        }
    }
}
