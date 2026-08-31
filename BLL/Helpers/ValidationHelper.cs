using System.Text.RegularExpressions;

namespace BLL.Helpers
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Kiểm tra chuỗi rỗng
        /// </summary>
        public static void CheckRequired(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{fieldName} không được để trống.");
            }
        }

        /// <summary>
        /// Kiểm tra Email
        /// </summary>
        public static void CheckEmail(string email)
        {
            CheckRequired(email, "Email");

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (!Regex.IsMatch(email, pattern))
            {
                throw new ArgumentException("Email không đúng định dạng.");
            }
        }

        /// <summary>
        /// Kiểm tra số điện thoại
        /// </summary>
        public static void CheckPhone(string phone)
        {
            CheckRequired(phone, "Số điện thoại");

            string pattern = @"^(0|\+84)[0-9]{9}$";

            if (!Regex.IsMatch(phone, pattern))
            {
                throw new ArgumentException("Số điện thoại không hợp lệ.");
            }
        }

        /// <summary>
        /// Kiểm tra độ dài chuỗi
        /// </summary>
        public static void CheckLength(string value, string fieldName, int min, int max)
        {
            CheckRequired(value, fieldName);

            if (value.Length < min || value.Length > max)
            {
                throw new ArgumentException($"{fieldName} phải từ {min} đến {max} ký tự.");
            }
        }

        /// <summary>
        /// Kiểm tra số lớn hơn 0
        /// </summary>
        public static void CheckPositive(decimal value, string fieldName)
        {
            if (value <= 0)
            {
                throw new ArgumentException($"{fieldName} phải lớn hơn 0.");
            }
        }

        /// <summary>
        /// Kiểm tra số nguyên lớn hơn 0
        /// </summary>
        public static void CheckPositive(int value, string fieldName)
        {
            if (value <= 0)
            {
                throw new ArgumentException($"{fieldName} phải lớn hơn 0.");
            }
        }

        /// <summary>
        /// Kiểm tra ngày
        /// </summary>
        public static void CheckDate(DateTime date, string fieldName)
        {
            if (date == DateTime.MinValue)
            {
                throw new ArgumentException($"{fieldName} không hợp lệ.");
            }
        }

        /// <summary>
        /// Kiểm tra mật khẩu
        /// </summary>
        public static void CheckPassword(string password)
        {
            CheckRequired(password, "Mật khẩu");

            if (password.Length < 6)
            {
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự.");
            }
        }
    }
}