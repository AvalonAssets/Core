namespace AvalonAssets.Core.Log
{
    public interface IPrefixLogger
    {
        int CalculatePrefixLenght(LogLevel logLevel, string tag);
    }
}