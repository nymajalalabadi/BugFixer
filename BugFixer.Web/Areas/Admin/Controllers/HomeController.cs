﻿using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.ViewModels.Admin.Tag;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BugFixer.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        #region Consractor

        private readonly IQuestionService _questionService;

        public HomeController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion

        #region Dashboard

        public async Task<IActionResult> Dashboard()
        {
            ViewData["ChartDataJson"] = JsonConvert.SerializeObject(await _questionService.GetTagViewModelJson());

            return View();
        }

        #endregion

        #region Filter Tags

        public async Task<IActionResult> LoadFilterTagsPartial(FilterTagAdminViewModel filter)
        {
            filter.TakeEntity = 3;

            var result = await _questionService.FilterTagAdmin(filter);

            return PartialView("_FilterTagsPartial", result);
        }

        #endregion

    }
}
