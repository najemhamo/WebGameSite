import React, {useState, useEffect} from "react";
import { useNavigate } from "react-router-dom";

export default function QuizHomePage() {
    const [questions, setQuestions] = useState([]);
    const [descriptions, setDescriptions] = useState([]);
    const [answers, setAnswers] = useState ([]);
    const [userAnswers, setUserAnswers] = useState([]);
    const [rightAnswers, setRightAnswers] = useState([]);

    const navigate = useNavigate();

    const startQuiz = () => {
        const initialUserAnswers = new Array(15).fill(null);
        setUserAnswers(initialUserAnswers);
        navigate('/Quiz/TakeQuiz/1', {state: {questions, descriptions, answers, userAnswers : initialUserAnswers, rightAnswers}});
    };

    const backToHomePage = () => {
        navigate('/');
    };

    useEffect(() =>{
        fetchQuizData();
    }, []);

    const fetchQuizData = async () => {
        try {
            const [questionsResponse, descriptionsResponse, answersResponse, rightAnswersResponse] = await Promise.all([
                fetch('http://localhost:5007/Quiz/questions'),
                fetch('http://localhost:5007/Quiz/descriptions'),
                fetch('http://localhost:5007/Quiz/answers'),
                fetch('http://localhost:5007/Quiz/rightAnswers')
            ]);

            if (!questionsResponse.ok || 
                !descriptionsResponse.ok || 
                !answersResponse.ok || 
                !rightAnswersResponse.ok)
                {
                throw new Error('Failed to fetch quiz data');
                }

            const [questionData, descriptionData, answersData, rightAnswersData] = await Promise.all([
                questionsResponse.json(),
                descriptionsResponse.json(),
                answersResponse.json(),
                rightAnswersResponse.json()
            ]);

            setQuestions(questionData);
            setDescriptions(descriptionData);
            setAnswers(answersData);
            setRightAnswers(rightAnswersData);
        }
        
        catch (error) 
        {
            console.error('Error fetching quiz data:', error.message);
            alert('Error fetching quiz data. Please try again later.')
        }
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
            <main className="button-container">
                <button onClick={startQuiz}>
                    Start Quiz
                </button>
            </main>
        </>
    );
}
