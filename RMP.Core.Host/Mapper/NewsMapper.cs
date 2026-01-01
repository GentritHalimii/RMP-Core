using Riok.Mapperly.Abstractions;
using RMP.Host.Entities;
using RMP.Host.Features.News.CreateNews;
using RMP.Host.Features.News.GetNews;
using RMP.Host.Features.News.GetAllNewsDescByDate;

namespace RMP.Host.Mapper;

[Mapper]
public static partial class NewsMapper
{
    public static partial GetNewsResult ToGetNewsResult(this NewsEntity news);
    public static partial GetNewsResponse ToGetNewsResponse(this GetNewsResult result);
    public static partial void ToNewsEntity(this UpdateNewsCommand command, NewsEntity entity);
    public static partial GetAllNewsDescByDateResult ToGetAllNewsDescByDateResult(this NewsEntity news);
    public static partial GetAllNewsDescByDateResponse ToGetAllNewsDescByDateResponse(this GetAllNewsDescByDateResult result);
    

}