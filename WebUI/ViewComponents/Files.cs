using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.ViewComponents;

public class Files : ViewComponent
{
    private readonly FileService _fileService;

    public Files(FileService fileService)
    {
        _fileService = fileService;
    }

    public IViewComponentResult Invoke()
    {
        List<FileModel> files = _fileService.GetFiles();

        return View(files);
    }
}
