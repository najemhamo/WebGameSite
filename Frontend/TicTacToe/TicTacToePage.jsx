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
    const newSocket = new WebSocket(
      "wss://backendwebsocket.webpubsub.azure.com/client/hubs/Hub?access_token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJ3c3M6Ly9iYWNrZW5kd2Vic29ja2V0LndlYnB1YnN1Yi5henVyZS5jb20vY2xpZW50L2h1YnMvSHViIiwiaWF0IjoxNzE4MzU3MzQwLCJleHAiOjE3MTgzNjA5NDB9.E9ScmL8R1f6Xo9OOdaIzDXhrV9x1PbNOgJFMZ-hVp4w"
      //"wss://backendwebsocket.webpubsub.azure.com/tictactoe"
    );
    setSocket(newSocket);

    newSocket.onopen = () => {
      console.log("Connected to WebSocket server");
    };

    newSocket.onclose = () => {
      console.log("Disconnected from WebSocket server");
    };

    newSocket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      console.log(data);
      // handle received data
    };
    // return () => {
    //   newSocket.close();
    // };
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
