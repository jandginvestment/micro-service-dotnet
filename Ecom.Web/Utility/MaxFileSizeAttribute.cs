﻿using System.ComponentModel.DataAnnotations;

namespace ECOM.Web.Utility;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;
    public MaxFileSizeAttribute(int maxFileSize) { _maxFileSize = maxFileSize; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var file = value as IFormFile; if (file == null) { return null; }
        else
        {
            if (file.Length > (_maxFileSize * 2048 * 2048)) { return new ValidationResult($"the maximum allowed file size is {_maxFileSize}MB"); }
        }
        return ValidationResult.Success;
    }
}
