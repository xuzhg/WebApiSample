namespace Question78956264AllowDeepNavigation;

public class Service
{
    public int ServiceId { get; set; }
    public ICollection<ServiceArticle> ServiceArticle { get; set; }
}

public class ServiceArticle
{
    public int ServiceArticleId { get; set; }

    public int ServiceId { get; set; }
    public Service Service { get; set; }
    public int ArticleId { get; set; }
    public Article Article { get; set; }
}

public class Article
{
    public int ArticleId { get; set; }
}
