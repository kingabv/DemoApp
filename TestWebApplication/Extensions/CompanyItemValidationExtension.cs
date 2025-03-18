using System.ComponentModel.DataAnnotations;
using TestWebApplication.Models;

namespace TestWebApplication.Extensions
{
    internal static class CompanyItemValidationExtension
    {
        /// <summary>
        ///	Validates a model decorated with System.ComponentModel.DataAnnotations attributes
        /// </summary>
        /// <returns>Validation errors</returns>
        internal static IEnumerable<ValidationResult> ValidateControllerModel<T>(this T? model) where T : class
        {
            if (model == null)
                return new List<ValidationResult> { new ValidationResult($"Cannot validate {nameof(model)}; argument is null") };

            var context = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(model, context, validationResults, true);

            return validationResults;
        }

        /// <summary>
        /// Validates the CompanyItem model
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Validation errors</returns>
        internal static IEnumerable<ValidationResult> ValidateCompanyItemModel(this CompanyItem model)
        {
            var validationResults = new List<ValidationResult>();
            validationResults.AddRange(model.ValidateControllerModel());

            if (model !=null && model.Isin != null  && (!Char.IsLetter(model.Isin[0]) || !Char.IsLetter(model.Isin[1])))
                validationResults.Add(new ValidationResult("The company Item must have an Isin field starting with two letters"));
            
            return validationResults;
        }
    }
}
