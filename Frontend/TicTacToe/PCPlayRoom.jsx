import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

export default function PCPlayRoom(props)
{
    const {playerName} = props
    const { roomId } = useParams();
    const navigate = useNavigate()

    const [board, setBoard] = useState(Array(9).fill(0))
    const [winner, setWinner] = useState(null)

    const makeMove = (index) =>
    {
        let tmpBoard = board
        tmpBoard[index] = "O"
        
        const postOptions = {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({
                roomId: roomId,
                board: tmpBoard,
                gameState: 0, // CHANGE remove this
                player: "O"
            })
        }
        
        fetch(`http://localhost:5007/tictactoe/rooms/${roomId}/move`, postOptions)
        .then(() => {

            fetch(`http://localhost:5007/tictactoe/rooms/${roomId}`)
            .then((response) => response.json())
            .then((data) => {
                console.log("WINNER IN PCPLAY", data)
                setBoard(data.board)

                if (data.winner)
                    setWinner(data.winner)
            })
        })
    }

    const restartGame = () =>
    {

    }

    // POST leave room
    const leaveRoom = () =>
    {
        const deleteOptions = {method: "DELETE"};
        fetch(`http://localhost:5007/tictactoe/rooms/${roomId}/delete`, deleteOptions)
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

            <body className="gameBody">
                <div className="grid">
                    {board.map((piece, index) => (
                        <button
                        key={index}
                        id={piece === 3 ? "redText" : ""}
                        className={winner ? "disabled playButton" : "playButton"}
                        disabled={winner}
                        onClick={() => makeMove(index)}>{piece === 1 ? "O" : piece === 2 ? "X" : piece === 3 ? winner : ""}
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