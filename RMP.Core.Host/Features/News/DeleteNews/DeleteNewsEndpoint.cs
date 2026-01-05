using RMP.Core.Host.Entities;

namespace RMP.Host.Entities;

public class NewsEntity: BaseEntity
{
    
    public  string Title { get; set; }
    public  string Content { get; set; }
    public  DateTime PublicationDate { get; set; }
    public  string Category { get; set; }
    public  string? ProfilePhotoPath { get; set; }
    
 
}