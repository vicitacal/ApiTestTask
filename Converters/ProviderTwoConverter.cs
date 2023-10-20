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
            DateTo = null,
            From = request.Departure,
            To = request.Arrival,
            Filter = new() { MinTimeLimit = request.MinTimeLimit }
        };
    }
}

public class ResponseTwoTypeConverter : TypeConverter {

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) {
        return sourceType == typeof(ResponseTwoTypeConverter);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) {
        return destinationType == typeof(ProviderRequest);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) {
        if (value == null) { return null; }
        if (!CanConvertTo(destinationType) || value is not ProviderTwoSearchResponse response) {
            throw new NotImplementedException();
        }
        var resultRoutes = new ProviderRoute[response.Routes.Length];
        for (int i = 0; i < response.Routes.Length; i++) {
            resultRoutes[i] = new ProviderRoute(
                new RoutePoint() { Date = response.Routes[i].Departure.Date, Point = response.Routes[i].Departure.Point}, 
                new RoutePoint() { Date = response.Routes[i].Arrival.Date, Point = response.Routes[i].Arrival.Point},
                response.Routes[i].Price,
                response.Routes[i].TimeLimit
            );
        }
        return new ProviderResponse(resultRoutes);
    }

}
