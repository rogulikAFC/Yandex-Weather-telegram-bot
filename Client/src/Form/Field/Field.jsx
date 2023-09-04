import "./Field.css";

function Field({ title, register, error, placeholder}) {
  return (
    <label className={`field form__field${error ? " field_error" : ""}`}>
      {error}
      
      <input type="text" className="field__input" placeholder={placeholder} {...register}/>
      
      <span className="field__title">{title}</span>
    </label>
  );
}

export default Field;
