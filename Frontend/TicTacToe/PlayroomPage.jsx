import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

export default function PlayroomPage(props)
{
    const { roomId, playerName } = useParams();
    const {socket} = props

    const navigate = useNavigate()
    const [id, setId] = useState(0)
    const [board, setBoard] = useState([])
    const [player, setPlayer] = useState(0) // CHANGE clean this up!
    const [winner, setWinner] = useState(null)
    const [players, setPlayers] = useState([])
    const [canStart, setCanStart] = useState(false)
    const [currentPlayer, setCurrentPlayer] = useState(0)

    // GET the room
    useEffect(() =>
    {
        fetch(`http://localhost:5007/tictactoe/rooms/${roomId}`)
        .then((response) => response.json())
        .then((data) => {

            setBoard(data.board)
            setId(data.id)
            const tmpPlayers = [data.playerO || "", data.playerX || ""]
            setPlayers(tmpPlayers)

            if (data.roomCapacity === 2)
            {
                if (socket.readyState === WebSocket.OPEN)
                    socket.send(JSON.stringify({type: "readyToStart", players: tmpPlayers}))
                
                if (playerName[0] === "X")
                    setPlayer(1)

                if (data.winner)
                    setWinner(data.winner)
                
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
        {
            setPlayers(messageObj.players)
            setCanStart(true)
        }

        // When a player has left the room
        else if (messageObj.type === "leaveRoom")
            setCanStart(false)

        // When the board should be updated with the new move
        else if (messageObj.type === "madeMove")
        {
            let newBoard = [...board]
            newBoard[messageObj.place] = currentPlayer + 1
            setCurrentPlayer((currentPlayer + 1) % 2)
            setBoard(newBoard)
        }

        // When a player has won the game
        else if (messageObj.type === "haveWon")
        {
            setWinner(messageObj.winner)
            setBoard(messageObj.board)
        }
    }

    const restartGame = () =>
    {
        // CHANGE implement restart of game
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
                gameState: 0, // CHANGE remove this
                player: player === 0 ? "O" : "X"
            })
        }
        
        fetch(`http://localhost:5007/tictactoe/rooms/${roomId}/move`, postOptions)
        .then(() => {
            socket.send(JSON.stringify({
            type: "madeMove",
            place: index
            }))

            fetch(`http://localhost:5007/tictactoe/rooms/${roomId}`) // CHANGE discuss this
            .then((response) => response.json())
            .then((data) => {
                console.log("DATA", data)
                setBoard(data.board)

                if (data.winner)
                {
                    setWinner(data.winner)
                    socket.send(JSON.stringify({
                    type: "haveWon",
                    winner: data.winner,
                    board: data.board
                    }))
                }
            })
        })

        setCurrentPlayer((currentPlayer + 1) % 2)
    }

    return (
        <div>
            <header>
                <h2 className="smallerHeader">Game Room {id}</h2>
                <div className="nameContainer">
                    <p id={winner ? "redText" : ""}>Player O{players[0]}</p>
                    <p>VS</p>
                    <p id={winner ? "redText" : ""}>{players[1]}</p>
                    {/* CHANGE fix this to correct player*/}
                </div>
            </header>

            <body className={canStart ? "gameBody" : "waiting"}>
                <div className="grid">
                    {canStart && board.map((piece, index) => (
                        <button
                        key={index}
                        id={piece === 3 ? "redText" : ""}
                        className={currentPlayer !== player || winner ? "disabled playButton" : "playButton"}
                        disabled={currentPlayer !== player || winner}
                        onClick={() => makeMove(index)}>{piece === 1 ? "O" : piece === 2 ? "X" : piece === 3 ? winner : ""}
                        </button>
                    ))}
                </div>
                
                {!canStart && <p>Waiting for another player...</p>}
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