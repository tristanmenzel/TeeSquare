using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace TeeSquare.Tests
{
    public static class Utilty
    {

        public static void CreateFileIfNotExists([CallerFilePath] string filePath = null,
            [CallerMemberName] string memberName = null)
        {
            var fileName = new Regex("\\.[a-zA-Z0-9]{1,3}$").Replace(filePath, $".{memberName}.txt");
            using (File.Open(fileName, FileMode.OpenOrCreate))
            {
                
            }
        }
    }
}