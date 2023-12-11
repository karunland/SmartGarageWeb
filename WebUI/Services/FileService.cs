
using System.Text.RegularExpressions;
using WebUI.Models;

namespace WebUI.Services;

public class FileService
{
    public List<FileModel> GetFiles()
    {
        List<FileModel> files = [];

        string[] aviDosyalari = Directory.GetFiles("Media", "*.avi");
        string[] pngDosyalari = Directory.GetFiles("Media", "*.png");

        foreach (string dosyaAdi in aviDosyalari)
        {
            FileModel fileModel = new()
            {
                Name = Path.GetFileNameWithoutExtension(dosyaAdi),
                Extension = ".avi"
            };

            files.Add(fileModel);
        }

        foreach (string dosyaAdi in pngDosyalari)
        {
            FileModel fileModel = new()
            {
                Name = Path.GetFileNameWithoutExtension(dosyaAdi),
                Extension = ".png"
            };

            files.Add(fileModel);
        }

        return files;
    }

    public string GetNewVideoFileName()
    {
        string folderPath = "Media";
        string[] files = Directory.GetFiles(folderPath, "*.avi");

        if (files.Length == 0)
        {
            return "video0.avi";
        }

        int maxNumber = 0;

        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string numberString = new string(fileName.Where(char.IsDigit).ToArray());

            if (int.TryParse(numberString, out int number))
            {
                if (number > maxNumber)
                {
                    maxNumber = number;
                }
            }
        }

        string newFileName = $"video{maxNumber + 1}.avi";

        return newFileName;
    }

    public string GetNewPhotoFileName()
    {
        string folderPath = "Media";
        string[] files = Directory.GetFiles(folderPath, "*.png");

        if (files.Length == 0)
        {
            return "photo0.png";
        }

        int maxNumber = 0;

        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string numberString = new string(fileName.Where(char.IsDigit).ToArray());

            if (int.TryParse(numberString, out int number))
            {
                if (number > maxNumber)
                {
                    maxNumber = number;
                }
            }
        }

        string newFileName = $"photo{maxNumber + 1}.png";

        return newFileName;
    }


    public string GetLatestPhoto()
    {
        string folderPath = "Media";
        string[] files = Directory.GetFiles(folderPath, "*.png");

        if (files.Length == 0)
        {
            return "photo0.png";
        }

        int maxNumber = 0;

        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string numberString = new string(fileName.Where(char.IsDigit).ToArray());

            if (int.TryParse(numberString, out int number))
            {
                if (number > maxNumber)
                {
                    maxNumber = number;
                }
            }
        }

        return $"photo{maxNumber}";
    }
}