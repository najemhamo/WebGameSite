import { useState } from "react"

export default function NameComponent(props)
{
    const {setPlayerName} = props
    const [errorMessage, setErrorMessage] = useState(" ")

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
            <div>
                <h1>Enter your name:</h1>
                <form onSubmit={checkName}>
                    <input type="text" name="username" placeholder="Your name"></input>
                    <button type="submit">Submit</button>
                </form>
                {errorMessage !== "" && <p>{errorMessage}</p>}
            </div>
        </>
    )
}