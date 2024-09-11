/* eslint-disable no-unused-vars */
import React, {useState, useEffect} from "react";
import { useNavigate } from "react-router-dom";

export default function QuizHomePage() {
    //declare constants
    const [questions, setQuestions] = useState([]);
    const [answers, setAnswers] = useState ([]);
    const [userAnswers, setUserAnswers] = useState([]);
    const [rightAnswers, setRightAnswers] = useState([]);
    const [time, setTime] = useState([]);

    //useNavigate, for navigating to other components
    const navigate = useNavigate();

    //function to start quiz, will navigate you to question 1
    const startQuiz = () => {

        //create two arrays with 15 null elements 
        const initialUserAnswers = new Array(15).fill(null);
        const initialUserTime = new Array(15).fill(null);

        //update state of userAnswers & time lists
        setUserAnswers(initialUserAnswers);
        setTime(initialUserTime);

        //navigate to first question
        navigate('/quiz/takeQuiz/1', {state: {questions, answers, userAnswers : initialUserAnswers, rightAnswers, time : initialUserTime}});
    };

    //navigate back to homePage
    const backToHomePage = () => {
        navigate('/');
    };

    //useEffect hook, to fetch quiz data
    useEffect(() =>{
        fetchQuizData();
    }, []);

    //function to decode HTML entities
    const decodeHtmlEntities = (text) => {
        const textarea = document.createElement('textarea');
        textarea.innerHTML = text;
        return textarea.value;
    };

    //async function to fetch data, using API request
    const fetchQuizData = async () => {
        try {

            const [questionsResponse, descriptionsResponse, answersResponse, rightAnswersResponse] = await Promise.all([
                fetch('http://localhost:5007/quiz/questions'),
                fetch('http://localhost:5007/quiz/descriptions'),
                fetch('http://localhost:5007/quiz/answers'),
                fetch('http://localhost:5007/quiz/rightAnswers')
            ]);

            //check if requests were 
            if (!questionsResponse.ok || 
                !descriptionsResponse.ok || 
                !answersResponse.ok || 
                !rightAnswersResponse.ok)
                {
                throw new Error('Failed to fetch quiz data');
                }

            //promise of all the data
            const [questionData, descriptionData, answersData, rightAnswersData] = await Promise.all([
                questionsResponse.json(),
                descriptionsResponse.json(),
                answersResponse.json(),
                rightAnswersResponse.json()
            ]);

            //update lists with the data fetced
            setQuestions(questionData);
            setDescriptions(descriptionData);
            setAnswers(answersData);
            setRightAnswers(rightAnswersData);

        }
    };

    //shuffle the order of given array (answers)
    const shuffleArray = (array) => {
        const shuffledArray = array.slice();

        for (let i = shuffledArray.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [shuffledArray[i], shuffledArray[j]] = 
            [shuffledArray[j], shuffledArray[i]];
        }
        return shuffledArray;
    };

    //append all elements to the lists
    const appendToLists = (fetchedData) => {
        const questionsList = [];
        const answersList = [];
        const rightAnswersList = [];

        //loop through fetched data
        fetchedData.forEach((element) => {
            questionsList.push(decodeHtmlEntities(element.question));
            rightAnswersList.push(decodeHtmlEntities(element.correct_answer));
            
            //fetch answers and shuffle order
            const answerAlternatives = 
            shuffleArray([...element.incorrect_answers.map(decodeHtmlEntities), decodeHtmlEntities(element.correct_answer)]);
            answersList.push(answerAlternatives);
        });

        //update state for all lists
        setQuestions(questionsList);
        setAnswers(answersList);
        setRightAnswers(rightAnswersList);
    };

    //Page:
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
