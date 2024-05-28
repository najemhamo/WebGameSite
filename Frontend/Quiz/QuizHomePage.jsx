import React, {useState, useEffect} from "react";
import { useNavigate } from "react-router-dom";

export default function QuizHomePage() {
    const [questions, setQuestions] = useState([]);
    const [descriptions, setDescriptions] = useState([]);
    const [answers, setAnswers] = useState ([]);
    // const [userAnswers, setUserAnswers] = useState([]);
    // const [score, setScore] = useState(null);

    const navigate = useNavigate();

    const startQuiz = () => {
        navigate('/Quiz/TakeQuiz');
    };

    const backToHomePage = () => {
        navigate('/');
    };

    useEffect(() =>{
        fetchQuizData();
    }, []);

    const fetchQuizData = async () => {
        try {
            const [questionsResponse, descriptionsResponse, answersResponse] = await Promise.all([
                fetch('http://localhost:5007/Quiz/questions'),
                fetch('http://localhost:5007/Quiz/descriptions'),
                fetch('http://localhost:5007/Quiz/answers')
            ]);

            if (!questionsResponse.ok || !descriptionsResponse.ok || !answersResponse.ok)
                {
                throw new Error('Failed to fetch quiz data');
                }

            const [questionData, descriptionData, answersData] = await Promise.all([
                questionsResponse.json(),
                descriptionsResponse.json(),
                answersResponse.json()
            ]);

            setQuestions(questionData);
            setDescriptions(descriptionData);
            setAnswers(answersData);

            console.log('Questions:', questionData);
            console.log('Descriptions:', descriptionData);
            console.log('Answers:', answersData);
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
            <main>
                <button onClick={startQuiz}>
                    Start Quiz
                </button>
            </main>
        </>
    );
}
