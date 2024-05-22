import { useEffect, useState } from "react";

export default function PlayroomPage()
{
    // const [socket] = useState(new WebSocket("ws://localhost:5007/tictactoe/connect"));

    return (
        <div>
            <h2>Game Room</h2>
            <p>Player Count:</p>
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