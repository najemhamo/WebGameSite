using Backend.Quiz;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Backend.Quiz
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly Scraper _scraper;

        public QuizController() 
        {
            _scraper = new Scraper();
        }

        [HttpGet]
        public IActionResult GetQuizData()
        {
            var (questions, descriptions, answers, rightAnswers) = _scraper.ScrapeQuiz();
            var result = new
            {
                Questions = questions,
                Descriptions = descriptions,
                Answers = answers,
                RightAnswers = rightAnswers
            };
            return Ok(result);
        }
    }
}
