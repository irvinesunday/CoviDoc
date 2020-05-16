using Newtonsoft.Json.Converters;

namespace CoviDoc.Common
{
    public static class Helpers
    {
        public static IsoDateTimeConverter DateTimeConverter()
        {
            var format = "dd/MM/yyyy";
            return new IsoDateTimeConverter { DateTimeFormat = format };
        }
    }
}
