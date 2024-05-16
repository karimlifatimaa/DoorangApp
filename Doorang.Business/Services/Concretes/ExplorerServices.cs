using Doorang.Business.Exceptions;
using Doorang.Business.Services.Abstacts;
using Doorang.Core.Models;
using Doorang.Core.RepositoryAbstacts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doorang.Business.Services.Concretes;

public class ExplorerServices : IExplorerServices
{
    private readonly IExplorerRepository _explorerRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ExplorerServices(IExplorerRepository explorerRepository, IWebHostEnvironment webHostEnvironment)
    {
        _explorerRepository = explorerRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    public void AddExplorer(Explorer explorer)
    {

        if (explorer == null) 
            throw new NullReferenceException();
        if(explorer.PhotoFile == null) 
            throw new NullReferenceException();
        if (!explorer.PhotoFile.ContentType.Contains("image/")) 
            throw new FileContentException("PhotoFile","File content type error!!!") ;
        if (explorer.PhotoFile.Length > 2097152)
            throw new FileSizeException("PhotoFile", "File size error!!!");
        string path = _webHostEnvironment.WebRootPath + @"\Uploads\Explorer\" + explorer.PhotoFile.FileName;
        using(FileStream stream =new FileStream(path, FileMode.Create))
        {
            explorer.PhotoFile.CopyTo(stream);
        }
        explorer.ImageUrl = explorer.PhotoFile.FileName;
        _explorerRepository.Add(explorer);
        _explorerRepository.Commit();
    }

    public List<Explorer> GetAllExplorer(Func<Explorer, bool>? func = null)
    {
        return _explorerRepository.GetAll(func);
    }

    public Explorer GetExplorer(Func<Explorer, bool>? func = null)
    {
        return _explorerRepository.Get(func);
    }

    public void RemoveExplorer(int id)
    {
        var item = _explorerRepository.Get(x => x.Id == id);
        if (item == null) throw new NullReferenceException();
        string path = _webHostEnvironment.WebRootPath + @"\Uploads\Explorer\" + item.ImageUrl;
        if (!File.Exists(path)) throw new FileNameNotFoundException("ImageUrl","File not found");
        File.Delete(path);
        _explorerRepository.Delete(item);
        _explorerRepository.Commit();

    }

    public void UpdateExplorer(int id, Explorer explorer)
    {
        var item = _explorerRepository.Get(x => x.Id == id);
        if (item == null) throw new NullReferenceException();
        if (explorer.PhotoFile != null)
        {
            if (!explorer.PhotoFile.ContentType.Contains("image/"))
                throw new FileContentException("PhotoFile", "File content type error!!!");
            if (explorer.PhotoFile.Length > 2097152)
                throw new FileSizeException("PhotoFile", "File size error!!!");
            string path = _webHostEnvironment.WebRootPath + @"\Uploads\Explorer\" + explorer.PhotoFile.FileName;
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                explorer.PhotoFile.CopyTo(stream);
            }
            item.ImageUrl = explorer.PhotoFile.FileName;
        }
        item.Title= explorer.Title;
        item.Description= explorer.Description;
        item.Subtitle= explorer.Subtitle;
        
        _explorerRepository.Commit();
    }
}
