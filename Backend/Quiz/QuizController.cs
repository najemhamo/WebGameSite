using Backend.Quiz;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Quiz
{
    [ApiController]
    [Route("quiz")]
    public class QuizController : ControllerBase
    {
        private readonly Scraper _scraper;

        public QuizController() 
        {
            _scraper = new Scraper();
        }

        [HttpGet("questions")]
        public IActionResult GetQuestions()
        {
            var (questions, _, _, _) = _scraper.ScrapeQuiz();
            return Ok(questions);
        }

        [HttpGet("descriptions")]
        public IActionResult GetDescriptions()
        {
            var (_, descriptions, _, _) = _scraper.ScrapeQuiz(); 
            return Ok(descriptions);
        }

        [HttpGet("answers")]
        public IActionResult GetAnswers()
        {
            var (_, _, answers, _) = _scraper.ScrapeQuiz();
            return Ok(answers);
        }

        [HttpGet("rightAnswers")]
        public IActionResult GetRightAnswers() 
        {
            var (_, _, _, rightAnswers) = _scraper.ScrapeQuiz(); 
            return Ok(rightAnswers);
        }
    }
}