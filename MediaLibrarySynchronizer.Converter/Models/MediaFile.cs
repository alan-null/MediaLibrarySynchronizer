﻿using System;
using System.IO;

namespace MediaLibrarySynchronizer.Converter.Models
{
    public class MediaFile
    {
        private readonly FileInfo _fileInfo;

        public string Blob { get; set; }

        public string FilePath
        {
            get { return _fileInfo.FullName; }
        }

        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(_fileInfo.Name); }
        }

        public MediaFile(string filePath)
        {
            _fileInfo = new FileInfo(filePath);

            byte[] bytes = ReadFile(filePath);
            Blob = Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
        }

        private static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }
    }
}
