import { useNavigate } from "react-router-dom";
import ChooseName from "./NameComponent";

export default function ChooseDifficulty(props) {
  const { playerName, setPlayerName } = props;
  const navigate = useNavigate();

  const requestPC = (difficulty) => {
    const postOptions = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        playerName: playerName,
        difficulty: difficulty,
      }),
    };

    fetch(
      `https://backend20240610112356.azurewebsites.net/tictactoe/rooms/create?playerName=${playerName}&difficulty=${difficulty}`,
      postOptions
    )
      .then((response) => response.json())
      .then((data) => navigate(`/TicTacToe/PC/${data.roomId}`));
  };

  return (
    <>
      {playerName === "" && <ChooseName setPlayerName={setPlayerName} />}
      {playerName !== "" && (
        <div className="container">
          <header>
            <button
              className="backButton"
              onClick={() => navigate("/TicTacToe")}
            >
              Back
            </button>
          </header>
          <header>
            <h1 className="smallerHeader">Choose Your Game Difficulty</h1>

            <button className="button" onClick={() => requestPC("Easy")}>
              Easy
            </button>
            <button className="button" onClick={() => requestPC("Hard")}>
              Hard
            </button>
          </header>
        </div>
      )}
    </>
  );
}
