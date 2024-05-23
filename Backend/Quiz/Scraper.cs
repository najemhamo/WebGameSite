using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Collections.Generic;

namespace Backend.Quiz
{
    public class Scraper
    {
        //Properties
        private readonly HttpClient _httpClient;
        private const string url = "https://nyheter24.se/quiz/977288-quiz-till-nyar-vad-hande-under-aret-som-gatt";

        //Constructor
        public Scraper() 
        {
            _httpClient = new HttpClient();
        }

        public 
        (
            List<string> questions, 
            List<string> descriptions, 
            List<List<string>> answers, 
            List<string> rightAnswers
        )

        //Scraper-method
        ScrapeQuiz()
        {
            List<string> questions = new List<string>();
            List<string> descriptions = new List<string>();
            List<List<string>> answers = new List<List<string>>();
            List<string> rightAnswers = new List<string>();

            var html = _httpClient.GetStringAsync(url).Result;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            int questionIndex = 3;
            int descriptionIndex = 9;
            int answerIndex = 10;

            //Loop through all questions in the URL
            for (int i = 0; i < 15; i++)
            {
                //Get questions
                var questionHeaderElement = htmlDocument.DocumentNode.SelectSingleNode($"(//h2)[{questionIndex}]");
                var question = questionHeaderElement?.InnerText.Trim();
                questions.Add(question);

                //Get description
                var paragraph = htmlDocument.DocumentNode.SelectSingleNode($"(//p)[{descriptionIndex}]");
                //Skip over the hyperlinks
                if (paragraph.ChildNodes.Count == 1 && paragraph.FirstChild.Name == "a")
                {
                    descriptionIndex += 1;
                    answerIndex += 1;
                }
                
                var questionDescriptionElement = htmlDocument.DocumentNode.SelectSingleNode($"(//p)[{descriptionIndex}]");
                var description = questionDescriptionElement?.InnerText.Trim();
                descriptions.Add(description);

                //Get answer alternatives
                var answerElement1 = htmlDocument.DocumentNode.SelectSingleNode($"(//p)[{answerIndex}]")?.InnerText.Trim();
                var answerElementX = htmlDocument.DocumentNode.SelectSingleNode($"(//p)[{answerIndex + 1}]")?.InnerText.Trim();
                var answerElement2 = htmlDocument.DocumentNode.SelectSingleNode($"(//p)[{answerIndex + 2}]")?.InnerText.Trim();

                List<string> answerAlternatives = new List<string>()
                {
                    answerElement1, answerElementX, answerElement2
                };
                answers.Add(answerAlternatives);

                //Update index
                questionIndex += 1;
                descriptionIndex += 5;
                answerIndex = descriptionIndex + 1;
            }

            //Get right answers
            var liNodes = htmlDocument.DocumentNode.SelectNodes("//li");
            foreach (var liNode in liNodes)
            {
                rightAnswers.Add(liNode.InnerText.Trim());
            }

            return (questions, descriptions, answers, rightAnswers);
        }
    }
}
