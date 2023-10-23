using ApiTestTask.Services.SearchDataProviderServices;
using ApiTestTask.Services.SearchServices;
using System.ComponentModel;
using System.Globalization;

namespace ApiTestTask.Converters
{
    public class RequestConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return destinationType == typeof(ProviderRequest);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(SearchRequest);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value == null) { return null; }
            if (!CanConvertTo(context, destinationType) || value is not ProviderRequest providerRequest) 
            { 
                throw new NotImplementedException(); 
            }
            return new SearchRequest()
            {
                Origin = providerRequest.From,
                OriginDateTime = providerRequest.DateFrom,
                Destination = providerRequest.To,
                Filters = new()
                {
                    DestinationDateTime = providerRequest.Filter.DestinationDateTime,
                    MaxPrice = providerRequest.Filter.MaxPrice,
                    MinTimeLimit = providerRequest.Filter.MinTimeLimit,
                    OnlyCached = false
                }
            };
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value == null) { return null; }
            if (!CanConvertFrom(context, value.GetType()) || value is not SearchRequest searchRequest) {  
                throw new NotImplementedException(); 
            }
            return new ProviderRequest()
            {
                From = searchRequest.Origin,
                DateFrom = searchRequest.OriginDateTime,
                To = searchRequest.Destination,
                Filter = new()
                {
                    DestinationDateTime = searchRequest.Filters?.DestinationDateTime,
                    MaxPrice = searchRequest.Filters?.MaxPrice,
                    MinTimeLimit = searchRequest.Filters?.MinTimeLimit
                }
            };
        }
    }
}
