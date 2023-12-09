
using WebUI.Models;

namespace WebUI.Services;

public class FileService
{
    public List<FileModel> GetFiles()
    {
        List<FileModel> files = [];

       

        string[] aviDosyalari = Directory.GetFiles("videos", "*.avi");
        string[] pngDosyalari = Directory.GetFiles("videos", "*.png");

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
        string folderPath = "videos";
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
}