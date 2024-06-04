import React from "react";
import { useLocation, useNavigate } from "react-router-dom";

export default function ScorePage () {
    const navigate = useNavigate();
    const {state} = useLocation();
    const { userAnswers, rightAnswers, answers, questions } = state;
    
    const backToHomePage = () => {
        navigate('/');
    };

    const processAnswers = () => {
        let score = 0;
        const results = questions.map((question, index) => {
            const userAnswer = userAnswers[index];
            const rightAnswer = rightAnswers[index];
            const answerOptions = answers[index];

            const userAnswerText = answerOptions.find(option => option.startsWith(userAnswer));
            const rightAnswerText = answerOptions.find(option => option.startsWith(rightAnswer));

            const isCorrect = userAnswer === rightAnswer;
            if (isCorrect) score += 1;

            return {
                question,
                userAnswerText,
                rightAnswerText,
                isCorrect
            };
        });

        return {results, score};
    };

    const {results, score} = processAnswers();

    return (
        <div className="center-col">
            <header>
                <h1>
                    Your score: {score}/15
                </h1>
                <h2>
                    Good job! please see your scores down below:
                </h2>
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
