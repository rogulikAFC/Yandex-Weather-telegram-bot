import ReCAPTCHA from "react-google-recaptcha";
import "./RegistrationPage.css";
import { useRef } from "react";
import BlueButton from "../BlueButton/BlueButton";
import { useParams } from "react-router-dom";

function RegistrationPage() {
  const captcha_ref = useRef(null);
  const recaptcha_key = import.meta.env.VITE_RECAPTCHA_SITE_KEY;

  const telegram_bot_url = import.meta.env.VITE_TELEGRAM_BOT_URL;

  const { userTelegramId } = useParams();

  async function handleSubmitAsync(e) {
    e.preventDefault();

    const token = captcha_ref.current.getValue();
    captcha_ref.current.reset();

    var url = "https://localhost:7137/api/users"

    var response = await fetch(url, {
      method: "POST",
      body: JSON.stringify({
        id: Number(userTelegramId),
        captchaToken: token,
      }),
      mode: "cors",
      headers: new Headers({
        "content-type": "application/json",
        'Access-Control-Allow-Origin': '*'
      }),
    });

    if (!response.ok) {
      console.error(response.status);

      return;
    }

    console.log("successful!");

    window.location.replace(telegram_bot_url);
  }

  return (
    <div className="registration-container registration-container_centered">
      <header className="registration-container__header">
        <h1 className="registration-container__title">
          Yandex weather scrapper
        </h1>
        <p className="registration-container__subtitle">
          We just need verify that you're human
        </p>
        <form
          className="form registration-container__form"
          onSubmit={handleSubmitAsync}
        >
          <ReCAPTCHA sitekey={recaptcha_key} ref={captcha_ref} />
          <BlueButton heightInPx={32} widthInPx={360} submit={true}>
            Sign up
          </BlueButton>
        </form>
      </header>
    </div>
  );
}

export default RegistrationPage;
