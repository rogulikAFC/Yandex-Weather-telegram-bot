import "./App.css";
import BlueButton from "./BlueButton/BlueButton";
import Field from "./Form/Field/Field";

function App() {
  return (
    <>
      <Field title="Логин" placeholder="Email"></Field>
      <BlueButton heightInPx={32} widthInPx={320} submit={true}>Войти</BlueButton>
    </>
  );
}

export default App;
