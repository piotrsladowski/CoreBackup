using System;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
// https://fluentvalidation.net/
using CoreBackup.ViewModels;
using CoreBackup.ViewModels.ConfigurationViewModels;

namespace CoreBackup.Validators
{
    class FTPConfViewModelValidator : AbstractValidator<FTPConfViewModel>
    {
        public FTPConfViewModelValidator()
        {
            RuleFor(ConfigurationViewModel => ConfigurationViewModel.ServerInput)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("IP {PropertyName} can not be empty")
                .Length(7, 15).WithMessage("Length ({TotalLength}) of IP {PropertyName} Invalid")
                .Must(BeAValidIpAddress).WithMessage("{PropertyName} is Invalid IPv4 Address");

            RuleFor(ConfigurationViewModel => ConfigurationViewModel.UsernameInput)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} can not be empty")
                .Length(2, 20).WithMessage("Length ({TotalLength}) of {PropertyName} Invalid")
                .Must(BeAValidUserName).WithMessage("{PropertyName} contains Invalid characters");

            RuleFor(ConfigurationViewModel => ConfigurationViewModel.PasswordInput)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} can not be empty")
                .Length(2, 20).WithMessage("Length ({TotalLength}) of {PropertyName} Invalid");

        }

        protected bool BeAValidUserName(string username)
        {
            username = username.Replace(" ", "");
            username = username.Replace("-", "");
            // All -> check if every character in username fulfill IsLetterOrDigit
            return username.All(Char.IsLetterOrDigit);
        }

        protected bool BeAValidIpAddress(string ipInput)
        {
            Match match = Regex.Match(ipInput, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
}
}
