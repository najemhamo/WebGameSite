import React from "react";
import { useLocation, useNavigate } from "react-router-dom";

export default function ScorePage () {
    const navigate = useNavigate();
    const {state} = useLocation();
    const score = state.score;
    
    const backToHomePage = () => {
        navigate('/');
    }

    return (
        <>
            <header>
                <h1>
                    Your score: {score}/15
                </h1>
            </header>
            <main>
                <h2>
                    Good job!
                </h2>
                <div>
                    Credits: All questions were retrieved from Nyheter24, and can be accessed through the link: 
                    <a href="https://nyheter24.se/quiz/977288-quiz-till-nyar-vad-hande-under-aret-som-gatt" target="_blank" rel="noopener noreferrer">
                        https://nyheter24.se/quiz/977288-quiz-till-nyar-vad-hande-under-aret-som-gatt
                    </a>
                </div>
                <button onClick={backToHomePage}>
                    Back to HomePage
                </button>
            </main>
        </>
    )
}
