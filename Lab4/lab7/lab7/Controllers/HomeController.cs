using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class HomeController : Controller
{
    private static int GuestCounter = 0;
    private const string gmail = "vlad.lemeshok@gmail.com";
    private const string AccessKey = "1111"; // ключ доступа
    private const string SessionKey = "guest";  // ключ сессий

    private readonly AppDbContext context;

    public HomeController(AppDbContext dbContext)
    {
        context = dbContext;
    }

    private string GenerateGuestSessionId()
    {
        string sessionId;
        do
        {
            int guestNumber = Interlocked.Increment(ref GuestCounter); 
            sessionId = $"guest-{guestNumber}";
        }
        while (context.Comments.Any(c => c.SessionId == sessionId));

        return sessionId;
    }

    public IActionResult Index(string filterKeyword)
    {

        // Проверка и установка уникального идентификатора сессии
        var sessionId = HttpContext.Session.GetString(SessionKey);
        if (string.IsNullOrEmpty(sessionId))
        {
            sessionId = GenerateGuestSessionId();
            HttpContext.Session.SetString(SessionKey, sessionId);
        }

        if (User.IsInRole("Owner"))
        {
            ViewBag.Mode = "owner";
        }
        else
        {
            ViewBag.Mode = "guest";
        }
        var mode = HttpContext.Session.GetString(SessionKey) ?? "guest";  
        ViewBag.Mode = mode;

        // Получаем ссылки с возможностью фильтрации
        var links = string.IsNullOrEmpty(filterKeyword)
            ? context.Links.Include(l => l.Comments).ToList()
            : context.Links.Include(l => l.Comments).Where(l => l.Keywords.Contains(filterKeyword)).ToList();

        ViewBag.Links = links;
        ViewBag.FilterKeyword = filterKeyword;  // Передаем текущее значение фильтра в представление
        return View();
    }

    [HttpPost]
    public IActionResult SwitchMode(string accessKey,string email)
    {
        if (accessKey == AccessKey && email == gmail)
        {
            HttpContext.Session.SetString(SessionKey, "owner");
        }
        else
        {
            HttpContext.Session.SetString(SessionKey,GenerateGuestSessionId());
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult AddLink(string url, string keywords)
    {

        if (HttpContext.Session.GetString(SessionKey) == "owner")
        {
            var link = new LinkModel { Url = url, Keywords = keywords };
            context.Links.Add(link);
            context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DeleteLink(int id)
    {
        if (HttpContext.Session.GetString(SessionKey) == "owner")
        {
            var link = context.Links.Find(id);
            if (link != null)
            {
                context.Links.Remove(link);
                context.SaveChanges();
            }
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult EditLink(int id, string url, string keywords)
    {
        if (HttpContext.Session.GetString(SessionKey) == "owner" && !string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(keywords))
        {
            var link = context.Links.FirstOrDefault(l => l.Id == id);
            if (link != null)
            {
                link.Url = url;
                link.Keywords = keywords;
                context.SaveChanges();
            }
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult AddComment(int linkId, string commentText)
    {
        if (string.IsNullOrEmpty(commentText))
        {
            return RedirectToAction("Index");
        }

        var link = context.Links.FirstOrDefault(l => l.Id == linkId);
        if (link == null)
        {
            return RedirectToAction("Index");
        }

        //--
        var sessionId = HttpContext.Session.GetString(SessionKey);
        if (string.IsNullOrEmpty(sessionId))
        {
            sessionId = GenerateGuestSessionId();
            HttpContext.Session.SetString(SessionKey, sessionId);
        }

        var newComment = new CommentModel
        {
            Text = commentText,
            CreatedAt = DateTime.Now,
            SessionId = sessionId,
            LinkId = linkId
        };

        context.Comments.Add(newComment);
        context.SaveChanges();

        return RedirectToAction("Index");
    }



    [HttpPost]
    public IActionResult EditComment(int commentId, string newText)
    {
        if (!string.IsNullOrEmpty(newText))
        {
            var comment = context.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment != null)
            {
                if (HttpContext.Session.GetString(SessionKey) == "owner")
                {
                    comment.Text = newText;
                    context.SaveChanges();
                }
                else
                {
                    var userSessionId = HttpContext.Session.GetString(SessionKey);
                    if (comment.SessionId == userSessionId)
                    {
                        comment.Text = newText;
                        context.SaveChanges();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Вы не можете изменять комментарий другого пользователя.";
                        return RedirectToAction("Error");
                    }
                }
            }
        }
        return RedirectToAction("Index");
    }


    [HttpPost]
    public IActionResult DeleteComment(int commentId)
    {
        var comment = context.Comments.FirstOrDefault(c => c.Id == commentId);

        if (comment != null)
        {
            var currentSessionId = HttpContext.Session.GetString(SessionKey);

            if (comment.SessionId == currentSessionId || HttpContext.Session.GetString(SessionKey) == "owner")
            {
                context.Comments.Remove(comment);
                context.SaveChanges();
            }
            else
            {
                TempData["ErrorMessage"] = "Вы не можете удалить комментарий другого пользователя.";
                return RedirectToAction("Error");
            }
        }

        return RedirectToAction("Index");
    }


    [HttpPost]
    public IActionResult Vote(int id, bool isUseful)
    {
        if (HttpContext.Session.GetString(SessionKey) == "owner")
        {
            var link = context.Links.Find(id);
            if (link != null)
            {
                if (isUseful)
                {
                    link.UsefulCount++;
                }
                else
                {
                    link.UselessCount++;
                }
                context.SaveChanges();
            }
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Error()
    {
        ViewBag.ErrorMessage = TempData["ErrorMessage"] ?? "An unknown error occurred.";
        return View();
    }
}
