import { useForm } from "react-hook-form";
import Field from "../Form/Field/Field";
import ToogleButton from "../Form/ToggleButton/ToogleButton";
import Error from "../Form/Error/Error";

function PlaceChangeForm({ blockName, place, placeId, userId }) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    values: place
      ? {
          name: place.name,
          isMain: place.isMain,
        }
      : null,
  });

  async function onSubmit(data) {
    const url = `https://localhost:7137/api/Places/${placeId}?user_id=${userId}`;

    let response = await fetch(url, {
      method: "PUT",
      body: JSON.stringify(data),
      headers: new Headers({
        "Content-Type": "application/json",
      }),
    });

    if (!response.ok) {
      console.log("Something went wrong:");

      var result = await response.json();

      console.log(result);

      return;
    }

    window.location.reload(false);
  }

  return (
    <form
      className={`place-change-form ${blockName}__place-change-form`}
      onSubmit={handleSubmit(onSubmit)}
      id="place-change-form"
    >
      <Field
        title="Name"
        hasError={errors.name}
        placeholder="Name"
        register={register("name", {
          maxLength: 64,
        })}
        error={
          errors.name &&
          errors.name.type === "maxLength" && (
            <Error>Max length is 64 symbols</Error>
          )
        }
      />

      <ToogleButton
        title="Main"
        register={register("isMain")}
        hasError={errors.isMain}
      />
    </form>
  );
}

export default PlaceChangeForm;
