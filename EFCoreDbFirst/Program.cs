using EFCoreDbFirst.Models; // Namespace für die generierten Modelle
using System;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Verbindung mit dem generierten DbContext
        using (var context = new BlogContext())
        {
            while (true)
            {
                Console.WriteLine("\nWähle eine Aktion:");
                Console.WriteLine("1 - Neuen Blog hinzufügen");
                Console.WriteLine("2 - Alle Blogs anzeigen");
                Console.WriteLine("3 - Neuen Post hinzufügen");
                Console.WriteLine("4 - Posts eines Blogs anzeigen");
                Console.WriteLine("0 - Beenden");
                Console.Write("Eingabe: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddBlog(context);
                        break;
                    case "2":
                        ListBlogs(context);
                        break;
                    case "3":
                        AddPost(context);
                        break;
                    case "4":
                        ListPosts(context);
                        break;
                    case "0":
                        Console.WriteLine("Programm beendet.");
                        return;
                    default:
                        Console.WriteLine("Ungültige Eingabe. Bitte erneut versuchen.");
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Fügt einen neuen Blog zur Datenbank hinzu.
    /// </summary>
    /// <param name="context">BlogContext für die Datenbankverbindung.</param>
    static void AddBlog(BlogContext context)
    {
        Console.Write("Gib die URL des neuen Blogs ein: ");
        var url = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(url))
        {
            Console.WriteLine("Ungültige URL. Vorgang abgebrochen.");
            return;
        }

        var newBlog = new Blog { Url = url };
        context.Blogs.Add(newBlog);
        context.SaveChanges();

        Console.WriteLine($"Blog mit der URL '{url}' wurde hinzugefügt.");
    }

    /// <summary>
    /// Listet alle Blogs aus der Datenbank sortiert nach BlogId.
    /// </summary>
    /// <param name="context">BlogContext für die Datenbankverbindung.</param>
    static void ListBlogs(BlogContext context)
    {
        var blogs = context.Blogs.OrderBy(b => b.BlogId).ToList();

        Console.WriteLine("\nAlle Blogs in der Datenbank:");
        foreach (var blog in blogs)
        {
            Console.WriteLine($"ID: {blog.BlogId}, URL: {blog.Url}");
        }
    }

    /// <summary>
    /// Fügt einen neuen Post zu einem Blog hinzu.
    /// </summary>
    /// <param name="context">BlogContext für die Datenbankverbindung.</param>
    static void AddPost(BlogContext context)
    {
        Console.Write("Gib die BlogId ein, zu der du einen Post hinzufügen möchtest: ");
        if (!int.TryParse(Console.ReadLine(), out var blogId))
        {
            Console.WriteLine("Ungültige BlogId. Vorgang abgebrochen.");
            return;
        }

        var blog = context.Blogs.Find(blogId);
        if (blog == null)
        {
            Console.WriteLine("Kein Blog mit der angegebenen ID gefunden.");
            return;
        }

        Console.Write("Gib den Titel des Posts ein: ");
        var title = Console.ReadLine();
        Console.Write("Gib den Inhalt des Posts ein: ");
        var content = Console.ReadLine();

        var newPost = new Post
        {
            Title = title,
            Content = content,
            BlogId = blogId
        };

        context.Posts.Add(newPost);
        context.SaveChanges();

        Console.WriteLine($"Post '{title}' wurde zu Blog '{blog.Url}' hinzugefügt.");
    }

    /// <summary>
    /// Listet alle Posts eines bestimmten Blogs.
    /// </summary>
    /// <param name="context">BlogContext für die Datenbankverbindung.</param>
    static void ListPosts(BlogContext context)
    {
        Console.Write("Gib die BlogId ein, deren Posts angezeigt werden sollen: ");
        if (!int.TryParse(Console.ReadLine(), out var blogId))
        {
            Console.WriteLine("Ungültige BlogId. Vorgang abgebrochen.");
            return;
        }

        var posts = context.Posts.Where(p => p.BlogId == blogId).ToList();
        if (!posts.Any())
        {
            Console.WriteLine("Keine Posts für diesen Blog gefunden.");
            return;
        }

        Console.WriteLine("\nPosts für diesen Blog:");
        foreach (var post in posts)
        {
            Console.WriteLine($"ID: {post.PostId}, Titel: {post.Title}, Inhalt: {post.Content}");
        }
    }
}
