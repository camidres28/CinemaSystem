using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CinemaSystem.Models.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string propertyName = bindingContext.ModelName;
            ValueProviderResult value = bindingContext.ValueProvider.GetValue(propertyName);
            if (value == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {
                T deserializedObject = JsonConvert.DeserializeObject<T>(value.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(deserializedObject);
            }
            catch (Exception)
            {
                bindingContext.ModelState.TryAddModelError(propertyName, $"Invalis value for type {typeof(T).GetType()}");
            }

            return Task.CompletedTask;
        }
    }
}
