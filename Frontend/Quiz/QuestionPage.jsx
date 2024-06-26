/* eslint-disable no-unused-vars */
import React, { useState, useRef, useEffect } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";

export default function QuestionPage() {
    //declare constants
    const {state} = useLocation();
    const navigate = useNavigate();
    const {questionId} = useParams();
    const currentQuestionIndex = parseInt(questionId) - 1;
    const [selectedAnswer, setSelectedAnswer] = useState('');
    const timestampRef = useRef(null);
    const [elapsedTime, setElapsedTime] = useState(0);

    //useEffect hook to measure time when page is loaded
    useEffect(() => {
            timestampRef.current = new Date().getTime();
            
            //update elapsed time every second
            const interval = setInterval(() => {
                const currentTime = new Date().getTime();
                const elapsed = 
                Math.floor((currentTime - timestampRef.current) / 1000);
                setElapsedTime(elapsed);
            }, 1000);

            //Clean interval on component unmount
            return () => clearInterval(interval);
    }, [questionId]);

    //check if all data is passed onto the component,
    //and return error otherwise
    if (!state || 
        !state.questions ||
        !state.answers || 
        !state.rightAnswers || 
        !state.time)
    {
        return <div>Error: Quiz data not found.</div>;
    }

    //declare all variables passed into the component via state
    const { questions, answers, userAnswers, rightAnswers, time } = state;

    //error handling, if questionIndex is incorrect
    if (currentQuestionIndex < 0 || currentQuestionIndex >= questions.length) {
        return <div>Error: Invalid question index, please try again</div>;
    }

    //error handling, check if the answerAlternatives are passed on as an array
    if (!Array.isArray(answers[currentQuestionIndex])) {
        return <div>Error: Answers data not found for this question.</div>;
    }

    //function to move on to next question
    const nextQuestion = () => {

        //error handling, check if an answer is selected
        if (selectedAnswer === '') {
            alert('Error: No answer selected, please select an answer to proceed')
            return;
        }

        //second timeStamp, to measure time elapsed
        const timestamp2 = new Date().getTime();
        const finalTimeStamp = Math.floor((timestamp2 - timestampRef.current)/1000);

        //set userAnswer and time elapsed
        userAnswers[currentQuestionIndex] = selectedAnswer;
        time[currentQuestionIndex] = finalTimeStamp;

        //navigate to next question, or submissionPage
        if (currentQuestionIndex < questions.length - 1){
            navigate(`/quiz/takeQuiz/${currentQuestionIndex + 2}`, { state });
        } else {
            navigate('/quiz/submission', { state });
        }

        //set selectedAnswer back to default
        setSelectedAnswer('');
    };

    //function to navigate back to previous question, or quizHomePage
    const previousQuestion = () => {
        if (currentQuestionIndex > 0) {
            navigate(`/quiz/takeQuiz/${currentQuestionIndex}`, { state });
        } else {
            navigate('/quiz');
        }
    };

    //set selectedAnswer
    const selectAnswer = (answer) => {
        setSelectedAnswer(answer);
    };

    //Page
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
                <div>
                    Elapsed Time: {elapsedTime} seconds
                </div>
            </header>
            <main>
                <h2>
                    {questions[currentQuestionIndex]}
                </h2>
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
