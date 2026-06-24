using System.Globalization;


namespace Assets.Code.ClientPart.Gameplay.Features.Navigation
{
    public static class DistanceFormat
    {
        private const double MetersPerKm = 1_000d;
        public const double MetersPerAu = 149_600_000_000d;
        private const double MetersToKmThreshold = 5_000d;
        private const double KmToAuThreshold = 14_960_000_000d;

        private static readonly NumberFormatInfo _format = new()
        {
            NumberGroupSeparator = " ",
            NumberDecimalSeparator = ".",
            NumberGroupSizes = new[] { 3 },
        };

        public static string Format(double meters)
        {
            if (meters < MetersToKmThreshold)
                return $"{meters.ToString("N0", _format)} m";

            if (meters < KmToAuThreshold)
                return $"{(meters / MetersPerKm).ToString("N0", _format)} km";

            return $"{FormatAu(meters / MetersPerAu)} au";
        }

        private static string FormatAu(double au)
        {
            return au < 10d
                ? au.ToString("N1", _format)
                : au.ToString("N0", _format);
        }
    }
}
