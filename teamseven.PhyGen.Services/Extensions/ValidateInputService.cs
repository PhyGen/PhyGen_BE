using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace teamseven.PhyGen.Services.Extensions
{
    internal static class ValidateInputService
    {
        /// <summary>
        /// Kiểm tra xem chuỗi có rỗng hay không.
        /// </summary>
        /// <param name="input">Chuỗi cần kiểm tra.</param>
        /// <returns>True nếu chuỗi không rỗng, ngược lại False.</returns>
        public static bool IsNotEmpty(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// Kiểm tra xem số nguyên có lớn hơn một giá trị nhất định hay không.
        /// </summary>
        /// <param name="number">Số nguyên cần kiểm tra.</param>
        /// <param name="compareValue">Giá trị so sánh.</param>
        /// <returns>True nếu số nguyên lớn hơn giá trị so sánh, ngược lại False.</returns>
        public static bool IsGreaterThan(int number, int compareValue)
        {
            return number > compareValue;
        }

        /// <summary>
        /// Kiểm tra xem chuỗi có phải là email hợp lệ hay không.
        /// </summary>
        /// <param name="email">Chuỗi email cần kiểm tra.</param>
        /// <returns>True nếu là email hợp lệ, ngược lại False.</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Biểu thức chính quy để kiểm tra định dạng email
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        /// <summary>
        /// Kiểm tra xem chuỗi có phải là số điện thoại hợp lệ hay không.
        /// </summary>
        /// <param name="phoneNumber">Chuỗi số điện thoại cần kiểm tra.</param>
        /// <returns>True nếu là số điện thoại hợp lệ, ngược lại False.</returns>
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Biểu thức chính quy để kiểm tra định dạng số điện thoại (10-12 chữ số)
            string phonePattern = @"^\d{10,12}$";
            return Regex.IsMatch(phoneNumber, phonePattern);
        }


        /// <summary>
        /// Kiểm tra các thuộc tính công khai của một object và trả về danh sách tên các thuộc tính có giá trị null.
        /// </summary>
        /// <param name="obj">Object cần kiểm tra.</param>
        /// <returns>Danh sách tên các thuộc tính có giá trị null. Nếu không có thuộc tính nào null, trả về danh sách rỗng.</returns>
        public static List<string> GetNullProperties(object obj)
        {
            if (obj == null)
                return new List<string> { "Object is null" };

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties
                .Where(prop => prop.GetValue(obj) == null)
                .Select(prop => prop.Name)
                .ToList();
        }

        /// <summary>
        /// Kiểm tra xem tất cả thuộc tính công khai của một object có null hay không.
        /// </summary>
        /// <param name="obj">Object cần kiểm tra.</param>
        /// <returns>True nếu tất cả thuộc tính không null, False nếu có ít nhất một thuộc tính null.</returns>
        public static bool ArePropertiesNotNull(object obj)
        {
            if (obj == null)
                return false;

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties.All(prop => prop.GetValue(obj) != null);
        }
    }
}