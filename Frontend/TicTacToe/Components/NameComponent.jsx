import { useState } from "react"
import { useNavigate } from "react-router-dom"

export default function NameComponent(props)
{
    const {setPlayerName} = props
    const [errorMessage, setErrorMessage] = useState(" ")
    const navigate = useNavigate()

    const checkName = (event) =>
    {
        event.preventDefault()
        const value = event.target.username.value

        if (value === undefined || value.length === 0 || value[0] === ' ')
            setErrorMessage("Must provide a name")
        else
            setPlayerName(value)
    }

    return (
        <>
            <header>
                <button className="backButton" onClick={() => navigate("/TicTacToe")}>Back</button>
            </header>
            <div>
                <h1 className="smallerHeader">Enter your name</h1>
                <form onSubmit={checkName}>
                    <input className="nameInput" type="text" name="username" placeholder="Your name"></input>
                    <button className="nameButton" type="submit">Submit</button>
                </form>
                {errorMessage !== "" && <p>{errorMessage}</p>}
            </div>
        </>
    )
}