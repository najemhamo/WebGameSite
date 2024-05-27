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
        <Route path='/Quiz' element={<QuizHomePage/>}/>
        <Route path='/Quiz/TakeQuiz' element={<QuestionPage/>}/>
        <Route path='/Quiz/Submission' element={<SubmissionPage/>}/>
        <Route path='/Quiz/ScorePage' element={<ScorePage/>}/>
      </Routes>
    </>
  );
}

export default App;
