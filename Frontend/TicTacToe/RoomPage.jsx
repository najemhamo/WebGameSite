import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import ChooseName from "./Components/NameComponent";

export default function RoomPage(props) {
  const { socket, playerName, setPlayerName } = props;
  const [rooms, setRooms] = useState([]);
  const navigate = useNavigate();

  // GET the rooms
  useEffect(() => {
    fetch("https://backend20240610112356.azurewebsites.net/tictactoe/rooms")
      .then((response) => response.json())
      .then((data) => setRooms(data));
  }, []);

  // POST join room
  const joinRoom = (roomId) => {
    const postOptions = { method: "POST" };
    fetch(
      `https://backend20240610112356.azurewebsites.net/tictactoe/rooms/${roomId}`,
      postOptions
    ).then(() => {
      socket.send(JSON.stringify({ type: "joinRoom", id: roomId }));
      navigate(`/TicTacToe/${roomId}`);
    });
  };

  // Socket listening
  socket.onmessage = function (event) {
    const messageObj = JSON.parse(event.data);

    // Shows an increased room capacity
    if (messageObj.type === "joinRoom") {
      if (rooms.length === 0) {
        setRooms((rooms) =>
          rooms.map((room) => {
            if (room.roomId === messageObj.id) room.roomCapacity++;
            return room;
          })
        );
      } else {
        const newRooms = rooms.map((room) => {
          if (room.roomId === messageObj.id) room.roomCapacity++;
          return room;
        });
        setRooms(newRooms);
      }
    }

    // Shows a decreased room capacity
    else if (messageObj.type === "leaveRoom") {
      if (rooms.length === 0) {
        setRooms((rooms) =>
          rooms.map((room) => {
            if (room.roomId === messageObj.id) room.roomCapacity--;
            return room;
          })
        );
      } else {
        const newRooms = rooms.map((room) => {
          if (room.roomId === messageObj.id) room.roomCapacity--;
          return room;
        });
        setRooms(newRooms);
      }
    }
  };

  const goHome = () => {
    setPlayerName("");
    navigate("/TicTacToe");
  };

  return (
    <>
      {playerName !== "" && (
        <div>
          <header>
            <h1 className="mediumHeader">Free Rooms</h1>
            <button className="backButton" onClick={goHome}>
              Back
            </button>
          </header>

          <div className="container">
            {rooms.map((room, index) => (
              <li className="NoListItemBullet" key={index}>
                <button
                  onClick={() => joinRoom(room.roomId)}
                  className={
                    room.roomCapacity > 1 ? "disabled button" : "button"
                  }
                  disabled={room.roomCapacity > 1}
                >
                  {room.roomCapacity < 2 ? "Free to join" : "Full"}
                </button>
                <p className="yellowText">{room.roomCapacity} / 2</p>
              </li>
            ))}
          </div>
        </div>
      )}

      {playerName === "" && <ChooseName setPlayerName={setPlayerName} />}
    </>
  );
}
