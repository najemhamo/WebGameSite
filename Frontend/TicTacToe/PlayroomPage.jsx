import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

export default function PlayroomPage(props)
{
    const { roomId } = useParams();
    const {socket} = props

    const navigate = useNavigate() // CHANGE to use Context ?
    const [board, setBoard] = useState(Array(3).fill(Array(3).fill(0)));
    const [canStart, setCanStart] = useState(false)

    // GET the rooms
    useEffect(() =>
    {
        if (canStart)
            return

        fetch(`http://localhost:5007/tictactoe/rooms/${roomId}`)
        .then((response) => response.json())
        .then((data) => {
            if (data.roomCapacity === 2)
            {
                socket.send(JSON.stringify({
                type: "readyToStart"
                }))
        
                setCanStart(true)
            }
        })
    }, [])

    // Socket listening
    socket.onmessage = function (event)
    {
        const messageObj = JSON.parse(event.data)
        
        // When the game can start
        if (messageObj.type === "readyToStart")
            setCanStart(true)

        // When a player has left the room
        if (messageObj.type === "leaveRoom")
            setCanStart(false)
    }

    // POST leave room
    const leaveRoom = () =>
    {
        const postOptions = {method: "POST"};
        fetch(`http://localhost:5007/tictactoe/rooms/${roomId}/leave`, postOptions)
        .then(() => {
            socket.send(JSON.stringify({
            type: "leaveRoom",
            id: roomId,
            }))
            navigate("/TicTacToe/Rooms")
        })
    }

    return (
        <div>
            <h2>Game Room</h2>
            
            <div className="grid">
                {canStart && board.map((row, x) => row.map((cell, y) => (
                    <div key={`${x}-${y}`} className="gridStyle"> 
                        <button></button>
                        {/* CHANGE use buttons or just a grid ? */}
                    </div>
                )))}
            </div>

            <button onClick={leaveRoom}>End Game</button>
            {!canStart && <p>Waiting for another player...</p>}
        </div>
    )
}