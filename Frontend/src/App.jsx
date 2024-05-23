import './App.css'
import { Route, Routes } from 'react-router-dom'
import HomePage from './HomePage'
import TicTacToePage from '../TicTacToe/TicTacToePage'
import TicTacToeHomePage from '../TicTacToe/TicTacToeHomePage'
import RoomPage from '../TicTacToe/RoomPage'
import PlayroomPage from '../TicTacToe/PlayroomPage'
import QuizHomePage from '../Quiz/QuizHomePage'

function App() {

  return (
    <>
      <Routes>
        <Route path='/' element={<HomePage/>}/>
        <Route path='/*' element={<TicTacToePage/>}/>
      </Routes>
        <Route path='/TicTacToe' element={<TicTacToeHomePage/>}/>
        <Route path='/TicTacToe/Rooms' element={<RoomPage/>}/>
        <Route path='/TickTacToe/PC' element={<PlayroomPage/>}/>
        <Route path='/Quiz' element={<QuizHomePage/>}/>
      </Routes>    
    </>
  )
}

export default App
