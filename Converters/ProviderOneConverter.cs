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
            DateTo = request.Filter.DestinationDateTime,
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
            From = request.From,
            To = request.To,
            Filter = new() { 
                MaxPrice = request.MaxPrice,
                DestinationDateTime = request.DateTo
            }
        };
    }
}

public class ResponseOneTypeConverter : TypeConverter {
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) {
        return sourceType == typeof(ProviderOneSearchResponse);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) {
        return destinationType == typeof(ProviderResponse);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) {
        if (value == null) { return null; }
        if (!CanConvertTo(destinationType) || value is not ProviderOneSearchResponse response) {
            throw new NotImplementedException();
        }
        var resultRoutes = new Services.SearchServices.Route[response.Routes.Length];
        for (int i = 0; i < response.Routes.Length; i++)
        {
            resultRoutes[i] = new Services.SearchServices.Route()
            {
              Id = new(),
              OriginDateTime = response.Routes[i].DateFrom,
              Origin = response.Routes[i].From,
              DestinationDateTime = response.Routes[i].DateTo,
              Destination = response.Routes[i].To,
              Price = response.Routes[i].Price,
              TimeLimit = response.Routes[i].TimeLimit
            };
        }
        return new ProviderResponse(resultRoutes);
    }
}
