﻿using System;
using System.Web.Mvc;
using AutoMapper;
using GForum.Services.Contracts;
using GForum.Web.Areas.Admin.Models.Categories;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        // GET: /Admin/Categories
        public ActionResult Index()
        {
            var model = new CategoriesViewModel
            {
                CategoriesQueriable = this.categoryService.GetAll(true),
                CategoryAdd = new CategoryAddViewModel()
            };

            return View(model);
        }

        // GET: /Admin/Categories/Add
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CategoryAddViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.User.Identity.GetUserId();
            this.categoryService.Create(userId, model.Title);

            return RedirectToAction("Index");
        }

        // GET: /Admin/Categories/Delete
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            this.categoryService.Delete(id);

            return RedirectToAction("Index");
        }

        // GET: /Admin/Categories/Restore
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Restore(Guid id)
        {
            this.categoryService.Restore(id);

            return RedirectToAction("Index");
        }
    }
}