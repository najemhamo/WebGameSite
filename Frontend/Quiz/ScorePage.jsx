/* eslint-disable no-unused-vars */
import React from "react";
import { useLocation, useNavigate } from "react-router-dom";

export default function ScorePage () {
    // declare constants
    const navigate = useNavigate();
    const {state} = useLocation();
    const { userAnswers, rightAnswers, answers, questions, time } = state;
    
    //function to navigate back to homepage
    const backToHomePage = () => {
        navigate('/');
    };

    //function to process answers,
    const processAnswers = () => {
        //set score and time to 0 by default
        let score = 0;
        let timeTotal = 0;

        //loop through each question
        const results = questions.map((question, index) => {

            //declare all constants
            const userAnswer = userAnswers[index];
            const rightAnswer = rightAnswers[index];
            const answerOptions = answers[index];
            const answerTime = time[index];

            //find the question based on input from user / rightAnswer
            const userAnswerText = answerOptions.find(option => option.startsWith(userAnswer));

            const rightAnswerText = answerOptions.find(option => option.startsWith(rightAnswer));

            //add elapsed time to the total
            timeTotal += answerTime;

            //add one point to score if answer is correct
            const isCorrect = userAnswer === rightAnswer;
            if (isCorrect) score += 1;

            //return the important variables
            return {
                question,
                userAnswerText,
                rightAnswerText,
                isCorrect,
                answerTime
            };
        });

        //return the important variables, outside each question
        return {results, score, timeTotal};
    };

    //call the function processAnswers
    const {results, score, timeTotal} = processAnswers();

    //Page
    return (
        <div className="center-col">
            <header>
                <h1>
                    Your score: {score}/15
                </h1>
                <h2>
                    Your total time answering: {timeTotal} seconds
                </h2>
                <h3>
                    Good job! please see your score down below:
                </h3>
            </header>
            <main className="scrollable-content">
                <div>
                    {results.map((result, index) => (
                        <div key={index}>
                            <p>
                                <strong>Question:</strong> {result.question}
                            </p>
                            <p>
                                <strong>Your answer:</strong> {result.userAnswerText}
                            </p>
                            <p>
                                <strong>Right answer:</strong> {result.rightAnswerText}
                            </p>
                            <p>
                                <strong>Correct:</strong> {result.isCorrect ? "Yes" : "No"}
                            </p>
                            <p>
                                <strong>Answer time:</strong> {result.answerTime} seconds
                            </p>
                            <hr />
                        </div>
                    ))}
                </div>
                <div>
                    <strong>Credits:</strong> 
                    <p>
                        All questions were retrieved from Nyheter24, and can be accessed through the link:
                    </p>
                    <a href="https://nyheter24.se/quiz/977288-quiz-till-nyar-vad-hande-under-aret-som-gatt" target="_blank" rel="noopener noreferrer">
                        https://nyheter24.se/quiz/977288-quiz-till-nyar-vad-hande-under-aret-som-gatt
                    </a>
                </div>
                <div className="button-container">
                    <button onClick={backToHomePage}>
                        Back to HomePage
                    </button>
                </div>
            </main>
        </div>
    )
}
