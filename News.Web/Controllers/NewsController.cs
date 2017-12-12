using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using News.Data;

namespace News.Web.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : Controller
    {
        private readonly NewsDbContext db;

        public NewsController(NewsDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetAllNews()
        {
            return Ok(this.db.News);
        }

        [HttpGet("{id}")]
        public IActionResult GetSingleNews([FromRoute]int id)
        {
            var singleNews = this.db.News.Find(id);

            if (singleNews == null)
            {
                return NotFound();
            }

            return Ok(singleNews);
        }
        
        [HttpPost]
        public IActionResult PostNews([FromBody]Data.Models.News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            this.db.Add(news);
            this.db.SaveChanges();

            return CreatedAtAction(nameof(GetSingleNews), news.Id, news);
        }
        
        [HttpPut("{id}")]
        public IActionResult PutNews([FromRoute]int id, [FromBody]Data.Models.News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var oldNews = this.db.News.Find(id);

            if (oldNews == null)
            {
                return BadRequest();
            }

            if (oldNews.Title != news.Title)
            {
                oldNews.Title = news.Title;
            }

            if (oldNews.Content != news.Content)
            {
                oldNews.Content = news.Content;
            }

            if (oldNews.PublishDate != news.PublishDate)
            {
                oldNews.PublishDate = news.PublishDate;
            }

            this.db.SaveChanges();

            return Ok();
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteNews([FromRoute]int id)
        {
            var newsToDelete = this.db.News.Find(id);

            if (newsToDelete == null)
            {
                return BadRequest();
            }

            this.db.News.Remove(newsToDelete);
            this.db.SaveChanges();

            return Ok();
        }
    }
}
