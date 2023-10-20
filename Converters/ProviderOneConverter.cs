using ApiTestTask.Services.SearchDataProviderServices;
using System.ComponentModel;
using System.Globalization;
namespace ApiTestTask.Converters;

public class RequestOneTypeConverter : TypeConverter {
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) {
        return sourceType == typeof(ProviderOneSearchRequest);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) {
        return destinationType == typeof(ProviderRequest);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value) {
        if (value == null) { return null; }
        if (!CanConvertFrom(value.GetType()) || value is not ProviderRequest request) {
            throw new NotImplementedException();
        }
        return new ProviderOneSearchRequest() {
            DateFrom = request.DateFrom,
            DateTo = request.DateTo,
            From = request.From,
            To = request.To,
            MaxPrice = request.Filter.MaxPrice
        };
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) {
        if (value == null) { return null; }
        if (!CanConvertTo(destinationType) || value is not ProviderOneSearchRequest request) {
            throw new NotImplementedException();
        }
        return new ProviderRequest() {
            DateFrom = request.DateFrom,
            DateTo = request.DateTo,
            From = request.From,
            To = request.To,
            Filter = new() { 
                MaxPrice = request.MaxPrice 
            }
        };
    }
}

public class ResponseOneTypeConverter {
}
