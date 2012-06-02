namespace MTP.Controllers
{
    using System;

    public static class StringUtils
    {
        public static string ToStringWithDbNullCheck(this object str)
        {
            if (str is DBNull || str == null)
            {
                return null;
            }

            return str.ToString();
        }

        public static string DateToShortWithDbNullCheck(this DateTime? date)
        {
            if (date == null)
            {
                return null;
            }

            return date.Value.ToShortDateString();
        }
    }
}