using ApiTestTask.Services.SearchDataProviderServices;
using ApiTestTask.Services.SearchServices;
using System.ComponentModel;
using System.Globalization;

namespace ApiTestTask.Converters;

public class RequestTwoTypeConverter : TypeConverter {
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) {
        return sourceType == typeof(ProviderTwoSearchRequest);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) {
        return destinationType == typeof(ProviderRequest);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value) {
        if (value == null) { return null; }
        if (!CanConvertFrom(value.GetType()) || value is not ProviderRequest request) {
            throw new NotImplementedException();
        }
        return new ProviderTwoSearchRequest() {
            DepartureDate = request.DateFrom,
            Departure = request.From,
            Arrival = request.To,
            MinTimeLimit = request.Filter.MinTimeLimit
        };
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) {
        if (value == null) { return null; }
        if (!CanConvertTo(destinationType) || value is not ProviderTwoSearchRequest request) {
            throw new NotImplementedException();
        }
        return new ProviderRequest() {
            DateFrom = request.DepartureDate,
            From = request.Departure,
            To = request.Arrival,
            Filter = new() { 
                MinTimeLimit = request.MinTimeLimit,
                DestinationDateTime = null
            }
        };
    }
}

public class ResponseTwoTypeConverter : TypeConverter {

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) {
        return sourceType == typeof(ProviderTwoSearchResponse);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) {
        return destinationType == typeof(ProviderResponse);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) {
        if (value == null) { return null; }
        if (!CanConvertTo(destinationType) || value is not ProviderTwoSearchResponse response) {
            throw new NotImplementedException();
        }
        var resultRoutes = new Services.SearchServices.Route[response.Routes.Length];
        for (int i = 0; i < response.Routes.Length; i++) {
            resultRoutes[i] = new Services.SearchServices.Route()
            {
                OriginDateTime = response.Routes[i].Departure.Date, 
                Origin = response.Routes[i].Departure.Point,
                DestinationDateTime = response.Routes[i].Arrival.Date, 
                Destination = response.Routes[i].Arrival.Point,
                Price = response.Routes[i].Price,
                TimeLimit = response.Routes[i].TimeLimit
            };
        }
        return new ProviderResponse(resultRoutes);
    }

}
