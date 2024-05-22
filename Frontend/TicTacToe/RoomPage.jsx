import { useEffect, useState } from "react"
import NameComponent from "./Components/NameComponent"

export default function RoomPage()
{
    const [rooms, setRooms] = useState([])
    const [playerName, setPlayerName] = useState("")

    // GET the rooms
    useEffect(() =>
    {
        fetch("https://localhost:7211/tictactoe/rooms")
        .then((response) => response.json())
        .then((data) => setRooms(data))
    }, [])

    return (
        <>
            {playerName !== "" &&
            <div>

                <header>
                    <h1>Free Rooms</h1>
                </header>

                <body>
                    {rooms.map((room, index) => (
                        <li className="NoListItemBullet" key={index}>
                            <button disabled={room.roomCapacity === 2}>{room.roomCapacity !== 2 ? "Free to join" : "Full"}</button>
                            <p>{room.roomCapacity} / 2</p>
                        </li>
                    ))}
                </body>
            </div>}

            {playerName === "" && <NameComponent setPlayerName={setPlayerName}/>}
        </>
    )
}