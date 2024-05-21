import { useNavigate } from "react-router-dom"

export default function HomePage()
{
    const navigate = useNavigate()

    return (
    <>
        <header>
            <h1>Web Game Site</h1>
        </header>

        <body>
            <ol>
                <li onClick={() => navigate("/TicTacToe")}>Tic Tac Toe</li>
                <li>Quiz</li> {/* Use this for getting to the Quiz */}
            </ol>
        </body>
    </>
    )
}