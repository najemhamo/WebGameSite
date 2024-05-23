import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

export default function PlayroomPage()
{
    const { roomId } = useParams();
    const navigate = useNavigate()

    // POST leave room
    const leaveRoom = () =>
    {
        const postOptions = {method: "POST"};

        fetch(`http://localhost:5007/tictactoe/rooms/${roomId}/leave`, postOptions)
        .then(navigate("/TicTacToe/Rooms"))
    }

    return (
        <div>
            <h2>Game Room</h2>
            <p>Player Count:</p>
            <button onClick={leaveRoom}>Leave Room</button>
            {/* <div style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: '10px' }}>
                {gameState.map((row, x) =>
                    row.map((cell, y) => (
                <div key={`${x}-${y}`} style={{ width: '50px', height: '50px' }}>
                    {renderCell(cell, x, y)}
                </div>
                )))}
            </div> */}
        </div>
    )
}