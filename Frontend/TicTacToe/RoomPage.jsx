import { useEffect, useState } from "react"
import { useNavigate } from "react-router-dom"
import NameComponent from "./Components/NameComponent"

export default function RoomPage()
{
    const [rooms, setRooms] = useState([])
    const [playerName, setPlayerName] = useState("")
    const navigate = useNavigate()

    // GET the rooms
    useEffect(() =>
    {
        fetch("http://localhost:5007/tictactoe/rooms")
        .then((response) => response.json())
        .then((data) => setRooms(data))
    }, [])

    return (
        <>
            {playerName !== "" &&
            <div>

                <header>
                    <h1 className="mediumHeader">Free Rooms</h1>
                    <button className="backButton" onClick={() => navigate("/TicTacToe")}>Back</button>
                </header>

                <div className="container">
                    {rooms.map((room, index) => (
                        <li className="NoListItemBullet" key={index}>
                            <button className="button" disabled={room.roomCapacity === 2}>{room.roomCapacity !== 2 ? "Free to join" : "Full"}</button>
                            <p className="yellowText">{room.roomCapacity} / 2</p>
                        </li>
                    ))}
                </div>
            </div>}

            {playerName === "" && <NameComponent setPlayerName={setPlayerName}/>}
        </>
    )
}