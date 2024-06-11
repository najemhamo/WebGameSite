import { useNavigate } from "react-router-dom";

export default function HomePage() {
  const navigate = useNavigate();

  return (
    <>
      <header>
        <h1>Web Game Site</h1>
      </header>

      <div className="container">
        <button className="button" onClick={() => navigate("/TicTacToe")}>
          Tic Tac Toe
        </button>
        <button className="button" onClick={() => navigate("/quiz")}>
          Quiz
        </button>
      </div>
    </>
  );
}
