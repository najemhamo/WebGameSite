import { useNavigate } from "react-router-dom"
import RoomComponent from "./Components/RoomComponent"

export default function TicTacToeHomePage()
{
    const navigate = useNavigate()

    return (
        <>
            <header>
                {/* <button onClick={() => navigate("/")}>Go back</button>
                <h1>Tic Tac Toe</h1> */}
            </header>
            
            <body>
                <div>
                    <RoomComponent/>
                    {/* <button>Play with PC</button>
                    <button>Play with friends</button> */}
                </div>
            </body>
        </>
    )
}