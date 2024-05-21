import { useState } from "react"

export default function NameComponent()
{
    const [errorMessage, setErrorMessage] = useState("")

    const submitName = (event) =>
    {
        event.preventDefault()
        if (event.target.value === "" || event.target.value === undefined)
            setErrorMessage("Must provide a name")
    }

    return (
        <>
            <div>
                <h1>Enter your name:</h1>
                <form onSubmit={submitName}>

                    <input type="text" placeholder="Your name"></input>
                    <input type="submit" value="Submit" ></input>
                </form>
                {errorMessage !== "" && <p>{errorMessage}</p>}
            </div>
        </>
    )
}