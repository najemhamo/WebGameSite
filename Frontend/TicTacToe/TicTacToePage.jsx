import { Route, Routes } from 'react-router-dom'
import { useState } from 'react';
import TicTacToeHomePage from './TicTacToeHomePage'
import RoomPage from './RoomPage'
import PlayroomPage from './PlayroomPage'

export default function TicTacToePage()
{
    const [socket] = useState(new WebSocket("ws://localhost:5007/tictactoe"));

    return (
        <>
        <Routes>
            <Route path='/TicTacToe' element={<TicTacToeHomePage/>}/>
            <Route path='/TicTacToe/Rooms' element={<RoomPage socket={socket}/>}/>
            <Route path='/TicTacToe/:roomId' element={<PlayroomPage socket={socket}/>}/>
        </Routes>
        </>
    )
}