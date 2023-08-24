import "./BlueButton.css";

function BlueButton({ children, submit, heightInPx, widthInPx }) {
  return (
    <button
      className="blue-button"
      type={submit ? "submit" : ""}
      style={{ height: heightInPx, width: widthInPx }}
    >
      {children}
    </button>
  );
}

export default BlueButton;
