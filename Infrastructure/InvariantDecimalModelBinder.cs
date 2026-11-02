using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace wt_lab4_4mvc_ostashevskaya.Infrastructure;

/// <summary>
/// Принимает десятичные числа и с точкой, и с запятой.
/// Пользователь может ввести цену в любом привычном формате, а браузер и текущая
/// культура могут использовать разные разделители, поэтому стандартного
/// связывания с одной культурой недостаточно.
/// </summary>
public sealed class InvariantDecimalModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (valueResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);
        var rawValue = valueResult.FirstValue;

        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return Task.CompletedTask;
        }

        if (decimal.TryParse(rawValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var value) ||
            decimal.TryParse(rawValue, NumberStyles.Number, CultureInfo.CurrentCulture, out value))
        {
            bindingContext.Result = ModelBindingResult.Success(value);
        }
        else
        {
            bindingContext.ModelState.TryAddModelError(
                bindingContext.ModelName,
                "Введите корректную цену, например 2490 или 2490,50");
        }

        return Task.CompletedTask;
    }
}

public sealed class InvariantDecimalModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(decimal) || context.Metadata.ModelType == typeof(decimal?))
        {
            return new InvariantDecimalModelBinder();
        }

        return null;
    }
}
