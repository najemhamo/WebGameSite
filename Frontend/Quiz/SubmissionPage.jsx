/* eslint-disable no-unused-vars */
import React, {useState} from "react";
import { useLocation, useNavigate } from "react-router-dom";

export default function SubmissionPage () {
    //declare constants
    const navigate = useNavigate();
    const {state} = useLocation();

    //function to navigate back to previous
    const backToPrevious = () => {
        const lastQuestionIndex = state.userAnswers.length;
        navigate(`/quiz/takeQuiz/${lastQuestionIndex}`, { state });
    };

    //declare the variables passed onto the component
    const { questions, answers, userAnswers, rightAnswers, time } = state;

    //function for submit answers
    const submit = async () => {
        try {

            //navigate to scorepage
            navigate('/quiz/scorePage', 
                { state: { questions, answers, userAnswers, rightAnswers, time } });

            //error handling
        } catch (error) {
            console.error('Error submitting answers:', error);
            alert('There was an error submitting your answers. Please try again.');
        }
    };

    return (
        <>
            <header>
                <button className="backButton" onClick={backToPrevious}>
                    Back
                </button>
                <h1>
                    Quiz Completed!
                </h1>
            </header>
            <main>
                <h2>
                    You have succesfully completed all questions, would you like to submit your answers?
                </h2>
                <button onClick={submit}>
                    Submit
                </button>
            </main>
        </>
    )
}
