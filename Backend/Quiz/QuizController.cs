using Backend.Quiz;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

//API controller for Quiz
namespace Backend.Quiz
{
    [ApiController]
    [Route("quiz")]
    public class QuizController : ControllerBase
    {
        //Create one instance of scraper
        private readonly Scraper _scraper;

        public QuizController() 
        {
            _scraper = new Scraper();
        }

        //fetch questions
        [HttpGet("questions")]
        public IActionResult GetQuestions()
        {
            var (questions, _, _, _) = _scraper.ScrapeQuiz();
            return Ok(questions);
        }

        //fetch descriptions
        [HttpGet("descriptions")]
        public IActionResult GetDescriptions()
        {
            var (_, descriptions, _, _) = _scraper.ScrapeQuiz(); 
            return Ok(descriptions);
        }

        //fetch answers
        [HttpGet("answers")]
        public IActionResult GetAnswers()
        {
            var (_, _, answers, _) = _scraper.ScrapeQuiz();
            return Ok(answers);
        }

        //fetch rightAnswers
        [HttpGet("rightAnswers")]
        public IActionResult GetRightAnswers() 
        {
            var (_, _, _, rightAnswers) = _scraper.ScrapeQuiz(); 
            return Ok(rightAnswers);
        }
    }
}