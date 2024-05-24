import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

export default function PlayroomPage(props)
{
    const { roomId } = useParams();
    const {socket} = props

    const navigate = useNavigate()
    const [board, setBoard] = useState(Array(9).fill(0))
    const [canStart, setCanStart] = useState(false)
    const [player, setPlayer] = useState(0)
    const [currentPlayer, setCurrentPlayer] = useState(0)

    // GET the room
    useEffect(() =>
    {
        if (canStart) // CHANGE fix the restart of page "GET BOARD" API ?
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
                setPlayer(1)
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
        else if (messageObj.type === "leaveRoom")
            setCanStart(false)

        // When the board should be updated with the new move
        else if (messageObj.type === "madeMove")
        {
            let newBoard = [...board]
            newBoard[messageObj.place] = messageObj.player + 1
            setCurrentPlayer((messageObj.player + 1) % 2)
            setBoard(newBoard)
        }
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

    // POST make a move
    const makeMove = (index) =>
    {
        let tmpBoard = board
        tmpBoard[index] = currentPlayer + 1
        
        const postOptions = {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({
                roomId: roomId,
                board: tmpBoard,
                gameState: 0,
                player: player === 0 ? "O" : "X"
            })
        }
        
        fetch(`http://localhost:5007/tictactoe/rooms/${roomId}/move`, postOptions)
        .then(() => {
            socket.send(JSON.stringify({
            type: "madeMove",
            place: index,
            player: currentPlayer,
            }))
        })

        setCurrentPlayer((currentPlayer + 1) % 2)
        setBoard(tmpBoard)
    }

    return (
        <div>
            <h2>Game Room *id*</h2>
            {/* CHANGE add a short id to the room */}
            
            <div className="grid">
                {canStart && board.map((place, index) => (
                        <button
                            key={index}
                            className={currentPlayer !== player ? "disabled" : ""}
                            disabled={currentPlayer !== player}
                            onClick={() => makeMove(index)}>{board[index] === 1 ? "O" : board[index] === 2 ? "X" : ""}
                        </button>
                    ))}
            </div>

            {!canStart && <p>Waiting for another player...</p>}
            <button onClick={leaveRoom}>End Game</button>
        </div>
    )
}