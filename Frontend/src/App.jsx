import './App.css'
import { Route, Routes } from 'react-router-dom'
import HomePage from './HomePage'
import TicTacToeHomePage from '../TicTacToe/TicTacToeHomePage'

function App() {

  return (
    <>
      <Routes>
        <Route path='/' element={<HomePage/>}/>
        <Route path='/TicTacToe' element={<TicTacToeHomePage/>}/>
      </Routes>    
    </>
  )
}

export default App