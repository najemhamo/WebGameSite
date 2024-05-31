import React, { useState } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";

export default function QuestionPage() {
    const {state} = useLocation();
    const navigate = useNavigate();
    const {questionId} = useParams();
    const currentQuestionIndex = parseInt(questionId) - 1;
    const [selectedAnswer, setSelectedAnswer] = useState('');

    if (!state || !state.questions || !state.descriptions || !state.answers)
    {
        return <div>Error: Quiz data not found.</div>;
    }

    const questions = state.questions;
    const descriptions = state.descriptions;
    const answers = state.answers;
    const userAnswers = state.userAnswers;

    const nextQuestion = () => {
        if (selectedAnswer === '') {
            alert('Error: No answer selected, please select an answer to proceed')
            return;
        }

        userAnswers[currentQuestionIndex] = selectedAnswer[0];

        if (currentQuestionIndex < questions.length - 1){
            navigate(`/Quiz/TakeQuiz/${currentQuestionIndex + 2}`, { state  });
        } else {
            navigate('/Quiz/Submission', { state });
        }
        setSelectedAnswer('');
    };

    const previousQuestion = () => {
        if (currentQuestionIndex > 0) {
            navigate(`/Quiz/TakeQuiz/${currentQuestionIndex}`, { state });
        } else {
            navigate('/Quiz');
        }
    };

    const selectAnswer = (answer) => {
        setSelectedAnswer(answer);
    };

    return (
        <>
            <header>
                <button className="backButton" onClick={previousQuestion}>
                    Back
                </button>
                <button className="nextButton" onClick={nextQuestion}>
                    Next
                </button>
                <h1>
                    Question {currentQuestionIndex + 1}/{questions.length}
                </h1>
            </header>
            <main>
                <h2>
                    {questions[currentQuestionIndex]}
                </h2>
                <p>
                    {descriptions[currentQuestionIndex]}
                </p>
                <div className="answerButtonsContainer">
                {answers[currentQuestionIndex].map((answer, index) => (
                        <button key={index} onClick={() => selectAnswer(answer)}
                        className={selectedAnswer === answer ? 'selected' : ''}>
                            {answer}
                        </button>
                    ))}
                </div>
            </main>
        </>
    );
}
