﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Quick.Properties.Utils
{
    public class PathUtils
    {
        private static bool hasSearchChar(String searchPath)
        {
            return searchPath.Contains("*") | searchPath.Contains("?");
        }

        public static string GetFullPath(string path)
        {
            path = path.Replace('/', Path.DirectorySeparatorChar);
            path = path.Replace('\\', Path.DirectorySeparatorChar);
            var twoDirectorySeparatorCharString = Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString();
            while (path.Contains(twoDirectorySeparatorCharString))
                path = path.Replace(twoDirectorySeparatorCharString, Path.DirectorySeparatorChar.ToString());
            return path;
        }

        /// <summary>
        /// 搜索文件
        /// </summary>
        /// <param name="searchPath"></param>
        /// <returns></returns>
        public static IEnumerable<String> SearchFile(String searchPath)
        {
            searchPath = GetFullPath(searchPath);
            if (hasSearchChar(searchPath))
            {
                var searchFileName = Path.GetFileName(searchPath);
                var searchBaseFolder = Path.GetDirectoryName(searchPath);
                var baseFolders = SearchFolder(searchBaseFolder);
                List<String> list = new List<string>();
                foreach (var baseFolder in baseFolders)
                {
                    list.AddRange(Directory.EnumerateFiles(baseFolder, searchFileName));
                }
                return list;
            }
            else
            {
                FileInfo file = new FileInfo(searchPath);
                if (file.Exists)
                    return new String[] { file.FullName };
            }
            return new String[0];
        }

        /// <summary>
        /// 搜索目录
        /// </summary>
        /// <param name="searchPath"></param>
        /// <returns></returns>
        public static IEnumerable<String> SearchFolder(String searchPath)
        {
            if (String.IsNullOrEmpty(searchPath))
                return new String[] { Directory.GetCurrentDirectory() };
            searchPath = GetFullPath(searchPath);

            if (hasSearchChar(searchPath))
            {
                var baseFolder = Path.GetDirectoryName(searchPath);
                var folderName = searchPath.Substring(baseFolder.Length);
                while (folderName.StartsWith(Path.DirectorySeparatorChar.ToString()))
                    folderName = folderName.Substring(1);
                IEnumerable<String> parentFolders = SearchFolder(baseFolder);
                List<String> list = new List<string>();
                foreach (var parentFolder in parentFolders)
                    list.AddRange(Directory.EnumerateDirectories(parentFolder, folderName));
                return list;
            }
            else
            {
                DirectoryInfo folder = new DirectoryInfo(searchPath);
                if (folder.Exists)
                    return new String[] { folder.FullName };
            }
            return new String[0];
        }
    }
}
