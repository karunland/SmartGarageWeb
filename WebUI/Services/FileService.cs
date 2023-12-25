
using System.Text.RegularExpressions;
using WebUI.Models;

namespace WebUI.Services;

public class FileService
{
    public List<FileModel> GetFiles()
    {
        List<FileModel> files = [];

        string[] videos = Directory.GetFiles("Media", "*.avi");
        string[] pictures = Directory.GetFiles("Media", "*.png");

        foreach (string picture in pictures)
        {
            FileModel fileModel = new()
            {
                Name = Path.GetFileNameWithoutExtension(picture),
                Extension = ".png",
                FullName = $"{Path.GetFileNameWithoutExtension(picture)}.png",
                CreatedDate = File.GetCreationTime(picture)
            };

            files.Add(fileModel);
        }

        foreach (string video in videos)
        {
            FileModel fileModel = new()
            {
                Name = Path.GetFileNameWithoutExtension(video),
                Extension = ".avi",
                FullName = $"{Path.GetFileNameWithoutExtension(video)}.avi",
                CreatedDate = File.GetCreationTime(video)
            };

            files.Add(fileModel);
        }
        files = files.OrderByDescending(f => int.Parse(Regex.Match(f.Name, @"\d+").Value)).ToList();

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
            return "photo0";
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

    public ApiResult DeleteFile(string fileName)
    {
        string folderPath = "Media";
        string filePath = Path.Combine(folderPath, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return new ApiResult
            {
                IsSuccess = true,
                Message = "Dosya silindi"
            };
        }
        else
        {
            return new ApiResult
            {
                IsSuccess = false,
                Message = "Dosya bulunamadı"
            };
        }
    }
}