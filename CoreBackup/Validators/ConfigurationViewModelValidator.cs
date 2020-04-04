using System;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
// https://fluentvalidation.net/
using CoreBackup.ViewModels;

namespace CoreBackup.Validators
{
    public class ConfigurationViewModelValidator : AbstractValidator<ConfigurationViewModel>
    {
    }
}