using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using TypeScript.Syntax;

namespace TypeScript.Converter
{
    class FileUtil
    {
        private static readonly string DirectorySeparator = "/";
        private static readonly string BackSlash = "\\";
        private static readonly string TsJsonFileExtension = "*.ts.json";

        public static string GetFileExtension(Lang lang)
        {
            switch (lang)
            {
                case Lang.CSharp:
                    return ".cs";
                case Lang.Java:
                    return ".java";
                default:
                    throw new NotSupportedException("lang");
            }
        }

        public static string NormalizeName(Document doc, string name, Lang lang)
        {
            switch (lang)
            {
                case Lang.Java:
                    name = name.TrimStart('_');

                    Node publicType = doc.GetPublicTypeDeclaration();
                    if (publicType != null)
                    {
                        string typeName = publicType.GetName().TrimStart('_');
                        if (name != typeName)
                        {
                            name = typeName;
                        }
                    }
                    return name;

                default:
                    return name;
            }
        }

        public static List<string> GetTsJsonFiles(string input)
        {
            if (Directory.Exists(input))
            {
                return GetDirectoryFiles(input);
            }

            if (File.Exists(input))
            {
                return new List<string>() { Path.GetFullPath(input) };
            }

            if (HasWildcard(input))
            {
                return GetWildcardFiles(NormalizePath(input), null);
            }

            return null;
        }

        public static List<string> FilterFiles(List<string> files, List<string> excludes)
        {
            if (excludes == null || excludes.Count == 0)
            {
                return files;
            }

            List<string> ret = new List<string>();
            foreach (string file in files)
            {
                bool excluded = false;
                foreach (string exclude in excludes)
                {
                    if (IsMatch(file, exclude))
                    {
                        excluded = true;
                        break;
                    }
                }
                if (!excluded)
                {
                    ret.Add(file);
                }
            }
            return ret;
        }

        public static string GetBasePath(List<string> files)
        {
            string basePath = string.Empty;
            foreach (string path in files)
            {
                if (!Directory.Exists(path) && !File.Exists(path))
                {
                    return string.Empty;
                }

                if (string.IsNullOrEmpty(basePath))
                {
                    basePath = NormalizePath(Path.GetDirectoryName(path));
                }
                else
                {
                    string commonPath = GetCommonPath(basePath, NormalizePath(path));
                    if (string.IsNullOrEmpty(commonPath))
                    {
                        return string.Empty;
                    }
                    basePath = commonPath;
                }
            }
            return basePath;
        }

        public static string NormalizePath(string path)
        {
            return path.Replace(BackSlash, DirectorySeparator);
        }

        public static bool IsMatch(string path, string pattern)
        {
            path = NormalizePath(path);
            pattern = NormalizePath(pattern);

            return Regex.IsMatch(path, ToRegexPattern(pattern));
        }

        private static string GetCommonPath(string path1, string path2)
        {
            List<string> sameParts = new List<string>();

            string[] parts1 = path1.Split(DirectorySeparator);
            string[] parts2 = path2.Split(DirectorySeparator);
            int count = Math.Min(parts1.Length, parts2.Length);
            for (int i = 0; i < count; i++)
            {
                if (string.Compare(parts1[i], parts2[i], true) == 0)
                {
                    sameParts.Add(parts1[i]);
                }
            }

            return string.Join(DirectorySeparator, sameParts);
        }

        private static List<string> GetDirectoryFiles(string path, SearchOption searchOption = SearchOption.AllDirectories)
        {
            List<string> files = new List<string>();
            string[] fs = Directory.GetFiles(path, TsJsonFileExtension, searchOption);
            foreach (var f in fs)
            {
                files.Add(Path.GetFullPath(f));
            }
            return files;
        }

        private static List<string> GetWildcardFiles(string path, DirectoryInfo dirInfo)
        {
            string pathSegment;
            string nextPath;
            int separatorIndex = path.IndexOf(DirectorySeparator);
            if (separatorIndex == -1)
            {
                pathSegment = path;
                nextPath = string.Empty;
            }
            else
            {
                pathSegment = path.Substring(0, separatorIndex);
                nextPath = path.Substring(separatorIndex + 1);
            }

            //
            if (string.IsNullOrEmpty(pathSegment))
            {
                if (dirInfo != null)
                {
                    return GetDirectoryFiles(dirInfo.FullName);
                }
                return null;
            }

            //
            if (pathSegment == "**")
            {
                dirInfo = dirInfo == null ? Directory.CreateDirectory(".") : dirInfo;
                return GetAsteriskAsteriskFiles(nextPath, dirInfo);
            }

            //
            if (pathSegment == "..")
            {
                dirInfo = dirInfo == null ? Directory.CreateDirectory("..") : dirInfo.Parent;
                return GetWildcardFiles(nextPath, dirInfo);
            }

            //
            if (dirInfo == null)
            {
                if (Directory.Exists(pathSegment + DirectorySeparator))
                {
                    dirInfo = Directory.CreateDirectory(pathSegment + DirectorySeparator);
                    return GetWildcardFiles(nextPath, dirInfo);
                }
                return null;
            }

            //
            bool hasWildcard = HasWildcard(pathSegment);
            string pattern = ToRegexPattern(pathSegment);
            List<string> files = new List<string>();
            foreach (FileInfo fs in dirInfo.GetFiles(pathSegment, SearchOption.TopDirectoryOnly))
            {
                if ((hasWildcard && Regex.IsMatch(fs.Name, pattern, RegexOptions.IgnoreCase)) || (!hasWildcard && fs.Name == pattern))
                {
                    files.Add(fs.FullName);
                }
            }
            foreach (DirectoryInfo subDirInfo in dirInfo.GetDirectories())
            {
                if ((hasWildcard && Regex.IsMatch(subDirInfo.Name, pattern, RegexOptions.IgnoreCase)) || (!hasWildcard && subDirInfo.Name == pattern))
                {
                    files.AddRange(GetWildcardFiles(nextPath, subDirInfo));
                }
            }
            return files;
        }

        private static List<string> GetAsteriskAsteriskFiles(string path, DirectoryInfo dirInfo)
        {
            List<string> files = new List<string>();

            files.AddRange(GetWildcardFiles(path, dirInfo));
            foreach (DirectoryInfo subDirInfo in dirInfo.GetDirectories())
            {
                files.AddRange(GetAsteriskAsteriskFiles(path, subDirInfo));
            }

            return files;
        }

        private static bool HasWildcard(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            return path.IndexOfAny(new char[] { '*', '?' }) >= 0;
        }

        private static string ToRegexPattern(string path)
        {
            string pattern = string.Empty;
            foreach (string pathSegment in path.Split(DirectorySeparator))
            {
                if (!string.IsNullOrEmpty(pattern))
                {
                    pattern += DirectorySeparator;
                }

                if (pathSegment == "**")
                {
                    pattern += ".*";
                }
                else if (!string.IsNullOrEmpty(pathSegment))
                {
                    string segment = pathSegment
                        .Replace(".", "\\.")
                        .Replace("?", ".{1}");

                    // Process *
                    if (segment[0] == '*')
                    {
                        segment = ".*" + segment.Substring(1);
                    }
                    for (int index = segment.IndexOf('*', 0); index >= 0; index = segment.IndexOf('*', ++index))
                    {
                        char prev = segment[index - 1];
                        if (prev == '_'
                            || prev == '$'
                            || ('0' <= prev && prev <= '9')
                            || ('a' <= prev && prev <= 'z')
                            || ('A' <= prev && prev <= 'Z'))
                        {
                            segment = segment.Substring(0, index) + ".*" + segment.Substring(index + 1);
                            index++;
                        }
                    }
                    pattern += segment;
                }
            }

            return pattern;
        }
    }
}
