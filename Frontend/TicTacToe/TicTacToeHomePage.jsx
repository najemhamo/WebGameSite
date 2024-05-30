import { useNavigate } from "react-router-dom"

export default function TicTacToeHomePage()
{
    const navigate = useNavigate()

    const requestPC = () =>
    {
        const postOptions = {method: "POST"};
        fetch(`http://localhost:5007/tictactoe/rooms/create`, postOptions)
        .then((response) => response.json())
        .then((data) => {
            console.log("PC ROOM", data)
            navigate(`/TicTacToe/PC/${data.roomId}`)
        })
    }

    return (
        <>
            <header>
                <button className="backButton" onClick={() => navigate("/")}>Back</button>
                <h1 className="header">Tic Tac Toe</h1>
            </header>
            <div className="container">
                <button className="button" onClick={requestPC}>Play with PC</button>
                <button className="button" onClick={() => navigate("/TicTacToe/Rooms")}>Play with friends</button>
            </div>
        </>
    )
}