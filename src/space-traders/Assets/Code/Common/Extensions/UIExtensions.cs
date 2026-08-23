namespace Assets.Code.Common.Extensions
{
    public static class UIExtensions
    {
        public static string ToMoney(this long money)
        {
            return (money / 100m).ToString("N2");
        }
    }
}
