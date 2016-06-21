using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using static System.Console;

namespace ConsoleApplication
{
    public class Program
    {
        public static readonly int NumberOfBlogs = 5;
        public static readonly int NumberOfPostsPerBlog = 10;
        public static void Main(string[] args)
        {
            var context = new BloggingContext();

            WriteLine("---------------------------------------------------");
            WriteLine("Entity frameworks Core 1.0 Demos");
            WriteLine("---------------------------------------------------");
            WriteLine($"{context.Posts.CountAsync().Result} Posts found.");
            WriteLine($"{context.Blogs.CountAsync().Result} Blogs found.");

            WriteLine("Removing All DB entries.");
            foreach(var post in context.Posts)
            {
                context.Posts.Remove(post);
            }
            foreach(var blog in context.Blogs)
            {
                context.Blogs.Remove(blog);
            }
            context.SaveChanges();

            WriteLine($"{context.Posts.CountAsync().Result} Posts found.");
            WriteLine($"{context.Blogs.CountAsync().Result} Blogs found.");
            WriteLine("---------------------------------------------------");

            WriteLine("Creating Data...");
            for(int blogIndex = 0; blogIndex < NumberOfBlogs; blogIndex++)
            {
                var blog = new Blog() 
                { 
                    Url = $"http://mysite/{blogIndex}/blog",
                    Name = $"Blog #{blogIndex + 1}"
                };
                context.Blogs.Add(blog);

                for(int postIndex = 0; postIndex < NumberOfPostsPerBlog; postIndex++)
                {
                    var post = new Post
                    {
                        Title = $"Post #{postIndex + 1} Blog #{blogIndex + 1}",
                        Content = "Lorem ipsum delorum est",
                        Blog = blog 
                    };
                    context.Posts.Add(post);
                }
            }
            context.SaveChanges();
            WriteLine("Created Data.");

            WriteLine("---------------------------------------------------");
            WriteLine($"{context.Posts.CountAsync().Result} Posts found.");
            WriteLine($"{context.Blogs.CountAsync().Result} Blogs found.");
            WriteLine("---------------------------------------------------");
            WriteLine("Complete.");
        }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseInMemoryDatabase();
            optionsBuilder.UseSqlite("Filename=./blog.db");
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
