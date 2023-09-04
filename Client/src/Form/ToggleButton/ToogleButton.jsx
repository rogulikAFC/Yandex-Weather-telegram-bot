import { useState } from "react";
import "./ToogleButton.css";

function ToogleButton({ title, register, hasError }) {
  const [isToogled, setIsToogled] = useState(false);

  const toogle = () => setIsToogled(!isToogled);

  return (
    <label className="toogle-button form__toogle-button">
      <input type="checkbox" onClick={toogle} className="toogle-button__input" {...register} />
      <span className="toogle-button__appearance" />
      <span className="toogle-button__title">{title}</span>
    </label>
  );
}

export default ToogleButton;
