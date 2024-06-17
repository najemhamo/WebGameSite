import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

export default function PCPlayRoom(props) {
  const { playerName } = props;
  const { roomId } = useParams();
  const navigate = useNavigate();

  const [board, setBoard] = useState(Array(9).fill(0));
  const [winner, setWinner] = useState(null);
  const [score, setScore] = useState([0, 0]);

  const makeMove = (index) => {
    let tmpBoard = board;
    tmpBoard[index] = 1;

    const postOptions = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        roomId: roomId,
        board: tmpBoard,
        gameState: 0, // CHANGE remove this
        player: "O",
      }),
    };

    fetch(
      `http://localhost:5007/tictactoe/rooms/${roomId}/SinglePlayerMove`,
      postOptions
    ).then(() => {
      fetch(`http://localhost:5007/tictactoe/rooms/${roomId}`) // CHANGE discuss this
        .then((response) => response.json())
        .then((data) => {
          setBoard(data.board);
          if (data.winner)
          {
            let tmpScore = [...score];
            if (data.winner === "O") tmpScore[0]++;
            else tmpScore[1]++;
            
            setScore(tmpScore);
            setWinner(data.winner)
          }
        });
    });
  };

  const restartGame = () => {
    const postOptions = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        roomId: roomId,
      }),
    };

    fetch(`http://localhost:5007/tictactoe/rooms/${roomId}/reset`, postOptions)
      .then((response) => response.json())
      .then((data) => {
        setBoard(data.board);
        setWinner(data.winner);
      });
  };

  // POST leave room
  const leaveRoom = () => {
    const deleteOptions = { method: "DELETE" };
    fetch(
      `http://localhost:5007/tictactoe/rooms/${roomId}/delete`,
      deleteOptions
    ).then(navigate("/TicTacToe"));
  };

  return (
    <div>
      <header>
        <h2 className="smallerHeader">Game Room</h2>
        <div className="nameContainer">
          <p>{score[0]}</p>
          <p>{playerName}</p>
          <p>VS</p>
          <p>PC</p>
          <p>{score[1]}</p>
        </div>
      </header>

      <body className="gameBody">
        <div className="grid">
          {board.map((piece, index) => (
            <button
              key={index}
              id={piece === 3 ? "redText" : ""}
              className={
                winner || piece !== 0 ? "disabled playButton" : "playButton"
              }
              disabled={winner || piece !== 0}
              onClick={() => makeMove(index)}
            >
              {piece === 1
                ? "O"
                : piece === 2
                ? "X"
                : piece === 3
                ? winner
                : ""}
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
  );
}
