import React from "react";
import { useNavigate } from "react-router-dom";

export default function QuizHomePage() {
    const navigate = useNavigate();
    const startQuiz = () => {
        navigate('/Quiz/TakeQuiz');
    };

    const backToHomePage = () => {
        navigate('/');
    };

    return (
        <>
            <header>
                <button className="backButton" onClick={backToHomePage}>
                    Back
                </button>
                <h1 className="header">
                    Quiz
                </h1>
            </header>
            <main>
                <button onClick={startQuiz}>
                    Start Quiz
                </button>
            </main>
        </>
    );
}
