namespace AccuracyVASMinimalAPI.Documents
{
    public class DocumentManager
    {
        public byte[] HexToRgb(string hex)
        {
            if (hex.StartsWith("#"))
                hex = hex[1..];

            if (hex.Length != 6)
                throw new ArgumentException("El color hexadecimal no es válido.");

            return new byte[]
            {
        Convert.ToByte(hex.Substring(0, 2), 16),
        Convert.ToByte(hex.Substring(2, 2), 16),
        Convert.ToByte(hex.Substring(4, 2), 16)
            };
        }
    }
}
