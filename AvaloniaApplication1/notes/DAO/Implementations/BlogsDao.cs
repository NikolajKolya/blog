﻿using blogs.DAO.Abstract;
using blogs.DAO.Models;
using blogs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog = blogs.DAO.Models.Blog;
using Comment = blogs.DAO.Models.Comment;

namespace blogs.DAO.Implementations
{
    public class BlogsDao : IBlogsDao
    {
        private MainDbContext _mainDbContext;

        public BlogsDao()
        {
            _mainDbContext = new MainDbContext();
        }

        public void AddBlog(Blog blog)
        {
            _ = blog ?? throw new ArgumentNullException(nameof(blog));

            _mainDbContext.Add(blog);
            _mainDbContext.SaveChanges();
        }

        public void AddComment(Comment newComment)
        {
            _ = newComment ?? throw new ArgumentNullException(nameof(newComment));

            _mainDbContext.Add(newComment);
            _mainDbContext.SaveChanges();
        }

        public void DeleteBlogById(Guid id)
        {
            _mainDbContext.Remove(GetBlogById(id));
            _mainDbContext.SaveChanges();
        }

        public IReadOnlyCollection<Blog> GetAllBlogs()
        {
            return _mainDbContext
                .Blogs
                .Include(b => b.Comments)
                .OrderByDescending(b => b.Timestamp)
                .ToList();
        }

        public Blog GetBlogById(Guid id)
        {
            return _mainDbContext
                .Blogs
                .Include(b => b.Comments)
                .Single(n => n.Id == id);
        }

        public void Update(Blog newBlog)
        {
            var existingBlog = GetBlogById(newBlog.Id);

            existingBlog.Name = newBlog.Name;
            existingBlog.Content = newBlog.Content;
            existingBlog.Timestamp = newBlog.Timestamp;

            _mainDbContext.SaveChanges();
        }
    }
}
