using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CommentModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Text { get; set; }  

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;  

    public string SessionId { get; set; } 

    
    [ForeignKey("Link")]
    public int LinkId { get; set; }
    public LinkModel Link { get; set; }  
}
