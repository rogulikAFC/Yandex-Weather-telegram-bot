import "./Button.css";

function Button({ children, submit, heightInPx, widthInPx, color, block, onClick, other }) {
  return (
    <button
      className={`button ${block}__button button_${color}`}
      type={submit ? "submit" : ""}
      style={{ height: heightInPx, width: widthInPx }}
      onClick={onClick}
      {...other}
    >
      {children}
    </button>
  );
}

export default Button;
