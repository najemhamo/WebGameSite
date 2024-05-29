import { useNavigate } from "react-router-dom"

export default function TicTacToeHomePage()
{
    const navigate = useNavigate()

    const requestPC = () =>
    {
        const postOptions = {method: "POST"};
        fetch(`http://localhost:5007/tictactoe/rooms/create`, postOptions)
        .then(navigate("/TicTacToe/PC"))
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