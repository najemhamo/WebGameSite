import './App.css'
import { Route, Routes } from 'react-router-dom'
import HomePage from './HomePage'
import TicTacToePage from '../TicTacToe/TicTacToePage'
import QuizHomePage from '../Quiz/QuizHomePage'
import QuestionPage from '../Quiz/QuestionPage'
import SubmissionPage from '../Quiz/SubmissionPage'
import ScorePage from '../Quiz/ScorePage'

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/*" element={<TicTacToePage />} />
        <Route path='/quiz' element={<QuizHomePage/>}/>
        <Route path='/quiz/takeQuiz/:questionId' element={<QuestionPage/>}/>
        <Route path='/quiz/submission' element={<SubmissionPage/>}/>
        <Route path='/quiz/scorePage' element={<ScorePage/>}/>
      </Routes>
    </>
  );
}

export default App;
