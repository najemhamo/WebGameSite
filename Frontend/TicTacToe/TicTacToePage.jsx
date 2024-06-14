import { Route, Routes } from "react-router-dom";
import { useState, useEffect } from "react";
import TicTacToeHomePage from "./TicTacToeHomePage";
import PlayroomPage from "./PlayroomPage";
import PCPlayRoom from "./PCPlayRoom";
import RoomPage from "./RoomPage";
import ChooseDifficulty from "./Components/DifficultyComponent";

export default function TicTacToePage() {
  const [socket, setSocket] = useState(null);
  const [playerName, setPlayerName] = useState("");

  useEffect(() => {
    fetch("http://localhost:5007/tictactoe")
      .then((response) => response.json())
      .then((data) => setSocket(data.uri));
  }, []);

  return (
    <>
      <Routes>
        <Route path="/TicTacToe" element={<TicTacToeHomePage />} />
        <Route
          path="/TicTacToe/Rooms"
          element={
            <RoomPage
              socket={socket}
              playerName={playerName}
              setPlayerName={setPlayerName}
            />
          }
        />
        <Route
          path="/TicTacToe/:roomId"
          element={<PlayroomPage socket={socket} playerName={playerName} />}
        />
        <Route
          path="/TicTacToe/PC"
          element={
            <ChooseDifficulty
              playerName={playerName}
              setPlayerName={setPlayerName}
            />
          }
        />
        <Route
          path="/TicTacToe/PC/:roomId"
          element={<PCPlayRoom playerName={playerName} socket={socket} />}
        />
      </Routes>
    </>
  );
}
