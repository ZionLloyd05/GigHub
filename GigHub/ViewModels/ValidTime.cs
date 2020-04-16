using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.ViewModels
{
    public class ValidTime : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime;
            var isValid = DateTime.TryParse(Convert.ToString(value), out dateTime);

            return (isValid);
        }
    }
}