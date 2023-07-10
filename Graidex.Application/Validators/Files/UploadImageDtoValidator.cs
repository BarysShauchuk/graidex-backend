using FluentValidation;
using Graidex.Application.DTOs.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Validators.Files
{
    public class UploadImageDtoValidator : AbstractValidator<UploadImageDto>
    {
        public UploadImageDtoValidator()
        {
            string[] allowedExtensions = { ".png", ".jpg", ".jpeg" };

            RuleFor(x => x.FileName)
                .NotEmpty()
                    .WithMessage("File name cannot be empty.")
                .Must(fileName => allowedExtensions.Any(fileName.EndsWith))
                    .WithMessage($"File should have one of the following extensions: \"{
                        string.Join("\", \"", allowedExtensions)}\".");

            RuleFor(x => x.Stream)
                .Must(Stream => Stream.Length > 0)
                    .WithMessage("File cannot be empty.")
                .Must(Stream => Stream.Length < 2 * 1024 * 1024)
                    .WithMessage("File cannot be larger than 2MB.");
        }
    }
}
