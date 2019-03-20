using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter
{
    class Utils
    {
        private static readonly string directorySeparator = "/";
        private static readonly string backSlash = "\\";
        private static readonly string tsJsonFileExtension = "*.ts.json";

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
                return GetWildcardFiles(NormalizeSlashes(input), null);
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
                string path = NormalizeSlashes(file);

                foreach (string exclude in excludes)
                {
                    string excludePattern = ToRegexPattern(NormalizeSlashes(exclude));
                    if (Regex.IsMatch(path, excludePattern))
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

        public static string GetBasePath(List<string> paths)
        {
            string basePath = string.Empty;
            foreach (string path in paths)
            {
                if (!Directory.Exists(path) && !File.Exists(path))
                {
                    return string.Empty;
                }

                string p = NormalizeSlashes(path);
                if (string.IsNullOrEmpty(basePath))
                {
                    basePath = p;
                }
                else
                {
                    string commonPath = GetCommonPath(basePath, p);
                    if (string.IsNullOrEmpty(commonPath))
                    {
                        return string.Empty;
                    }
                    basePath = commonPath;
                }
            }
            return basePath;
        }

        private static string GetCommonPath(string path1, string path2)
        {
            List<string> sameParts = new List<string>();

            string separator = directorySeparator;
            string[] parts1 = path1.Split(separator);
            string[] parts2 = path2.Split(separator);
            int count = Math.Min(parts1.Length, parts2.Length);
            for (int i = 0; i < count; i++)
            {
                if (string.Compare(parts1[i], parts2[i], true) == 0)
                {
                    sameParts.Add(parts1[i]);
                }
            }

            return string.Join(separator, sameParts);
        }

        private static List<string> GetDirectoryFiles(string path, SearchOption searchOption = SearchOption.AllDirectories)
        {
            List<string> files = new List<string>();
            string[] fs = Directory.GetFiles(path, tsJsonFileExtension, searchOption);
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
            int separatorIndex = path.IndexOf(directorySeparator);
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
                if (Directory.Exists(pathSegment + directorySeparator))
                {
                    dirInfo = Directory.CreateDirectory(pathSegment + directorySeparator);
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

        private static string NormalizeSlashes(string path)
        {
            return path.Replace(backSlash, directorySeparator);
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
            if (!HasWildcard(path))
            {
                return path;
            }

            string ret = string.Empty;
            string path2 = path.Replace(".", "\\.");
            foreach (string pathSegment in path2.Split(directorySeparator))
            {
                if (!string.IsNullOrEmpty(ret))
                {
                    ret += directorySeparator;
                }

                if (pathSegment == "**")
                {
                    ret += ".*";
                }
                else
                {
                    ret += pathSegment.Replace("*", ".*").Replace("?", ".{1}");
                }
            }
            return ret;
        }


    }
}
