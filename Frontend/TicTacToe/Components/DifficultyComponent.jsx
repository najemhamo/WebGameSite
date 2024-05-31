import { useNavigate } from "react-router-dom"
import ChooseName from "./NameComponent"

export default function ChooseDifficulty(props)
{
    const {playerName, setPlayerName} = props
    const navigate = useNavigate()

    const requestPC = (difficulty) =>
    {
        const postOptions = {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({
                playerName: playerName, // CHANGE fix this
                difficulty: difficulty
            })
        }

        fetch(`http://localhost:5007/tictactoe/rooms/create?playerName=${playerName}&difficulty=${difficulty}`, postOptions)
        .then((response) => response.json())
        .then((data) => {
            console.log("PC ROOM", data)
            navigate(`/TicTacToe/PC/${data.roomId}`)
        })
    }

    return (
        <>
            {playerName === "" && <ChooseName setPlayerName={setPlayerName}/>}
            {playerName !== "" &&
                <div>
                    <button onClick={() => requestPC("Easy")}>Easy</button>
                    <button onClick={() => requestPC("Hard")}>Hard</button>
                </div>
            }
        </>
    )
}