import "./Field.css";

function Field({ title, placeholder, hasError }) {
  return (
    <div className={`field form__field${hasError ? " field_error" : ""}`}>
      <input type="text" className="field__input" placeholder={placeholder} />
      
      <span className="field__title">{title}</span>
    </div>
  );
}

export default Field;
