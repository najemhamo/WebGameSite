import { useNavigate } from "react-router-dom"

export default function HomePage()
{
    const navigate = useNavigate()

    return (
    <>
        <header>
            <h1>Web Game Site</h1>
        </header>

        <div>
            <ol>
                <li onClick={() => navigate("/TicTacToe")}>Tic Tac Toe</li>
                <li onClick={() => navigate("/Quiz")}>Quiz</li>
            </ol>
        </div>
    </>
    )
}