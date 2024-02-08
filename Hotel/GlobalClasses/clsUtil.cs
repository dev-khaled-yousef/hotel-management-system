﻿using Hotel.GlobalClasses;
using System;
using System.IO;

namespace CarRental.GlobalClasses
{
    public class clsUtil
    {
        public enum enSourceImage { Person, Item };

        public static string GenerateGUID()
        {

            // Generate a new GUID
            Guid newGuid = Guid.NewGuid();

            // convert the GUID to a string
            return newGuid.ToString();

        }

        public static bool CreateFolderIfDoesNotExist(string FolderPath)
        {

            // Check if the folder exists
            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    // If it doesn't exist, create the folder
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch (Exception ex)
                {
                    clsLogError.LogError("General Exception", ex);
                    return false;
                }
            }

            return true;

        }

        public static string ReplaceFileNameWithGUID(string sourceFile)
        {
            // Full file name. Change your file name   
            string fileName = sourceFile;
            FileInfo fi = new FileInfo(fileName);
            string extn = fi.Extension;
            return GenerateGUID() + extn;

        }

        public static bool CopyImageToProjectImagesFolder(ref string sourceFile, enSourceImage SourceImage = enSourceImage.Person)
        {
            // this function will copy the image to the
            // project images folder after renaming it
            // with GUID with the same extension, then it will update the sourceFileName with the new name.

            string DestinationFolder = (SourceImage == enSourceImage.Person) ? @"D:\hotel-people-images\" : @"D:\hotel-Items-images\";

            if (!CreateFolderIfDoesNotExist(DestinationFolder))
            {
                return false;
            }

            string destinationFile = DestinationFolder + ReplaceFileNameWithGUID(sourceFile);
            try
            {
                File.Copy(sourceFile, destinationFile, true);

            }
            catch (IOException iox)
            {
                clsLogError.LogError("IO Exception", iox);
                return false;
            }
            catch (Exception ex)
            {
                clsLogError.LogError("General Exception", ex);
                return false;
            }

            sourceFile = destinationFile;
            return true;
        }
    }
}
