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
                <button onClick={backToHomePage}>
                    Back to HomePage
                </button>
            </main>
        </>
    )
}
