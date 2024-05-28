import React, {useState, useEffect} from "react";
import { useNavigate } from "react-router-dom";

export default function QuestionPage() {

    const navigate = useNavigate();

    const nextQuestion = () => {
        navigate('/Quiz/Submission');
    }

    const previousQuestion = () => {
        navigate('/');
    }

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
                    Question 1/15
                </h1>
                <h2>
                    I vilket land vann Loreen årets upplaga av Eurovision Song Contest med låten ”Tattoo”?
                </h2>
            </header>
            <main>
                <p>
                    När Loreen, för andra gången, representerade Sverige i Eurovision Song Contest gick hon vinnande ur tävlingen. Men var avgjordes 2023 års upplaga av Eurovision Song Contest?
                </p>
                <div className="answerButtonsContainer">
                    <button>1: Birmingham</button>
                    <button>X: Liverpool</button>
                    <button>2: London</button>
                </div>
            </main>
        </>
    );
}
