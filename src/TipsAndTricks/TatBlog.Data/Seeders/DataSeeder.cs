using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly BlogDbContext _dbConText;

        public DataSeeder(BlogDbContext dbConText)
        {
            _dbConText = dbConText;
        }

        public void Initialize()
        {
            _dbConText.Database.EnsureCreated();

            if (_dbConText.Posts.Any()) return;

            var authors = AddAuthors();
            var categories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categories, tags);
        }

        private IList<Author> AddAuthors() 
        {
            var authors = new List<Author>()
            {
                new()
                {
                    FullName = "Jacon Mouth",
                    UrlSlug = "jason-mouth",
                    Email = "json@gmail.com",
                    JoinedDate = new DateTime(2022, 10, 21)
                },
                new()
                {
                    FullName = "Jessica Wonder",
                    UrlSlug = "jessica-wonder",
                    Email = "jessica665@motip.com",
                    JoinedDate = new DateTime(2020, 4, 19)
                },
                 new()
                {
                    FullName = "Blank Mouth",
                    UrlSlug = "blank-mouth",
                    Email = "blank@gmail.com",
                    JoinedDate = new DateTime(2021, 10, 10)
                },
                new()
                {
                    FullName = "Juniver David",
                    UrlSlug = "juniver-david",
                    Email = "juniver@motip.com",
                    JoinedDate = new DateTime(2020, 6, 14)
                },
                 new()
                {
                    FullName = "Jacon Jack",
                    UrlSlug = "jacon-jack",
                    Email = "jackjj@gmail.com",
                    JoinedDate = new DateTime(2019, 10, 1)
                },
                new()
                {
                    FullName = "Herry Mio",
                    UrlSlug = "herry-mio",
                    Email = "mio@motip.com",
                    JoinedDate = new DateTime(2019, 4, 7)
                },
                 new()
                {
                    FullName = "Blu Tree",
                    UrlSlug = "blu-tree",
                    Email = "tree@gmail.com",
                    JoinedDate = new DateTime(2021, 8, 22)
                },
                new()
                {
                    FullName = "Hanna Baki",
                    UrlSlug = "hana-baki",
                    Email = "baki@motip.com",
                    JoinedDate = new DateTime(2022, 3, 2)
                }
            };

            _dbConText.Authors.AddRange(authors);
            _dbConText.SaveChanges();

            return authors;
        
        }

        private IList<Category> AddCategories() 
        {
            var categories = new List<Category>()
            {
                new() {Name = ".NET Core", UrlSlug = ".net-core",
                    Description = ".NET Core", ShowOnMenu = false},
                new() {Name = "Architecture", UrlSlug = "architecture",
                    Description = "Architecture", ShowOnMenu = false},
                new() {Name = "Messaging", UrlSlug = "messaging",
                    Description = "Messaging", ShowOnMenu = false},
                new() {Name = "OOP", UrlSlug = "object-oriented-program",
                    Description = "Object-Oriented-Program", ShowOnMenu = false},
                new() {Name = "Design Pattern", UrlSlug = "design-pattern",
                    Description = "Design Pattern", ShowOnMenu = false},
                new() {Name = "JavaScrip", UrlSlug = "javascrip",
                    Description = "JavaScrip", ShowOnMenu = false},
                new() {Name = "Travel", UrlSlug = "travel",
                    Description = "Travel", ShowOnMenu = false},
                new() {Name = "Python", UrlSlug = "python",
                    Description = "Python", ShowOnMenu = false},
                new() {Name = "Fontend Development", UrlSlug = "fontend-development",
                    Description = "Fontend Development", ShowOnMenu = false},
                new() {Name = "Animal", UrlSlug = "animal",
                    Description = "Animal", ShowOnMenu = false},
                new() {Name = "Nature", UrlSlug = "nature",
                    Description = "Nature", ShowOnMenu = false},
                new() {Name = "Economic", UrlSlug = "economic",
                    Description = "Economic", ShowOnMenu = false},
                new() {Name = "The war", UrlSlug = "the-war",
                    Description = "The war", ShowOnMenu = false},
                new() {Name = "Martial arts", UrlSlug = "martial-arts",
                    Description = "Martial arts", ShowOnMenu = false},
                new() {Name = "Graphic design", UrlSlug = "graphic-design",
                    Description = "graphic design", ShowOnMenu = false}
            };

            _dbConText.AddRange(categories);
            _dbConText.SaveChanges();

            return categories;
        
        }

        private IList<Tag> AddTags() 
        {
            var tags = new List<Tag>()
            {
                new() {Name = "Google", UrlSlug = "google",
                    Description = "Google applications"},
                new() {Name = "ASP.NET MVC", UrlSlug = "asp.net-mvc",
                    Description = "ASP.NET MVC"},
                new() {Name = "Razor Page", UrlSlug = "razor-page",
                    Description = "Razor Page"},
                new() {Name = "Deep Learning", UrlSlug = "deep-learning",
                    Description = "Deep Learning"},
                new() {Name = "Neural", UrlSlug = "neural",
                    Description = "Neural Network"},
                new() {Name = "Chrome", UrlSlug = "google-chrome",
                    Description = "Google Chrome"},
                new() {Name = "Javascrip program", UrlSlug = "javascrip-program",
                    Description = "Javascrip program"},
                new() {Name = "Dog", UrlSlug = "dog",
                    Description = "Dog"},
                new() {Name = "Mountain range", UrlSlug = "mountain-range",
                    Description = "Mountain range"},
                new() {Name = "HTML & CSS", UrlSlug = "html-css",
                    Description = "HTML & CSS"},
                new() {Name = "Python", UrlSlug = "python",
                    Description = "Python program"},
                new() {Name = "Boxing", UrlSlug = "boxing",
                    Description = "Boxing"},
                new() {Name = "Duck", UrlSlug = "duck",
                    Description = "Duck"},
                new() {Name = "Cat", UrlSlug = "cat",
                    Description = "Cat"},
                new() {Name = "Photoshop", UrlSlug = "photoshop",
                    Description = "Photoshop"},
                new() {Name = "ShortCut", UrlSlug = "shortcut",
                    Description = "Shortcut"},
                new() {Name = "Factory method", UrlSlug = "factory-method",
                    Description = "Factory method"},
                new() {Name = "Technology", UrlSlug = "technology",
                    Description = "Technology"},
                new() {Name = "Twitter", UrlSlug = "twitter",
                    Description = "Twitter"},
                new() {Name = "Economics", UrlSlug = "economics",
                    Description = "Economics"},
                new() {Name = "Http", UrlSlug = "http",
                    Description = "Http"},
                new() {Name = "Sql Server", UrlSlug = "sql-server",
                    Description = "Sql Server"},
                new() {Name = "Tip", UrlSlug = "tip",
                    Description = "Tip & Trick"},
                new() {Name = "Figma", UrlSlug = "figma",
                    Description = "Figma Learning"},
                new() {Name = "Internet", UrlSlug = "internet",
                    Description = "Internet"}
            };

            _dbConText.AddRange(tags);
            _dbConText.SaveChanges();

            return tags;

        }

        private IList<Post> AddPosts(
            IList<Author> authors,
            IList<Category> categories,
            IList<Tag> tags) 
        {
            var posts = new List<Post>()
            {
                new()
                {
                    Title = "ASP.NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled",
                    Description = "Here's few great DON'T an DO examples",
                    Meta = "David and friends has a great repository filled",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[0]
                    }
                },
                new()
                {
                    Title = "HTML & CSS All in one",
                    ShortDescription = "How to learn html and css",
                    Description = "How to learn html and css",
                    Meta = "How to learn html and css",
                    UrlSlug = "html-css-all-in-one",
                    Published = true,
                    PostedDate = new DateTime(2020, 8, 20, 11, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 20,
                    Author = authors[2],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[1]
                    }
                },
                new()
                {
                    Title = "Dog and cat",
                    ShortDescription = "Raise cats and dogs",
                    Description = "How to raise cats and dogs so they don't fight?",
                    Meta = "Raise cats and dogs",
                    UrlSlug = "dog-and-cat",
                    Published = true,
                    PostedDate = new DateTime(2022, 8, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 15,
                    Author = authors[3],
                    Category = categories[9],
                    Tags = new List<Tag>()
                    {
                        tags[7]
                    }
                },
                new()
                {
                    Title = "What qualifications are required for the architecture industry?",
                    ShortDescription = "Qualifications are required...",
                    Description = "What qualifications are required for the architecture industry?",
                    Meta = "Qualifications are required...",
                    UrlSlug = "what-qualifications-are",
                    Published = true,
                    PostedDate = new DateTime(2019, 6, 20, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 30,
                    Author = authors[3],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                        tags[4]
                    }
                },
                new()
                {
                    Title = "Sapiens: A Brief History of Humankind",
                    ShortDescription = "Explore the history of Homo sapiens and how we became the dominant species on Earth.",
                    Description = "Sapiens is a fascinating exploration of human history, " +
                    "from the emergence of Homo sapiens in Africa to the present day.",
                    Meta = "History, Anthropology, Sociology",
                    UrlSlug = "sapiens-brief-history-humankind",
                    Published = true,
                    PostedDate = new DateTime(2021, 3, 3, 11, 2, 0),
                    ModifiedDate = null,
                    ViewCount = 14,
                    Author = authors[4],
                    Category = categories[3],
                    Tags = new List<Tag>()
                    {
                        tags[3]
                    }
                },
                new()
                {
                    Title = "The Alchemist",
                    ShortDescription = "A young shepherd embarks on a journey to fulfill his dreams and find his destiny.",
                    Description = "he Alchemist is a captivating tale of self-discovery and personal growth. " +
                    "The story follows a young shepherd named Santiago as he embarks on a journey to fulfill his dreams and find his destiny.",
                    Meta = "Fiction, Self-help, Philosophy",
                    UrlSlug = "the-alchemist",
                    Published = true,
                    PostedDate = new DateTime(2021, 5, 7, 11, 22, 0),
                    ModifiedDate = null,
                    ViewCount = 15,
                    Author = authors[2],
                    Category = categories[4],
                    Tags = new List<Tag>()
                    {
                        tags[4]
                    }
                },
                new()
                {
                    Title = "The Great Gatsby",
                    ShortDescription = "A wealthy man tries to win back the love of his life in the Roaring Twenties.",
                    Description = "The Great Gatsby is a classic novel set in the Roaring Twenties, " +
                    "a time of great social change and decadence in America.",
                    Meta = "Fiction, Classic Literature, American History",
                    UrlSlug = "great-gatsby",
                    Published = true,
                    PostedDate = new DateTime(2020, 3, 28, 11, 22, 0),
                    ModifiedDate = null,
                    ViewCount = 17,
                    Author = authors[5],
                    Category = categories[5],
                    Tags = new List<Tag>()
                    {
                        tags[5]
                    }
                },
                new()
                {
                    Title = "The 7 Habits of Highly Effective People",
                    ShortDescription = "Discover seven habits that can help you achieve personal and professional success.",
                    Description = "The 7 Habits of Highly Effective People is a self-help book that outlines seven habits " +
                    "that can help you achieve personal and professional success. ",
                    Meta = "Self-help, Personal Development, Business",
                    UrlSlug = "7-habits-highly-effective-people",
                    Published = true,
                    PostedDate = new DateTime(2020, 4, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[0]
                    }
                },
                new()
                {
                    Title = "To Kill a Mockingbird",
                    ShortDescription = "A young girl learns about racial injustice and courage in the Deep South.",
                    Description = "To Kill a Mockingbird is a classic novel set in the Deep South during the 1930s.",
                    Meta = "Fiction, Classic Literature, American History",
                    UrlSlug = "to-kill-a-mockingbird",
                    Published = true,
                    PostedDate = new DateTime(2018, 4, 28, 9, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 20,
                    Author = authors[6],
                    Category = categories[6],
                    Tags = new List<Tag>()
                    {
                        tags[6]
                    }
                },
                new()
                {
                    Title = "The Power of Now",
                    ShortDescription = "Learn to live in the present moment and find inner peace.",
                    Description = "The Power of Now is a spiritual guidebook that teaches readers " +
                    "how to live in the present moment and find inner peace. ",
                    Meta = "Classic Literature, American History",
                    UrlSlug = "diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2022, 6, 28, 14, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 18,
                    Author = authors[7],
                    Category = categories[9],
                    Tags = new List<Tag>()
                    {
                        tags[9]
                    }
                },
                new()
                {
                    Title = "The camper",
                    ShortDescription = "A shepherd boy follows his dreams to find a treasure.",
                    Description = "Here's few great DON'T an DO examples",
                    Meta = "David and friends has a great repository filled",
                    UrlSlug = "core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 7, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 18,
                    Author = authors[7],
                    Category = categories[7],
                    Tags = new List<Tag>()
                    {
                        tags[7]
                    }
                }
            };

            _dbConText.AddRange(posts); 
            _dbConText.SaveChanges();
        
            return posts;
        }
        
    }
}
