import './App.css'
import { Route, Routes } from 'react-router-dom'
import HomePage from './HomePage'
import TicTacToeHomePage from '../TicTacToe/TicTacToeHomePage'
import RoomPage from '../TicTacToe/RoomPage'

function App() {

  return (
    <>
      <Routes>
        <Route path='/' element={<HomePage/>}/>
        <Route path='/TicTacToe' element={<TicTacToeHomePage/>}/>
        <Route path='/TicTacToe/Rooms' element={<RoomPage/>}/>
      </Routes>    
    </>
  )
}

export default App