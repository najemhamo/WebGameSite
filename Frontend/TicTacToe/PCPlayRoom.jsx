import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function PCPlayRoom(props)
{
    const {playerName} = props
    const navigate = useNavigate()
    const [board, setBoard] = useState(Array(9).fill(0))

    const makeMove = () =>
    {

    }

    const restartGame = () =>
    {

    }

    // POST leave room
    const leaveRoom = () =>
    {
        const deleteOptions = {method: "DELETE"};
        fetch(`http://localhost:5007/tictactoe/rooms/create`, deleteOptions) // CHANGE roomId ?
        .then(navigate("/TicTacToe"))
    }

    return (
        <div>
            <header>
                <h2 className="smallerHeader">Game Room</h2>
                <div className="nameContainer">
                    <p>{playerName}</p>
                    <p>VS</p>
                    <p>PC</p>
                </div>
            </header>

            <body className="waiting">
                <div className="grid">
                    {board.map((piece, index) => (
                        <button
                        key={index}
                        onClick={() => makeMove(index)}>{piece === 1 ? "O" : piece === 2 ? "X" : ""}
                        </button>
                    ))}
                </div>
            </body>

            <div className="gameFooter">
                <button onClick={restartGame}>Restart Game</button>
                <div className="buttonRight">
                    <button onClick={leaveRoom}>End Game</button>
                </div>
            </div>
        </div>
    )
}