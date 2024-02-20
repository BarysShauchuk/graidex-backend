using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Extensions
{   
    public static class UserValidationExtensions
    {   
        /// <summary>
        /// An extension method to add rules for user password.
        /// </summary>
        /// <returns>Rule set for user password.</returns>
        public static IRuleBuilderOptions<T, string> UserPasswordRules<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .Length(8, 16).WithMessage("Password length must be from 8 to 16 symbols")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
                .Matches(@"[\!\?\*\.\$]+").WithMessage("Password must contain at least one of [!?*.$].");
        }

        /// <summary>
        /// An extension method to add rules for user name.
        /// </summary>
        /// <returns>Rule set for user name.</returns>
        public static IRuleBuilderOptions<T, string> UserNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .Length(1, 50)
                .Matches(@"^[\p{L}\- ]+$").WithMessage("This field must contain only letters, dashes or space symbols.")
                .Matches(@"[\p{L}]+").WithMessage("This field must contain at least one letter.");
        }

        /// <summary>
        /// An extension method to add rules for user email.
        /// </summary>
        /// <returns>Rule set for user email.</returns>
        public static IRuleBuilderOptions<T, string> UserEmailRules<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .EmailAddress();
        }

        /// <summary>
        /// An extension method to add rules for user custom id.
        /// </summary>
        /// <returns>Rule set for user custom id</returns>
        public static IRuleBuilderOptions<T, string?> UserCustomIdRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .Length(0, 15);
        }
    }
}
