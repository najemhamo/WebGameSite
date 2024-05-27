import React from "react";
import { useNavigate } from "react-router-dom";


export default function ScorePage () {
    const navigate = useNavigate();
    const backToHomePage = () => {
        navigate('/');
    }

    return (
        <>
            <header>
                <h1>
                    Your score: 8/10
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
