using PdfSharp.Fonts;
using System.IO;

namespace SpaceXDashboard.Commands
{
    public class FileFontResolver : IFontResolver
    {
        public string DefaultFontName => "arial";

        public byte[] GetFont(string faceName)
        {
            var fontsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            var path = Path.Combine(fontsFolder, faceName);

            if (File.Exists(path))
                return File.ReadAllBytes(path);

            return File.ReadAllBytes(Path.Combine(fontsFolder, "arial.ttf"));
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (isBold && isItalic) return new FontResolverInfo("arialbi.ttf");
            if (isBold) return new FontResolverInfo("arialbd.ttf");
            if (isItalic) return new FontResolverInfo("ariali.ttf");
            return new FontResolverInfo("arial.ttf");
        }
    }
}