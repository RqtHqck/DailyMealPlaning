using System.Xml.Linq;

namespace WebApp.Services.Data;

public class XmlUserFileLoader : IDisposable
{
    private readonly string _xmlPath;
    private readonly ReaderWriterLockSlim _lock = new();
    private XDocument _document;

    public XDocument Document 
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _document ??= XDocument.Load(_xmlPath);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    public XmlUserFileLoader(IWebHostEnvironment env, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Имя файла обязательно", nameof(fileName));

        var appDataPath = Path.Combine(env.WebRootPath, "App_Data");
        Directory.CreateDirectory(appDataPath);
        
        _xmlPath = Path.Combine(appDataPath, fileName);
        InitializeFile();
    }

    private void InitializeFile()
    {
        if (!File.Exists(_xmlPath))
        {
            _lock.EnterWriteLock();
            try
            {
                new XDocument(new XElement("Users")).Save(_xmlPath);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }

    public void Save(XDocument doc)
    {
        _lock.EnterWriteLock();
        try
        {
            doc.Save(_xmlPath);
            _document = doc;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void Dispose() => _lock.Dispose();
}
