import { Route, Routes } from 'react-router-dom'
import { useState } from 'react';
import TicTacToeHomePage from './TicTacToeHomePage'
import PlayroomPage from './PlayroomPage'
import PCPlayRoom from './PCPlayRoom';
import RoomPage from './RoomPage'

export default function TicTacToePage()
{
    const [socket] = useState(new WebSocket("ws://localhost:5007/tictactoe"));
    const [playerName, setPlayerName] = useState("KLARA")

    return (
        <>
        <Routes>
            <Route path='/TicTacToe' element={<TicTacToeHomePage/>}/>
            <Route path='/TicTacToe/Rooms' element={<RoomPage socket={socket} playerName={playerName} setPlayerName={setPlayerName}/>}/>
            <Route path='/TicTacToe/:roomId/:playerName' element={<PlayroomPage socket={socket}/>}/>
            <Route path='/TicTacToe/PC/:roomId' element={<PCPlayRoom playerName={playerName}/>}/>
        </Routes>
        </>
    )
}