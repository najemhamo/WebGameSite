import React from "react";
import { useNavigate } from "react-router-dom";

export default function SubmissionPage () {
    const navigate = useNavigate();
    const submit = () => {
        navigate('/Quiz/ScorePage');
    }
    const backToPrevious = () => {
        navigate('/');
    }

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
