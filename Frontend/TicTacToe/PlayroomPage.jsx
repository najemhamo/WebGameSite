import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

export default function PlayroomPage(props)
{
    const {socket, playerName} = props
    const { roomId } = useParams();
    const navigate = useNavigate()

    const [id, setId] = useState(0)
    const [board, setBoard] = useState([])
    const [player, setPlayer] = useState(0)
    const [winner, setWinner] = useState(null)
    const [players, setPlayers] = useState([])
    const [canStart, setCanStart] = useState(false)
    const [currentPlayer, setCurrentPlayer] = useState(0)
    
    const [score, setScore] = useState([])
    const [restart, setRestart] = useState(false)
    const [restartText, setRestartText] = useState("")

    // GET the room
    useEffect(() =>
    {
        fetch(`http://localhost:5007/tictactoe/rooms/${roomId}`)
        .then((response) => response.json())
        .then((data) => {

            setId(data.id)
            setBoard(data.board)
            // setScore(data.score)

            const tmpPlayers = [data.playerO || "", data.playerX || ""]
            setPlayers(tmpPlayers)

            if (data.roomCapacity === 2)
            {
                if (socket.readyState === WebSocket.OPEN)
                    socket.send(JSON.stringify({type: "readyToStart", players: tmpPlayers}))
                
                if (playerName === data.playerX)
                    setPlayer(1)

                if (data.winner)
                    setWinner(data.winner)
                
                setCanStart(true)
                checkBoard(data.board)
            }
        })
    }, [])

    // Checks for which player's turn it is
    const checkBoard = (board) =>
    {
        let Xes = 0
        let Oes = 0

        board.map((piece) => {
            if (piece === 1) Oes++
            else if (piece === 2) Xes++
        })

        if (Oes > Xes)
            setCurrentPlayer(1)
    }

    // Socket listening
    socket.onmessage = function (event)
    {
        const messageObj = JSON.parse(event.data)
        
        // When the game can start
        if (messageObj.type === "readyToStart")
        {
            // If a new player has joined the room
            if (messageObj.players[1] !== players[1])
            {
                socket.send(JSON.stringify({type: "restart!"}))
                setPlayers(messageObj.players)
                setRestartText("")
                restartGame()
            }

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
            setScore(messageObj.score)
        }

        // When a player wants to restart the game
        else if (messageObj.type === "restart?")
        {
            setRestartText("1/2")
            setRestart(true)
        }

        // When the board should restart
        else if (messageObj.type === "restart!")
        {
            setRestartText("")
            restartGame()
        }
    }

    const tryRestart = () =>
    {
        if (!restart)
        {
            socket.send(JSON.stringify({type: "restart?"}))
            setRestartText("1/2")
            setRestart(true)
        }
        else
        {
            socket.send(JSON.stringify({type: "restart!"}))
            setRestartText("")
            restartGame()
        }
    }

    // POST restart game
    const restartGame = () =>
    {
        const postOptions = {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({
                roomId: roomId
            })
        }
        
        fetch(`http://localhost:5007/tictactoe/rooms/${roomId}/reset`, postOptions)
        .then((response) => response.json())
        .then((data) => {
            setBoard(data.board)
            setScore(data.score)
            setWinner(data.winner)
            setCurrentPlayer(0)
            setRestart(false)
        })
    }

    // POST leave room
    const endGame = () =>
    {
        const postOptions = {method: "POST"};
        fetch(`http://localhost:5007/tictactoe/rooms/${roomId}/leave`, postOptions)
        .then(() => {
            socket.send(JSON.stringify({type: "leaveRoom", id: roomId}))
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
        
        fetch(`http://localhost:5007/tictactoe/rooms/${roomId}/MultiPlayerMove`, postOptions)
        .then(() => {
            socket.send(JSON.stringify({type: "madeMove", place: index}))

            fetch(`http://localhost:5007/tictactoe/rooms/${roomId}`) // CHANGE discuss this
            .then((response) => response.json())
            .then((data) => {
                setBoard(data.board)

                if (data.winner)
                {
                    setWinner(data.winner)

                    let tmpScore = [...score]
                    if (data.winner === "O")
                        tmpScore[0]++
                    else
                        tmpScore[1]++

                    setScore(tmpScore)

                    socket.send(JSON.stringify({
                    type: "haveWon",
                    winner: data.winner,
                    board: data.board,
                    score: data.score
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
                    <p>{score[0]}</p>
                    <p id={winner === "O" ? "redText" : ""}>Player O: {players[0]}</p>
                    <p>VS</p>
                    <p id={winner === "X" ? "redText" : ""}>Player X: {players[1]}</p>
                    <p>{score[1]}</p>
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
                <div className="restartText">

                <p>{restartText}</p>
                </div>
                <button onClick={tryRestart}>Restart Game</button>
                <div className="buttonRight">
                    <button onClick={endGame}>End Game</button>
                </div>
            </div>
        </div>
    )
}