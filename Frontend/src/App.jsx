import './App.css'
import { Route, Routes } from 'react-router-dom'
import HomePage from './HomePage'
import TicTacToePage from '../TicTacToe/TicTacToePage'

function App() {

  return (
    <>
      <Routes>
        <Route path='/' element={<HomePage/>}/>
        <Route path='/*' element={<TicTacToePage/>}/>
      </Routes>
    </>
  )
}

export default App
