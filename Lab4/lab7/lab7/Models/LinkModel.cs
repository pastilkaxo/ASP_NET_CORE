using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class LinkModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Url { get; set; }

    [Required]
    public string Keywords { get; set; }

    public int UsefulCount { get; set; } = 0;
    public int UselessCount { get; set; } = 0;

    public List<CommentModel> Comments { get; set; } = new List<CommentModel>();  
}
